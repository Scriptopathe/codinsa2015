using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PonyCarpetExtractor.ExpressionTree;
using PonyCarpetExtractor.SyntaxParsing;
using TokenList = PonyCarpetExtractor.SyntaxParsing.SyntaxicParser.TokenList;
using Token = PonyCarpetExtractor.SyntaxParsing.SyntaxicParser.Token;
using TokenType = PonyCarpetExtractor.SyntaxParsing.SyntaxicParser.TokenType;
using ExpressionGroupToken = PonyCarpetExtractor.SyntaxParsing.SyntaxicParser.ExpressionGroupToken;
using InfoToken = PonyCarpetExtractor.SyntaxParsing.SyntaxicParser.InfoToken;
using OperandToken = PonyCarpetExtractor.SyntaxParsing.SyntaxicParser.OperandToken;
using TokenContainer = PonyCarpetExtractor.SyntaxParsing.SyntaxicParser.TokenContainer;
using BlockGroupToken = PonyCarpetExtractor.SyntaxParsing.SyntaxicParser.BlockGroupToken;
using ParenthesisGroupToken = PonyCarpetExtractor.SyntaxParsing.SyntaxicParser.ParenthesisGroupToken;
using PrefixedOperatorToken = PonyCarpetExtractor.SyntaxParsing.SyntaxicParser.PrefixedOperatorToken;
using EvaluableGroupToken = PonyCarpetExtractor.SyntaxParsing.SyntaxicParser.EvaluableBlockToken;
using PonyCarpetExtractor.ExpressionTree.Instructions;
using System.Reflection;
namespace PonyCarpetExtractor.SemanticParsing
{
    /// <summary>
    /// Parseur sémantique : transforme des jetons en Expressions.
    /// </summary>
    public static class SemanticParser
    {
        /// <summary>
        /// Parse une liste de jetons et retourne un block d'instruction.
        /// </summary>
        public static Block ParseBlock(TokenList tokens, GlobalContext mainContext)
        {
            Block mainBlock = new Block(mainContext);
            mainBlock.Instructions = new List<ExpressionTree.Instructions.Instruction>();

            // Détermine si on a commencé à parser un block if.
            bool parseIfStarted = false;

            // On commence le parsing
            TokenList tokenStack = new TokenList();
            tokens.Add(new Token(TokenType.EndOfBlock));
            foreach (Token token in tokens)
            {
                if (parseIfStarted)
                {
                    if (token.Type == TokenType.BlockGroup)
                    {
                        tokenStack.Add(token);
                        // Si le dernier statement était un else, alors on termine sur ce jeton.
                        if (tokenStack.Last().Type == TokenType.Statement && ((InfoToken)token).Content == "else")
                        {
                            mainBlock.Instructions.Add(ParseIfStatement(tokenStack, mainContext));
                            tokenStack.Clear();
                            parseIfStarted = false;
                        }
                        continue;
                    }
                    else if (token.Type == TokenType.OperandTokens)
                    {
                        // Si on a un statement avant ça (if ou elsif en particulier)
                        if (tokenStack.Last().Type == TokenType.Statement)
                        {
                            // On ne prend une opérande qu'après un if ou elsif
                            InfoToken last = (InfoToken)tokenStack.Last();
                            if (last.Content == "elsif" || last.Content == "if")
                            {
                                tokenStack.Add(token);
                                continue;
                            }
                        }
                        // Si ce n'est pas le cas, c'est que cette opérande vient après le block
                        // if ou elsif, donc on finit.
                        mainBlock.Instructions.Add(ParseIfStatement(tokenStack, mainContext));
                        tokenStack.Clear();
                        parseIfStarted = false;
                        
                    }
                    // Si c'est un elsif : on prend le prochain block
                    else if (token.Type == TokenType.Statement)
                    {
                        InfoToken iToken = (InfoToken)token;
                        if (iToken.Content == "elsif")
                        {
                            tokenStack.Add(iToken);
                            continue;
                        }
                        else if (iToken.Content == "else")
                        {
                            tokenStack.Add(iToken);
                            continue;
                        }
                        else
                        {
                            // c'est un statement qui n'a rien avoir avec ça, donc on termine.
                            mainBlock.Instructions.Add(ParseIfStatement(tokenStack, mainContext));
                            tokenStack.Clear();
                            parseIfStarted = false;
                        }
                    }
                    else
                    {
                        mainBlock.Instructions.Add(ParseIfStatement(tokenStack, mainContext));
                        tokenStack.Clear();
                        parseIfStarted = false;
                    }
                }

                // Les possibilités de patterns :
                // OperandGroup + ";" : c'est une affectation, s'il n'y a pas de signe 'égal' dedans,
                // bah c'est rien du tout.
                // Une Opérande + ";" : c'est soit une déclaration de variable (en deux temps donc),
                //                      soit un appel de fonction.
                // Un statement + une opérande + début de block : statement bien entendu.
                // Stratégie : on stack les jetons jusqu'à rencontrer un début de block ou
                // un point virgule. On analyse ensuite ce que l'on trouve :D
                if (token.Type == TokenType.EndOfInstruction)
                {
                    mainBlock.Instructions.Add(ParseInstruction(tokenStack, mainContext));
                    tokenStack.Clear();
                }
                else if (token.Type == TokenType.EvaluableBlock && tokenStack.Count == 0)
                {
                    // Si la pile de jetons est vide, on est en début d'instruction, on a une déclaration
                    // de fonction.
                    // Sinon, on ajoute seulement le jeton à la liste de jetons (dans le else).
                    EvaluableGroupToken blockToken = (EvaluableGroupToken)token;
                    FunctionDeclarationInstruction ins = new FunctionDeclarationInstruction();
                    ins.Function = ParseFunction(blockToken, mainContext);
                    ins.FunctionName = blockToken.Name;
                    mainBlock.Instructions.Add(ins);
                    tokenStack.Clear();
                    
                }
                else if (token.Type == TokenType.Statement && ((InfoToken)token).Content == "if")
                {
                    if (tokenStack.Count != 0)
                        throw new Exception("Syntax error : unexpected token(s) before 'if' statement");

                    tokenStack.Add(token);
                    parseIfStarted = true;
                }
                else if (token.Type == TokenType.BlockGroup) // fin de block.
                {
                    tokenStack.Add(token);
                    mainBlock.Instructions.Add(ParseBlockStatement(tokenStack, mainContext));
                    tokenStack.Clear();
                }
                else
                {
                    tokenStack.Add(token);
                }
            }
            return mainBlock;
        }
        /// <summary>
        /// Parse une liste de jetons et retourne un block.
        /// Cette version ajoute automatiquement certains namespaces
        /// et assemblies au contexte principal avant d'évaluer le code.
        /// </summary>
        public static Block ParseBlock(TokenList tokens)
        {
            GlobalContext mainContext = new GlobalContext();
            // Namespaces de base
            mainContext.LoadedNamespaces.Add("");
            mainContext.LoadedNamespaces.Add("System");
            mainContext.LoadedNamespaces.Add("System.Text");
            mainContext.LoadedNamespaces.Add("System.Collections.Generic");
            mainContext.LoadedNamespaces.Add("Interpreter");
            // Assemblies de base
            mainContext.LoadedAssemblies = new Dictionary<string, Assembly>();
            mainContext.LoadedAssemblies.Add("mscorlib", Assembly.GetAssembly(typeof(char)));
            mainContext.LoadedAssemblies.Add("local", Assembly.GetExecutingAssembly());

            return ParseBlock(tokens, mainContext);
        }
        /// <summary>
        /// Parse une expression d'arguments (expressions) séparés
        /// par des virgules.
        /// </summary>
        /// <returns></returns>
        static List<IGettable> ParseArgExpression(List<Token> tokens, GlobalContext mainContext)
        {
            List<IGettable> args = new List<IGettable>();
            bool needComa = false;
            foreach (Token tok in tokens)
            {
                if (tok.Type == TokenType.Separator)
                {
                    if (!needComa)
                        throw new Exception("Invalid arg expression");
                    continue;
                }
                args.Add(ParseExpression(tok, mainContext));
                needComa = true;
            }
            return args;
        }
        /// <summary>
        /// Parse une sous-expression de la forme :
        /// 5.machin(haha, truc).bidule.chose[6].truc(hoho)
        /// <param name="tokens">
        /// Liste de jetons, en particulier :
        ///     - Noun, Number
        ///     - ParenthesisGroup, OperandGroup
        ///     - Dot
        /// </param>
        /// </summary>
        static SubExpression ParseSubExpression(TokenList tokens, GlobalContext mainContext)
        {
            tokens = tokens.Copy();
            SubExpression exp = new SubExpression();
            exp.Parts = new List<SubExpressionPart>();
            // Si on a un new, on ne vérifie pas qu'on ait un nom de type de cette manière :
            // on parse la SubExpression (donc new <Nom.Du.Type> () ) directement.
            if (tokens.First().Type != TokenType.New)
            {

                // Déja on vérifie qu'il y ait un nom de type avant, pour voir
                // si on appelle une méthode statique.
                bool containsTypeName = false;

                // Permet de savoir à quel jeton démarrer lors du prochain parsing :
                // on skip les jetons correspondant au type.
                int lastTypeTokenId = 0;
                int id = 0;
                string lastName = null;
                StringBuilder name = new StringBuilder();
                foreach (Token token in tokens)
                {
                    id++;
                    if (token.Type == TokenType.Dot)
                        name.Append(".");
                    else if (token.Type == TokenType.Noun)
                        name.Append(((InfoToken)token).Content);
                    else if (token.Type == TokenType.GenericParametersGroup)
                        name.Append("`" + ((TokenContainer)token).Tokens.Count);
                    else if (token.Type == TokenType.ParenthesisGroup)
                        break;


                    bool isType = ReflectionUtils.IsTypeName(mainContext.LoadedAssemblies, mainContext.LoadedNamespaces, name.ToString());
                    if (isType)
                    {
                        lastTypeTokenId = id;
                        containsTypeName = true;
                        lastName = name.ToString();
                        // on finit pas au cas où on ait un NestedType.
                    }
                }
                // On skip les token du type.
                // TODO : ça bugue s'il n'y a pas de points.
                if (containsTypeName)
                {
                    SubExpressionPart part = new SubExpressionPart(lastName, SubExpressionPart.ExpTypes.ConstantTypeName);
                    exp.Parts.Add(part);
                    // On enlève ce nom de type pour le prochain parsing :
                    if(tokens.Count >= lastTypeTokenId + 1)
                    {
                        tokens.RemoveRange(0, lastTypeTokenId + 1);
                    }
                    else
                    {
                        // l'expression est déja terminée.
                        return exp;
                    }
                }
            }
            else // new
            {
                // Dans ce cas, la sub expression part du type s'étend jusqu'à la prochaine
                // parenthèse.

                // Liste de jetons représentant l'expression new
                TokenList newExprList = new TokenList();
                bool isOK = false;
                // On parcours la liste de jetons originale en 
                // supprimant les objets que l'on va parser.
                while(true && tokens.Count != 0)
                {
                    if (tokens[0].Type == TokenType.ParenthesisGroup)
                    {
                        newExprList.Add(tokens[0]);
                        tokens.RemoveAt(0);
                        isOK = true;
                        break;
                    }
                    newExprList.Add(tokens[0]);
                    tokens.RemoveAt(0);
                }
                exp.Parts.Add(ParseSubExpressionPart(newExprList, mainContext)); 
            }

            TokenList tempTokens = new TokenList();
            foreach (Token token in tokens)
            {
                if (token.Type != TokenType.Dot)
                    tempTokens.Add(token);
                else if (tempTokens.Count != 0)
                {
                    SubExpressionPart part = ParseSubExpressionPart(tempTokens, mainContext);
                    tempTokens.Clear();
                    exp.Parts.Add(part);
                }
            }
            // Car à la fin il n'y a pas de point.
            if (tempTokens.Count != 0)
            {
                SubExpressionPart part = ParseSubExpressionPart(tempTokens, mainContext);
                tempTokens.Clear();
                exp.Parts.Add(part);
            }

            return exp;
        }
        /// <summary>
        /// Parse une partie de sous expression.
        /// Ex : machin(haha, truc), 1, truc...
        /// TODO : support indexation et génériques.
        /// </summary>
        /// <param name="tokens">
        /// Les jetons acceptés :
        ///     - Une liste d'un jeton pouvant être : Number, String => Constante
        ///                 OperandTokens, OperandOperatorGroup, ParenthesisGroup. => rééval des jetons sous-jacents.
        ///     - Une liste de jetons commençant par new, avec un Noun et un ParenthesisGroup
        ///     - Une liste d'un jeton Noun. (=> variable)
        ///     - Une liste comprenant un jeton Noun et un jeton ParenthesisGroup (=> appel de méthode).
        /// </param>
        /// <param name="part"></param>
        /// <returns></returns>
        static SubExpressionPart ParseSubExpressionPart(TokenList tokens, GlobalContext mainContext)
        {
            // Là on a le choix : string, nombre.
            if (tokens.Count == 1 && tokens.First().Type != TokenType.Noun)
            {
                Token first = tokens.First();
                if (first.Type == TokenType.Number)
                {
                    InfoToken token = (InfoToken)first;
                    return new SubExpressionPart(token.Content, SubExpressionPart.ExpTypes.ConstantObject);
                }
                else if (first.Type == TokenType.String)
                {
                    InfoToken token = (InfoToken)first;
                    return new SubExpressionPart(token.Content, SubExpressionPart.ExpTypes.ConstantObject);
                }
                else if (first.Type == TokenType.Bool)
                {
                    InfoToken token = (InfoToken)first;
                    return new SubExpressionPart(token.Content, SubExpressionPart.ExpTypes.ConstantObject);
                }
                else if (first.Type == TokenType.OperandTokens)
                    return ParseSubExpressionPart(((OperandToken)first).Tokens, mainContext);
                else if (first.Type == TokenType.ExpressionGroup)
                    return new SubExpressionPart(ParseExpressionGroup((ExpressionGroupToken)first, mainContext));
                else if (first.Type == TokenType.ParenthesisGroup)
                    return ParseSubExpressionPart(((ParenthesisGroupToken)first).Tokens, mainContext);
                else
                    throw new Exception("Invalid token in operand-token");
            }
            else if (tokens.First().Type == TokenType.New)
            {
                // Création d'une instance.
                SubExpressionPart part = new SubExpressionPart("", SubExpressionPart.ExpTypes.NewObject);
                // Contiendra le nom du type, et donc le nom de la SubExpressionPart.
                StringBuilder typename = new StringBuilder();
                // Etape 1 : on parse tous les jetons constituant le nom du type.
                int index = 1;
                foreach (Token token in tokens)
                {
                    if (token == tokens.First())
                        continue;

                    if (token.Type == TokenType.Noun)
                    {
                        InfoToken info = (InfoToken)token;
                        typename.Append(info.Content);
                    }
                    else if (token.Type == TokenType.Dot)
                    {
                        typename.Append('.');
                    }
                    else if (token.Type == TokenType.ParenthesisGroup ||
                        token.Type == TokenType.IndexingParametersGroup ||
                        token.Type == TokenType.GenericParametersGroup)
                    {
                        // On arrête si on tombe sur un groupe de parenthèses ou autre.
                        break;
                    }
                    else
                    {
                        throw new Exception("Unexpected token in new expression : " + token.ToString());
                    }
                    index++;
                }
                part.Name = typename.ToString();

                // Etape 2 : on ajoute tous les groupes d'arguments.
                List<IGettable> arguments = new List<IGettable>();
                List<List<IGettable>> indexingArguments = new List<List<IGettable>>();
                List<IGettable> genericArguments = new List<IGettable>();
                for (int i = index; i < tokens.Count; i++)
                {
                    Token tok = tokens[i];
                    switch (tok.Type)
                    {
                        case TokenType.ParenthesisGroup:
                            arguments = ParseArgExpression(((TokenContainer)tok).Tokens, mainContext);
                            break;
                        case TokenType.GenericParametersGroup:
                            genericArguments = ParseArgExpression(((TokenContainer)tok).Tokens, mainContext);
                            break;
                        case TokenType.IndexingParametersGroup:
                            indexingArguments.Add(ParseArgExpression(((TokenContainer)tok).Tokens, mainContext));
                            break;
                        default:
                            throw new Exception("Unexpected token in new expression : " + tok.ToString());
                    }
                }
                part.IndexingParameters = indexingArguments;
                part.GenericParameters = genericArguments;
                part.Parameters = arguments;
                return part;
            }
            else
            {
                // Variable, méthode.
                // Si parenthèses : méthode et extraire params, sinon variable ou nom de type.
                if (tokens.First().Type != TokenType.Noun)
                    throw new Exception("Invalid token type in SubExpressionPart");

                // Nom du membre.
                string memberName = ((InfoToken)tokens.First()).Content;

                bool hasArguments = false;

                List<IGettable> arguments = new List<IGettable>();
                List<List<IGettable>> indexingArguments = new List<List<IGettable>>();
                List<IGettable> genericArguments = new List<IGettable>();
                bool first = true;
                foreach (Token token in tokens)
                {
                    if (first) { first = false; continue; }
                    switch (token.Type)
                    {
                        case TokenType.ParenthesisGroup:
                            hasArguments = true;
                            arguments = ParseArgExpression(((TokenContainer)token).Tokens, mainContext);
                            break;
                        case TokenType.GenericParametersGroup:
                            genericArguments = ParseArgExpression(((TokenContainer)token).Tokens, mainContext);
                            break;
                        case TokenType.IndexingParametersGroup:
                            indexingArguments.Add(ParseArgExpression(((TokenContainer)token).Tokens, mainContext));
                            break;
                        default:
                            throw new Exception("Unexpected token in sub expression");
                    }
                }
                // Type de la sous expression.
                SubExpressionPart.ExpTypes subExprPartType = hasArguments ? SubExpressionPart.ExpTypes.Method : SubExpressionPart.ExpTypes.Variable;
                SubExpressionPart part = new SubExpressionPart(memberName, subExprPartType);
                part.Parameters = arguments;
                part.IndexingParameters = indexingArguments;
                part.GenericParameters = genericArguments;

                return part;

            }
            throw new Exception();
        }
        /// <summary>
        /// Parse une expression quelconque contenue dans un jeton,
        /// retournant un IGettable.
        /// </summary>
        /// <param name="token">
        /// Le jeton peut être un jeton contenant une liste de jetons :
        ///     - OperandTokens
        ///     - OperandOperatorGroup
        ///     - ParenthesisGroup
        /// </param>
        /// <returns></returns>
        static IGettable ParseExpression(Token token, GlobalContext mainContext)
        {
            if (token.Type == TokenType.OperandTokens)
            {
                TokenList tokens = ((OperandToken)token).Tokens;
                if (tokens.Count == 1 && tokens.First().Type == TokenType.ExpressionGroup)
                    return ParseExpressionGroup((ExpressionGroupToken)tokens.First(), mainContext);
                return ParseSubExpression(tokens, mainContext);
            }
            else if (token.Type == TokenType.ExpressionGroup)
                return ParseExpressionGroup((ExpressionGroupToken)token, mainContext);
            else if (token.Type == TokenType.ParenthesisGroup)
                return ParseExpression(new OperandToken(((ParenthesisGroupToken)token).Tokens), mainContext);
            else if (token.Type == TokenType.EvaluableBlock)
                return ParseEvaluableBlock((EvaluableGroupToken)token, mainContext);
            else
                throw new Exception("Invalid token in expression"); // return Project.RpgGameRessources.ToAssetNamenew TokenList() { token }, mainContext);
        }
        /// <summary>
        /// Parse un block évaluable afin de retourner un IGettable.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        static IGettable ParseEvaluableBlock(EvaluableGroupToken block, GlobalContext mainContext)
        {
            EvaluableBlockExpression expr = new EvaluableBlockExpression();
            expr.Function = ParseFunction(block, mainContext);
            expr.Name = block.Name;
            return expr;
        }
        /// <summary>
        /// Parse une liste de jetons de modificateurs et retourne le flag modificateur correspondant.
        /// TODO : implémenter ça correctement
        /// </summary>
        static EvaluableBlockModifiers ParseEvaluableBlockModifier(List<InfoToken> tokens)
        {
            foreach (InfoToken token in tokens)
            {
                switch (token.Content)
                {
                    case "ref":
                        return EvaluableBlockModifiers.Ref;
                }
            }
            return EvaluableBlockModifiers.None;
        }
        /// <summary>
        /// Retourne le flag modificateur correspondant au jeton modificateur donné.
        /// </summary>
        static PrimaryEvaluableBlockModifiers ParsePrimaryEvaluableBlockModifier(InfoToken token)
        {
            switch (token.Content)
            {
                case SyntaxParsing.SyntaxicParser.LAMBDA_KW:
                    return PrimaryEvaluableBlockModifiers.Lambda;
                case SyntaxParsing.SyntaxicParser.FUNCTION_KW:
                    return PrimaryEvaluableBlockModifiers.Function;
            }
            throw new Exception();
        }

