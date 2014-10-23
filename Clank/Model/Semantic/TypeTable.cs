using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Token = Clank.Core.Tokenizers.ExpressionToken;
using TokenType = Clank.Core.Tokenizers.ExpressionToken.ExpressionTokenType;
namespace Clank.Core.Model.Semantic
{
    /// <summary>
    /// Permet la recherche, la sauvegarde et l'accès aux types standards et à ceux créés à partir du script.
    /// </summary>
    public class TypeTable
    {
        /// <summary>
        /// Event appelé lorsqu'une erreur doit être ajoutée au log.
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public event Tools.EventLog.EventLogHandler OnLog;
        /// <summary>
        /// Obtient ou définit une valeur indiquant si une exception est levée lors d'une erreur "récupérable".
        /// </summary>
        public bool RaiseOnError { get; set; }
        /// <summary>
        /// Représente les instances de types disponibles.
        /// </summary>
        public Dictionary<string, Language.ClankTypeInstance> TypeInstances = new Dictionary<string, Language.ClankTypeInstance>();
        /// <summary>
        /// Représente les instances de types.
        /// </summary>
        public Dictionary<string, Language.ClankType> Types = new Dictionary<string, Language.ClankType>();
        /// <summary>
        /// Représente les différentes fonctions créées et adressables.
        /// </summary>
        public Dictionary<string, Language.Function> Functions = new Dictionary<string, Language.Function>();


        /// <summary>
        /// Retourne la fonction dont le nom est passé en paramètre.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Language.Function GetFunction(string functionName, Context context)
        {
            string fullName = functionName;
            if (Functions.ContainsKey(fullName))
            {
                Language.Function func = Functions[fullName];
                return Functions[fullName];
            }
            return null;
        }

        /// <summary>
        /// Retourne la fonction d'instance pour un type donné dont le nom est passé en paramètre.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Language.Function GetInstanceFunction(Language.ClankTypeInstance instance, string functionName, List<Language.Evaluable> args, Context context)
        {
            Language.ClankType owner = instance.BaseType;
            string fullName = "";

            // Pour les fonctions du bloc state, elles ne sont pas préfixées dans la table.
            if (owner.GetFullName() == Language.SemanticConstants.StateClass)
                fullName = functionName + Language.Evaluable.GetArgTypesString(args);
            else
                fullName = owner.GetFullName() + "." + functionName + Language.Evaluable.GetArgTypesString(args);

            var instanciatedOverloads = InstanciatedFunc(instance);
            if (instanciatedOverloads.ContainsKey(fullName))
            {
                Language.Function func = instanciatedOverloads[fullName];
                // Vérification : la fonction est-elle une méthode d'instance ?
                if (func.IsStatic)
                {
                    throw new SemanticError("La fonction " + functionName + " n'est pas une méthode d'instance de " + owner.GetFullName() + ".");
                }
                return instanciatedOverloads[fullName];
            }
            return null;
        }

        /// <summary>
        /// Retourne la fonction statique pour un type donné dont le nom est passé en paramètre.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Language.Function GetStaticFunction(Language.ClankTypeInstance instancedType, string functionName, List<Language.Evaluable> args, Context context)
        {
            // Obtient le nom complet de la fonction (avec les args).
            Language.ClankType owner = instancedType.BaseType;
            string fullName = owner.GetFullName() + "." + functionName + Language.Evaluable.GetArgTypesString(args);

            var instanciatedOverloads = InstanciatedFunc(instancedType);
            if (instanciatedOverloads.ContainsKey(fullName))
            {
                Language.Function func = instanciatedOverloads[fullName];
                // Vérification : la fonction est-elle statique ?
                if (!func.IsStatic)
                {
                    throw new SemanticError("La fonction " + functionName + " n'est pas une méthode statique de " + owner.GetFullName() + ".");
                }
                return instanciatedOverloads[fullName];
            }
            return null;
        }


