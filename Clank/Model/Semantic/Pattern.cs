using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Token = Clank.Core.Tokenizers.ExpressionToken;
using TokenType = Clank.Core.Tokenizers.ExpressionToken.ExpressionTokenType;
namespace Clank.Core.Model.Semantic
{
    public class Pattern
    {
        public class MatchUnit
        {
            /// <summary>
            /// Jeton ayant matché l'expression.
            /// </summary>
            public Token MatchedToken { get; set; }
            /// <summary>
            /// Identifiant du jeton : permet de retrouver sa place dans le match.
            /// </summary>
            public string Identifier;
        }

        public class Match
        {
            public bool MatchPattern;
            /// <summary>
            /// Liste des match.
            /// </summary>
            public List<MatchUnit> MatchUnits;

            /// <summary>
            /// Retourne tous les matchs dont l'identifier est "identifier".
            /// </summary>
            public List<MatchUnit> FindByIdentifier(string identifier)
            {
                List<MatchUnit> units = new List<MatchUnit>();
                foreach (MatchUnit unit in MatchUnits)
                {
                    if (unit.Identifier == identifier)
                        units.Add(unit);
                }
                return units;
            }
        }
        /// <summary>
        /// Identificateur du Pattern : pour retrouver de quel morceau on parle lors du match.
        /// </summary>
        public string Identifier;
        /// <summary>
        /// Contenu que doit avoir le jeton pour que le match soit valide.
        /// Si null ou "" : on ne tient pas conte du content du jeton.
        /// </summary>
        public List<string> Content;
        /// <summary>
        /// Types possibles que doit avoir le jeton pour que le match soit valide.
        /// </summary>
        public List<TokenType> TkType;
        /// <summary>
        /// Prochain élément du pattern.
        /// </summary>
        public Pattern Next;
        /// <summary>
        /// Indique si cet élément du patern peut se répéter.
        /// </summary>
        public bool Repeats;
        /// <summary>
        /// Indique si cet élément du patern est optionel.
        /// </summary>
        public bool Optional;
        /// <summary>
        /// Crée une nouvelle instance de Pattern.
        /// </summary>
        public Pattern() { }


        /// <summary>
        /// Retourne vrai si les jetons contenus dans token matchent ce pattern.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public Match MatchPattern(List<Token> tokens)
        {
            Match m = new Match() { MatchPattern = false, MatchUnits = new List<MatchUnit>() };
            for (int i = 0; i < tokens.Count; i++)
            {
                Token token = tokens[i];
                if (MatchPattern(token))
                {
                    m.MatchUnits.Add(new MatchUnit() { Identifier = this.Identifier, MatchedToken = token });

                    Match nextMatch;
                    // Si répétition autorisée, on regarde si ce motif se répète.
                    if (Repeats)
                    {
                        nextMatch = MatchPattern(tokens.GetRange(i + 1, tokens.Count - i - 1));
                        if (nextMatch.MatchPattern)
                        {
                            m.MatchUnits.AddRange(nextMatch.MatchUnits);
                            m.MatchPattern = true;
                            break;
                        }
                        // Pas de répétition : on passe au suivant.
                    }

                    // Motif suivant
                    if (Next != null)
                    {
                        nextMatch = Next.MatchPattern(tokens.GetRange(i + 1, tokens.Count - i - 1));
                        if (nextMatch.MatchPattern)
                        {
                            m.MatchUnits.AddRange(nextMatch.MatchUnits);
                            m.MatchPattern = true;
                            break;
                        }
                    }
                    else
                    {
                        // Si on a fini, on veut que l'instruction ne contienne plus rien.
                        if (i == tokens.Count - 1)
                            m.MatchPattern = true;
                    }
                }
                else if (Optional && Next != null)
                {
                    return Next.MatchPattern(tokens.GetRange(i, tokens.Count - i));
                }
            }
            return m;
        }

        /// <summary>
        /// Retourne vrai si le jeton donné match avec ce bout de pattern.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool MatchPattern(Token token)
        {
            return (Content == null || Content.Count == 0 || Content.Contains(token.Content)) && (TkType.Contains(token.TkType));
        }

