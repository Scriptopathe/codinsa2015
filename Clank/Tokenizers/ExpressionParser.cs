using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TType = Clank.Core.Tokenizers.ExpressionToken.ExpressionTokenType;
namespace Clank.Core.Tokenizers
{
    /// <summary>
    /// Permet de parser des expressions et de regrouper des jetons au sein de mêmes expressions.
    /// Regroupements possibles :
    ///     (101, 50, 9*(5+3)) => ParenthesisList(101,  50, BinOp*(BinOp+(5, 3), 9))
    /// </summary>
    public class ExpressionParser
    {
        /// <summary>
        /// Parse la liste de jeton passée en paramètre.
        /// </summary>
        /// <param name="tokens"></param>
        public static List<ExpressionToken> Parse(List<Token> tokens)
        {
            var expressionTokens = Separate(MakeGroups(tokens));
            return Separate(MakeGroups(tokens));
        }

        static List<string> group_chars = new List<string>()
        {
            "{", "(", "<", "[", "]", ">", ")", "}"
        };

        static List<string> conditional_statements_keywords = new List<string>()
        {
            "if", "else", "elsif", "while"
        };

        static List<TType> s_listSeparators = new List<TType>() { TType.Separator };
        static List<TType> s_codeBlockSeparators = new List<TType>() { TType.EndOfInstruction, TType.Separator, 
            TType.NamedCodeBlock, TType.NamedGenericCodeBlock, TType.CodeBlock, TType.FunctionDeclaration };
        /// <summary>
        /// Supprime les jetons séparateurs de la liste, et met tout ce qu'il y a entre dans des jetons séparés.
        /// (bla ',' bla ',' bla) => List(bla, bla, bla)
        /// Présuppose que MakeGroups a été appelé sur la liste de jetons passée en paramètre.
        /// </summary>
        static List<ExpressionToken> Separate(List<ExpressionToken> lstToken, bool isInCodeBlock=false)
        {
            List<ExpressionToken> newTokens = new List<ExpressionToken>();
            List<ExpressionToken> currentEvaluableTokens = new List<ExpressionToken>();

            List<TType> separators = isInCodeBlock ? s_codeBlockSeparators : s_listSeparators;

            foreach(ExpressionToken token in lstToken)
            {
                // On crée une jeton évaluable à partir de ce qu'il y a avant le séparateur.
                if(separators.Contains(token.TkType))
                {
                    // Si le séparateur d'instruction est un block de code, on le laisse dans la liste des jetons.
                    if (token.TkType == TType.NamedCodeBlock || token.TkType == TType.NamedGenericCodeBlock ||
                        token.TkType == TType.CodeBlock || token.TkType == TType.FunctionDeclaration)
                        currentEvaluableTokens.Add(token);

                    // Erreur de syntaxe : liste de jetons vide entre 2 séparateurs.
                    if(currentEvaluableTokens.Count == 0)
                    {
                        throw new SyntaxError("Jeton '" + token.ToString() + "' inattendu.", token.Line, token.Source);
                    }

                    newTokens.Add(ParseEvaluable(currentEvaluableTokens));
                    currentEvaluableTokens.Clear();
                }
                else
                    currentEvaluableTokens.Add(token);
            }

            // Vide la pile de jetons évaluables.
            if(currentEvaluableTokens.Count != 0)
            {
                newTokens.Add(ParseEvaluable(currentEvaluableTokens));
                currentEvaluableTokens.Clear();
            }
            return newTokens;
        }
        /// <summary>
        /// Transforme le jeton passé en paramètre en un jeton d'expression.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        static ExpressionToken ToExpressionToken(Token token)
        {
            switch(token.TkType)
            {
                case Token.TokenType.Name:
                    // Gestion des keywords ici !
                    ExpressionToken.ExpressionTokenType tkType;
                    if (conditional_statements_keywords.Contains(token.Content))
                        tkType = TType.ConditionalStatement;
                    else
                        tkType = TType.Name;
 
                    return new ExpressionToken() { TkType = tkType, Content = token.Content,
                        Line = token.Line, Character = token.Character, Source = token.Source };
                case Token.TokenType.NumberLiteral:
                    return new ExpressionToken() { TkType = ExpressionToken.ExpressionTokenType.NumberLiteral, Content = token.Content, 
                        Line = token.Line, Character = token.Character, Source = token.Source };
                case Token.TokenType.Operator:
                    return new ExpressionToken() { TkType = ExpressionToken.ExpressionTokenType.Operator, Content = token.Content,
                        Line = token.Line, Character = token.Character, Source = token.Source };
                case Token.TokenType.StringLiteral:
                    return new ExpressionToken() { TkType = ExpressionToken.ExpressionTokenType.StringLiteral, Content = token.Content,
                        Line = token.Line, Character = token.Character, Source = token.Source };
                case Token.TokenType.BoolLiteral:
                    return new ExpressionToken() { TkType = ExpressionToken.ExpressionTokenType.BoolLiteral, Content = token.Content,
                        Line = token.Line, Character = token.Character, Source = token.Source };
                case Token.TokenType.SpecialChar:
                    switch(token.Content)
                    {
                        case "(": case "{": case "[": case "<":
                        case ")": case "}": case "]": case ">":
                        case ";": case ",": case ":": case "'":
                        case "/": case "#":
                            throw new SyntaxError("Jeton '" + token.Content + "' inattendu.", token.Line, token.Source);
                        case "$":
                        case "@":
                            return new ExpressionToken() { TkType = ExpressionToken.ExpressionTokenType.Modifier, Content = token.Content,
                                Line = token.Line, Character = token.Character, Source = token.Source };
                        default:
                            throw new NotImplementedException();

                    }
                case Token.TokenType.Comment:
                    return new ExpressionToken()
                    {
                        TkType = ExpressionToken.ExpressionTokenType.Comment,
                        Content = token.Content,
                        Line = token.Line,
                        Character = token.Character,
                        Source = token.Source
                    };


                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Effectue une recherche d'un opérateur d'affectation dans la liste de tokens donnée.
        /// Retourne vrai si l'opérateur a été trouvé.
        /// Position indique dans ce cas la position du caractère d'affectation dans la liste de jeton donnée.
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        static bool FindAffectation(List<ExpressionToken> tokens, out int position)
        {
            for(int i = 0; i < tokens.Count; i++)
            {
                if(tokens[i].IsBinaryOperator && tokens[i].Priority == 0)
                {
                    position = i;
                    return true;
                }
            }

            position = -1;
            return false;
        }
        /// <summary>
        /// Parse la liste de jeton pour en sortir un jeton évaluable.
        /// Sont évaluables :
        ///     - machin.truc
        ///     - 6 + machin.truc
        ///     - (6*9 + (6+machin.truc*3) * 3 + 2 == 0).truc(machin, chouette)
        /// En pratique : 
        ///     - Encapsule les jetons qu'il trouve dans des ExpressionGroup.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        static ExpressionToken ParseEvaluable(List<ExpressionToken> tokens)
        {
            List<ExpressionToken> newTokens = new List<ExpressionToken>();
            List<ExpressionToken> expressionGroupCandidate = new List<ExpressionToken>();

            // Supprime l'eventuel "return".
            if(tokens.Count > 0)
            {
                if(tokens[0].TkType == TType.Name && tokens[0].Content == Clank.Core.Model.Language.SemanticConstants.Return)
                {
                    newTokens.Add(tokens[0]);
                    tokens.RemoveAt(0);
                }
            }

            int affectationPosition;
            bool hasAffectation = FindAffectation(tokens, out affectationPosition);

            if (!hasAffectation)
            {
                // vrai : attend une opérande ou opérateur unaire
                // faux : attend un opérateur binaire.
                const int BIN_OP = 0x01;
                const int UNARY_OP = 0x02;
                const int OPERATOR = BIN_OP | UNARY_OP;
                const int OPERAND = 0x04;
                const int OPERAND_OR_UNARY = OPERAND | UNARY_OP;
                const int ALL = 0xFF;
                int expect = ALL;
                foreach (ExpressionToken token in tokens)
                {
                    // Fait des groupes ne contenant que des opérateurs et opérandes consécutives.
                    // Une fois le groupe constitué, appelle CreateExpressionGroup sur ces jetons afin
                    // de créer des groupes par priorité.

                    int type = token.TkType == TType.Operator ? (token.IsBinaryOperator ? BIN_OP : UNARY_OP) : OPERAND;
                    bool gotExpected = (type & expect) == type;

                    // Si on a ce qu'on attend, on continue le groupe.
                    if (gotExpected)
                        expressionGroupCandidate.Add(token);
                    else if (expressionGroupCandidate.Count != 0)
                    {
                        // Sinon, on termine le groupe précédent
                        newTokens.Add(CreateExpressionGroup(expressionGroupCandidate));
                        expressionGroupCandidate.Clear();
                        expressionGroupCandidate.Add(token);
                    }
                    else
                        throw new InvalidOperationException();

                    // Prochain jeton
                    // 1 + 2
                    // !true && !false && true
                    if (token.TkType == TType.Operator)
                    {
                        if (token.IsBinaryOperator)
                            expect = OPERAND_OR_UNARY;
                        else
                            expect = OPERAND;
                    }
                    else
                        expect = OPERATOR;
                }

                // Ajoute les restes.
                if (expressionGroupCandidate.Count != 0)
                {
                    newTokens.Add(CreateExpressionGroup(expressionGroupCandidate));
                }
                

                return new ExpressionToken() { TkType = ExpressionToken.ExpressionTokenType.List, ListTokens = newTokens, 
                    Line = newTokens.First().Line, Character = newTokens.First().Character, Source = newTokens.First().Source };
            }
            else
            {
                // Affectation
                return new ExpressionToken()
                {
                    TkType = TType.List,
                    Line = tokens[affectationPosition].Line,
                    Character = tokens[affectationPosition].Character,
                    ListTokens = new List<ExpressionToken>()
                    {
                        new ExpressionToken()
                        {
                            TkType = TType.ExpressionGroup,
                            Line = tokens[affectationPosition].Line,
                            Character = tokens[affectationPosition].Character,
                            ListTokens = new List<ExpressionToken>()
                            {
                                tokens[affectationPosition],
                                ParseEvaluable(tokens.GetRange(0, affectationPosition)),
                                ParseEvaluable(tokens.GetRange(affectationPosition+1, tokens.Count - affectationPosition - 1))
                            }

                        }
                    }
                };
            }
        }

        /// <summary>
        /// Retourne, à partir d'une expression ne contenant QUE DES OPERATEURS ET OPERANDES
        /// un jeton contenant l'intégralité de l'expression décomposée en opérations.
        /// 
        /// Ex d'entrée : 3+2*9+3 == 5 && !3*3 == 9
        /// 
        /// TODO : considérer les listes comme des opérateurs unaires préfixés. 
        ///     => ajout de IsOperator (vérif des conséquences)
        ///     => problème des bracquets (vérifier dans le parsing syntaxique si ce sont des opérateurs
        ///     ou des trucs de liste).
        /// </summary>
        /// <returns></returns>
        static ExpressionToken CreateExpressionGroup(List<ExpressionToken> tokens)
        {
            if (tokens.Count == 1)
            {
                // Désencapsulation.
                return tokens.First();
            }
            else if (tokens.Count == 2 && tokens.First().TkType == TType.Operator &&
                tokens.First().IsBinaryOperator)
            {
                // Gére les cas où des opérateurs binaires sont utilisés comme opérateurs unaires.
                // Ex: -5 => 0-6
                // Ex: +6 => 0+6
                // Rq: TODO : implémenter ça directement dans le parseur afin d'avoir des litéraux +/-
                ExpressionToken op = tokens[0];
                ExpressionToken operand = tokens[1];
                return new ExpressionToken()
                {
                    TkType = TType.ExpressionGroup,
                    Line = op.Line,
                    Source = op.Source,
                    Character = op.Character,
                    Operator = op,
                    Operands1 = new ExpressionToken() { TkType = TType.NumberLiteral, Content = "0" },
                    Operands2 = operand
                };
            }
            // Trouve l'opération ayant la priorité la plus faible.
            int lowestPriority = 10;
            int lowestPriorityOpIndex = 0;
            bool isUnary = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                ExpressionToken token = tokens[i];
                if (token.IsBinaryOperator && token.Priority <= lowestPriority)
                {
                    lowestPriority = token.Priority;
                    lowestPriorityOpIndex = i;
                    isUnary = false;
                }
                else if(token.IsUnaryOperator && token.Priority <= lowestPriority)
                {
                    lowestPriority = token.Priority;
                    lowestPriorityOpIndex = i;
                    isUnary = true;
                }
            }

            // Opérateur unaire : on remplace l'opérateur et l'opérande par une expression groupe et on recommence !
            if(isUnary)
            {
                List<ExpressionToken> cpy = new List<ExpressionToken>(); cpy.AddRange(tokens);
                cpy[lowestPriorityOpIndex] = new ExpressionToken()
                {
                    TkType = ExpressionToken.ExpressionTokenType.ExpressionGroup,
                    ListTokens = new List<ExpressionToken>()
                    {
                        tokens[lowestPriorityOpIndex], // opérateur
                        tokens[lowestPriorityOpIndex+1]  // opérande
                    },
                    Line = tokens[lowestPriorityOpIndex].Line,
                    Character = tokens[lowestPriorityOpIndex].Character,
                    Source = tokens[lowestPriorityOpIndex].Source
                };
                cpy.RemoveAt(lowestPriorityOpIndex + 1);
                return CreateExpressionGroup(cpy);
            }
            else
            {
                // S'il n'y a pas au moins 3 jetons, on a quelque chose de mal formé.
                if(tokens.Count < 3)
                {
                    throw new SyntaxError("L'expression est mal formée, il y a peut être un opérateur en trop.", tokens.First().Line, tokens.First().Source);
                }
                // Opérateur binaire : facile on crée l'expression en parsant ce qu'il y a à droite et à gauche.
                return new ExpressionToken()
                {
                    TkType = ExpressionToken.ExpressionTokenType.ExpressionGroup,
                    Line = tokens[lowestPriorityOpIndex].Line,
                    Character = tokens[lowestPriorityOpIndex].Character,
                    Source = tokens[lowestPriorityOpIndex].Source,
                    ListTokens = new List<ExpressionToken>()
                    {
                        tokens[lowestPriorityOpIndex],
                        CreateExpressionGroup(tokens.GetRange(0, lowestPriorityOpIndex)),
                        CreateExpressionGroup(tokens.GetRange(lowestPriorityOpIndex+1, tokens.Count - lowestPriorityOpIndex - 1))
                    }
                };
            }
        }


        /// <summary>
        /// Trie les jetons de façon d'une expression ne contenant QUE DES OPERATEURS ET OPERANDES
        /// à les avoir en notation polonaise inversée.
        /// 
        /// Ex d'entrée : 3+2*9+3 == 5 && 3*3 == 9
        /// </summary>
        /// <returns></returns>
        static List<ExpressionToken> ToNpi(List<ExpressionToken> tokens)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Transforme une liste de jetons sous la forme de NPI en une seule expression group contenant 
        /// opérandes et opérateur.
        /// 
        /// Ordre :  3 2 -    => 3 - 2
        /// Ordre dans la liste : 3 2 -
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        static ExpressionToken NpiToExpressionGroup(List<ExpressionToken> tokens)
        {
            Stack<ExpressionToken> operands = new Stack<ExpressionToken>();
            foreach(ExpressionToken token in tokens)
            {
                if(token.IsBinaryOperator)
                {
                    var op2 = operands.Pop();
                    var op1 = operands.Pop();
                    operands.Push(new ExpressionToken() { 
                        ListTokens = new List<ExpressionToken>() {
                           token,
                           op1,
                           op2
                        },
                        Line = token.Line,
                        Character = token.Character,
                        Source = token.Source
                    });
                }
                else if(token.IsUnaryOperator)
                {
                    operands.Push(new ExpressionToken() { 
                       ListTokens = new List<ExpressionToken>() {
                           token,
                           operands.Pop(),
                       },
                       Line = token.Line,
                       Character = token.Character,
                       Source = token.Source
                    });
                    operands.Push(token);
                }
                else // opérande
                {
                    operands.Push(token);
                }
            }
            return operands.Pop();
        }
        /// <summary>
        /// Parse une liste de token dans le but de transformer les jetons en ExpressionToken et de créer des groupes.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        static List<ExpressionToken> MakeGroups(List<Token> tokens)
        {
            int i = 0;
            int groupDepth = 0;
            ExpressionToken.ExpressionTokenType groupKindStarted = 0;
            bool shouldNotProcess;
            List<ExpressionToken> exprs = new List<ExpressionToken>();
            List<Token> currentExpr = new List<Token>();

            Func<Boolean> terminateGroup = delegate()
            {
                exprs.AddRange(currentExpr.Select(x => ToExpressionToken(x)).ToList());
                currentExpr.Clear();
                return true;
            };

            while(i < tokens.Count)
            {
                Token token = tokens[i];
                i++;

                #region Gestion du groupe
                // Si cette valeur vaut vrai, on est à l'intérieur d'une parenthèse / autre groupe, on le doit pas traiter les jetons
                // qu'on ajoute.
                // cad : on les ajoute juste à currentExpr.
                shouldNotProcess = groupKindStarted != 0;
                if(shouldNotProcess)
                {
                    bool endsGroup = false;
                    // Cela vérifie si le caractère ferme le groupe.
                    if (token.TkType == Token.TokenType.SpecialChar)
                    {
                        // Mise à jour de la profondeur.
                        #region Depth update
                        Func<string, ExpressionToken.ExpressionTokenType, int> multiplier = delegate(string content, ExpressionToken.ExpressionTokenType type)
                        {
                            switch(content)
                            {
                                case "(":
                                case ")":
                                    return ((type == ExpressionToken.ExpressionTokenType.List || type == ExpressionToken.ExpressionTokenType.FunctionCall)) ? 1 : 0;
                                case "[":
                                case "]":
                                    return ((type == ExpressionToken.ExpressionTokenType.ArrayType || type == ExpressionToken.ExpressionTokenType.BracketList)) ? 1 : 0;
                                case ">":
                                case "<":
                                    return ((type == ExpressionToken.ExpressionTokenType.GenericType || type == ExpressionToken.ExpressionTokenType.GenericParametersList)) ? 1 : 0;
                                case "{":
                                case "}":
                                    return (type == ExpressionToken.ExpressionTokenType.CodeBlock) ? 1 : 0;
                                default:
                                    return 0;
                            }
                        };

                        switch(token.Content)
                        {
                            case "(": case "[": case "{": case "<":
                                groupDepth = groupDepth + multiplier(token.Content, groupKindStarted);
                                break;
                            case ")": case "]": case "}": case ">":
                                groupDepth = groupDepth - multiplier(token.Content, groupKindStarted);
                                break;
                        }
                        #endregion

                        // Vérification de la fermeture du groupe.
                        #region Ends group ?
                        switch (groupKindStarted)
                        {
                            case ExpressionToken.ExpressionTokenType.List:
                            case ExpressionToken.ExpressionTokenType.FunctionCall:
                                if( token.Content == ")")
                                {
                                    if (groupDepth == 0)
                                    {
                                        endsGroup = true;
                                    }
                                }
                                break;
                            case  ExpressionToken.ExpressionTokenType.CodeBlock:
                                if (token.Content == "}")
                                {
                                    if (groupDepth == 0)
                                    {
                                        endsGroup = true;
                                    }
                                }
                                break;
                            case ExpressionToken.ExpressionTokenType.GenericParametersList:
                            case ExpressionToken.ExpressionTokenType.GenericType:
                                if (token.Content == ">")
                                {
                                    if (groupDepth == 0)
                                    {
                                        endsGroup = true;
                                    }
                                }
                                break;
                            case ExpressionToken.ExpressionTokenType.BracketList:
                            case ExpressionToken.ExpressionTokenType.ArrayType:
                                if (token.Content == "]")
                                {
                                    if(groupDepth == 0)
                                    {
                                        endsGroup = true;
                                    }
                                }
                                break;
                            default:
                                endsGroup = false;
                                break;
                        };
                        #endregion
                    }
                       
                    // Si le token est un token fermant le groupe, on continue l'exécution, 
                    // sinon, on l'ajoute à l'expression qui va contenir le groupe.
                    if (endsGroup)
                    {
                        #region endsgroup
                        ExpressionToken e = new ExpressionToken();
                        e.Line = token.Line;
                        e.Character = token.Character;
                        e.Source = token.Source;

                        e.TkType = groupKindStarted;         
                        e.ListTokens = Separate(MakeGroups(currentExpr), groupKindStarted == TType.CodeBlock);

                        // Gestion des appels de fonction / types génériques / types array
                        Dictionary<Tuple<TType, TType>, TType> patterns = new Dictionary<Tuple<TType,TType>,TType>()
                        {
                            // Précédent          Suivant                       Jeton résultant
                            {new Tuple<TType, TType>(TType.Name, TType.List) , TType.FunctionCall},
                            {new Tuple<TType, TType>(TType.Name, TType.GenericParametersList) , TType.GenericType},
                            {new Tuple<TType, TType>(TType.Name, TType.BracketList) , TType.ArrayType},
                            {new Tuple<TType, TType>(TType.ArrayType, TType.BracketList), TType.ArrayType},
                            {new Tuple<TType, TType>(TType.GenericType, TType.List) , TType.FunctionCall},
                            {new Tuple<TType, TType>(TType.Name, TType.CodeBlock) , TType.NamedCodeBlock},
                            {new Tuple<TType, TType>(TType.GenericType, TType.CodeBlock), TType.NamedGenericCodeBlock},
                            {new Tuple<TType, TType>(TType.FunctionCall, TType.CodeBlock) , TType.FunctionDeclaration},
                        };

                        // Vérifie on peut former un groupe à partir du jeton actuel et du précédent.
                        bool matchPattern = false;
                        Tuple<TType, TType> key = null;
                        if (exprs.Count != 0)
                        {
                            foreach (var pattern in patterns)
                            {
                                if (pattern.Key.Item1 == exprs.Last().TkType && pattern.Key.Item2 == groupKindStarted)
                                {
                                    matchPattern = true;
                                    key = pattern.Key;
                                }
                            }
                        }

                        if (matchPattern)
                        {
                            // Encapsule les jetons dans une liste;
                            List<ExpressionToken> capsule = new List<ExpressionToken>();
                            capsule.Add(exprs.Last()); // Identifier 
                            capsule.Add(new ExpressionToken()
                            {
                                TkType = ((patterns[key] == TType.NamedCodeBlock || patterns[key] == TType.FunctionDeclaration || patterns[key] == TType.NamedGenericCodeBlock) ?
                                            TType.InstructionList : TType.ArgList),
                                ListTokens = e.ListTokens,
                                Line = e.Line,
                                Character = e.Character
                            }); // Arguments encapsulés dans la liste

                            e.TkType = patterns[key];
                            e.ListTokens = capsule;
                            
                            // Supprime le jeton Name que l'on a mis dans le jeton de type function call.
                            exprs.RemoveAt(exprs.Count - 1);
                        }
                        else
                        {
                            /*if(e.ListTokens.Count == 1)
                            {
                                // Désencapsulation de la liste.
                                // e.ListTokens = e.ListTokens[0].ListTokens;
                            }
                            else if(e.ListTokens.Count > 1) // > 1
                            {
                                throw new InvalidOperationException();
                            }*/
                        }

                        exprs.Add(e);
                        currentExpr.Clear();
                        groupKindStarted = 0;
                        #endregion
                    }
                    else
                    {
                        currentExpr.Add(token);
                    }
                    continue;
                }
                #endregion

                // Gestion de tous les token n'étant pas des expressions.
                switch(token.TkType)
                {
                    #region Special Char
                    case Token.TokenType.SpecialChar:
                        ExpressionToken lst = new ExpressionToken();
                        switch(token.Content)
                        {
                            case "(":
                                groupKindStarted = ExpressionToken.ExpressionTokenType.List;
                                groupDepth++;
                                terminateGroup();
                                break;
                            case "[":
                                groupKindStarted = ExpressionToken.ExpressionTokenType.BracketList;
                                groupDepth++;
                                terminateGroup();
                                break;
                            case "<":
                                groupKindStarted = ExpressionToken.ExpressionTokenType.GenericParametersList;
                                groupDepth++;
                                terminateGroup();
                                break;
                            case  "{":
                                groupKindStarted = ExpressionToken.ExpressionTokenType.CodeBlock;
                                groupDepth++;
                                terminateGroup();
                                break;
                            case ")":
                                throw new SyntaxError("Jeton ')' inattendu. Une parenthèse a été mal fermée." + 
                                    " (attention : la ligne de l'erreur n'est peut être pas celle où il y a la parenthèse à supprimer.).", token.Line, token.Source);
                            case "]":
                                throw new SyntaxError("Jeton ']' inattendu. Un crochet a été mal fermé." +
                                    " (attention : la ligne de l'erreur n'est peut être pas celle où il y a le crochet à supprimer.).", token.Line, token.Source);
                            case ">":
                                throw new SyntaxError("Jeton '>' inattendu. Un chevron a été mal fermé." +
                                    " (attention : la ligne de l'erreur n'est peut être pas celle où il y a le chevron à supprimer.).", token.Line, token.Source);
                            case "}":
                                throw new SyntaxError("Jeton '}' inattendu. Une accolade a été mal fermée." +
                                    " (attention : la ligne de l'erreur n'est peut être pas celle où il y a l'accolade à supprimer.).", token.Line, token.Source);
                            case ";":
                                // Jetons précédent le ;
                                exprs.AddRange(currentExpr.Select(x => ToExpressionToken(x)).ToList());
                                currentExpr.Clear();

                                // Jeton de fin d'instruction.
                                lst = new ExpressionToken();
                                lst.TkType = ExpressionToken.ExpressionTokenType.EndOfInstruction;
                                lst.Content = ";";
                                lst.Line = token.Line;
                                lst.Character = token.Character;
                                lst.Source = token.Source;
                                exprs.Add(lst);
                                break;
                            case ",":
                                // Jetons précédent le ;
                                exprs.AddRange(currentExpr.Select(x => ToExpressionToken(x)).ToList());
                                currentExpr.Clear();

                                lst = new ExpressionToken();
                                lst.TkType = ExpressionToken.ExpressionTokenType.Separator;
                                lst.Content = ",";
                                lst.Line = token.Line;
                                lst.Character = token.Character;
                                lst.Source = token.Source;
                                exprs.Add(lst);
                                break;
                            default:
                                currentExpr.Add(token);

                                break;
                        }

                        break; // special char
                    #endregion
                    // Nom
                    case Token.TokenType.Name:
                        currentExpr.Add(token);
                        break;

                    // Littéral
                    case Token.TokenType.NumberLiteral:
                        currentExpr.Add(token);
                        break;

                    case Token.TokenType.Operator:
                        currentExpr.Add(token);
                        break;

                    case Token.TokenType.StringLiteral:
                        currentExpr.Add(token);
                        break;

                    case Token.TokenType.BoolLiteral:
                        currentExpr.Add(token);
                        break;

                    case Token.TokenType.Comment:
                        currentExpr.Add(token);
                        break;
                }
            }

            if(currentExpr.Count != 0)
            {
                terminateGroup();
            }
            return exprs;
        }
    }
}