        /// <summary>
        /// Retourne le constructeur (instancié) pour un type donné.
        /// </summary>
        public Language.Function GetConstructor(Language.ClankType owner, Language.ClankTypeInstance inst, List<Language.Evaluable> args, Context context)
        {
            string fullName = owner.GetFullName() + "." + Language.SemanticConstants.New + Language.Evaluable.GetArgTypesString(args);
            var instanciedOverloads = InstanciatedFunc(inst);
            if (instanciedOverloads.ContainsKey(fullName))
            {
                Language.Function func = instanciedOverloads[fullName];

                // Vérification : la fonction est-elle bien marquée comme constructeur ?
                if (!func.IsConstructor)
                {
                    throw new SemanticError("La fonction " + Language.SemanticConstants.New + " n'est pas un constructeur de " + owner.GetFullName() + ".");
                }
                return func.Instanciate(inst.GenericArguments);
            }

            if (args.Count != 0)
                throw new SemanticError("Aucun constructeur de '" + owner.GetFullName() + "' ne matche les arguments de type : " +
                    Language.Evaluable.GetArgTypesString(args) + ". Candidats possibles : " + GetCandidatesStr(inst, Language.SemanticConstants.New));

            // Si le constructeur n'existe pas, on crée le constructeur par défaut.
            Language.Function cons = new Language.Function()
            {
                Modifiers = new List<string>() { Language.SemanticConstants.Public, Language.SemanticConstants.Constructor },
                ReturnType = inst,
                Arguments = new List<Language.FunctionArgument>(),
                Code = new List<Language.Instruction>(),
                Owner = owner,
                Type = inst,
                Name = Language.SemanticConstants.New
            };
            return cons;
        }
        /// <summary>
        /// Retourne une copie de Functions dont les arguments génériques dans les clefs sont remplacés par les arguments donnés.
        /// Ex :
        /// Args = (5)
        /// List.add(List'T) => List.add(int)
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public Dictionary<string, Language.Function> InstanciatedFunc(Language.ClankTypeInstance instance)
        {
            Dictionary<string, Language.Function> funcs = new Dictionary<string, Language.Function>();
            foreach (var kvp in Functions)
            {
                string key = kvp.Key;
                for (int i = 0; i < instance.BaseType.GenericArgumentNames.Count; i++)
                {
                    string argName = instance.BaseType.GenericArgumentNames[i];
                    key = key.Replace(instance.BaseType.GetFullName() + "'" + argName, instance.GenericArguments[i].GetFullName());
                }
                funcs.Add(key, kvp.Value);
            }
            return funcs;
        }
        /// <summary>
        /// Retourne les fonctions candidates ayant le nom donné.
        /// (utile pour afficher des erreurs à l'utilisateur).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<string> GetCandidates(Language.ClankTypeInstance inst, string name)
        {
            var funcs = InstanciatedFunc(inst);
            List<string> candidates = new List<string>();
            foreach(var kvp in funcs)
            {
                if (kvp.Key.Contains(inst.BaseType.GetFullName() + "." + name))
                    candidates.Add(kvp.Key);
            }
            return candidates;
        }
        /// <summary>
        /// Retourne sous forme de string les fonctions candidates ayant le nom donné.
        /// (utile pour afficher des erreurs à l'utilisateur).
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetCandidatesStr(Language.ClankTypeInstance inst, string name)
        {
            var funcs = InstanciatedFunc(inst);
            string val = "";

            // Fullname : si on est sur une méthode d'instance de State, celle ci n'est pas préfixée
            // de state dans la table.
            string fullname = "";
            if (inst.BaseType.GetFullName() == Language.SemanticConstants.StateClass)
                fullname = name;
            else
                fullname = inst.BaseType.GetFullName() + "." + name;

            foreach(var kvp in funcs)
            {
                if (kvp.Key.Contains(fullname))
                    val += kvp.Key + "  ";
            }
            return val;
        }
        static int instCOunt;
        /// <summary>
        /// Crée une nouvelle instance de TypeTable.
        /// </summary>
        public TypeTable()
        {
            Types.Add("string", Language.ClankType.String);
            Types.Add("int", Language.ClankType.Int32);
            Types.Add("bool", Language.ClankType.Bool);
            Types.Add("void", Language.ClankType.Void);
            Types.Add("Array", new Language.ClankType() { Name = "Array", IsPublic = true, JType = Language.JSONType.Array, IsMacro = true, GenericArgumentNames = new List<string>() { "T" }});
            Types.Add("float", Language.ClankType.Float);
            Types.Add(Language.SemanticConstants.StateClass, new Language.ClankType() { Name = Language.SemanticConstants.StateClass, IsPublic = true });
            Types.Add("Type", new Language.ClankType() { Name = "Type" });

            TypeInstances.Add("string", new Language.ClankTypeInstance() { BaseType = Language.ClankType.String });
            TypeInstances.Add("void", new Language.ClankTypeInstance() { BaseType = Language.ClankType.Void });
            TypeInstances.Add("bool", new Language.ClankTypeInstance() { BaseType = Language.ClankType.Bool });
            TypeInstances.Add("int", new Language.ClankTypeInstance() { BaseType = Language.ClankType.Int32 });
            TypeInstances.Add("float", new Language.ClankTypeInstance() { BaseType = Language.ClankType.Float });
            TypeInstances.Add(Language.SemanticConstants.StateClass, new Language.ClankTypeInstance() { BaseType = Types[Language.SemanticConstants.StateClass] });
            TypeInstances.Add("Type", new Language.ClankTypeInstance() { BaseType = Types["Type"] });

            // Setup de Array
            Types["Array"].JArrayElementType = Types["Array"].AsInstance().GenericArguments[0].BaseType;
            instCOunt++;
        }