        #region Patterns
        /// <summary>
        /// Pattern de statement if/else/elsif/while.
        /// </summary>
        public static Pattern ConditionalStatementPattern = new Pattern()
        {
            Identifier = "Comment",
            Repeats = true,
            Content = null,
            TkType = new List<TokenType>() { TokenType.Comment },
            Optional = true,
            Next = new Pattern()
            {
                Identifier = "Statement",
                Repeats = false,
                Content = new List<string>() { "if", "else", "elsif", "while" },
                TkType = new List<TokenType>() { TokenType.ConditionalStatement },
                Optional = false,
                Next = new Pattern()
                {
                    Identifier = "Condition",
                    Repeats = false,
                    Optional = true,
                    TkType = new List<TokenType>() { TokenType.List },
                    Next = new Pattern()
                    {
                        Identifier = "Code",
                        Repeats = false,
                        Optional = false,
                        TkType = new List<TokenType>() { TokenType.CodeBlock },
                        Next = null
                    }
                }
            }
        };
        /// <summary>
        /// Pattern de déclaration de fonction.
        /// </summary>
        public static Pattern FunctionDeclarationPattern = new Pattern()
        {
            Identifier = "Comment",
            Repeats = true,
            Content = null,
            TkType = new List<TokenType>() { TokenType.Comment },
            Optional = true,
            Next = new Pattern()
            {
                Identifier = "Modifiers",
                Repeats = true,
                Content = new List<string>() { Language.SemanticConstants.Public, Language.SemanticConstants.Constructor, Language.SemanticConstants.Static },
                TkType = new List<TokenType>() { TokenType.Name },
                Optional = true,
                Next = new Pattern()
                {
                    Identifier = "Type",
                    Repeats = false,
                    Content = null,
                    TkType = new List<TokenType>() { TokenType.Name, TokenType.GenericType, TokenType.ArrayType }, // plusieurs types autorisés.
                    Next = new Pattern()
                    {
                        Identifier = "Declaration",
                        Repeats = false,
                        Content = null,
                        TkType = new List<TokenType>() { TokenType.FunctionDeclaration },
                        Next = null
                    }
                }
            }
        };
        /// <summary>
        /// Pattern de déclaration de variable.
        /// </summary>
        public static Pattern VariableDeclarationPattern = new Pattern()
        {
            Identifier = "Comment",
            Repeats = true,
            Content = null,
            TkType = new List<TokenType>() { TokenType.Comment },
            Optional = true,
            Next = new Pattern()
            {
                Identifier = "Modifiers",
                Repeats = false,
                Content = new List<string>() { Language.SemanticConstants.Public },
                TkType = new List<TokenType>() { TokenType.Name },
                Optional = true,
                Next = new Pattern()
                {
                    Identifier = "Type",
                    Repeats = false,
                    Content = null,
                    TkType = new List<TokenType>() { TokenType.Name, TokenType.ArrayType, TokenType.GenericType },
                    Next = new Pattern()
                    {
                        Identifier = "Name",
                        Repeats = false,
                        Content = null,
                        TkType = new List<TokenType>() { TokenType.Name },
                        Next = null
                    }
                }
            }
        };
        /// <summary>
        /// Pattern de déclaration de block de code nommé.
        /// </summary>
        public static Pattern NamedCodeBlockDeclarationPattern = new Pattern()
        {
            Identifier = "Comment",
            Repeats = true,
            Content = null,
            TkType = new List<TokenType>() { TokenType.Comment },
            Optional = true,
            Next = new Pattern()
            {
                Identifier = "Block",
                Repeats = false,
                Content = null,
                TkType = new List<TokenType>() { TokenType.NamedCodeBlock },
                Optional = false,
                Next = null,
                
            }
        };
        /// <summary>
        /// Pattern de déclaration de classe.
        /// </summary>
        public static Pattern ClassDeclarationPattern = new Pattern()
        {
            Identifier = "Comment",
            Repeats = true,
            Content = null,
            TkType = new List<TokenType>() { TokenType.Comment },
            Optional = true,
            Next = new Pattern()
            {
                Identifier = "Modifiers",
                Repeats = true,
                Content = new List<string>() { Language.SemanticConstants.Public, Language.SemanticConstants.IsSerializable },
                TkType = new List<TokenType>() { TokenType.Name },
                Optional = true,
                Next = new Pattern()
                {
                    Repeats = false,
                    Content = new List<string>() { Language.SemanticConstants.JsonArray, Language.SemanticConstants.JsonObject },
                    TkType = new List<TokenType>() { TokenType.Name },
                    Optional = true,
                    Identifier = "JsonModifiers",
                    Next = new Pattern()
                    {
                        Identifier = "StructKW",
                        Repeats = false,
                        Content = new List<string>() { Language.SemanticConstants.Class },
                        TkType = new List<TokenType>() { TokenType.Name },
                        Next = new Pattern()
                        {
                            Identifier = "Block",
                            Repeats = false,
                            Content = null,
                            TkType = new List<TokenType>() { TokenType.NamedCodeBlock },
                            Next = null,
                        }
                    }
                }
            }
        };
        /// <summary>
        /// Pattern de déclaration d'enum.
        /// </summary>
        public static Pattern EnumDeclarationPattern = new Pattern()
        {
            Identifier = "Comment",
            Repeats = true,
            Content = null,
            TkType = new List<TokenType>() { TokenType.Comment },
            Optional = true,
            Next = new Pattern()
            {
                Identifier = "Modifiers",
                Repeats = true,
                Content = new List<string>() { Language.SemanticConstants.Public },
                TkType = new List<TokenType>() { TokenType.Name },
                Optional = true,

                Next = new Pattern()
                {
                    Identifier = "StructKW",
                    Repeats = false,
                    Content = new List<string>() { Language.SemanticConstants.Enum },
                    TkType = new List<TokenType>() { TokenType.Name },
                    Next = new Pattern()
                    {
                        Identifier = "Block",
                        Repeats = false,
                        Content = null,
                        TkType = new List<TokenType>() { TokenType.NamedCodeBlock },
                        Next = null,
                    }
                }
            }

        };
        /// <summary>
        /// Pattern de déclaration de classe générique.
        /// </summary>
        public static Pattern GenericClassDeclarationPattern = new Pattern()
        {
            Identifier = "Comment",
            Repeats = true,
            Content = null,
            TkType = new List<TokenType>() { TokenType.Comment },
            Optional = true,
            Next = new Pattern()
            {
                Identifier = "Modifiers",
                Repeats = true,
                Content = new List<string>() { Language.SemanticConstants.Public, Language.SemanticConstants.IsSerializable },
                TkType = new List<TokenType>() { TokenType.Name },
                Optional = true,
                Next = new Pattern()
                {
                    Identifier = "JsonModifiers",
                    Repeats = false,
                    Content = new List<string>() { Language.SemanticConstants.JsonArray, Language.SemanticConstants.JsonObject },
                    TkType = new List<TokenType>() { TokenType.Name },
                    Optional = true,
                    Next = new Pattern()
                    {
                        Identifier = "StructKW",
                        Repeats = false,
                        Content = new List<string>() { Language.SemanticConstants.Class },
                        TkType = new List<TokenType>() { TokenType.Name },
                        Next = new Pattern()
                        {
                            Identifier = "Block",
                            Repeats = false,
                            Content = null,
                            TkType = new List<TokenType>() { TokenType.NamedGenericCodeBlock },
                            Next = null,
                        }
                    }
                }
            }
        };

