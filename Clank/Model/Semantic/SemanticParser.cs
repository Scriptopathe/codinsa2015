using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Token = Clank.Core.Tokenizers.ExpressionToken;
using TokenType = Clank.Core.Tokenizers.ExpressionToken.ExpressionTokenType;
using SyntaxError = Clank.Core.Tokenizers.SyntaxError;
namespace Clank.Core.Model.Semantic
{
    public class SemanticError : Exception
    {
        public SemanticError() : base() { }
        public SemanticError(string msg) : base(msg) { }
    }
    /// <summary>
    /// Classe permettant de transformer un ensemble de jetons en Arbre syntaxique dans le langage Clank.Core.
    /// </summary>
    public class SemanticParser
    {
        public bool StrictCompilation = false;
        static Tools.EventLog s_log = new Tools.EventLog();

        /// <summary>
        /// Obtient le log utilisé pour la journalisation des warning et erreurs renvoyés par le parseur.
        /// </summary>
        public Tools.EventLog Log { get { return s_log; } }

        /// <summary>
        /// Obtient ou définit les types disponibles dans le contexte.
        /// </summary>
        public TypeTable Types { get; set; }

        /// <summary>
        /// Parse le block donné pour en déduire des instructions à language Clank.Core.
        /// </summary>
        public Language.NamedBlockDeclaration Parse(Token mainBlock)
        {
            if (mainBlock.TkType != TokenType.NamedCodeBlock)
                throw new InvalidOperationException();

            // Crée la table de types.
            if(Types == null)
                Types = new TypeTable();
            Types.FetchTypes(mainBlock.ListTokens);

            // Préparse le block pour ajouter les fonctions et variables d'instance à la table des types.
            ParseNamedBlock(mainBlock, new TypeTable.Context(), true);
            
            // Crée le block et le retourne.
            return ParseNamedBlock(mainBlock, new TypeTable.Context(), false);
        }
        /// <summary>
        /// Parse un token de type NamedCodeBlock afin d'en extraire une déclaration
        /// de bloc nommé.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="considerAsClass">Si ce paramètre vaut vrai, le block est considéré comme une classe. (des variables d'instances
        /// seront ajoutés au type conteneur du contexte). Cela sert pour la simulation de la classe State dans le block state.</param>
        /// <returns></returns>
        Language.NamedBlockDeclaration ParseNamedBlock(Token block, TypeTable.Context context, bool preparse, bool considerAsClass=false)
        {
            if (block.TkType != TokenType.NamedCodeBlock)
                throw new InvalidOperationException();

            Language.NamedBlockDeclaration main = new Language.NamedBlockDeclaration();

            main.Instructions = ParseBlock(block.NamedCodeBlockInstructions, context, considerAsClass, preparse);
            main.Name = block.NamedCodeBlockIdentifier.Content;

            

            return main;
        }

        /// <summary>
        /// Parse un jeton de type InstructionList afin d'en extraire une liste d'instructions.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        List<Language.Instruction> ParseBlock(Token block, TypeTable.Context context, bool fromClass, bool preparse)
        {
            if (block.TkType != TokenType.InstructionList && block.TkType != TokenType.CodeBlock)
                throw new InvalidOperationException();

            List<Language.Instruction> instructions = new List<Language.Instruction>();
            
            List<Token> previous = new List<Token>();
            for(int i = 0; i < block.ListTokens.Count; i++)
            {
                Token instructionToken = block.ListTokens[i];

                Language.Instruction instruction = ParseInstruction(instructionToken, context, fromClass, preparse);
                if (instruction != null)
                    instructions.Add(instruction);
            }

            return instructions;
        }