        /// <summary>
        /// Parse une fonction dans un EvaluableGroupToken.
        /// Retourne uniquement la fonction.
        /// </summary>
        static Function ParseFunction(EvaluableGroupToken token, GlobalContext mainContext)
        {
            TokenList nameTokens = token.ArgumentList.Tokens;

            // Ici on parcours les jetons de exprToken (qui doit être un OperandToken)
            // afin de trouver les noms des arguments.

            // Liste contenant les noms des arguments
            List<string> argsNamesLists = new List<string>();

            // Indique si le prochain jeton doit être une virgule
            bool needComa = false;
            foreach (Token tok in nameTokens)
            {
                if (needComa && tok.Type != TokenType.Separator)
                    throw new Exception("Expected ',' token in function declaration");
                else
                    needComa = false;
                // Si c'est un nom :
                if (tok.Type == TokenType.OperandTokens && ((OperandToken)tok).Tokens.First().Type == TokenType.Noun)
                {
                    argsNamesLists.Add(((InfoToken)((OperandToken)tok).Tokens.First()).Content);
                    needComa = true;
                }
            }
            // Setup de la déclaration de fonction.
            Function fun = new Function();
            fun.ArgumentNames = argsNamesLists;
            fun.Body = ParseBlock(token.Block.Tokens, mainContext);
            fun.Modifiers = ParseEvaluableBlockModifier(token.Modifiers);
            fun.PrimaryModifier = ParsePrimaryEvaluableBlockModifier(token.PrimaryModifier);
            return fun;
        }

