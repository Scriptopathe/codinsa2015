using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Token = Clank.Tokenizers.ExpressionToken;
using TokenType = Clank.Tokenizers.ExpressionToken.ExpressionTokenType;
namespace Clank.Model.Semantic
{
    /// <summary>
    /// Permet la recherche, la sauvegarde et l'accès aux types standards et à ceux créés à partir du script.
    /// </summary>
    public class TypeTable
    {
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
        /// Retourne la fonction d'instance pour un type donné dont le nom est passé en paramètre.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Language.Function GetInstanceFunction(Language.ClankType owner, string functionName, Context context)
        {
            string fullName = owner.GetFullName() + "." + functionName;
            if (Functions.ContainsKey(fullName))
            {
                Language.Function func = Functions[fullName];
                if (func.IsStatic)
                    throw new InvalidOperationException("La fonction " + functionName + " n'est pas une méthode d'instance de " + owner.GetFullName() + ".");
                return Functions[fullName];
            }
            return null;
        }

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
        /// Retourne la fonction statique pour un type donné dont le nom est passé en paramètre.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Language.Function GetStaticFunction(Language.ClankType owner, string functionName, Context context)
        {
            string fullName = owner.GetFullName() + "." + functionName;
            if (Functions.ContainsKey(fullName))
            {
                Language.Function func = Functions[fullName];
                if (!func.IsStatic)
                    throw new InvalidOperationException("La fonction " + functionName + " n'est pas une méthode statique de " + owner.GetFullName() + ".");
                return Functions[fullName];
            }
            return null;
        }
        
        /// <summary>
        /// Retourne le constructeur (instancié) pour un type donné.
        /// </summary>
        public Language.Function GetConstructor(Language.ClankType owner, Language.ClankTypeInstance inst, Context context)
        {
            string fullName = owner.GetFullName() + "." + Language.SemanticConstants.New;
            if (Functions.ContainsKey(fullName))
            {
                Language.Function func = Functions[fullName];
                if (!func.IsConstructor)
                    throw new InvalidOperationException("La fonction " + Language.SemanticConstants.New + " n'est pas un constructeur de " + owner.GetFullName() + ".");
                return func.Instanciate(inst.GenericArguments);
            }

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
        /// Crée une nouvelle instance de TypeTable.
        /// </summary>
        public TypeTable()
        {
            Types.Add("string", Language.ClankType.String);
            Types.Add("int", Language.ClankType.Int32);
            Types.Add("bool", Language.ClankType.Bool);
            Types.Add("void", Language.ClankType.Void);
            Types.Add("Array", Language.ClankType.Array);
            Types.Add("float", Language.ClankType.Float);
            Types.Add("State", new Language.ClankType() { Name = "State", IsPublic = true });
            Types.Add("Type", new Language.ClankType() { Name = "Type" });

            TypeInstances.Add("string", new Language.ClankTypeInstance() { BaseType = Language.ClankType.String, IsGeneric = false });
            TypeInstances.Add("void", new Language.ClankTypeInstance() { BaseType = Language.ClankType.Void, IsGeneric = false });
            TypeInstances.Add("bool", new Language.ClankTypeInstance() { BaseType = Language.ClankType.Bool, IsGeneric = false });
            TypeInstances.Add("int", new Language.ClankTypeInstance() { BaseType = Language.ClankType.Int32, IsGeneric = false });
            TypeInstances.Add("float", new Language.ClankTypeInstance() { BaseType = Language.ClankType.Float, IsGeneric = false });
            TypeInstances.Add("State", new Language.ClankTypeInstance() { BaseType = Types["State"], IsGeneric = false });
            TypeInstances.Add("Type", new Language.ClankTypeInstance() { BaseType = Types["Type"], IsGeneric = false });
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
                    if (ParentContext.Container == Container || (Container.Name == "State")) // TODO : fix crade à rendre propre.
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
                        IsGeneric = false
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
                            IsGeneric = false,
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
                        if (newType.Name == "Array" && newType.GenericArgumentNames.Count == 1)
                        {
                            Types[newType.Name] = newType;

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
                                IsGeneric = false,
                            });
                        }
                        
         

                        // Recherche d'autres types dans les instructions.
                        FetchTypes(token.NamedGenericCodeBlockInstructions.ListTokens, childContext);
                    }
                    else
                    {
                        throw new InvalidOperationException("Déclaration de classe attendue.");

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
                baseType = Language.ClankType.Array;
                genericArguments.Add(FetchInstancedType(token.ArrayTypeIdentifier, context));
                isGeneric = true;
            }
            else if (token.TkType == TokenType.GenericType)
            {
                baseType = Types[token.GenericTypeIdentifier.Content];
                isGeneric = true;
                // TODO : gérer le cas du nom des arguments génériques.
                foreach(Token tok in token.GenericTypeArgs.ListTokens)
                {
                    genericArguments.Add(FetchInstancedType(tok, context));
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
                IsGeneric = isGeneric,
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