        /// <summary>
        /// Parse une instruction dans un jeton de type liste.
        /// </summary>
        Language.Instruction ParseInstruction(Token instructionToken, TypeTable.Context context, bool fromClass, bool preparse)
        {
            if (instructionToken.TkType != TokenType.List)
                throw new InvalidOperationException();

            Pattern.Match match;
            #region Return
            // Mot clef return.
            match = Pattern.ReturnPattern.MatchPattern(instructionToken.ListTokens);
            if (!preparse && match.MatchPattern)
            {
                Language.ReturnInstruction ret = new Language.ReturnInstruction();
                ret.Line = instructionToken.ChildToken.Line;
                ret.Character = instructionToken.ChildToken.Character;
                ret.Source = instructionToken.ChildToken.Source;
                // Commentaire optionnel attaché.
                var commentMatch = match.FindByIdentifier("Comment");
                if (commentMatch.Count != 0)
                    ret.Comment = commentMatch.First().MatchedToken.Content;
                
                if (match.FindByIdentifier("Evaluable").Count > 0)
                {
                    ret.Value = ParseEvaluable(match.FindByIdentifier("Evaluable").First().MatchedToken, context, preparse);

                    // Type checking
                    if(context.ContainingFunc.Func.IsConstructor)
                    {
                        string error = "Instruction 'return' inattendue dans un constructeur.";

                        Log.AddWarning(error, ret.Line, ret.Character, ret.Source);
                        if (StrictCompilation)
                            throw new SemanticError(error);
                    }
                    else
                        if (ret.Value.Type.GetFullName() != context.ContainingFunc.Func.ReturnType.GetFullName())
                        {
                            string error = "Type de retour invalide. Attendu : " + context.ContainingFunc.Func.ReturnType.GetFullName() +
                                ". Obtenu : " + ret.Value.Type.GetFullName() + ".";
                            Log.AddWarning(error, ret.Line, ret.Character, ret.Source);

                            if (StrictCompilation)
                                throw new SemanticError(error);
                        }
                }
                else
                {
                    string error = "Le mot clef return doit être suivi d'un jeton évaluable";
                    Log.AddError(error, instructionToken.Line, instructionToken.Character, instructionToken.Source);
                    throw new SemanticError(error);
                }
                return ret;
            }
            #endregion

            // Déclaration de fonction
            #region Function Declaration
            match = Pattern.FunctionDeclarationPattern.MatchPattern(instructionToken.ListTokens);
            if (match.MatchPattern)
            {
                Token matchedToken = match.FindByIdentifier("Declaration").First().MatchedToken;
                var identifier = matchedToken.FunctionDeclIdentifier;
                var args = matchedToken.FunctionDeclArgs;
                var codeBlock = matchedToken.FunctionDeclCode;
                var type = match.FindByIdentifier("Type").First().MatchedToken;
                Language.FunctionDeclaration decl = new Language.FunctionDeclaration();

                // Commentaire optionnel attaché.
                var commentMatch = match.FindByIdentifier("Comment");
                if (commentMatch.Count != 0)
                {
                    decl.Comment = commentMatch.First().MatchedToken.Content;
                    decl.DocComment = Semantic.DocumentationParser.Parse(decl.Comment);
                }
                decl.Line = identifier.Line;
                decl.Character = identifier.Character;
                decl.Source = identifier.Source;

                decl.Func = new Language.Function();
                // Nom
                decl.Func.Name = identifier.Content;


                // Arguments
                decl.Func.Arguments = new List<Language.FunctionArgument>();
                for (int j = 0; j < args.ListTokens.Count; j++)
                    decl.Func.Arguments.Add(ParseArgument(args.ListTokens[j], context));

                // Modifiers
                decl.Func.Modifiers = match.FindByIdentifier("Modifiers").Select(delegate(Pattern.MatchUnit unit)
                {
                    return unit.MatchedToken.Content;
                }).ToList();

                // Return type
                decl.Func.ReturnType = this.Types.FetchInstancedType(type, context);

                // Propriétaire.
                decl.Func.Owner = context.Container;

                // Macro ?
                decl.Func.IsMacro = context.BlockName == Language.SemanticConstants.MacroBk;

                // Vérifications de validté en pre-parsing.
                if (!preparse)
                {

                    // Fonction spéciale : getClassMetadata_langName() => 
                    if(decl.Func.Name.Contains("getClassMetadata") && decl.Func.Name.Contains("_"))
                    {
                        string srcLang = decl.Func.Name.Split('_')[1];
                        Language.ClankType metaClass = decl.Func.Owner == null ? Types.Types[Language.SemanticConstants.StateClass] : decl.Func.Owner;

                        ParseClassMetadata(metaClass, srcLang, ParseBlock(codeBlock, context, false, false));

                        return new Language.PlaceholderInstruction();
                    }

                    // Type de retour
                    if(decl.Func.ReturnType == null)
                    {
                        string error = "Le type '" + type.Content.ToString() + "' n'existe pas.";
                        Log.AddError(error, decl.Line, decl.Character, decl.Source);
                        throw new SemanticError(error);
                        
                    }

                    // Vérification de validité (si constructeur):
                    if (decl.Func.IsConstructor && decl.Func.ReturnType.BaseType != decl.Func.Owner)
                    {
                        string ownerFullName = decl.Func.Owner.GetFullNameAndGenericArgs();
                        decl.Func.ReturnType = this.Types.FetchInstancedType(ownerFullName, context);

                        // L'erreur n'est pas fatale, mais on la signale.
                        string error = "Un constructeur doit avoir comme type de retour le type dont il est le constructeur. Attendu : " +
                            ownerFullName + ", Obtenu " + this.Types.FetchInstancedType(type, context);
                        this.Log.AddWarning(error, decl.Line, decl.Character, decl.Source);

                        if (StrictCompilation)
                            throw new SemanticError(error);
                    }

                    // Vérification d'accessibilité incohérente.
                    if (decl.Func.HasPrivateReturnTypeOrParameterType() && decl.Func.Owner != null && decl.Func.Owner.IsPublic)
                    {
                        string error = "Accessibilité incohérente : le type '" + context.Container.GetFullName() +
                            "' est public et contient une fonction '" + decl.Func.Name +
                            "' de type dont le type de retour ou le type des arguments " +
                            "est un type privé ou contient des types privés en paramètre générique.";
                        Log.AddWarning(error, decl.Line, decl.Character, decl.Source);

                        if (StrictCompilation)
                            throw new SemanticError(error);
                    }

                    // Constructeur privé : le code généré n'a pas les bonnes garanties d'accessibilité des données.
                    if (decl.Func.IsConstructor && !decl.Func.IsPublic)
                    {
                        string error = "Un constructeur doit être public.";
                        this.Log.AddWarning(error, decl.Line, decl.Character, decl.Source);

                        if (StrictCompilation)
                            throw new SemanticError(error);
                    }
                }
                // -- Code
                // Contexte fils
                TypeTable.Context childContext = new TypeTable.Context();
                childContext.Container = context.Container;
                childContext.ParentContext = context;
                childContext.BlockName = context.BlockName;
                childContext.ContainingFunc = decl;

                // Ajout des arguments au contexte.
                foreach(Language.FunctionArgument arg in decl.Func.Arguments)
                {
                    // Vérification anti-doublons.
                    if(childContext.Variables.ContainsKey(arg.ArgName))
                    {
                        string error = "La variable '" + arg.ArgName + "' est déjà définie dans le contexte actuel.";
                        Log.AddError(error, decl.Line, decl.Character, decl.Source);
                        throw new SemanticError(error);
                    }
                    childContext.Variables.Add(arg.ArgName, new Language.Variable() { Name = arg.ArgName, Type = arg.ArgType });
                }
                
                // Si on est dans le block access / write, ajout de la variable spéciale id !
                if((context.BlockName == Language.SemanticConstants.AccessBk || context.BlockName == Language.SemanticConstants.WriteBk))
                    if(childContext.Variables.ContainsKey(Language.SemanticConstants.ClientID))
                    {
                        string warning = "La variable '" + Language.SemanticConstants.ClientID + "' est implicitement déclarée dans le contexte actuel.";
                        Log.AddWarning(warning, decl.Line, decl.Character, decl.Source);
                    }
                    else
                    {
                        childContext.Variables.Add(Language.SemanticConstants.ClientID,
                            new Language.Variable() { Name = Language.SemanticConstants.ClientID, Type = Types.FetchInstancedType("int", childContext) });
                    }


                // Si on préparse, on ajoute juste la fonction à la table des types
                // Sinon, on parse le code.
                if (preparse)
                {
                    string fullname = decl.Func.GetFullName();

                    // Vérification de l'unicité de la fonction.
                    if(Types.Functions.ContainsKey(fullname))
                    {
                        string error = "Reféfinition de la fonction " + fullname + ".";
                        Log.AddError(error, decl.Line, decl.Character, decl.Source);
                        throw new SemanticError(error);
                    }

                    Types.Functions.Add(fullname, decl.Func);
                    if (fromClass)
                        context.Container.InstanceMethods.Add(fullname, decl);
                }
                else
                { 
                    // Ajout du code.
                    decl.Code = ParseBlock(codeBlock, childContext, false, preparse); // depuis une fonction
                    
                    // Vérification de la présence d'une instruction return dans une fonction.
                    if(!decl.Func.IsConstructor && decl.Func.ReturnType.GetFullName() != "void")
                    {
                        bool hasReturn = false;
                        // TODO
                        // Vérififier la présence d'un return récursivement dans les instructions.
                    }
                }
                

                return decl;
            }
            #endregion

            // Déclaration de classe.
            #region Class Declaration
            Pattern.Match match2 = Pattern.GenericClassDeclarationPattern.MatchPattern(instructionToken.ListTokens);
            match = Pattern.ClassDeclarationPattern.MatchPattern(instructionToken.ListTokens);
            if(match.MatchPattern || match2.MatchPattern)
            {
                match = (match.MatchPattern ? match : match2);
                var modifiers = match.FindByIdentifier("Modifiers");
                var block = match.FindByIdentifier("Block").First();
                var jsonModifierTk = match.FindByIdentifier("JsonModifiers");

                // Récupération du modificateur json (JsonObject par défaut).
                string jsonModifier;
                if(jsonModifierTk.Count == 0)
                    jsonModifier = Language.SemanticConstants.JsonObject;
                else
                    jsonModifier = match.FindByIdentifier("JsonModifiers").First().MatchedToken.Content;

                // Modificateurs
                Language.ClassDeclaration decl = new Language.ClassDeclaration();
                decl.Modifiers = modifiers.Select(delegate(Pattern.MatchUnit unit)
                {
                    return unit.MatchedToken.Content;
                }).ToList();

                // Commentaire optionnel attaché.
                var commentMatch = match.FindByIdentifier("Comment");
                if (commentMatch.Count != 0)
                    decl.Comment = commentMatch.First().MatchedToken.Content;

                decl.Line = block.MatchedToken.Line;
                decl.Source = block.MatchedToken.Source;
                decl.Character = block.MatchedToken.Character;

                // Nom
                if(block.MatchedToken.TkType == TokenType.NamedCodeBlock)
                {
                    decl.Name = block.MatchedToken.NamedCodeBlockIdentifier.Content;
                }
                else
                {
                    decl.Name = block.MatchedToken.NamedGenericCodeBlockNameIdentifier.Content;
                }

                // Paramètres génériques
                if(block.MatchedToken.TkType == TokenType.NamedGenericCodeBlock)
                {
                    decl.GenericParameters = block.MatchedToken.NamedGenericCodeBlockArgs.ListTokens.Select(delegate(Token token)
                    {
                        return token.ChildToken.Content;
                    }).ToList();
                }

                // Si le modifier JsonArray est donné, on marque le type comme un array.
                if(jsonModifier == Language.SemanticConstants.JsonArray && !preparse)
                {
                    Language.ClankType type = Types.Types[decl.Name];
                    type.JType = Language.JSONType.Array;

                    // Détermine le type des éléments de l'array.
                    string functionName = type.GetFullName() + "." + Language.SemanticConstants.ArrayElementTypeFunc;
                    if (type.InstanceMethods.ContainsKey(functionName))
                    {
                        type.JArrayElementType = type.InstanceMethods[functionName].Func.ReturnType.BaseType;
                    }
                    else
                    {
                        string error = "Les types array doivent contenir une fonction '" + Language.SemanticConstants.ArrayElementTypeFunc + 
                            "' ayant pour type de retour le type des éléments contenus dans l'array, afin de permettre la sérialisation du type.";
                        Log.AddError(error, instructionToken.Line, instructionToken.Character, instructionToken.Source);
                        throw new SemanticError(error);
                    }
                }

                // Indique si la sérialisation est supportée.
                if(!preparse)
                {
                    Language.ClankType type = Types.Types[decl.Name];
                    bool containsSerializeKW = decl.Modifiers.Contains(Language.SemanticConstants.IsSerializable);

                    // Tous les types ayant le mot clef serialize supportent la serialisation en tant 
                    // que type générique. 
                    // Les cas où SupportSerializationAsGeneric != SupportSerialization sont
                    // - types built-in (string, etc...)
                    type.SupportSerializationAsGeneric = containsSerializeKW;
                    type.SupportSerialization = containsSerializeKW;
                }

                // Prefixe
                decl.ContextPrefix = context.GetContextPrefix();

                // Création du contexte fils
                TypeTable.Context childContext = new TypeTable.Context();
                childContext.ParentContext = context;
                childContext.Container = Types.Types[decl.Name];
                childContext.BlockName = context.BlockName;

                // Ajout des instructions de la classe.
                if(block.MatchedToken.TkType == TokenType.NamedCodeBlock)
                {
                    decl.Instructions = ParseBlock(block.MatchedToken.NamedCodeBlockInstructions, childContext, true, preparse);
                }
                else
                {
                    decl.Instructions = ParseBlock(block.MatchedToken.NamedGenericCodeBlockInstructions, childContext, true, preparse);
                }

                return decl;
            }
            #endregion


            // Déclaration de classe.
            #region Enum Declaration
            match = Pattern.EnumDeclarationPattern.MatchPattern(instructionToken.ListTokens);
            if (match.MatchPattern)
            {
                var modifiers = match.FindByIdentifier("Modifiers");
                var block = match.FindByIdentifier("Block").First();

                // Modificateurs
                Language.EnumDeclaration decl = new Language.EnumDeclaration();
                decl.Modifiers = modifiers.Select(delegate(Pattern.MatchUnit unit)
                {
                    return unit.MatchedToken.Content;
                }).ToList();

                decl.Line = block.MatchedToken.Line;
                decl.Source = block.MatchedToken.Source;
                decl.Character = block.MatchedToken.Character;

                // Commentaire optionnel attaché.
                var commentMatch = match.FindByIdentifier("Comment");
                if (commentMatch.Count != 0)
                    decl.Comment = commentMatch.First().MatchedToken.Content;

                // Nom
                if (block.MatchedToken.TkType == TokenType.NamedCodeBlock)
                {
                    decl.Name = block.MatchedToken.NamedCodeBlockIdentifier.Content;
                }
                else
                {
                    decl.Name = block.MatchedToken.NamedGenericCodeBlockNameIdentifier.Content;
                }
                decl.Members = Types.Types[decl.Name].EnumValues;
                return decl;
            }
            #endregion
            // Déclaration de variable
            #region Variable Declaration
            match = Pattern.VariableDeclarationPattern.MatchPattern(instructionToken.ListTokens);
            if(match.MatchPattern)
            {
                Language.VariableDeclarationInstruction decl = new Language.VariableDeclarationInstruction();
                var nameToken = match.FindByIdentifier("Name").First().MatchedToken;
                decl.Var = new Language.Variable();
                decl.Var.Type = Types.FetchInstancedType(match.FindByIdentifier("Type").First().MatchedToken, context);
                decl.Var.Name = nameToken.Content;
                decl.Var.Modifiers = match.FindByIdentifier("Modifiers").Select((Pattern.MatchUnit unit) =>
                {
                    return unit.MatchedToken.Content;
                }).ToList();
                
                decl.Line = nameToken.Line;
                decl.Source = nameToken.Source;
                decl.Character = nameToken.Character;
                decl.IsInstanceVariable = context.ContainingFunc == null;

                var commentMatch = match.FindByIdentifier("Comment");
                if (commentMatch.Count != 0)
                    decl.Comment = commentMatch.First().MatchedToken.Content;

                if (decl.Var.Type != null)
                {
                    // Vérification anti-doublon.
                    if(context.Variables.ContainsKey(decl.Var.Name))
                    {
                        string error = "La variable '" + decl.Var.Name + "' est déjà définie dans le contexte actuel.";
                        Log.AddError(error, decl.Line, decl.Character, decl.Source);
                        throw new SemanticError(error);
                    }

                    context.Variables.Add(decl.Var.Name, decl.Var);

                    // On n'ajoute les variables d'instance qu'au moment du preparsing et seulement pour les déclarations dans
                    // les classes.
                    if (fromClass && preparse)
                        context.Container.InstanceVariables.Add(decl.Var.Name, decl.Var);

                    // Vérifie que cette variable ne présente pas de symptome d'accessibilité incohérente.
                    if (fromClass && !preparse && decl.Var.Type.IsPrivateOrHasPrivateGenericArgs() && context.Container.IsPublic)
                    {
                        string error = "Accessibilité incohérente : le type '" + context.Container.GetFullName() +
                            "' est public et contient une variable '" + decl.Var.Name +
                            "' de type '" + decl.Var.Type.GetFullName() +
                            "' qui est un type privé ou contient des types privés en paramètres génériques.";
                        Log.AddWarning(error, decl.Line, decl.Character, decl.Source);

                        if(StrictCompilation)
                            throw new SemanticError(error);
                    }

                    return decl;
                }
                else
                {
                    string error = "Le type " + match.FindByIdentifier("Type").First().MatchedToken.ToReadableCode() + " n'existe pas dans le contexte actuel.";
                    Log.AddError(error, decl.Line, decl.Character, decl.Source);
                    throw new SemanticError(error);
                }
                    
            }

            #endregion

            // Affectation de variable.
            #region Variable Affectation
            match = Pattern.VariableAffectationPattern.MatchPattern(instructionToken.ListTokens);
            if(match.MatchPattern && !preparse)
            {
                Token expr = match.MatchUnits.First().MatchedToken;
                Token operatorr = expr.Operator;
                if(operatorr.IsBinaryOperator && operatorr.Content == "=")
                {
                    Token lValue = expr.Operands1;
                    Token rValue = expr.Operands2;
                    if (lValue.ListTokens.Count == 1) // affectation sans déclaration de variable.
                    {
                        // Création de l'expression group.
                        Language.BinaryExpressionGroup grp = new Language.BinaryExpressionGroup();
                        grp.Operator = Language.Operators.ParseOperator(operatorr);
                        grp.Operand1 = ParseEvaluable(lValue.ChildToken, context, preparse);
                        grp.Operand2 = ParseEvaluable(rValue.ChildToken, context, preparse);

                        Language.AffectationInstruction aff = new Language.AffectationInstruction();
                        // Vérification
                        if (!grp.Operand1.Type.Equals(grp.Operand2.Type))
                        {
                            string error = "Impossible d'affecter une valeur de type " + grp.Operand2.Type.GetFullName() + " à une variable de type " +
                                grp.Operand1.Type.GetFullName() + ".";

                            Log.AddWarning(error, instructionToken.Line, instructionToken.Character, instructionToken.Source);
                            aff.Comment = error;

                            if (StrictCompilation)
                                throw new SemanticError(error);
                        }
                        // Création de l'instruction.
                        aff.Line = operatorr.Line;
                        aff.Character = operatorr.Character;
                        aff.Source = operatorr.Source;
                        aff.Expression = grp;
                        return aff;
                    }
                    else if (lValue.ListTokens.Count == 2)
                    {
                        Token typeToken = lValue.ListTokens[0];
                        Token nameToken = lValue.ListTokens[1];

                        // Jeton de déclaration de fonction
                        Token declToken = new Token()
                        {
                            TkType = TokenType.List,
                            Line = nameToken.Line,
                            Source = nameToken.Source,
                            Character = nameToken.Character,
                            ListTokens = new List<Token>()
                            {
                                typeToken, 
                                nameToken
                            }
                        };

                        // Jeton d'affectation
                        Token affectationToken = new Token()
                        {
                            TkType = TokenType.List,
                            Line = nameToken.Line,
                            Source = nameToken.Source,
                            Character = nameToken.Character,
                            ListTokens = new List<Token>()
                            {
                                new Token()
                                {
                                    Line = nameToken.Line,
                                    Source = nameToken.Source,
                                    Character = nameToken.Character,
                                    TkType = TokenType.ExpressionGroup,
                                    ListTokens = new List<Token>()
                                    {
                                        operatorr,
                                        new Token()
                                        {
                                            Line = nameToken.Line,
                                            Source = nameToken.Source,
                                            Character = nameToken.Character,
                                            TkType = TokenType.List,
                                            ListTokens = new List<Token>()
                                            {
                                                nameToken
                                            }
                                        },
                                        rValue
                                    }
                                }
                            }
                        };

                        // Création des instructions.
                        Language.VariableDeclarationInstruction declInstruction = (Language.VariableDeclarationInstruction)ParseInstruction(declToken, context, fromClass, preparse);
                        Language.AffectationInstruction affInstruction = (Language.AffectationInstruction)ParseInstruction(affectationToken, context, fromClass, preparse);

                        // Création de l'instruction de déclaration + assignement.
                        Language.VariableDeclarationAndAssignmentInstruction instruction = new Language.VariableDeclarationAndAssignmentInstruction()
                        {
                            Assignment = affInstruction,
                            Declaration = declInstruction,
                            Line = declInstruction.Line,
                            Character = declInstruction.Character,
                            Source = declInstruction.Source
                        };
                        return instruction;

                    }
                    else
                        throw new Clank.Core.Tokenizers.SyntaxError("Instruction d'affectation mal formée.", instructionToken.Line, instructionToken.Source);
                }
            }
            #endregion

            // Appel de fonction
            if(instructionToken.ListTokens.Count == 1)
            {
                Token tok = instructionToken.ListTokens[0];
                if(tok.TkType == TokenType.ExpressionGroup && tok.Operator.Content == ".")
                {
                    Token functionTk = tok.Operands2;
                    // Dans ce cas, on a un appel de fonction sur un objet.
                    if (functionTk.TkType == TokenType.FunctionCall)
                    {
                        return new Language.FunctionCallInstruction()
                        {
                            Call = ParseFunctionCall(tok, context, preparse),
                            Line = tok.Line,
                            Source = tok.Source,
                            Character = tok.Character
                        };
                    }
                    // Si le jeton est un nom, c'est un accès à une variable, mais ça n'a pas d'intérêt.
                    else if(functionTk.TkType == TokenType.Name)
                    {
                        throw new Clank.Core.Tokenizers.SyntaxError("Une référence à une variable ne peut pas être considérée comme instruction.", instructionToken.Line, instructionToken.Source); 
                    }
                    else
                    {
                        throw new Clank.Core.Tokenizers.SyntaxError("Instruction mal formée.", instructionToken.Line, instructionToken.Source);
                    }
                        
                }
                else if(tok.TkType == TokenType.FunctionCall)
                {
                    // Appel de fonction sans objet source.
                    return new Language.FunctionCallInstruction()
                    {
                        Call = ParseFunctionCall(tok, context, preparse),
                        Line = tok.Line,
                        Source = tok.Source,
                        Character = tok.Character
                    };
                }
            }

            #region Conditional statements (if, else, elsif, while)
            match = Pattern.ConditionalStatementPattern.MatchPattern(instructionToken.ListTokens);
            if(match.MatchPattern && !preparse)
            {
                // Tokens condition et code.
                Token type = match.FindByIdentifier("Statement").First().MatchedToken;
                Token cond = match.FindByIdentifier("Condition").First().MatchedToken;
                Token code = match.FindByIdentifier("Code").First().MatchedToken;


                // Contexte fils (pour la portée des variables)
                TypeTable.Context childContext = new TypeTable.Context();
                childContext.ParentContext = context;
                childContext.Container = context.Container;
                childContext.BlockName = context.BlockName;
                childContext.ContainingFunc = context.ContainingFunc;

                // Statement
                Language.ConditionalStatement statement = new Language.ConditionalStatement();
                statement.StatementType = Language.ConditionalStatement.StatementTypes[type.Content];
                statement.Code = ParseBlock(code, childContext, fromClass, preparse);
                statement.Condition = ParseEvaluable(cond, context, preparse);
                statement.Line = cond.Line;
                statement.Character = cond.Character;
                statement.Source = cond.Source;

                // Vérification de type
                if(statement.Condition.Type.GetFullName() != "bool")
                {
                    string error = "Terme de condition invalide. Attendu : bool" +
                        ". Obtenu : " + statement.Condition.Type.GetFullName() + ".";
                    Log.AddWarning(error, statement.Line, statement.Character, statement.Source);

                    if (StrictCompilation)
                        throw new SemanticError(error);
                }

                return statement;
            }
            #endregion



            // Si on y trouve un autre bloc
            match = Pattern.NamedCodeBlockDeclarationPattern.MatchPattern(instructionToken.ListTokens);
            #region Named code block
            if (match.MatchPattern)
            {
                var codeBlock = match.FindByIdentifier("Block").First().MatchedToken;
                if (codeBlock.NamedCodeBlockIdentifier.Content == Language.SemanticConstants.State && preparse)
                {
                    // On simule le fait d'être dans la classe State pour y ajouter les variables d'instance.
                    // Création du contexte fils
                    TypeTable.Context childContext = new TypeTable.Context();
                    childContext.Container = Types.Types[Language.SemanticConstants.StateClass];
                    childContext.ParentContext = context;
                    childContext.BlockName = codeBlock.NamedCodeBlockIdentifier.Content;
                    return ParseNamedBlock(codeBlock, childContext, preparse, true);
                }
                else
                {
                    // Création du contexte fils
                    TypeTable.Context childContext = new TypeTable.Context();
                    childContext.Container = context.Container;
                    childContext.ParentContext = context;
                    childContext.BlockName = codeBlock.NamedCodeBlockIdentifier.Content;

                    // Bloc access ou write : ajoute le mot clef state en temps que variable.
                    if (codeBlock.NamedCodeBlockIdentifier.Content == Language.SemanticConstants.AccessBk ||
                        codeBlock.NamedCodeBlockIdentifier.Content == Language.SemanticConstants.WriteBk)
                    {
                        childContext.Variables.Add(Language.SemanticConstants.State, new Language.Variable() {
                            Name = Language.SemanticConstants.State, 
                            Type = Types.TypeInstances[Language.SemanticConstants.StateClass] 
                        });
                    }

                    return ParseNamedBlock(codeBlock, childContext, preparse);
                }
            }
            #endregion

            // Commentaires
            #region Comments
            if (instructionToken.TkType == TokenType.Comment)
            {
                return new Language.PlaceholderInstruction() { Comment = instructionToken.Content };
            }
            else if(instructionToken.TkType == TokenType.List)
            {
                bool allComment = true;
                string comment = "";
                foreach(var inst in instructionToken.ListTokens)
                {
                    if (inst.TkType != TokenType.Comment)
                        allComment = false;
                    else
                        comment += inst.Content + "\n";
                }

                if(allComment)
                {
                    return new Language.PlaceholderInstruction() { Comment = comment.Trim('\n') };
                }
            }
            #endregion

            throw new Clank.Core.Tokenizers.SyntaxError("Instruction mal formée.", instructionToken.Line, instructionToken.Source);
        }

