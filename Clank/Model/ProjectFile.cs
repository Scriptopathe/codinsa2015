using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Model
{
    /// <summary>
    /// Représente un "fichier" projet, contenant les compartiments
    ///     - Access
    ///     - Header
    ///     - State
    ///     - Macro
    /// </summary>
    public class ProjectFile
    {
        /// <summary>
        /// Block access du projet.
        /// </summary>
        public AccessContainer Access { get; set; }
        /// <summary>
        /// Block state du projet.
        /// </summary>
        public StateContainer State { get; set; }
        /// <summary>
        /// Block macro du projet.
        /// </summary>
        public MacroContainer Macros { get; set; }
        /// <summary>
        /// Block write du projet.
        /// </summary>
        public WriteContainer Write { get; set; }
        /// <summary>
        /// Table des types.
        /// </summary>
        public Semantic.TypeTable Types { get; set; }

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de ProjectFile 
        /// </summary>
        public ProjectFile()
        {
            Access = new AccessContainer();
            State = new StateContainer();
            Macros = new MacroContainer();
            Write = new WriteContainer();
        }

        /// <summary>
        /// Parse le script et ajoute les champs trouvés dans les différents blocks.
        /// </summary>
        public void ParseScript(Language.NamedBlockDeclaration block)
        {
            Access.AddDeclarationsFromScript(block, Types);
            State.AddDeclarationsFromScript(block, Types);
            Macros.AddDeclarationsFromScript(block, Types);
            Write.AddDeclarationsFromScript(block, Types);
        }

        /// <summary>
        /// Génère un modèle abstrait représentant les classes à créer pour le serveur du projet cible.
        /// </summary>
        /// <returns></returns>
        public List<Language.Instruction> GenerateClientProject()
        {
            /*   
            int GetMyVar(int arg1, int arg2, Something<bool> arg3)
            {
                // Send
                List<object> args = new List<object>() { arg1, arg2, arg3 };
                int funcId = 1;
                List<object> obj = new List<object>() { funcId, args };
                TCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));

                // Receive
                string str = TCPHelper.Receive();
                Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);

                return o.Value<int>(0);
            }*/


            List<Language.Instruction> classes = new List<Language.Instruction>();

            // Copie de la classe state
            Language.ClassDeclaration stateClass = State.StateClass.Copy();
            stateClass.Instructions = stateClass.Instructions.Where((Language.Instruction inst) =>
            {
                // Ne garde que les fonctions.
                return inst is Language.FunctionDeclaration;
            }).ToList();

            stateClass.Comment = "Contient toutes les informations concernant l'état du serveur.";
            
            // Copie des classes state PUBLIQUES
            List<Language.ClassDeclaration> stateClasses = new List<Language.ClassDeclaration>();
            foreach (Language.ClassDeclaration decl in State.Classes) { if(decl.Modifiers.Contains(Language.SemanticConstants.Public)) stateClasses.Add(decl.Copy()); };


            // Ajoute les méthodes access / write.
            int id = 0;
            foreach (Language.FunctionDeclaration method in Access.Declarations)
            {
                stateClass.Instructions.Add(new Model.Language.Macros.RemoteFunctionWrapper(method, id));
                id++;
            }

            foreach (Language.FunctionDeclaration method in Write.Declarations)
            {
                stateClass.Instructions.Add(new Model.Language.Macros.RemoteFunctionWrapper(method, id));
                id++;
            }


            classes.AddRange(stateClasses);
            classes.Add(stateClass);

            return classes;
        }
        /// <summary>
        /// Génère un modèle abstrait représentant les classes à créer pour le serveur du projet cible. 
        /// </summary>
        /// <returns></returns>
        public List<Language.Instruction> GenerateServerProject()
        {
            List<Language.Instruction> classes = new List<Language.Instruction>();

            // Copie de la classe state
            Language.ClassDeclaration stateClass = State.StateClass.Copy();
            stateClass.Comment = "Contient toutes les informations concernant l'état du serveur.";
            // Copie des classes state
            List<Language.ClassDeclaration> stateClasses = new List<Language.ClassDeclaration>();
            foreach (Language.ClassDeclaration decl in State.Classes) { stateClasses.Add(decl.Copy()); };

           
            // Ajoute les méthodes access / write.
            foreach(Language.FunctionDeclaration method in Access.Declarations)
            {
                Language.FunctionDeclaration cpy = method.FuncCopy();
                cpy.Func.Arguments.Add(new Language.FunctionArgument() { ArgType = Types.FetchInstancedType("int", new Semantic.TypeTable.Context()), ArgName = Language.SemanticConstants.ClientID });
                stateClass.Instructions.Add(cpy);
            }

            foreach(Language.FunctionDeclaration method in Write.Declarations)
            {
                Language.FunctionDeclaration cpy = method.FuncCopy();
                cpy.Func.Arguments.Add(new Language.FunctionArgument() { ArgType = Types.FetchInstancedType("int", new Semantic.TypeTable.Context()), ArgName = Language.SemanticConstants.ClientID });
                stateClass.Instructions.Add(cpy);
            }

            // Ajoute la méthode de traitement des messages.
            // Le code effectif de cette méthode est créé par le générateur de code
            // du language cible.
            Language.Macros.ProcessMessageMacro processFunc = new Language.Macros.ProcessMessageMacro(Access, Write);
            processFunc.Comment = "Génère le code pour la fonction de traitement des messages.";
            stateClass.Instructions.Add(processFunc);


            // Classe de message
            //Language.ClassDeclaration messageClass = new Language.ClassDeclaration() { Name = "_Message" };
            //messageClass.Instructions.Add(new Language.VariableDeclarationInstruction() { Var = new Language.Variable() { Name = "Type", Type = enumMessageTypeInstance } });
            
            // Classes de message
            //List<Language.ClassDeclaration> messageClasses = new List<Language.ClassDeclaration>();
#if false
            // Ajout des fonctions de Write.
            #region Write
            foreach (Language.FunctionDeclaration ex in Write.Declarations)
            {
                // Membre de l'enum de message
                enumDecl.Members.Add("MessageType_" + ex.Func.Name);

                // Classe de message
                Language.ClankType klassType = new Language.ClankType() { Name = "Write_" + ex.Func.Name + "_Message" };
                Language.ClassDeclaration klass = new Language.ClassDeclaration();
                klass.Name = klassType.Name;
                
                // Création d'un constructeur.
                Language.Constructor cons = new Language.Constructor();
                cons.Owner = klassType;
                
                // Création :
                // Des variables d'instance de la classe.
                // Des paramètres du constructeur, de l'initialisation de ces paramètres
                foreach(Language.FunctionArgument arg in ex.Func.Arguments)
                {
                    
                    Language.Variable argVar = new Language.Variable() { Name = "_" + arg.ArgName, Type = arg.ArgType };

                    // Déclaration de la variable
                    klass.Instructions.Add(new Language.VariableDeclarationInstruction() { Var = argVar, Modifiers = new List<string>() { "public"} });

                    // Ajout de l'argument au constructeur.
                    cons.Arguments.Add(arg);
                    
                    // Ajout de l'assignement de la variable
                    cons.Code.Add(new Language.AffectationInstruction()
                    {
                        Expression = new Language.BinaryExpressionGroup()
                        {
                            Operand1 = argVar,
                            Operand2 = new Language.Variable() { Name = arg.ArgName, Type = arg.ArgType },
                            Operator = Language.Operator.Affectation
                        }
                    });
                }

                // Ajout de la nouvelle classe.
                klass.Instructions.Add(new Language.ConstructorDeclaration() { Func = cons });
                messageClasses.Add(klass);
                
                // Déclaration de fonction dans State.
                stateClass.Instructions.Add(ex);
            }
            #endregion


            // Ajout des fonctions de Access.
            #region Access
            foreach (Language.FunctionDeclaration decl in Access.Declarations)
            {
                // Membre de l'enum de message
                enumDecl.Members.Add("MessageType_" + decl.Func.Name);

                // Classe de message
                Language.ClankType klassType = new Language.ClankType() { Name = "Access_" + decl.Func.Name + "_Message" };
                Language.ClassDeclaration klass = new Language.ClassDeclaration();
                klass.Name = klassType.Name;

                // Création d'un constructeur.
                Language.Constructor cons = new Language.Constructor();
                cons.Owner = klassType;

                // Création :
                // Des variables d'instance de la classe.
                // Des paramètres du constructeur, de l'initialisation de ces paramètres
                foreach (Language.FunctionArgument arg in decl.Func.Arguments)
                {

                    Language.Variable argVar = new Language.Variable() { Name = "_" + arg.ArgName, Type = arg.ArgType };

                    // Déclaration de la variable
                    klass.Instructions.Add(new Language.VariableDeclarationInstruction() { Var = argVar, Modifiers = new List<string>() { "public" } });

                    // Ajout de l'argument au constructeur.
                    cons.Arguments.Add(arg);

                    // Ajout de l'assignement de la variable
                    cons.Code.Add(new Language.AffectationInstruction()
                    {
                        Expression = new Language.BinaryExpressionGroup()
                        {
                            Operand1 = argVar,
                            Operand2 = new Language.Variable() { Name = arg.ArgName, Type = arg.ArgType },
                            Operator = Language.Operator.Affectation
                        }
                    });
                }

                // Ajout de la nouvelle classe.
                klass.Instructions.Add(new Language.ConstructorDeclaration() { Func = cons });
                messageClasses.Add(klass);

                // Déclaration de fonction dans State.
                stateClass.Instructions.Add(decl);
            }
            #endregion
#endif

            //classes.AddRange(messageClasses);
            classes.AddRange(stateClasses);
            classes.Add(stateClass);
            return classes;
        }
        #endregion
    }
}