        #region Classes
        public class Context
        {
            /// <summary>
            /// Nom du bloc dans lequel est contenu le contexte.
            /// </summary>
            public string BlockName
            {
                get;
                set;
            }
            /// <summary>
            /// Contexte parent.
            /// </summary>
            public Context ParentContext
            {
                get;
                set;
            }

            /// <summary>
            /// Classe contenant ce contexte.
            /// </summary>
            public Language.ClankType Container { get; set; }

            /// <summary>
            /// Retourne les variables de ce contexte.
            /// </summary>
            public Dictionary<string, Language.Variable> Variables { get; set; }

            /// <summary>
            /// Retourne la fonction contenant les éléments de ce contexte.
            /// Vaut nulle si aucune.
            /// </summary>
            public Language.FunctionDeclaration ContainingFunc { get; set; }

            /// <summary>
            /// Crée une nouvelle instance de Contexte.
            /// </summary>
            public Context()
            {
                ParentContext = null;
                Container = null;
                Variables = new Dictionary<string, Language.Variable>();
            }
            /// <summary>
            /// Retourne le préfixe représentant ce contexte.
            /// Il sera associé au nom des types dans la table des types.
            /// </summary>
            public string GetContextPrefix()
            {
                if (ParentContext == null)
                    if (Container == null)
                        return "";
                    else
                        return Container.Name;
                else
                    if (ParentContext.Container == Container || (Container.Name == Language.SemanticConstants.StateClass)) // TODO : fix crade à rendre propre.
                        return ParentContext.GetContextPrefix();
                    else
                        return ParentContext.GetContextPrefix() + Container.Name + ".";
            }
            /// <summary>
            /// Retourne les variables de ce contexte et des contextes parents.
            /// </summary>
            /// <returns></returns>
            public Dictionary<string, Language.Variable> GetAllVariables()
            {
                Dictionary<string, Language.Variable> variables = new Dictionary<string, Language.Variable>();
                foreach(var kvp in Variables)
                    variables.Add(kvp.Key, kvp.Value);

                // Variables du contexte parent
                if(ParentContext != null)
                    foreach (var kvp in ParentContext.Variables)
                        variables.Add(kvp.Key, kvp.Value);

                // Ajout de this
                Language.Variable thisVar = new Language.Variable()
                {
                    Name = "this",
                    Type = new Language.ClankTypeInstance()
                    {
                        BaseType = Container,
                    }
                };

                if (variables.ContainsKey("this"))
                {
                    variables["this"] = thisVar;
                }
                else
                    variables.Add("this", thisVar);

                return variables;
            }
        }
        #endregion
        #region API
        /// <summary>
        /// Effectue une recherche des types déclarés dans le script passé en paramètre.
        /// Seule la section "State" du script doit être passée.
        /// </summary>
        public void FetchTypes(List<Token> tokens)
        {
            FetchTypes(tokens, new Context());
            FetchInstancedTypes(tokens, new Context());
        }
        /// <summary>
        /// Effectue une recherche des types déclarés dans le script passé en paramètre.
        /// 
        /// Seule la section "State" du script doit être passée.
        /// </summary>
        void FetchTypes(List<Token> tokens, Context context)
        {
            bool foundClass = false;
            bool isPublic = false;
            foreach(Token token in tokens)
            {
                if (foundClass)
                {
                    #region FoundClass
                    if (token.TkType == TokenType.NamedCodeBlock)
                    {
                        // SECU : vérifier que CodeBlockIdentifier est un Name.

                        // Crée le type de base.
                        Language.ClankType newType = new Language.ClankType()
                        {
                            Name = token.NamedCodeBlockIdentifier.Content,
                            IsPublic = isPublic,
                            IsMacro = context.BlockName == Language.SemanticConstants.MacroBk
                        };

                        // Crée l'instance associée.
                        Language.ClankTypeInstance newTypeInstance = new Language.ClankTypeInstance()
                        {
                            BaseType = newType,
                            GenericArguments = new List<Language.ClankTypeInstance>(),
                        };

                        TypeInstances.Add(newTypeInstance.GetFullName(), newTypeInstance);
                        Types.Add(newType.Name, newType);
                    }
                    // On cherche les paramètres génériques et on en crée des instances.
                    else if (token.TkType == TokenType.NamedGenericCodeBlock)
                    {
                        List<string> genericArgsNames = new List<string>();

                        // Arguments du type générique
                        foreach (Token t in token.NamedGenericCodeBlockArgs.ListTokens)
                        {
                            if (t.ListTokens.Count != 1 && t.ChildToken.TkType != TokenType.Name)
                                throw new InvalidOperationException("Déclaration d'argument générique invalide");

                            // Identificateur du type : contient son nom.
                            genericArgsNames.Add(t.ChildToken.Content);
                        }

                        // Nouveau type générique
                        Language.ClankType newType = new Language.ClankType()
                        {
                            Name = token.NamedGenericCodeBlockNameIdentifier.Content,
                            GenericArgumentNames = genericArgsNames,
                            IsPublic = isPublic,
                            IsMacro = context.BlockName == Language.SemanticConstants.MacroBk
                        };

                        // Override pour le type array.
                        if (newType.Name == "Array" && newType.GenericArgumentNames.Count == 1 && newType.IsMacro)
                        {
                            // Types[newType.Name] = newType;
                            newType = Types[newType.Name];
                        }
                        else
                        {
                            Types.Add(newType.Name, newType);
                        }
                        // Création du contexte fils
                        Context childContext = new Context();
                        childContext.ParentContext = context;
                        childContext.Container = newType;

                        // Création des types génériques et de leur instance.
                        string contextPrefix = childContext.GetContextPrefix().Trim('.') + "'";
                        for(int i = 0; i < genericArgsNames.Count; i++)
                        {
                            Language.GenericParameterType genericType = new Language.GenericParameterType() 
                            {
                                ParamId = i,
                                Name = genericArgsNames[i],
                                Prefix = contextPrefix
                            };

                            Types.Add(genericType.GetFullName(), genericType);
                            TypeInstances.Add(contextPrefix + genericArgsNames[i], new Language.ClankTypeInstance()
                            {
                                BaseType = genericType,
                            });
                        }
                        
         

                        // Recherche d'autres types dans les instructions.
                        FetchTypes(token.NamedGenericCodeBlockInstructions.ListTokens, childContext);
                    }
                    else
                    {
                        // throw new InvalidOperationException("Déclaration de classe attendue.");
                        continue;
                    }
                    #endregion

                    foundClass = false;
                    isPublic = false;
                }
                else // if !foundClass
                {
                    switch (token.TkType)
                    {
                        // Parsing récursif des Listes
                        case TokenType.ArgList:
                        case TokenType.CodeBlock:
                        case TokenType.List:
                        case TokenType.InstructionList:
                            FetchTypes(token.ListTokens, context);
                            break;
                        case TokenType.NamedCodeBlock:
                            context.BlockName = token.NamedCodeBlockIdentifier.Content;
                            FetchTypes(token.ListTokens, context);
                            break;
                        // Vérification du mot clef class
                        case TokenType.Name:
                            if (token.Content == Language.SemanticConstants.Class)
                            {
                                foundClass = true;
                            }
                            else if(token.Content == Language.SemanticConstants.Public)
                            {
                                isPublic = true;
                            }
                            break;
                    }
                }
            }

        }
        /// <summary>
        /// Retourne vrai si la table contient le type dont le nom est passé en paramètre.
        /// </summary>
        public bool ContainsType(string typename, out Language.ClankTypeInstance type)
        {
            type = null;
            if(TypeInstances.ContainsKey(typename))
            {
                type = TypeInstances[typename];
                return true;
            }
            return false;
        }
        #endregion
   