        /// <summary>
        /// Recherche des propriétés de méta données dans une liste d'instructions (string propertyName = "value"; etc...),
        /// et les ajoute au type donné pour le langage donné.
        /// </summary>
        void ParseClassMetadata(Language.ClankType type, string langIdentifier, List<Language.Instruction> instructions)
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            foreach(Language.Instruction inst in instructions)
            {
                if(inst is Language.VariableDeclarationAndAssignmentInstruction)
                {
                    Language.VariableDeclarationAndAssignmentInstruction decl = (Language.VariableDeclarationAndAssignmentInstruction)inst;
                    if(!(decl.Assignment.Expression.Operand2 is Language.StringLiteral))
                    {
                        Log.AddError("Les métadonnées contenues dans les fonctions 'getClassMetadata' doivent être des litéraux de string. Obtenu : "
                            + decl.Assignment.Expression.Operand2.ToString(), inst.Line, inst.Character, inst.Source);
                    }

                    metadata.Add(decl.Declaration.Var.Name, ((Language.StringLiteral)decl.Assignment.Expression.Operand2).Value);

                }
                else
                {
                    Log.AddWarning("Les métadonnées contenues dans les fonctions 'getClassMetadata' doivent être des variables de type string. Obtenu : " + 
                        inst.GetType().ToString(), inst.Line, inst.Character, inst.Source);
                }
            }

