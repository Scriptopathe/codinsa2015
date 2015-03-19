using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Clank.Core.Generation
{
    public class OutputFile
    {
        public string Name;
        public string Content;
        public OutputFile(string name, string content)
        {
            Name = name;
            Content = content;
        }
    }
    /// <summary>
    /// Classe permettant de générer un projet.
    /// </summary>
    public class ProjectGenerator
    {
        #region Variables
        /// <summary>
        /// Contient une table des langages utilisables pour la génération.
        /// </summary>
        Dictionary<string, Languages.ILanguageGenerator> m_languagesTable;
        /// <summary>
        /// Représente le préprocesseur utilisé par cette instance de ProjectGenerator.
        /// </summary>
        Preprocessor.Preprocessor m_preprocessor;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient une référence vers le préprocesseur à utiliser pour le pre-process des fichiers de script.
        /// </summary>
        public Preprocessor.Preprocessor Preprocessor
        {
            get { return m_preprocessor; }
            private set { m_preprocessor = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de ProjectGenerator.
        /// </summary>
        public ProjectGenerator()
        {
            GenerateLanguagesTable();
            Preprocessor = new Preprocessor.Preprocessor();
        }

        /// <summary>
        /// Génère la table des languages à partir des classes héritées de ILanguageGenerator et ayant
        /// un attribut de type LanguageGeneratorAttribute.
        /// </summary>
        void GenerateLanguagesTable()
        {
            m_languagesTable = new Dictionary<string, Languages.ILanguageGenerator>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            foreach(Type t in types)
            {
                object[] attributes = t.GetCustomAttributes(typeof(Languages.LanguageGeneratorAttribute), false);
                foreach(object attr in attributes)
                {
                    Languages.LanguageGeneratorAttribute langGen = (Languages.LanguageGeneratorAttribute)attr;
                    Languages.ILanguageGenerator lang = (Languages.ILanguageGenerator)Activator.CreateInstance(t);
                    m_languagesTable.Add(langGen.LanguageIdentifier, lang);
                }
            }
        }

        /// <summary>
        /// Génère le projet à partir d'un script.
        /// </summary>
        /// <param name="script">Script à parser.</param>
        /// <param name="serverTarget">Langage/Filename dans lequel générer le serveur.</param>
        /// <param name="clientTargets">Langages/Filenames dans lesquels générer</param>
        /// <param name="generationLog">Sortie d'erreurs / warnings du parseur de code.</param>
        /// <returns></returns>
        public List<OutputFile> Generate(string script, GenerationTarget serverTarget, List<GenerationTarget> clientTargets, out string generationLog)
        {
            StringBuilder log = new StringBuilder();
            Tools.EventLog.EventLogHandler logHandler = new Tools.EventLog.EventLogHandler((Tools.EventLog.Entry entry) =>
            {
                log.AppendLine(entry.Type.ToString() + " (" + entry.Source + ") : " + "line " + entry.Line + ". " + entry.Message + ". ");
            });
            var output = Generate(script, serverTarget, clientTargets, logHandler);
            generationLog = log.ToString();
            return output;
        }

        string plinemapping(Dictionary<int, Generation.Preprocessor.Preprocessor.LineInfo> map, string script)
        {
            StringBuilder b = new StringBuilder();
            string[] lines = script.Split('\n');

            foreach(var kvp in map)
            {
                if (kvp.Key < lines.Length)
                    b.Append(kvp.Key + ":" + kvp.Value.ToString() + ":" + lines[kvp.Key]);
                else
                    b.Append(kvp.Key + ":" + kvp.Value.ToString() + ":" + " out of range");
            }
            return b.ToString();
        }
        /// <summary>
        /// Génère le projet à partir d'un script.
        /// TODO :  - préprocesseur (include)
        ///         - Crée une classe de chargement de fichier + interface.
        /// </summary>
        /// <param name="processedScript">Script à parser.</param>
        /// <param name="serverTarget">Langage/Filename dans lequel générer le serveur.</param>
        /// <param name="clientTargets">Langages/Filenames dans lesquels générer</param>
        /// <param name="logHandler">Fonction que le parseur va appeler lorsqu'un event 
        /// (erreur / warning / autre) doit être journalisé.
        /// </param>
        public List<OutputFile> Generate(string script, GenerationTarget serverTarget, List<GenerationTarget> clientTargets, 
                                        Tools.EventLog.EventLogHandler logHandler)
        {
            List<OutputFile> outputFiles = new List<OutputFile>();

            // Lance le pré-processeur sur le script
            Dictionary<int, Generation.Preprocessor.Preprocessor.LineInfo> lineMapping = new Dictionary<int,Preprocessor.Preprocessor.LineInfo>();
            Model.ProjectFile project = null;
            string processedScript;
            try
            {
                processedScript = Preprocessor.Run(script, ref lineMapping);
            }
            catch(Generation.Preprocessor.RessourceNotFoundException e)
            {
                logHandler(new Tools.EventLog.Entry(Tools.EventLog.EntryType.Error, "[Preprocessor] La ressource '" + e.Ressource + "' est introuvable", 0, 0, "[unknown]"));
                return new List<OutputFile>();
            }
            try
            {
                // Crée les jetons d'expression à partir du script.
                var tokens = Tokenizers.Tokenizer.Parse(processedScript, lineMapping);
                var exprTokens = Tokenizers.ExpressionParser.Parse(tokens);
               

                // Crée la table des types à partir des jetons d'expression.
                // A ce point de l'exécution, la table ne contient pas les fonctions.
                Model.Semantic.TypeTable table = new Model.Semantic.TypeTable();
                table.OnLog += logHandler;

                // Initialise le parseur sémantique et le log du parseur sémantique.
                Model.Semantic.SemanticParser parser = new Model.Semantic.SemanticParser();
                parser.Types = table;
                parser.Log.OnEvent += logHandler;


                // Project file.
                project = new Model.ProjectFile() { Types = parser.Types };
                project.Log.OnEvent += logHandler;

                // Parse les différents blocks "main".
                try
                {
                    foreach (var tk in exprTokens[0].ListTokens)
                    {
                        if (tk.TkType != Tokenizers.ExpressionToken.ExpressionTokenType.Comment)
                        {
                            var mainBlock = parser.Parse(tk);
                            project.ParseScript(mainBlock);
                        }
                    }
                }
                finally
                {
                    parser.Log.OnEvent -= logHandler;
                    table.OnLog -= logHandler;
                }

                // Crée les instructions pour le projet Client et Serveur.
                List<Model.Language.Instruction> clientDecl = project.GenerateClientProject();
                List<Model.Language.Instruction> servDecl = project.GenerateServerProject();

                List<string> classes = new List<string>();
                StringBuilder builder = new StringBuilder();
                StringBuilder servBuilder = new StringBuilder();
                

                // Crée le code du serveur.
                if (!m_languagesTable.ContainsKey(serverTarget.LanguageIdentifier))
                    throw new Exception("Le langage sélectionné pour le serveur '" + serverTarget.LanguageIdentifier + "' n'est pas implémenté");

                Generation.Languages.ILanguageGenerator servGen = m_languagesTable[serverTarget.LanguageIdentifier];
                servGen.SetProject(project);
                outputFiles.AddRange(servGen.GenerateProjectFiles(servDecl, serverTarget.OutputDirectory, true));

                // Crée le code pour les clients
                foreach(GenerationTarget clientTarget in clientTargets)
                {
                    builder.Clear();

                    if (!m_languagesTable.ContainsKey(clientTarget.LanguageIdentifier))
                    {
                        logHandler(new Tools.EventLog.Entry(Tools.EventLog.EntryType.Error,
                            "Le langage sélectionné pour le client '" + clientTarget.LanguageIdentifier + "' n'est pas implémenté. Compilation pour ce langage annulée", 0, 0, "compiler"));
                        continue;
                    }

                    Generation.Languages.ILanguageGenerator clientGen = m_languagesTable[clientTarget.LanguageIdentifier];
                    clientGen.SetProject(project);
                    outputFiles.AddRange(clientGen.GenerateProjectFiles(clientDecl, clientTarget.OutputDirectory, false));
                }

            }
            catch (Core.Tokenizers.SyntaxError syntaxError)
            {
                logHandler(new Tools.EventLog.Entry(Tools.EventLog.EntryType.Error, "Erreur de syntaxe : " + syntaxError.Message, syntaxError.Line, 0, syntaxError.Source));
                outputFiles.Clear();
            }
            catch (Core.Model.Semantic.SemanticError semError)
            {
                // Le parseur sémantique utilise déjà le log.
                outputFiles.Clear();
            }
            finally
            {
                if(project != null)
                    project.Log.OnEvent -= logHandler;
            }
            return outputFiles;
        }

        #endregion
    }
}