        /// <summary>
        /// Cherche les types dérivés des types créés instanciés dans le script.
        /// </summary>
        void FetchInstancedTypes(List<Token> tokens, Context context)
        {
            bool foundClass = false;
            foreach(Token token in tokens)
            {

                if (!foundClass)
                {
                    foundClass = false;
                    switch (token.TkType)
                    {
                        // Si on trouve un type HORS déclaration, on l'ajoute aux types instanciés.
                        case TokenType.ArrayType:
                        case TokenType.GenericType:
                        case TokenType.Name:
                            FetchInstancedType(token, context);
                            break;
                        // Parsing récursif des Listes
                        case TokenType.ArgList:
                        case TokenType.CodeBlock:
                        case TokenType.NamedCodeBlock:
                        case TokenType.List:
                        case TokenType.InstructionList:
                            FetchInstancedTypes(token.ListTokens, context);
                            break;
                        case TokenType.FunctionDeclaration:
                            FetchInstancedTypes(token.FunctionDeclArgs.ListTokens, context);
                            FetchInstancedTypes(token.FunctionDeclCode.ListTokens, context);
                            break;
                    }
                }
                else if(foundClass)
                {
                    // Si on trouve une classe générique, on ajoute ses types au contexte.
                    if(token.TkType == TokenType.NamedGenericCodeBlock)
                    {

                        // Création du contexte fils
                        Context childContext = new Context();
                        childContext.ParentContext = context;
                        childContext.Container = Types[token.NamedGenericCodeBlockNameIdentifier.Content];

                        FetchInstancedTypes(token.NamedGenericCodeBlockInstructions.ListTokens, childContext);
                    }
                }


                foundClass = token.TkType == TokenType.Name && token.Content == Language.SemanticConstants.Class;
            }
        }
        /// <summary>
        /// Obtient le type correspondant au nom donné dans le contexte donné.
        /// </summary>
        public Language.ClankTypeInstance FetchInstancedType(string name, Context context)
        {
            return FetchInstancedType(new Token() { Content = name, TkType = TokenType.Name }, context);
        }
        /// <summary>
        /// Parse le jeton pour en extraire une instance de type de base en fonction du contexte.
        /// </summary>
        public Language.ClankTypeInstance FetchInstancedType(Token token, Context context)
        {
            Language.ClankType baseType = null;
            bool isGeneric = false;
            List<Language.ClankTypeInstance> genericArguments = new List<Language.ClankTypeInstance>();
            if(token.TkType == TokenType.List)
            {
                if (token.ListTokens.Count != 1)
                    throw new InvalidOperationException();

                return FetchInstancedType(token.ListTokens[0], context);
            }
            else if (token.TkType == TokenType.ArrayType)
            {
                // On crée un array avec comme param générique le type de cette array.
                baseType = Types["Array"];
                Language.ClankTypeInstance inst = FetchInstancedType(token.ArrayTypeIdentifier, context);

                if (inst == null)
                {
                    string error = "Le type " + token.ArrayTypeIdentifier.Content + " n'existe pas.";
                    if (OnLog != null)
                        OnLog(new Tools.EventLog.Entry(Tools.EventLog.EntryType.Error, error, token.Line, token.Character, token.Source));
                    throw new SemanticError(error);
                }

                genericArguments.Add(inst);
                isGeneric = true;
            }
            else if (token.TkType == TokenType.GenericType)
            {
                if(!Types.ContainsKey(token.GenericTypeIdentifier.Content))
                {
                    // Erreur type inconnu.
                    string error = "Le type générique '" + token.GenericTypeIdentifier.Content + "' est inconnu.";
                    OnLog(new Tools.EventLog.Entry(Tools.EventLog.EntryType.Error, error, token.Line, token.Character, token.Source));
                    throw new SemanticError(error);
                }
                baseType = Types[token.GenericTypeIdentifier.Content];
                isGeneric = true;
                foreach(Token tok in token.GenericTypeArgs.ListTokens)
                {
                    Language.ClankTypeInstance inst = FetchInstancedType(tok, context);
                    if(inst == null)
                    {
                        string error = "Le type " + token.Content + " n'existe pas.";
                        if(OnLog != null)
                            OnLog(new Tools.EventLog.Entry(Tools.EventLog.EntryType.Error, error, token.Line, token.Character, token.Source));
                        throw new SemanticError(error);
                    }
                    genericArguments.Add(inst);
                
                }
                // Vérification : le nombre d'arguments du type générique est-il correct ?
                if(baseType.GenericArgumentNames.Count != token.GenericTypeArgs.ListTokens.Count)
                {
                    string error = "Nombre d'arguments pour le type '" + baseType.GetFullName() + "' incorrect. Attendu : " + baseType.GetFullNameAndGenericArgs() +
                        ". Obtenu " + baseType.GetFullName() + "<" + token.GenericTypeArgs.ToReadableCode() + "> (" + token.GenericTypeArgs.ListTokens.Count + " args).";

                    if(OnLog != null)
                        OnLog(new Tools.EventLog.Entry(Tools.EventLog.EntryType.Error, error, token.Line, token.Character, token.Source));
                }
            }
            else if (token.TkType == TokenType.Name)
            {
                
                if (TypeInstances.ContainsKey(token.Content))
                    return TypeInstances[token.Content];
                else
                {
                    Context c = context;
                    while(c != null)
                    {
                        string contextPrefix = c.GetContextPrefix().Trim('.') + "'";
                        if(TypeInstances.ContainsKey(contextPrefix + token.Content))
                        {
                            // Instance de type générique
                            return TypeInstances[contextPrefix + token.Content];
                        }
                        c = c.ParentContext;
                    }
                }

                // Rien n'a été trouvé, on retourne null. (ce n'est pas un type)
                return null;
            }

            Language.ClankTypeInstance type = new Language.ClankTypeInstance()
            {
                BaseType = baseType,
                GenericArguments = genericArguments
            };

            string fullName = type.GetFullName();
            if (!TypeInstances.ContainsKey(fullName))
            {
                TypeInstances.Add(fullName, type);
                return type;
            }
            else
                return TypeInstances[fullName];
        }
    }
}