            type.LanguageMetadata.Add(langIdentifier, metadata);
        }
        /// <summary>
        /// Parse un jeton pour en extraire une expression évaluable.
        /// Sont évaluables :
        ///     - Les expression group
        ///     - Les littéraux
        ///     - Les variables
        /// </summary>
        Language.Evaluable ParseEvaluable(Token token, TypeTable.Context context, bool preparse)
        {
            string error;
            System.Globalization.NumberFormatInfo enUS = System.Globalization.CultureInfo.GetCultureInfo("en-US").NumberFormat;
            switch (token.TkType)
            {
                // Appels de fonction
                case TokenType.FunctionCall:
                    // ce bout de code n'est appelé que pour les function call simples sans srcObj.
                    return ParseFunctionCall(token, context, preparse);
                // Litéraux string
                case TokenType.StringLiteral:
                    return new Language.StringLiteral()
                    {
                        Value = token.Content,
                        Type = Types.FetchInstancedType(new Token() { TkType = TokenType.Name, Content = "string" }, context)
                    };
                // Litéraux booléens
                case TokenType.BoolLiteral:
                    return new Language.BoolLiteral()
                    {
                        Value = bool.Parse(token.Content),
                        Type = Types.FetchInstancedType("bool", context)
                    };
                // Litéraux numériques
                case TokenType.NumberLiteral:
                    if (token.Content.Contains("."))
                    {
                        return new Language.FloatLiteral()
                        {
                            Value = float.Parse(token.Content, enUS),
                            Type = Types.FetchInstancedType(new Token() { TkType = TokenType.Name, Content = "float" }, context)
                        };
                    }
                    else
                    {
                        return new Language.IntegerLiteral()
                        {
                            Value = int.Parse(token.Content),
                            Type = Types.FetchInstancedType(new Token() { TkType = TokenType.Name, Content = "int" }, context)
                        };
                    }
                // Variables OU Typenames
                case TokenType.Name:
                    var variables = context.GetAllVariables();
                    if (variables.ContainsKey(token.Content))
                        return variables[token.Content];

                    // On cherche parmi les typenames.
                    Language.ClankTypeInstance type;
                    if (Types.ContainsType(token.Content, out type))
                    {
                        return new Language.Typename() { Name = type, Type = Types.TypeInstances["Type"] };
                    }

                    error = "Variable ou type : " + token.Content + " inexistant(e) dans le contexte actuel.";
                    Log.AddError(error, token.Line, token.Character, token.Source);
                    throw new SemanticError(error);
                case TokenType.GenericType:
                    string fullname = token.GetTypeFullName();
                    type = Types.FetchInstancedType(token, context);
                    if (type != null)
                    {
                        // Vérification : nombre d'arguments du type générique.
                        return new Language.Typename() { Name = type, Type = Types.TypeInstances["Type"] };
                    }

                    
                    error = "Type générique : " + fullname + " inexistant(e) dans le contexte actuel.";
                    Log.AddError(error, token.Line, token.Character, token.Source);
                    throw new SemanticError(error);
                case TokenType.ArrayType:
                    // Type d'array (param générique du type array)
                    var arrayType = token.ArrayTypeIdentifier;
                    type = Types.FetchInstancedType(arrayType, context);
                    if(type == null)
                    {
                        error = "Type : " + arrayType.GetTypeFullName() + " inexistant(e) dans le contexte actuel.";
                        Log.AddError(error, token.Line, token.Character, token.Source);
                        throw new SemanticError(error);
                    }

                    // Type de l'array.
                    var arrType = Types.FetchInstancedType(new Language.ClankTypeInstance()
                    {
                        BaseType = Types.Types["Array"],
                        GenericArguments = new List<Language.ClankTypeInstance>() { type },
                    }.GetFullName(), context);

                    if (arrType != null)
                    {
                        return new Language.Typename { Name = arrType, Type = Types.TypeInstances["Type"] };
                    }

                    error = "Type array : " + arrayType + " inexistant(e) dans le contexte actuel.";
                    Log.AddError(error, token.Line, token.Character, token.Source);
                    throw new SemanticError(error);
                // List
                case TokenType.List:
                    // Dans certains cas, le tokenizer génère une liste contenant 1 seule expression group, il faut
                    // alors la désencapsuler.
                    return ParseEvaluable(token.ListTokens[0], context, preparse);
                // Expression group
                case TokenType.ExpressionGroup:
                    // TODO : typer les opérations, bien faire attention à l'opérateur ".".
                    if (token.Operator.IsBinaryOperator)
                    {
                        #region Binary Operator
                        // Cas spécial : fonction call / accès à une variable.
                        if(token.Operator.Content == ".")
                        {
                            if(token.Operands2.TkType == TokenType.FunctionCall)
                            {
                                // Function call
                                return ParseFunctionCall(token, context, preparse);
                            }
                            else if(token.Operands2.TkType == TokenType.Name)
                            {
                                
                                // Access
                                Language.VariableAccess access = new Language.VariableAccess();
                                access.Left = ParseEvaluable(token.Operands1, context, preparse);
                                access.VariableName = token.Operands2.Content;

                                // Vérification de l'accessibilité
                                Language.ClankType ctype = access.Left.Type.BaseType;
                                if(ctype.Name == "Type")
                                {
                                    // Ici, on parse soit un accès à un membre d'enum, soit une membre statique (non supporté).
                                    #region Static / enum
                                    ctype = ((Language.Typename) access.Left).Name.BaseType;
                                    Language.EnumAccess enumAccess = new Language.EnumAccess();
                                    enumAccess.Left = access.Left;
                                    enumAccess.Name = access.VariableName;
                                    enumAccess.Type = ctype.AsInstance();

                                    // Vérification de l'existance du membre.
                                    if(!ctype.EnumValues.ContainsKey(access.VariableName))
                                    {
                                        error = "Le membre d'énumération '" + enumAccess.Name + "' n'existe pas pour l'énumération : '" + ctype.GetFullName() + "'.";
                                        Log.AddError(error, token.Line, token.Character, token.Source);
                                        throw new SemanticError(error);
                                    }
                                    return enumAccess;

                                    #endregion
                                }
                                else
                                {
                                    #region Instance variable
                                    if (!ctype.InstanceVariables.ContainsKey(access.VariableName))
                                    {
                                        error = "La variable d'instance '" + access.VariableName + "' n'existe pas pour les objets de type : '" + ctype.GetFullName() + "'.";
                                        Log.AddError(error, token.Line, token.Character, token.Source);
                                        throw new SemanticError(error);
                                    }

                                    try
                                    {
                                        access.Type = ctype.InstanceVariables[access.VariableName].Type.Instanciate(access.Left.Type.GenericArguments);
                                    }
                                    catch(SemanticError e)
                                    {
                                        error = e.Message;
                                        Log.AddError(error, token.Line, token.Character, token.Source);
                                        throw new SemanticError(error);
                                    }

                                    // Vérification de l'accessibilité
                                    Language.ClankType current = context.Container;
                                    Language.ClankType owner = ctype;
                                    if (!ctype.InstanceVariables[access.VariableName].IsPublic)
                                    {
                                        // Si on n'est pas dans le type, ou qu'on opère pas sur un objet state depuis write/access.
                                        if (current != owner && 
                                            !(owner.GetFullName() == Language.SemanticConstants.StateClass && 
                                            (context.BlockName == Language.SemanticConstants.WriteBk || context.BlockName == Language.SemanticConstants.AccessBk)))
                                        {
                                            error = "La variable d'instance " + access.VariableName + " du type " + ctype.GetFullName() +
                                                " existe mais n'est pas accessible dans le contexte actuel. (mot clef public manquant ?).";
                                            Log.AddWarning(error, token.Line, token.Character, token.Source);

                                            if (StrictCompilation)
                                                throw new SemanticError(error);
                                        }
                                    }
                              
                                    return access;

                                    #endregion
                                }
                            }


                            throw new SemanticError("Source d'erreur inconnue [Semantic Parser.ParseEvaluable:ExpressionGroup]");
                        }

                        Language.BinaryExpressionGroup grp = new Language.BinaryExpressionGroup();
                        grp.Operator = Language.Operators.ParseOperator(token.Operator);
                        grp.Operand1 = ParseEvaluable(token.Operands1, context, preparse);
                        grp.Operand2 = ParseEvaluable(token.Operands2, context, preparse);
                        // Typage du groupe
                        if(grp.Operator == Language.Operator.Equals || grp.Operator == Language.Operator.NotEquals)
                        {
                            // TODO : plus de sécurité là dedans.
                            grp.Type = Types.FetchInstancedType("bool", context);
                        }
                        else
                        {
                            string typename = null;
                            // Essaie de trouver le type de retour de l'opération.
                            if(Language.Operators.OperatorTypingMapping.ContainsKey(grp.Operator))
                            {
                                var firstOpDict = Language.Operators.OperatorTypingMapping[grp.Operator];
                                if(firstOpDict.ContainsKey(grp.Operand1.Type.GetFullName()))
                                {
                                    var secondOpDict = firstOpDict[grp.Operand1.Type.GetFullName()];
                                    if(secondOpDict.ContainsKey(grp.Operand2.Type.GetFullName()))
                                    {
                                        typename = secondOpDict[grp.Operand2.Type.GetFullName()];
                                    }
                                }
                            }
                            else if (grp.Operator == Language.Operator.Affectation)
                            {
                                // Une affectation n'est pas évaluable.
                                error = "Une affectation n'est pas une expression évaluable. Peut être avez-vous oublié un point-virgule, ou peut être vous êtes vous cru en C?";
                                Log.AddError(error, token.Operator.Line, token.Operator.Character, token.Operator.Source);
                                throw new Clank.Core.Tokenizers.SyntaxError(error, token.Operator.Line, token.Operator.Source);
                            }

                            // Si le type n'existe pas, on lève une erreur.
                            if(typename == null)
                            {
                                error = ("L'opérateur " + grp.Operator.ToString() + " ne peut pas être utilisé avec des opérandes " +
                                    "de type " + grp.Operand1.Type.GetFullName() + " et " + grp.Operand2.Type.GetFullName());

                                Log.AddError(error, token.Line, token.Character, token.Source);
                                throw new SemanticError(error);
                            }

                            // Type OK.
                            grp.Type = Types.FetchInstancedType(typename, context);
                        }
           
                        return grp;
                        #endregion
                    }
                    else if(token.Operator.IsUnaryOperator)
                    {
                        #region Unary Operator
                        // Opérateur new.
                        if(token.Operator.Content == "new")
                        {
                            // TODO.
                            throw new NotImplementedException();
                        }

                        // Autre opérateur.
                        Language.UnaryExpressionGroup grp = new Language.UnaryExpressionGroup();
                        grp.Operator = Language.Operators.ParseOperator(token.Operator);
                        grp.Operand = ParseEvaluable(token.Operands1, context, preparse);

                        // Essaie de trouver le type de retour de l'opération.
                        string typename = null;
                        if (Language.Operators.OperatorTypingMapping.ContainsKey(grp.Operator))
                        {
                            var firstOpDict = Language.Operators.OperatorTypingMapping[grp.Operator];
                            if (firstOpDict.ContainsKey(grp.Operand.Type.GetFullName()))
                            {
                                var secondOpDict = firstOpDict[grp.Operand.Type.GetFullName()];
                                if (secondOpDict.ContainsKey(grp.Operand.Type.GetFullName()))
                                {
                                    typename = secondOpDict[grp.Operand.Type.GetFullName()];
                                }
                            }
                        }

                        // Si le type n'existe pas, on lève une erreur.
                        if (typename == null)
                        {
                            error = ("L'opérateur " + grp.Operator.ToString() + " ne peut pas être utilisé avec l'opérande " +
                                "de type " + grp.Operand.Type.GetFullName() + ".");
                            Log.AddError(error, token.Line, token.Character, token.Source);
                            throw new SemanticError(error);
                        }

                        // Type OK.
                        grp.Type = Types.FetchInstancedType(typename, context);

                        return grp;
                        #endregion
                    }

                    break;
            }
            // TODO : array
            throw new NotImplementedException();
        }