        /// <summary>
        /// Pattern d'affectation de variable.
        /// </summary>
        public static Pattern VariableAffectationPattern = new Pattern()
        {
            Identifier = "Comment",
            Repeats = true,
            Content = null,
            TkType = new List<TokenType>() { TokenType.Comment },
            Optional = true,
            Next = new Pattern()
            {
                Identifier = "Affectation",
                Repeats = true,
                Content = null,
                TkType = new List<TokenType>() { TokenType.ExpressionGroup },
                Next = null,
            }
        };

        /// <summary>
        /// Pattern de return.
        /// </summary>
        public static Pattern ReturnPattern = new Pattern()
        {
            Identifier = "Comment",
            Repeats = true,
            Content = null,
            TkType = new List<TokenType>() { TokenType.Comment },
            Optional = true,
            Next = new Pattern()
            {
                Identifier = "Return",
                Repeats = false,
                Content = new List<string>() { Language.SemanticConstants.Return },
                TkType = new List<TokenType>() { TokenType.Name },
                Next = new Pattern()
                {
                    Identifier = "Evaluable",
                    Repeats = false,
                    Content = null,
                    TkType = new List<TokenType>() {  TokenType.Name, TokenType.New, TokenType.FunctionCall, TokenType.ExpressionGroup, TokenType.List, TokenType.BoolLiteral,
                        TokenType.NumberLiteral, TokenType.StringLiteral, TokenType.Access }
                }
            }
        };
        #endregion
    }
}