        /// <summary>
        /// Parse une expression opérande / opérateur / opérande et retourne un ExpressionGroup.
        /// </summary>
        static ExpressionGroup ParseExpressionGroup(ExpressionGroupToken token, GlobalContext mainContext)
        {
            var op = Operators.Mapping[((InfoToken)token.Operator).Content];
            ExpressionTree.ExpressionGroup group = new ExpressionGroup(ParseExpression(token.Operand1, mainContext),
                op, ParseExpression(token.Operand2, mainContext));
            return group;
        }
        /// <summary>
        /// Parse un string et retourne le nombre approprié.
        /// TODO : prendre en compte l'hexadécimal, binaire etc...
        /// </summary>
        static object ParseNumber(string str)
        {
            if (str.Contains("."))
                if (str.EndsWith("f"))
                    return float.Parse(str);
                else
                    return double.Parse(str);
            else
                return long.Parse(str);
        }
        /// <summary>
        /// Parse une intruction basique : appel de méthode ou affectation.
        /// La liste doit comprendre un seul jeton et pas de ;
        /// </summary>
        /// <returns></returns>
        static Instruction ParseInstruction(TokenList tokens, GlobalContext mainContext)
        {
            // Traitement spécial si return.
            if ((tokens.First().Type == TokenType.Statement))
            {
                if (tokens.Count == 2)
                {
                    InfoToken firstToken = (InfoToken)tokens[0];
                    switch (firstToken.Content)
                    {
                        case "return":
                            ReturnInstruction returnInst = new ReturnInstruction();
                            returnInst.Expression = ParseExpression(tokens[1], mainContext);
                            return returnInst;
                        case "patchkey":
                            PatchkeyInstruction patchkeyInst = new PatchkeyInstruction();
                            patchkeyInst.Key = ((InfoToken)((OperandToken)tokens[1]).Tokens.First()).Content;
                            return patchkeyInst;
                        default:
                            throw new Exception("Unexpected instruction");
                    }
                }
                else if (tokens.Count == 1)
                {
                    return new ReturnInstruction();
                }
                else
                    throw new Exception("Invalid 'return' expression");
            }

            if (tokens.Count != 1)
            {
                // TODO : traiter les using, etc...
                throw new Exception("Unexpected token list");
            }
            Token token = tokens.First();
            // On commence les deux types d'instructions habituels.
            if (token.Type == TokenType.ExpressionGroup)
            {
                // OperandGroup + ";" : c'est une affectation, s'il n'y a pas de signe 'égal' dedans,
                // bah c'est rien du tout.
                ExpressionGroupToken tok = (ExpressionGroupToken)token;
                AffectationInstruction ins = new AffectationInstruction();

                List<string> validTokens = new List<string>() { "=", "+=", "-=",
                                                                "/=", "*=" };

                // Si on a pas de "=", on a une expression.
                if (!validTokens.Contains(((InfoToken)tok.Operator).Content))
                    throw new Exception("An expression can't be used as instruction");

                ins.LeftMember = ParseSubExpression(((OperandToken)tok.Operand1).Tokens, mainContext);
                switch (((InfoToken)tok.Operator).Content)
                {
                    case "=":
                        ins.RightMember = ParseExpression(tok.Operand2, mainContext);
                        break;
                    case "+=":
                    case "-=":
                    case "/=":
                    case "*=":
                        string opString = ((InfoToken)tok.Operator).Content.Remove(1);
                        Operator op = Operators.Mapping[opString];
                        ins.RightMember = new ExpressionGroup((IGettable)ins.LeftMember, op,
                            ParseExpression(tok.Operand2, mainContext));
                        break;
                    default:
                        throw new Exception();
                }
                //ParseOperand((OperandToken)tok.Operand2);
                return ins;
            }
            else if (token.Type == TokenType.OperandTokens)
            {
                TokenList internalTokens = ((TokenContainer)token).Tokens;
                Token first = internalTokens.First();

                // Instructions pré-faites.
                if (first.Type == TokenType.Noun)
                {
                    InfoToken itoken = (InfoToken)first;
                    switch (itoken.Content)
                    {
                        case "using":
                            if (internalTokens.Count != 2 || internalTokens[1].Type != TokenType.String)
                                throw new Exception("Invalid using instruction");
                            InfoToken itoken2 = (InfoToken)internalTokens[1];
                            mainContext.LoadedNamespaces.Add(itoken2.Content);
                            return new UseNamespaceInstruction(itoken2.Content);
                        case "include":
                            if (internalTokens.Count != 2 || internalTokens[1].Type != TokenType.String)
                                throw new Exception("Invalid using instruction");
                            InfoToken itoken3 = (InfoToken)internalTokens[1];
                            mainContext.LoadedAssemblies.Add(itoken3.Content, Assembly.LoadWithPartialName(itoken3.Content));
                            return new LoadAssemblyInstruction(itoken3.Content);
                    }
                }

                // C'est un appel de méthode.
                return new MethodCallInstruction(ParseSubExpression(internalTokens, mainContext));

            }
            throw new Exception("Uncorrect instruction");
        }
        /// <summary>
        /// Parse une instruction if suivi de ses éventuels blocks else et elsif.
        /// </summary>
        public static Instruction ParseIfStatement(TokenList tokens, GlobalContext mainContext)
        {
            // Parse le if
            // On a a coup sûr : if + operand + block
            IfStatement statement = new IfStatement();
            bool nextIsElse = false;
            int tokenId = 0;
            ConditionalBlock currentBlock = null;
            foreach(Token tok in tokens)
            {
                if (nextIsElse)
                {
                    statement.ElseBlock = ParseBlock(((BlockGroupToken)tok).Tokens, mainContext);;
                }
                else
                {
                    if (tokenId % 3 == 0) // if, else, elsif
                    {
                        currentBlock = new ConditionalBlock();
                        // Si le prochain jeton statement est un jeton else, alors on
                        // prévient que c'est le cas pour la boucle suivante.
                        if (((InfoToken)tok).Content == "else")
                            nextIsElse = true;
                    }
                    else if (tokenId % 3 == 1) // operand
                    {
                        currentBlock.Condition = ParseExpression((OperandToken)tok, mainContext);
                    }
                    else if (tokenId % 3 == 2) // block
                    {
                        currentBlock.Block = ParseBlock(((BlockGroupToken)tok).Tokens, mainContext);
                        statement.Blocks.Add(currentBlock);
                    }
                }
                tokenId++;
            }
            return statement;
        }
        /// <summary>
        /// Parse un Statement suivi de son block.
        /// En général, on obtient :
        /// Statement + Operand + BlockGroup.
        /// Le statement if pouvant être composé différemment (avec else et elsif), il n'est pas traité
        /// dans cette fonction.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static Instruction ParseBlockStatement(TokenList tokens, GlobalContext mainContext)
        {
            // Patch
            if (tokens.Count == 4 && tokens.First().Type == TokenType.Statement)
            {
                PatchInstruction patchInstruction = new PatchInstruction();

                InfoToken itoken = (InfoToken)tokens.First();
                if (itoken.Content != "patch")
                    throw new Exception("Invalid statement format");
                patchInstruction.FuncName = ((InfoToken)((OperandToken)tokens[1]).Tokens[0]).Content;
                InfoToken keyToken  = (InfoToken)((OperandToken)(((PrefixedOperatorToken)tokens[2]).Operand)).Tokens.First();
                patchInstruction.Key = keyToken.Content;
                patchInstruction.Instructions = ParseBlock(((BlockGroupToken)tokens[3]).Tokens).Instructions;
                return patchInstruction;
            }

            if (tokens.Count != 3)
                throw new Exception("Invalid instruction format.");

            // On récupère les jetons.
            InfoToken statementToken = tokens[0] as InfoToken;
            Token exprToken = tokens[1];
            BlockGroupToken blockToken = tokens[2] as BlockGroupToken;

            if (statementToken == null || blockToken == null || statementToken.Type != TokenType.Statement)
                throw new Exception("Invalid instruction format.");

            Block block = ParseBlock(blockToken.Tokens, mainContext);

            switch (statementToken.Content)
            {
                case "return":
                    throw new Exception();
                case "function":
                    throw new Exception();
                    /*TokenList exprTokens = ((OperandToken)exprToken).Tokens;
                    FunctionDeclarationInstruction declaration = new FunctionDeclarationInstruction();
                    TokenList nameTokens = ((ParenthesisGroupToken)exprTokens[1]).Tokens;

                    // Ici on parcours les jetons de exprToken (qui doit être un OperandToken)
                    // afin de trouver les noms des arguments.

                    // Liste contenant les noms des arguments
                    List<string> argsNamesLists = new List<string>(); 
                    string funName = ((InfoToken)(exprTokens[0])).Content;

                    // Indique si le prochain jeton doit être une virgule
                    bool needComa = false;
                    foreach (Token tok in nameTokens)
                    {
                        if (needComa && tok.Type != TokenType.Separator)
                            throw new Exception("Expected ',' token in function declaration");
                        else
                            needComa = false;
                        // Si c'est un nom :
                        if (tok.Type == TokenType.OperandTokens && ((OperandToken)tok).Tokens.First().Type == TokenType.Noun)
                        {
                            argsNamesLists.Add(((InfoToken)((OperandToken)tok).Tokens.First()).Content);
                            needComa = true;
                        }
                    }
                    // Setup de la déclaration de fonction.
                    declaration.Function = new Function();
                    declaration.Function.ArgumentNames = argsNamesLists;
                    declaration.Function.Body = block;
                    declaration.FunctionName = funName;
                    
                    return declaration;*/
                    break;
                case "while":
                    IGettable expr = ParseExpression(exprToken, mainContext);
                    block = ParseBlock(blockToken.Tokens, mainContext);
                    WhileStatement statement = new WhileStatement();
                    statement.Block = block;
                    statement.Condition = expr;
                    return statement;
                case "for":
                    // Dans le cas d'une boucle for, expr est une opérande, contenant
                    // une instruction, une expression, et une autre instruction. 
                    // (bizarre, certes)
                    TokenList initializationInstruction = new TokenList();
                    TokenList stepInstruction = new TokenList();
                    TokenList conditionExpr = new TokenList();
                    int step = 0;
                    foreach (Token tok in ((OperandToken)exprToken).Tokens)
                    {
                        if (tok.Type == TokenType.EndOfInstruction)
                            step++;
                        else
                            switch (step)
                            {
                                case 0:
                                    initializationInstruction.Add(tok);
                                    break;
                                case 1:
                                    conditionExpr.Add(tok);
                                    break;
                                case 2:
                                    stepInstruction.Add(tok);
                                    break;
                            }

                    }
                    // On vérifie qu'on ait bien le bon nombre.
                    if (step != 2)
                        throw new Exception("Incorrect for statement.");

                    // On crée et on retourne le for.
                    ForStatement forStatement = new ForStatement();
                    forStatement.Initialisation = ParseInstruction(initializationInstruction, mainContext);
                    forStatement.Condition = ParseExpression(new OperandToken(conditionExpr), mainContext);
                    forStatement.Update = ParseInstruction(stepInstruction, mainContext);
                    forStatement.Block = block;

                    return forStatement;
                default:
                    throw new NotImplementedException("Not implemented statement");
            }

        }
    }
}