        /// <summary>
        /// A partir d'un jeton de type ExpressionGroup, extrait un fonction call instancié.
        /// </summary>
        Language.FunctionCall ParseFunctionCall(Token exprGrp, TypeTable.Context context, bool preparse)
        {
            if ((exprGrp.TkType != TokenType.ExpressionGroup || exprGrp.Operator.Content != ".") && (exprGrp.TkType != TokenType.FunctionCall))
                throw new InvalidOperationException();

            // Fonction à appeler.
            Language.Function instanciatedFunction;
            // Objet sur lequel appeler la fonction (null si aucun).
            Language.Evaluable srcObj;
            // Arguments de la fonction
            List<Language.Evaluable> args;
            // Jeton contenant le function call
            Token functionTk;
            // Indique si la fonction est un constructeur.
            bool isConstructor = false;
            // Indique si la fonction est une fonction contenue dans la même classe que celle
            // correspondant à ce contexte.
            bool thisClassFunction = false;
            if(exprGrp.TkType == TokenType.ExpressionGroup)
            {
                #region Expr group
                Token lMember = exprGrp.Operands1;
                functionTk = exprGrp.Operands2; //  FunctionCall
                args = functionTk.FunctionCallArgs.ListTokens.Select(delegate(Token tk)
                {
                    return ParseEvaluable(tk.ChildToken, context, preparse); // TODO : à vérifier.
                }).ToList();
                // Vérifie que le jeton function est un appel de fonction.
                if (functionTk.TkType != TokenType.FunctionCall)
                    throw new InvalidOperationException();


                // Récupération des paramètres génériques de l'objet source (s'ils existent), et
                // instanciation de la fonction.
                srcObj = ParseEvaluable(lMember, context, preparse);
                Language.ClankTypeInstance srcType = srcObj.Type;
                List<Language.ClankTypeInstance> srcGenArgs = srcType.GenericArguments;
            
                // Récupère la fonction.
                if(srcType.GetFullName() == "Type")
                {
                    // Constructor
                    if (functionTk.FunctionCallIdentifier.Content == Language.SemanticConstants.New)
                    {
                        Language.Typename typename = (Language.Typename)srcObj;
                        try
                        {
                            instanciatedFunction = Types.GetConstructor(typename.Name.BaseType, typename.Name, args, context);
                        }
                        catch(SemanticError err)
                        {
                            Log.AddError(err.Message, functionTk.Line, functionTk.Character, functionTk.Source);
                            throw new SemanticError(err.Message);
                        }
                        isConstructor = true;
                    }
                    else
                    {
                        // Méthodes statiques
                        Language.Function function;
                        Language.Typename typename = (Language.Typename)srcObj;

                        // Obtient le nom complet de la fonction.
                        function = Types.GetStaticFunction(typename.Name, functionTk.FunctionCallIdentifier.Content, args, context);
                        if (function == null)
                        {
                            string candidates = Types.GetCandidatesStr(typename.Name, functionTk.FunctionCallIdentifier.Content);
                            string error;
                            if (candidates == "") // la fonction n'existe carrément pas
                                error = "La fonction statique " + functionTk.FunctionCallIdentifier.Content + " n'existe pas dans le contexte actuel";
                            else
                                error = "Aucune fonction statique de " + typename.Name.GetFullName() + " ne matche les arguments de type " + 
                                        Language.Evaluable.GetArgTypesString(args) + ". Candidats possibles : " + candidates;

                            Log.AddError(error, exprGrp.Line, exprGrp.Character, exprGrp.Source);
                            throw new SemanticError(error);
                        }

                        // Instanciation de la fonction.
                        srcGenArgs = typename.Name.GenericArguments;
                        instanciatedFunction = function.Instanciate(srcGenArgs);
                    }
                }
                else
                {
                    // Méthodes d'instance.
                    Language.Function function;
                    function = Types.GetInstanceFunction(srcType, functionTk.FunctionCallIdentifier.Content, args, context);
                    if (function == null)
                    {
                        string candidates = Types.GetCandidatesStr(srcType, functionTk.FunctionCallIdentifier.Content);
                        string error;
                        if (candidates == "") // la fonction n'existe carrément pas
                            error = "La fonction d'instance " + functionTk.FunctionCallIdentifier.Content + " n'existe pas dans le contexte actuel";
                        else
                            error = "Aucune fonction d'instance de " + srcType.GetFullName() + " ne matche les arguments de type " +
                                    Language.Evaluable.GetArgTypesString(args) + ". Candidats possibles : " + candidates;

                        Log.AddError(error, exprGrp.Line, exprGrp.Character, exprGrp.Source);


                        throw new SemanticError(error);
                    }
                    // Instanciation de la fonction.
                    try
                    {

                        instanciatedFunction = function.Instanciate(srcGenArgs);
                    }
                    catch(SemanticError err)
                    {
                        Log.AddError(err.Message, exprGrp.Line, exprGrp.Character, exprGrp.Source);
                        throw err;
                    }
                }
            #endregion
            }
            else
            {
                #region Function call
                // Fonctions sans objet source
                functionTk = exprGrp;
                args = functionTk.FunctionCallArgs.ListTokens.Select(delegate(Token tk)
                {
                    return ParseEvaluable(tk.ChildToken, context, preparse); // TODO : à vérifier.
                }).ToList();
                instanciatedFunction = Types.GetFunction(functionTk.FunctionCallIdentifier.Content, context);
                isConstructor = false;
                srcObj = null;
                if (instanciatedFunction == null)
                {
                    // La fonction est peut être une fonction d'instance du contexte actuel !
                    Language.ClankType owner = context.Container;
                    Language.ClankTypeInstance ownerInstance = null;
                    if(owner != null)
                        ownerInstance = owner.AsInstance();


                    if (owner != null)
                    {
                        instanciatedFunction = Types.GetInstanceFunction(ownerInstance, functionTk.FunctionCallIdentifier.Content, args, context);
                        if(instanciatedFunction == null)
                        {
                            string candidates = Types.GetCandidatesStr(ownerInstance, functionTk.FunctionCallIdentifier.Content);
                            string error;
                            if (candidates == "") // la fonction n'existe carrément pas
                                error = "La fonction  " + functionTk.FunctionCallIdentifier.Content + " n'existe pas dans le contexte actuel";
                            else
                                error = "Aucune fonction d'instance de " + ownerInstance.GetFullName() + " ne matche les arguments de type " +
                                        Language.Evaluable.GetArgTypesString(args) + ". Candidats possibles : " + candidates;

                            Log.AddError(error, exprGrp.Line, exprGrp.Character, exprGrp.Source);
                            throw new SemanticError(error);
                        }
                        thisClassFunction = true;
                    }
                    else
                    {
                        instanciatedFunction = Types.GetInstanceFunction(ownerInstance, functionTk.FunctionCallIdentifier.Content, args, context);
                        if (instanciatedFunction == null)
                        {
                            string candidates = Types.GetCandidatesStr(ownerInstance, functionTk.FunctionCallIdentifier.Content);
                            string error;
                            if (candidates == "") // la fonction n'existe carrément pas
                                error = "La fonction globale '" + functionTk.FunctionCallIdentifier.Content + "' n'existe pas dans le contexte actuel";
                            else
                                error = "Aucune fonction globale '" + functionTk.FunctionCallIdentifier.Content + "' ne matche les arguments de type " +
                                        Language.Evaluable.GetArgTypesString(args) + ". Candidats possibles : " + candidates;

                            Log.AddError(error, exprGrp.Line, exprGrp.Character, exprGrp.Source);
                            throw new SemanticError(error);
                        }
                        thisClassFunction = true;
                    }
                }
                #endregion
            }

            // Vérification de l'accessibilité.
            if(!instanciatedFunction.IsPublic)
            {
                Language.ClankType owner = instanciatedFunction.Owner;
                Language.ClankType current = context.Container;
                if(owner != current)
                {
                    // Type non accessible.
                    string error = "La fonction " + functionTk.FunctionCallIdentifier.Content + 
                        Language.Evaluable.GetArgTypesString(args) + " n'est pas accessible dans le contexte actuel. (mot clef public manquant ?)";
                    Log.AddWarning(error, exprGrp.Line, exprGrp.Character, exprGrp.Source);

                    if(StrictCompilation)
                        throw new SemanticError(error);
                }
            }

            // Création du function call.
            Language.FunctionCall call = new Language.FunctionCall();
            call.Func = instanciatedFunction;
            call.Type = instanciatedFunction.ReturnType;
            call.IsConstructor = isConstructor;
            call.Src = srcObj;
            call.IsThisClassFunction = thisClassFunction;
            
            call.Arguments = args;

            // Vérification du nombre d'arguments..
            if (call.Arguments.Count != call.Func.Arguments.Count)
            {
                string error = "Le nombre d'arguments passés à la fonction " + call.Func.GetFullName() + " est incorrect";
                Log.AddError(error, exprGrp.Line, exprGrp.Character, exprGrp.Source);
                throw new SemanticError(error);
            }
            else
            {
                // Vérification du type des arguments.
                for (int i = 0; i < call.Arguments.Count; i++)
                {
                    if (call.Arguments[i].Type.GetFullName() != call.Func.Arguments[i].ArgType.GetFullName())
                    {

                        string error = "Le type de l'argument " + i + " passé à la fonction '" + call.Func.Name + "' est invalide. Attendu : " +
                            call.Func.Arguments[i].ArgType.GetFullName() + ". Obtenu : " + call.Arguments[i].Type.GetFullName() + ".";
                        Log.AddWarning(error, exprGrp.Line, exprGrp.Character, exprGrp.Source);
                        if (StrictCompilation)
                            throw new SemanticError(error);

                    }
                }
            }
            return call;
        }
        /// <summary>
        /// Parse un argument depuis une liste de jetons représentant un argument.
        /// (habituellement type nom)
        /// </summary>
        /// <returns></returns>
        Language.FunctionArgument ParseArgument(Token token, TypeTable.Context context)
        {
            Language.FunctionArgument arg = new Language.FunctionArgument();
            arg.ArgName = token.ListTokens[1].Content;
            arg.ArgType = Types.FetchInstancedType(token.ListTokens[0], context);
            
            // Type inexistant.
            if(arg.ArgType == null)
            {
                string error = "Le type '" + token.ChildToken.GetTypeFullName() + " n'existe pas dans le contexte actuel.";
                Log.AddError(error, token.Line, token.Character, token.Source);
                throw new SemanticError(error);
            }

            return arg;
        }
    }
}
