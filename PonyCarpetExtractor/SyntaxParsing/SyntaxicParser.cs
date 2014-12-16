using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.SyntaxParsing
{
    /// <summary>
    /// Parseur syntaxique.
    /// Décompose le code en jetons, dont le sens sera déterminé par un parseur
    /// sémantique, et les passes pré-sémentiques.
    /// </summary>
    public static class SyntaxicParser
    {
        #region const
        public const string IF_KW = "if";
        public const string ELSE_KW = "else";
        public const string ELSIF_KW = "elsif";
        public const string WHILE_KW = "while";
        public const string FOR_KW = "for";
        public const string FUNCTION_KW = "function";
        public const string LAMBDA_KW = "lambda";
        public const string PATCH_KW = "patch";
        public const string PATCHKEY_KW = "patchkey";
        public const string NEW_KW = "new";
        public const string TRUE_KW = "true";
        public const string FALSE_KW = "false";
        public const string RETURN_KW = "return";
        public const string PUBLIC_MODIFIER_KW = "public";
        public const string REF_MODIFIER_KW = "ref";
        public static string[] STATEMENTS_KW = new string[] { IF_KW, ELSE_KW, ELSIF_KW, WHILE_KW,
            FOR_KW, FUNCTION_KW, LAMBDA_KW, RETURN_KW, PATCH_KW, PATCHKEY_KW};
        public static string[] MODIFIERS_KW = new string[] { REF_MODIFIER_KW, PUBLIC_MODIFIER_KW };
        public static string[] EVALUABLE_STATEMENTS_KW = new string[] { LAMBDA_KW, FUNCTION_KW };
        #endregion
        /// <summary>
        /// Classe utilisée pour stocker des listes de jetons.
        /// Utile pour le debug.
        /// </summary>
        public class TokenList : List<Token>
        {
            /// <summary>
            /// Constructeur sans paramètre...
            /// </summary>
            public TokenList() : base() { }
            /// <summary>
            /// Propriété affichant la liste de jetons d'une manière
            /// lisible pour le debug.
            /// </summary>
            public string Display
            {
                get { return PrintableTokens(this); }
            }
            /// <summary>
            /// Crée une copie de cette instance de TokenList.
            /// </summary>
            /// <returns></returns>
            public TokenList Copy()
            {
                TokenList lst = new TokenList();
                foreach (Token token in this)
                {
                    lst.Add(token);
                }
                return lst;
            }
        }

        #region Debug
        /// <summary>
        /// Nom des jetons en fonction de leur type. (pour debug)
        /// </summary>
        static Dictionary<TokenType, string> tokNames = new Dictionary<TokenType, string>()
        {
            { TokenType.Separator, ","},
            { TokenType.Dot, "."},
            { TokenType.EndOfInstruction, ";"},
        };
        /// <summary>
        /// Retourne un string contenant une version affichable de la liste de token pour
        /// une meilleure compréhension au debugging.
        /// </summary>
        public static string PrintableTokens(TokenList tokens)
        {
            StringBuilder builder = new StringBuilder();
            Token previousToken = null;
            foreach (Token token in tokens)
            {
                if (previousToken != null && previousToken.Type == TokenType.Noun &&
                        token.Type == TokenType.Noun)
                    builder.Append(" ");
                if (token.Type == TokenType.New)
                    builder.Append("new ");
                else if (token.Type == TokenType.String)
                    builder.AppendFormat("\"{0}\"", ((InfoToken)token).Content);
                else if (token.Type == TokenType.EndOfInstruction)
                    builder.Append(";\n");
                else if (token is InfoToken)
                    builder.Append(token.ToString());
                else
                    if (tokNames.ContainsKey(token.Type))
                        builder.Append(tokNames[token.Type]);
                    else
                        builder.Append(token.ToString());

                previousToken = token;
            }
            return builder.ToString();
        }
        /// <summary>
        /// Exception envoyée par le parseur lors d'une erreur de syntaxe.
        /// </summary>
        public class SyntaxErrorException : Exception {
            public SyntaxErrorException(int line, string message)
                : base(message)
            {

            }
        }
        #endregion
        /* ------------------------------------------------------------------------------
         * Token classes
         * ----------------------------------------------------------------------------*/
        #region Token Classes
        /// <summary>
        /// Représente des jetons de métadonnées.
        /// </summary>
        public enum TokenType
        {
            // Base
            Dot,
            Noun,
            Bool,
            Number,
            String,
            Operator,
            Separator, // ,
            EndOfInstruction, // ;
           
            // Macros
            New,
            Statement, // if, else, etc...
            BlockGroup, // groupe représentant un bloc d'instructions.
            OperandTokens, // groupe de jetons représentant une opérande (Operande)
            EvaluableBlock, // block évaluable (function, lambda, etc...) (Operande)
            ExpressionGroup, // groupe opérandes / opérateurs (Operande)
            ParenthesisGroup, // groupe englobé dans des parenthèses (Operande)
            GenericParametersGroup, // groupe d'arguments génériques
            IndexingParametersGroup, // groupe d'arguments d'indexation.

            // Pour faire une itération en plus dans certains cas :
            EndOfBlock,
        }
        /// <summary>
        /// Représente un jeton de métadonnées.
        /// </summary>
        public class Token
        {
            /// <summary>
            /// Type du jeton
            /// </summary>
            public TokenType Type { get; set; }
            /// <summary>
            /// Crée une nouvelle instance de Token, en précisant le type de jeton désiré.
            /// </summary>
            public Token(TokenType type)
            {
                Type = type;
            }
            /// <summary>
            /// Retourne un string représentant le jeton actuel (debug).
            /// </summary>
            public override string ToString()
            {
                if (tokNames.ContainsKey(Type))
                    return tokNames[Type];
                return Type.ToString();
            }
        }

        const int PREFIXED_OPERATOR_PRIORITY = 5;
        /// <summary>
        /// Représente un jeton de métadonnées contenant des informations supplémentaires.
        /// </summary>
        public class InfoToken : Token
        {
            /// <summary>
            /// Contenu du jeton (le contenu d'un string ou le string représentant un nombre)
            /// </summary>
            public string Content { get; set; }
            /// <summary>
            /// Crée une instance de InfoToken.
            /// </summary>
            public InfoToken(TokenType type, string content)
                : base(type)
            {
                Content = content;
            }
            /// <summary>
            /// Priorité de l'opérateur si le jeton est un opérateur.
            /// (un peu crade OK)
            /// </summary>
            public int Priority
            {
                get
                {
                    if (Type != TokenType.Operator)
                        throw new Exception();

                    switch (Content)
                    {
                        case "=":
                        case "+=":
                        case "-=":
                        case "*=":
                        case "/=":
                            return 0;
                        case "||":
                        case "&&":
                        case "|":
                        case "&":
                        case "^":
                            return 1;
                        case "==":
                        case "!=":
                        case ">=":
                        case "<=":
                        case "<":
                        case ">":
                            return 2;
                        case "+":
                        case "-":
                            return 3;
                        case "*":
                        case "/":
                        case "**":
                            return 4;
                        case "!":
                        case "$":
                        case "@":
                        case ":":
                            return PREFIXED_OPERATOR_PRIORITY;
                        default:
                            throw new Exception();
                    }
                }
                
            }

            /// <summary>
            /// Retourne un string représentant le jeton actuel (debug).
            /// </summary>
            public override string ToString()
            {
                return Content;
            }
        }
        /// <summary>
        /// Classe de base pour les Jetons contenant d'autres jetons.
        /// </summary>
        public class TokenContainer : Token
        {
            /// <summary>
            /// Liste de jetons contenus dans ce jeton.
            /// </summary>
            public TokenList Tokens { get; set; }

            public TokenContainer(TokenType type, TokenList tokens)
                : base(type)
            {
                Tokens = tokens;
            }
        }
        /// <summary>
        /// Représente un bloc évaluable :
        /// [function|lambda] [+modifiers] [Name] (args) { }
        /// </summary>
        public class EvaluableBlockToken : Token
        {
            public InfoToken PrimaryModifier; // function, lambda etc... 
            public List<InfoToken> Modifiers; // optional
            public string Name; // optional
            public ParenthesisGroupToken ArgumentList;
            public BlockGroupToken Block;
            public EvaluableBlockToken(TokenList list)
                : base(TokenType.EvaluableBlock)
            {
                Modifiers = new List<InfoToken>();
                foreach (Token token in list)
                {
                    if(token is InfoToken)
                    {
                        InfoToken itoken = (InfoToken)token;
                        if (EVALUABLE_STATEMENTS_KW.Contains(itoken.Content))
                        {
                            if (PrimaryModifier == null)
                                PrimaryModifier = itoken;
                            else
                                throw new InterpreterException(@"Error in function declaration :
     Only one primary modifier is allowed. Previous was " + PrimaryModifier.Content + " ; new is " + itoken.Content + "."); 
                        }
                        else if (MODIFIERS_KW.Contains(itoken.Content))
                        {
                            Modifiers.Add(itoken);
                        }
                        else if (itoken.Type == TokenType.Noun)
                        {
                            if(Name == null)
                                Name = itoken.Content;
                            else
                                throw new InterpreterException(@"Error in function declaration :
    Only one name is allowed");
                        }
                        else
                        {
                            throw new InterpreterException(@"Unexpected InfoToken in function declaration");
                        }
                    }
                    else if (token is TokenContainer)
                    {
                        TokenContainer ctoken = (TokenContainer)token;
                        if(ctoken.Type == TokenType.ParenthesisGroup)
                        {
                            if(ArgumentList == null)
                                ArgumentList = (ParenthesisGroupToken)ctoken;
                            else
                                throw new InterpreterException(@"Error in function declaration :
    Only one argument list is allowed");
                        }
                        else if (ctoken.Type == TokenType.BlockGroup)
                        {
                            if (Block == null)
                                Block = (BlockGroupToken)token;
                            else
                                throw new InterpreterException(@"Error in function declaration :
    Only one block is allowed");
                        }
                    } 
                }
            }
        }
        /// <summary>
        /// Représente un groupe Opérande - Opérateur - Opérande.
        /// </summary>
        public class ExpressionGroupToken : Token
        {
            public Token Operand1 { get; set; }
            public Token Operand2 { get; set; }
            public Token Operator { get; set; }
            /// <summary>
            /// Crée une instance de OperandGroupToken.
            /// </summary>
            public ExpressionGroupToken(Token operand1, Token operand2, Token @operator)
                : base(TokenType.ExpressionGroup)
            {
                Operand1 = operand1;
                Operand2 = operand2;
                Operator = @operator;
            }
            /// <summary>
            /// Retourne un string représentant le jeton actuel (debug).
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "\\" + Operand1.ToString() + " " + Operator.ToString() + " " + Operand2.ToString() + "/";
            }
        }

        /// <summary>
        /// Représente un groupe de jetons qui forme une seule opérande.
        /// </summary>
        public class OperandToken : TokenContainer
        {
            /// <summary>
            /// Crée une instance de OperandToken.
            /// </summary>
            public OperandToken(TokenList tokens)
                : base(TokenType.OperandTokens, tokens)
            {

            }
            /// <summary>
            /// Retourne un string représentant le jeton actuel (debug).
            /// </summary>
            public override string ToString()
            {
                StringBuilder build = new StringBuilder();
                build.Append("`");
                foreach (Token token in Tokens)
                {
                    build.Append(token.ToString());
                    build.Append(" ");
                }
                build.Remove(build.Length - 1, 1);
                build.Append("'");
                return build.ToString();
            }
        }

        /// <summary>
        /// Représente un groupe opérateur préfixé + opérande.
        /// </summary>
        public class PrefixedOperatorToken : Token
        {
            public Token Operator { get; set; }
            public Token Operand { get; set; }
            /// <summary>
            /// Crée une instance de PrefixedOperatorToken.
            /// </summary>
            public PrefixedOperatorToken(Token @operator, Token operand)
                : base(TokenType.Operator)
            {
                Operator = @operator;
                Operand = operand;
            }
            /// <summary>
            /// Retourne un string représentant le jeton actuel (debug).
            /// </summary>
            public override string ToString()
            {
                return Operator.ToString() + Operand.ToString();
            }
        }

        /// <summary>
        /// Représente un groupe englobé par des parenthèses.
        /// </summary>
        public class ParenthesisGroupToken : TokenContainer
        {
            /// <summary>
            /// Crée une instance de ParenthesisGroupToken.
            /// </summary>
            public ParenthesisGroupToken(TokenList tokens)
                : base(TokenType.ParenthesisGroup, tokens)
            {
            }
            /// <summary>
            /// Retourne un string représentant le jeton actuel (debug).
            /// </summary>
            public override string ToString()
            {
                StringBuilder build = new StringBuilder();
                build.Append("(");
                foreach (Token token in Tokens)
                {
                    build.Append(token.ToString());
                    build.Append(" ");
                }
                if(Tokens.Count != 0)
                    build.Remove(build.Length - 1, 1);
                build.Append(")");
                return build.ToString();
            }
        }

        /// <summary>
        /// Représente un groupe d'arguments génériques.
        /// </summary>
        public class GenericArgumentsGroupToken : TokenContainer
        {
            /// <summary>
            /// Crée une instance de GenericArgumentsGroupToken.
            /// </summary>
            public GenericArgumentsGroupToken(TokenList tokens)
                : base(TokenType.GenericParametersGroup, tokens)
            {
            }
            /// <summary>
            /// Retourne un string représentant le jeton actuel (debug).
            /// </summary>
            public override string ToString()
            {
                StringBuilder build = new StringBuilder();
                build.Append("<");
                foreach (Token token in Tokens)
                {
                    build.Append(token.ToString());
                    build.Append(" ");
                }
                if(Tokens.Count != 0)
                    build.Remove(build.Length - 1, 1);
                build.Append(">");
                return build.ToString();
            }
        }

        /// <summary>
        /// Représente un groupe représentant des paramètres d'indexation.
        /// </summary>
        public class IndexingParametersGroupToken : TokenContainer
        {
            /// <summary>
            /// Crée une instance de IndexingArgumentGroupToken.
            /// </summary>
            public IndexingParametersGroupToken(TokenList tokens)
                : base(TokenType.IndexingParametersGroup, tokens)
            {
            }
            /// <summary>
            /// Retourne un string représentant le jeton actuel (debug).
            /// </summary>
            public override string ToString()
            {
                StringBuilder build = new StringBuilder();
                build.Append("[");
                foreach (Token token in Tokens)
                {
                    build.Append(token.ToString());
                    build.Append(" ");
                }
                if(Tokens.Count != 0)
                    build.Remove(build.Length - 1, 1);
                build.Append("]");
                return build.ToString();
            }
        }

        /// <summary>
        /// Représente un groupe représentant un block d'instructions.
        /// </summary>
        public class BlockGroupToken : TokenContainer
        {
            public BlockGroupToken(TokenList tokens)
                : base(TokenType.BlockGroup, tokens)
            {
            }
            /// <summary>
            /// Retourne un string représentant le jeton actuel (debug).
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                StringBuilder build = new StringBuilder();
                build.Append("{\n");
                foreach (Token token in Tokens)
                {
                    build.Append(token.ToString());
                    build.Append(" ");
                }
                if (Tokens.Count != 0)
                    build.Remove(build.Length - 1, 1);
                build.Append("\n}");
                return build.ToString();
            }
        }
        #endregion
        /* ------------------------------------------------------------------------------
         * Recognition functions
         * ----------------------------------------------------------------------------*/
        #region Recognition functions
        /// <summary>
        /// Indique si le caractère passé en argument est un indicateur de base.
        /// </summary>
        static bool isBaseIndicator(char chr)
        {
            return chr == 'b' || chr == 'x';
        }
        /// <summary>
        /// Indique si le caractère passé en argument est une fin d'instruction.
        /// </summary>
        static bool isEndOfInstruction(char chr)
        {
            return chr == ';';
        }
        /// <summary>
        /// Transforme une liste de char en string.
        /// </summary>
        /// <param name="chrs"></param>
        /// <returns></returns>
        static string charArrayToString(List<char> chrs)
        {
            StringBuilder b = new StringBuilder();
            foreach (char chr in chrs)
            {
                b.Append(chr);
            }
            return b.ToString();
        }
        #endregion
        /* ------------------------------------------------------------------------------
         * Passes
         * ----------------------------------------------------------------------------*/
        #region Passes
        /// <summary>
        /// Liste d'opérateurs à plusieurs caractères.
        /// </summary>
        static List<Tuple<string, string>> multiCharOperators = new List<Tuple<string,string>>()
        {
            new Tuple<string, string>("+", "="),
            new Tuple<string, string>("-", "="),
            new Tuple<string, string>("*", "="),
            new Tuple<string, string>("/", "="),
            new Tuple<string, string>("=", "="),
            new Tuple<string, string>("!", "="),
            new Tuple<string, string>(">", "="),
            new Tuple<string, string>("<", "="),
            new Tuple<string, string>("|", "|"),
            new Tuple<string, string>("&", "&"),
            new Tuple<string, string>("*", "*"),
        };

        /// <summary>
        /// Retourne le jeton approprié à partir du jeton de type Noun,
        /// dont le contenu indique un jeton spécifique (mot clef etc...).
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        static InfoToken CorrectToken(string content)
        {
            if(STATEMENTS_KW.Contains(content))
            {
                return new InfoToken(TokenType.Statement, content);
            }
            else
            {
                switch (content)
                {
                    /*case IF_KW:
                    case ELSE_KW:
                    case ELSIF_KW:
                    case WHILE_KW:
                    case FOR_KW:
                    case FUNCTION_KW:
                    case LAMBDA_KW:
                    case RETURN_KW:
                    case PATCH_KW:
                    case PATCHKEY_KW:
                        return new InfoToken(TokenType.Statement, content);*/
                    case NEW_KW:
                        return new InfoToken(TokenType.New, content);
                    case TRUE_KW:
                    case FALSE_KW:
                        return new InfoToken(TokenType.Bool, content);
                    default:
                        return new InfoToken(TokenType.Noun, content);
                }
            }
        }
        /// <summary>
        /// Applique une première passe de vérification à la liste de jetons donnée :
        /// opérateurs à deux caractère et détection des mots clefs.
        /// </summary>
        /// <returns></returns>
        static TokenList ApplyPass1(TokenList tokens)
        {
            if (tokens.Count == 0)
                return tokens;
            // Première vérification : opérateurs <=, >=, &&, ||
            TokenList newTokens = new TokenList();
            Token previousToken = tokens.First();
            bool first = true;
            foreach (Token token in tokens)
            {
                Token tempToken = token;
                bool forceAdd = true;
                if (first)
                {
                    // Premier item, on le corrige si c'est un mot clef.
                    first = false;
                    if (token.Type == TokenType.Noun)
                        previousToken = CorrectToken(((InfoToken)token).Content);
                    else
                        previousToken = token;
                }
                else
                {
                    // Si on a deux opérateurs, et que ces derniers correspondent à un seul
                    // opérateur (ex : '+' et '=' => "+="), on les fusionne.
                    if (token.Type == TokenType.Operator && previousToken.Type == TokenType.Operator)
                    {
                        InfoToken previousTokenI = (InfoToken)previousToken;
                        InfoToken tokenI = (InfoToken)token;
                        forceAdd = true;
                        foreach (Tuple<string, string> tup in multiCharOperators)
                        {
                            if (previousTokenI.Content == tup.Item1 &&
                                tokenI.Content == tup.Item2)
                            {
                                forceAdd = false;
                                tempToken = new InfoToken(TokenType.Operator, tup.Item1 + tup.Item2);
                                break;
                            }
                        }
                        // Gestion des opérateurs postfixés ++ et --
                        if (previousTokenI.Content == "+" && tokenI.Content == "+")
                        {
                            forceAdd = false;
                            newTokens.Add(new InfoToken(TokenType.Operator, "+="));
                            tempToken = new InfoToken(TokenType.Number, "1");
                        }
                        else if (previousTokenI.Content == "-" && tokenI.Content == "-")
                        {
                            forceAdd = false;
                            newTokens.Add(new InfoToken(TokenType.Operator, "-="));
                            tempToken = new InfoToken(TokenType.Number, "1");
                        }
                    }
                    else if (token.Type == TokenType.Noun)
                    {
                        // Si on a un nom on vérifie que ce ne soit pas un mot clef.
                        InfoToken tokenI = (InfoToken)token;
                        tempToken = CorrectToken(tokenI.Content);
                        forceAdd = true;
                    }

                    // Si on doit ajouter le jeton à la liste (càd si on veut pas 
                    // en supprimer un, comme un des deux jetons servant à décrire
                    // un même opérateur) :
                    if(forceAdd)
                        newTokens.Add(previousToken);

                    previousToken = tempToken;
                }

            }
            newTokens.Add(previousToken);
            return newTokens;
        }
        /// <summary>
        /// Applique une passe qui transforme des groupes de jetons en
        /// opérandes, facilitant une lecture ultérieure.
        /// Ex de transformation :
        /// machin = truc(bidule + chose) * machin + (machin * truc)
        /// [Operand = Operand [operand group [operand op operand]] op operand op group [operand op operand]]
        /// if(bidule(truc+chose) * machin == 10) {}
        /// 
        /// Ce qui délimite les opérandes :
        ///     - un opérateur
        ///     - un séparateur
        ///     - fin d'instruction    
        /// 
        /// </summary>
        /// <returns></returns>
        public static TokenList ApplyPass2(TokenList tokens)
        {
            TokenList newTokens = new TokenList();
            
            // liste de jetons constituant une opérande.
            TokenList operandTokenStack = new TokenList();
            // Indique si un block de type :
            // function blabla() { } a commencé.
            bool evaluable_block_started = false;
            foreach (Token token in tokens)
            {
                if (evaluable_block_started)
                {
                    operandTokenStack.Add(token);
                    // Si le parsing d'un bloc évaluable a commencé.
                    if (token.Type == TokenType.BlockGroup)
                    {
                        // Préviens de la fin du bloc évaluable.
                        evaluable_block_started = false;
                        // Ajoute ce block à la liste des jetons de sortie.
                        // Ajoute l'opérande si elle existe.
                        if (operandTokenStack.Count != 0)
                            newTokens.Add(new EvaluableBlockToken(operandTokenStack.Copy()));

                        // Nettoie la pile.
                        operandTokenStack.Clear();
                    }
                }
                else if (token.Type == TokenType.Statement && EVALUABLE_STATEMENTS_KW.Contains(((InfoToken)token).Content))
                {
                    // Ici, notre jeton est un jeton de block évaluable.
                    evaluable_block_started = true;

                    // Termine la suite de jetons d'opérande si elle existe
                    if (operandTokenStack.Count != 0)
                        newTokens.Add(new OperandToken(operandTokenStack.Copy()));
                    operandTokenStack.Clear();

                    // Ajoute le premier jeton du block à la liste d'opérandes.
                    operandTokenStack.Add(token);
                }
                else if (token.Type != TokenType.Operator && token.Type != TokenType.Separator &&
                    token.Type != TokenType.EndOfInstruction && token.Type != TokenType.Statement && 
                    token.Type != TokenType.BlockGroup)
                {
                    // Les jetons ne constituent pas une opérande.
                    // On continue à regrouper les jetons dans l'opérande courante.
                    operandTokenStack.Add(token);
                }
                else
                {
                    // Ajoute l'opérande si elle existe.
                    if(operandTokenStack.Count != 0)
                        newTokens.Add(new OperandToken(operandTokenStack.Copy()));

                    // Nettoie la pile.
                    operandTokenStack.Clear();

                    // Ajoute le jeton qui a marqué la fin de l'opérande à la nouvelle liste
                    // de jetons (pas sur la pile des opérandes).
                    newTokens.Add(token);
                    
                }
            }

            // Ajoute la dernière opérande en cours de réalisation.
            if (operandTokenStack.Count != 0)
                newTokens.Add(new OperandToken(operandTokenStack.Copy()));

            return newTokens;
        }

        /// <summary>
        /// Retourne true si le jeton donné est un opérateur préfixé.
        /// </summary>
        static bool isPrefixedOperator(Token token)
        {
            InfoToken tok = (InfoToken)token;
            return tok.Content == "!" || tok.Content == ":" || tok.Content == "$" || tok.Content == "@";
        }
        /// <summary>
        /// Retourne true si le jeton donné est un jeton de groupe.
        /// </summary>
        static bool isGroup(Token token)
        {
            return (token.Type == TokenType.OperandTokens ||
                    token.Type == TokenType.ExpressionGroup ||
                    token.Type == TokenType.ParenthesisGroup ||
                    token.Type == TokenType.GenericParametersGroup ||
                    token.Type == TokenType.BlockGroup ||
                    token.Type == TokenType.IndexingParametersGroup);
        }
        /// <summary>
        /// Retourne la priorité actuelle du dernier opérateur
        /// pour la pile de jetons donnée.
        /// </summary>
        /// <param name="tokenStack"></param>
        /// <returns></returns>
        static int GetCurrentPriority(Stack<Token> tokenStack)
        {
            Stack<Token> save = new Stack<Token>();
            int ret = 0;
            while (true)
            {
                Token token = tokenStack.Pop();
                save.Push(token);
                if (token.Type == TokenType.Operator)
                {
                    ret = ((InfoToken)token).Priority;
                    break;
                }
            }
            while (save.Count != 0)
                tokenStack.Push(save.Pop());
            return ret;
        }
        /// <summary>
        /// Retourne true si le jeton donné peut être interprété comme une opérande.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        static bool IsOperandToken(Token token)
        {
            return token.Type == TokenType.OperandTokens ||
                   token.Type == TokenType.ExpressionGroup ||
                   token.Type == TokenType.ParenthesisGroup ||
                   token.Type == TokenType.EvaluableBlock;
        }
        /// <summary>
        /// Applique la dernière passe qui crée les OperandGroupToken
        /// </summary>
        public static TokenList ApplyPass3(TokenList tokens)
        {
            return ApplyPass3(tokens, 0);
        }
        /// <summary>
        /// Applique la dernière passe qui crée les OperandGroupToken à partir des Operator / Operands.
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static TokenList ApplyPass3(TokenList tokens, int depth)
        {
            // Pile de jetons contenant opérateurs et opérandes.
            Stack<Token> tokenStack = new Stack<Token>();
            TokenList newList = new TokenList();
            int currentPriority = 10;
            // Indique la présence ou non d'un éventuel opérateur préfixé à ajouter
            // à l'opérande suivante.
            InfoToken prefixedOperator = null;
            // Fonction de création de groupe.
            Func<bool> createGroup = delegate()
            {
                // On met les deux derniers dans la boite
                var op2 = tokenStack.Pop();
                var optor = tokenStack.Pop();
                var op1 = tokenStack.Pop();

                // Crée le groupe et le place dans la pile
                ExpressionGroupToken grp = new ExpressionGroupToken(op1, op2, optor);
                tokenStack.Push(grp);
                return true;
            };

            // Simplifie le procédé s'il n'y a qu'un seul jeton.
            if (tokens.Count == 1)
            {
                if (tokens.First() is TokenContainer)
                    if(((TokenContainer)tokens.First()).Tokens.Count == 1)
                        return new TokenList() { ((TokenContainer)tokens.First()).Tokens.First() };
                    else
                        return ApplyPass3(((TokenContainer)tokens.First()).Tokens, depth+1);
                else
                    return tokens;
            }

            foreach (Token token in tokens)
            {
                // Si le jeton est un groupe pouvant être interprété comme une
                // opérande, on l'ajoute à la pile des opérandes.
                if (IsOperandToken(token))
                {
                    // Si le jeton est un block évaluable, et qu'il est tout seul, c'est
                    // une déclaration de fonction, donc à ne pas tenter de grouper.
                    if (token.Type == TokenType.EvaluableBlock && tokenStack.Count == 0)
                    {
                        newList.Insert(newList.Count, token);
                    }
                    else
                    {
                        if (token.Type == TokenType.OperandTokens)
                            ((TokenContainer)token).Tokens = ApplyPass3(((TokenContainer)token).Tokens);

                        // On vérifie si cette opérande a un opérateur préfixé. 
                        if (prefixedOperator != null)
                        {
                            tokenStack.Push(new PrefixedOperatorToken(prefixedOperator, token));
                            prefixedOperator = null;
                        }
                        else
                            tokenStack.Push(token);
                    }
                }
                else
                {
                    // Sinon 2 cas :
                    // Si c'est un opérateur, on l'ajoute à la pile pour regrouper le reste
                    // ensuite.
                    // Sinon, il termine le groupe actuel et en fait démarrer un nouveau
                    // (en vidant la pile de jetons).
                    bool isOperator = token.Type == TokenType.Operator;

                    // Si c'est un ; (ou autre), plus faible priorité pour terminer.
                    int tokenPriority = isOperator ? ((InfoToken)token).Priority : 0;

                    if (isOperator)
                    {
                        InfoToken itoken = (InfoToken)token;
                        // Vérifie si l'opérateur est préfixé :
                        if (isPrefixedOperator(itoken))
                        {
                            prefixedOperator = itoken;
                            continue;
                        }
                    }

                    if (tokenStack.Count >= 3)
                        currentPriority = GetCurrentPriority(tokenStack);
                    // On regarde si on peut regrouper des opérandes / opérateurs :
                    // On commence seulement si on voit un opérateur de priorité
                    // plus faible que la précédente.
                    if (tokenPriority <= currentPriority && tokenStack.Count >= 3)
                    {
                        // On repart en marche arrière pour faire les opérations
                        // à plus forte priorité.
                        while (tokenPriority <= currentPriority && tokenStack.Count >= 3)
                        {
                            createGroup();
                            if (tokenStack.Count >= 3)
                                currentPriority = GetCurrentPriority(tokenStack);
                        }
                        tokenStack.Push(token);
                    }
                    else
                    {
                        tokenStack.Push(token);
                    }

                    // On vide toute la pile dans ce cas.
                    if (!isOperator) // séparateur ou fin d'instruction.
                    {
                        int listCount = newList.Count;
                        while (tokenStack.Count != 0)
                            newList.Insert(listCount, tokenStack.Pop());
                    }
                }
            }

            // On vide la pile.
            int lstCount = newList.Count;
            while (tokenStack.Count != 0)
                newList.Insert(lstCount, tokenStack.Pop());

            return newList;
        }
        #endregion
        /* ------------------------------------------------------------------------------
         * Parsing
         * ----------------------------------------------------------------------------*/
        #region Parse
        /// <summary>
        /// Supprime les espace blanc en fin de liste de char.
        /// </summary>
        static void RemoveEndingWhitespaces(List<char> lst)
        {
            if (lst.Count == 0)
                return;

            while(lst.Count != 0 && char.IsWhiteSpace(lst.Last()))
            {
                lst.RemoveAt(lst.Count - 1);
            }
        }
        /// <summary>
        /// Parse le string passé en argument et retourne une liste de jetons.
        /// </summary>
        /// <returns></returns>
        public static TokenList Parse(string str)
        {
            TokenList tokens = new TokenList();
            // Indique la ligne courante.
            int line = 0;
            // Indique si on commence à parser un nombre.
            bool startedNumberParsing = false;
            // Indique si on commence à parser un nom.
            bool startedNounParsing = false;
            // Indique si on commence à parser un string. (double quote)
            bool startedStringParsing = false;
            // Indique la profondeur de parsing du groupe actuellement
            // entrain d'être parsé. Une valeur de -1 indique qu'aucun groupe
            // n'est entrain de se former.
            int groupParsingDepth = -1;
            // Indique le type de groupe à créer une fois le parsing du groupe terminé.
            TokenType parseGroupType = TokenType.GenericParametersGroup;
            // Indique si on a commencé à faire un commentaire single line.
            bool startedSingleLineComment = false;
            // Pile de caractères d'un nom ou d'un nombre
            List<char> charStack = new List<char>();

            // Fonction permettant de terminer l'accumulation de lettres
            // pour un jeton Noun ou Number.
            Func<bool> terminateWord = delegate()
            {
                // Marque la fin d'un mot ou d'un nombre.
                if (startedNounParsing || startedNumberParsing)
                {
                    string final = charArrayToString(charStack);
                    TokenType type;
                    if (startedNumberParsing)
                        type = TokenType.Number;
                    else
                        type = TokenType.Noun;
                    tokens.Add(new InfoToken(type, final));
                    charStack.Clear();
                    startedNounParsing = false;
                    startedNumberParsing = false;
                }
                return true;
            };

            // Index du caractère.
            int chrIndex = 0;
            foreach(char chr in str)
            {
                chrIndex++;

                // Commentaires ignorés
                if (startedSingleLineComment)
                    if (chr == '\n')
                        startedSingleLineComment = false;
                    else
                        continue; // ignore le char

                // Si on a commencé à parser un groupe, on ajoute les caractères formant ce 
                // groupe.
                if (groupParsingDepth >= 0)
                {
                    

                    // Partie moche, à optimiser...
                    char closingChar;
                    char openingChar;
                    switch (parseGroupType)
                    {
                        case TokenType.ParenthesisGroup:
                            openingChar = '(';
                            closingChar = ')';
                            break;
                        case TokenType.GenericParametersGroup:
                            openingChar = '<';
                            closingChar = '>';
                            break;
                        case TokenType.BlockGroup:
                            openingChar = '{';
                            closingChar = '}';
                            break;
                        case TokenType.IndexingParametersGroup:
                            openingChar = '[';
                            closingChar = ']';
                            break;
                        default:
                            throw new Exception();
                    }
                    if (chr == openingChar)
                        groupParsingDepth++;

                    if (chr == closingChar)
                        groupParsingDepth--;

                    // On termine le groupe.
                    if (groupParsingDepth == -1)
                    {
                        

                        // Pour un parsing complet
                        bool removeLast = false;
                        RemoveEndingWhitespaces(charStack);
                        TokenList newTokens; // nouveaux jetons du groupe : déjà parsés.
                        if (charStack.Count != 0)
                        {

                            if (charStack.Last() != ';')
                            {
                                charStack.Add(';');
                                removeLast = true;
                            }

                            // On parse totalement le groupe
                            newTokens = Parse(charArrayToString(charStack));

                            // enlève le ; si on l'a ajouté avant
                            if (removeLast)
                                newTokens.RemoveAt(newTokens.Count - 1);
                        }
                        else
                            newTokens = new TokenList();

                        switch (parseGroupType)
                        {
                            case TokenType.ParenthesisGroup:
                                tokens.Add(new ParenthesisGroupToken(newTokens));
                                break;
                            case TokenType.GenericParametersGroup:
                                tokens.Add(new GenericArgumentsGroupToken(newTokens));
                                break;
                            case TokenType.BlockGroup:
                                tokens.Add(new BlockGroupToken(newTokens));
                                break;
                            case TokenType.IndexingParametersGroup:
                                tokens.Add(new IndexingParametersGroupToken(newTokens));
                                break;
                            default:
                                throw new Exception();
                        }
                        charStack.Clear();
                    }
                    else
                        charStack.Add(chr);

                    continue;
                }
                // On n'est pas entrain de parser un nom ou un string
                if (!startedStringParsing)
                {
                    if (chr == '#') // commentaire
                    {
                        startedSingleLineComment = true;
                    }
                    else if (char.IsDigit(chr)) // Nombre
                    {
                        // On a commencé à parser le nombre ou un nom, on continue.
                        if (startedNumberParsing || startedNounParsing)
                        {
                            charStack.Add(chr);
                        }
                        else
                        {
                            // On commence le parsing du nombre
                            charStack.Add(chr);
                            startedNumberParsing = true;
                        }
                    }
                    else if (char.IsLetter(chr) || chr == '_')
                    {
                        // On a commencé à parser le nombre : la lettre indique
                        // la base du nombre tapé.
                        if (startedNumberParsing)
                        {
                            // Vérifie que la lettre soit bel et bien un
                            // indicateur de base.
                            if (isBaseIndicator(chr))
                            {
                                // Vérifie que ce soit bien la seule lettre présente dans le nombre, et
                                // précédée d'un 0.
                                if (charStack.Count == 1 && charStack[0] == '0')
                                    charStack.Add(chr);
                                else
                                    throw new SyntaxErrorException(line, "unexpected" + chr.ToString() + "token.");
                            }
                            else
                            {
                                // Prise en charge de la notation hexadécimale.
                                if (charStack.Contains('x')) // => hexa
                                {
                                    if (chr == 'A' || chr == 'B' || chr == 'C' || chr == 'D' || chr == 'E'
                                        || chr == 'F')
                                        charStack.Add(chr);
                                    else
                                        throw new SyntaxErrorException(line, "unexpected " + chr.ToString() + " token.");
                                }
                                else
                                    throw new SyntaxErrorException(line, "unexpected " + chr.ToString() + " token.");
                            }
                        }
                        else if (startedNounParsing)
                        {
                            charStack.Add(chr);
                        }
                        else
                        {
                            charStack.Add(chr);
                            startedNounParsing = true;
                        }
                    }
                    else if (chr == '"')
                        startedStringParsing = true;
                    else if (char.IsWhiteSpace(chr) || isEndOfInstruction(chr))
                    {
                        terminateWord();
                        // Jeton de fin d'instruction si tel est le cas.
                        if (isEndOfInstruction(chr))
                            tokens.Add(new Token(TokenType.EndOfInstruction));
                    }
                    else if (chr == '.')
                    {
                        if (startedNumberParsing)
                            if (!charStack.Contains('.'))
                                charStack.Add('.');
                            else
                                throw new SyntaxErrorException(line, "unexpected '.' token in number");
                        else
                        {
                            terminateWord();
                            tokens.Add(new Token(TokenType.Dot));
                        }
                    }
                    else
                    {
                        // Permet de savoir si on était entrain de parser un nom,
                        // ce qui permet de déterminer la signification de '<' : 
                        // commencement de groupe générique, ou opérateur.
                        bool startedNounParsingTmp = startedNounParsing;

                        // Termine le mot en cours.
                        terminateWord();

                        if (chr == '(')
                        {
                            groupParsingDepth = 0;
                            parseGroupType = TokenType.ParenthesisGroup;
                        }
                        else if (chr == '[')
                        {
                            groupParsingDepth = 0;
                            parseGroupType = TokenType.IndexingParametersGroup;
                        }
                        else if (chr == '{')
                        {
                            groupParsingDepth = 0;
                            parseGroupType = TokenType.BlockGroup;
                        }
                        else if (chr == '<')
                        {
                            // On tombe sur un générique
                            if (startedNounParsingTmp)
                            {
                                groupParsingDepth = 0;
                                parseGroupType = TokenType.GenericParametersGroup;
                            }
                            else
                            {
                                // Sinon c'est l'opérateur.
                                tokens.Add(new InfoToken(TokenType.Operator, "<"));
                            }
                        }
                        else if (chr == ']' || chr == '}' || chr == ')')
                        {
                            throw new SyntaxErrorException(line, "Unexpected closing token " + chr.ToString());
                        }
                        else if (chr == ',')
                            tokens.Add(new Token(TokenType.Separator));
                        else if (chr == '+' || chr == '-' || chr == '*' || chr == '/' ||
                                    chr == '!' || chr == '='  || chr == '>' ||
                                    chr == '&' || chr == '|' || chr == '^' || chr == ':' ||
                                    chr == '@' || chr == '$')
                            tokens.Add(new InfoToken(TokenType.Operator, chr.ToString()));
                    }
                }
                else // startedStringParsing
                {
                    // Fin du string.
                    if (chr == '"' && (charStack.Count == 0 || charStack.Last() != '\\'))
                    {

                        tokens.Add(new InfoToken(TokenType.String, charArrayToString(charStack)));
                        charStack.Clear();
                        startedStringParsing = false;
                    }
                    else
                        charStack.Add(chr);
                }
            }
            var passed = ApplyPass3(ApplyPass2(ApplyPass1(tokens)));
            return passed;
        }
        #endregion
    }
}