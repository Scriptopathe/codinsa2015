﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace Clank.Generation
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
            string processedScript = Preprocessor.Run(script);

            // Crée les jetons d'expression à partir du script.
            var tokens = Tokenizers.Tokenizer.Parse(processedScript);
            var exprTokens = Tokenizers.ExpressionParser.Parse(tokens);

            // Crée la table des types à partir des jetons d'expression.
            // A ce point de l'exécution, la table ne contient pas les fonctions.
            Model.Semantic.TypeTable table = new Model.Semantic.TypeTable();
            table.FetchTypes(exprTokens);

            // Initialise le parseur sémantique et le log du parseur sémantique.
            Model.Semantic.SemanticParser parser = new Model.Semantic.SemanticParser();
            parser.Types = table;
            parser.Log.OnEvent += logHandler;

            try
            {
                // Project file.
                Model.ProjectFile project = new Model.ProjectFile() { Types = parser.Types };

                // Parse les différents blocks "main".
                foreach(var tk in exprTokens[0].ListTokens)
                {
                    var mainBlock = parser.Parse(tk);
                    project.ParseScript(mainBlock);
                }
               

                // Crée les instructions pour le projet Client et Serveur.
                List<Model.Language.Instruction> clientDecl = project.GenerateClientProject();
                List<Model.Language.Instruction> servDecl = project.GenerateServerProject();

                List<string> classes = new List<string>();
                StringBuilder builder = new StringBuilder();
                StringBuilder servBuilder = new StringBuilder();
                

                // Crée le code du serveur.
                Generation.Languages.ILanguageGenerator servGen = m_languagesTable[serverTarget.LanguageIdentifier];
                servGen.SetProject(project);
                foreach(Model.Language.Instruction inst in servDecl) { builder.AppendLine(servGen.GenerateInstruction(inst)); }
                outputFiles.Add(new OutputFile(serverTarget.OutputFilename, builder.ToString()));

                // Crée le code pour les clients
                foreach(GenerationTarget clientTarget in clientTargets)
                {
                    builder.Clear();
                    Generation.Languages.ILanguageGenerator clientGen = m_languagesTable[clientTarget.LanguageIdentifier];
                    clientGen.SetProject(project);
                    foreach (Model.Language.Instruction inst in clientDecl) { builder.AppendLine(clientGen.GenerateInstruction(inst)); }
                    outputFiles.Add(new OutputFile(clientTarget.OutputFilename, builder.ToString()));
                }

            }
            catch (EncoderFallbackException e)
            {

            }

            return outputFiles;
        }

        #endregion
    }
}
