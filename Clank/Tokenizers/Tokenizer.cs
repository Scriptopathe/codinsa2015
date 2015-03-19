using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Tokenizers
{
    /// <summary>
    /// Représente le parseur syntaxique utilisé pour la décomposition du fichier en tokens.
    /// </summary>
    public class Tokenizer
    {
        /// <summary>
        /// Tokenize le script passé en paramètre.
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static List<Token> Parse(string script, Dictionary<int, Generation.Preprocessor.Preprocessor.LineInfo> lineMappings)
        {
            List<Token> tokens = new List<Token>(); // jetons construits à partir du script
            List<Char> currentWord = new List<char>(); // mot en cours de "traitement"
            bool isParsingString = false;
            bool isParsingNumber = false;
            bool isParsingName = false;
            bool isParsingOperator = false;
            bool isParsingComment = false;
            bool isEscapedChar =false;
            int _line = 0;
            int charIndex = 0;
            int line;
            string source;
            
            script = script + " ";
            Lexer lex = new Lexer(script);
            while(!lex.IsOEF())
            {
                char chr = lex.GetCurrentChar();
                _line = lex.GetLine();
                charIndex = lex.GetCharacterIndex();

                // Récupère la ligne / source correcte depuis le mapping entre ligne du
                // script final et ligne des scripts included.
                line = lineMappings[_line].Line;
                source = lineMappings[_line].Source;

                // Début de commentaire.
                bool parseComments = !isParsingString;
                string comment = "";
                bool commentFound = parseComments && lex.MatchComment(out comment, true);
                if (commentFound)
                {
                    #region Comment
                    Token c = new Token(comment, Token.TokenType.Comment, line, charIndex, source);
                    tokens.Add(c);
                    #endregion

                    continue;
                }
                else
                {
                    // Si c'est un chiffre.
                    #region Digit
                    if (char.IsDigit(chr) || (isParsingNumber && chr == '.'))
                    {
                        if (isParsingName || isParsingString || isParsingNumber)
                        {
                            // On continue le mot / string
                            currentWord.Add(chr);
                        }
                        else if (isParsingOperator)
                        {
                            // On a fini de parser l'opérateur, on peut le terminer et parser un nouveau mot (number).
                            Token newToken = new Token(new String(currentWord.ToArray()), Token.TokenType.Operator, line, charIndex, source);
                            currentWord.Clear();
                            currentWord.Add(chr);
                            isParsingNumber = true;
                            isParsingOperator = false;
                            tokens.Add(newToken);
                        }
                        else
                        {
                            isParsingNumber = true;
                            currentWord.Add(chr);
                        }
                    }
                    #endregion
                    // Lettre
                    #region Letter
                    else if (char.IsLetter(chr) || chr == '_')
                    {
                        if (isParsingName || isParsingString || isParsingNumber)
                        {
                            // On continue le mot / string
                            currentWord.Add(chr);
                        }
                        else if (isParsingOperator)
                        {
                            // On a fini de parser l'opérateur, on peut le terminer et parser un nouveau mot (bame).
                            Token newToken = new Token(new String(currentWord.ToArray()), Token.TokenType.Operator, line, charIndex, source);
                            currentWord.Clear();
                            currentWord.Add(chr);
                            isParsingName = true;
                            isParsingOperator = false;
                            tokens.Add(newToken);
                        }
                        else
                        {
                            isParsingName = true;
                            currentWord.Add(chr);
                        }
                    }
                    #endregion
                    // Opérateur
                    #region Operator
                    else if (isOperatorChar(chr, currentWord, isParsingOperator))
                    {
                        // Nom / Number / Special char : l'opérateur les termine.
                        if (isParsingName)
                        {
                            Token newToken = new Token(new String(currentWord.ToArray()), Token.TokenType.Name, line, charIndex, source);
                            currentWord.Clear();
                            currentWord.Add(chr);
                            isParsingName = false;
                            isParsingOperator = true;
                            tokens.Add(newToken);
                        }
                        else if (isParsingNumber)
                        {
                            Token newToken = new Token(new String(currentWord.ToArray()), Token.TokenType.NumberLiteral, line, charIndex, source);
                            currentWord.Clear();
                            currentWord.Add(chr);
                            isParsingNumber = false;
                            isParsingOperator = true;
                            tokens.Add(newToken);
                        }
                        else if (isParsingString || isParsingOperator)
                        {
                            // Il ne se passe rien de spécial.
                            currentWord.Add(chr);
                        }
                        else
                        {
                            isParsingOperator = true;
                            currentWord.Add(chr);
                        }
                    }
                    #endregion
                    // Caractère spécial.
                    #region Special Char
                    else if (isSpecialChar(chr))
                    {
                        bool isUnresolved = chr == '>' || chr == '<';
                        Token.TokenType type = isUnresolved ? Token.TokenType.Unresolved : Token.TokenType.SpecialChar;
                        // Nom / Number / Special char : l'opérateur les termine.
                        if (isParsingName)
                        {
                            Token newToken = new Token(new String(currentWord.ToArray()), Token.TokenType.Name, line, charIndex, source);
                            currentWord.Clear();
                            isParsingName = false;
                            tokens.Add(newToken);
                            if (!isIgnoredChar(chr))
                                tokens.Add(new Token(chr.ToString(), type, line, charIndex, source));
                        }
                        else if (isParsingNumber)
                        {
                            Token newToken = new Token(new String(currentWord.ToArray()), Token.TokenType.NumberLiteral, line, charIndex, source);
                            currentWord.Clear();
                            isParsingNumber = false;
                            tokens.Add(newToken);
                            if (!isIgnoredChar(chr))
                                tokens.Add(new Token(chr.ToString(), type, line, charIndex, source));
                        }
                        else if (isParsingOperator)
                        {
                            Token newToken = new Token(new String(currentWord.ToArray()), Token.TokenType.Operator, line, charIndex, source);
                            currentWord.Clear();
                            isParsingOperator = false;
                            tokens.Add(newToken);
                            if (!isIgnoredChar(chr))
                                tokens.Add(new Token(chr.ToString(), type, line, charIndex, source));
                        }
                        else if (isParsingString)
                        {
                            // Il ne se passe rien de spécial.
                            currentWord.Add(chr);
                        }
                        else if (!isIgnoredChar(chr)) // espaces, fin de lignes etc...
                        {
                            // A ce point, currentWord doit être vide.
                            Token newToken = new Token(chr.ToString(), type, line, charIndex, source);

                            tokens.Add(newToken);
                        }
                    }
                    #endregion
                    // Début de string.
                    #region String
                    else if (chr == '"' || chr == '`')
                    {
                        // Nom / Number / Special char : l'opérateur les termine.
                        if (isParsingName)
                        {
                            Token newToken = new Token(new String(currentWord.ToArray()), Token.TokenType.Name, line, charIndex, source);
                            currentWord.Clear();
                            isParsingName = false;
                            isParsingString = true;
                            tokens.Add(newToken);
                        }
                        else if (isParsingNumber)
                        {
                            Token newToken = new Token(new String(currentWord.ToArray()), Token.TokenType.NumberLiteral, line, charIndex, source);
                            currentWord.Clear();
                            isParsingNumber = false;
                            isParsingString = true;
                            tokens.Add(newToken);
                        }
                        else if (isParsingOperator)
                        {
                            Token newToken = new Token(new String(currentWord.ToArray()), Token.TokenType.Operator, line, charIndex, source);
                            currentWord.Clear();
                            isParsingOperator = false;
                            isParsingString = true;
                            tokens.Add(newToken);
                        }
                        else if (isParsingString) // termine le string s'il a déjà commencé.
                        {
                            // S'il est escapé : on l'ajoute au string.
                            if (isEscapedChar)
                            {
                                currentWord.Add(chr);
                            }
                            else
                            {
                                Token newToken = new Token(new String(currentWord.ToArray()), Token.TokenType.StringLiteral, line, charIndex, source);
                                currentWord.Clear();
                                isParsingString = false;
                                tokens.Add(newToken);
                            }
                        }
                        else
                        {
                            isParsingString = true;
                        }
                    }

                    // Escape
                    if (isParsingString && chr == '\\')
                        isEscapedChar = true;
                    else
                        isEscapedChar = false;
                    #endregion
                }

                lex.Next();
            }

            return SolveUnresolvedTokens(tokens);
        }
        
        /// <summary>
        /// Donne le type correct aux jetons de type 'unresolved'.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        static List<Token> SolveUnresolvedTokens(List<Token> tokens)
        {
            int brackedDepth = 0;
            List<Token> currentGroup = new List<Token>();


            Func<Token.TokenType, bool> markGroupAs = delegate(Token.TokenType type)
            {
                foreach(Token t in currentGroup)
                {
                    if(t.TkType == Token.TokenType.Unresolved)
                    {
                        t.TkType = type;
                    }
                }
                currentGroup.Clear();
                return true;
            };

            foreach(Token token in tokens)
            {
                // Mot clef new :
                // Litéraux booléens.
                if(token.TkType == Token.TokenType.Name && (token.Content == "true" || token.Content == "false"))
                {
                    token.TkType = Token.TokenType.BoolLiteral;
                }

                // Jetons invalides pour une déclaration de générique :
                if(token.TkType == Token.TokenType.Operator || token.TkType == Token.TokenType.StringLiteral || 
                   token.TkType == Token.TokenType.NumberLiteral || 
                   (token.TkType == Token.TokenType.SpecialChar && !(new List<string>() { "[", "]", ",", "." }.Contains(token.Content))))
                {
                    markGroupAs(Token.TokenType.Operator);
                    continue;
                }

                if(token.TkType == Token.TokenType.Unresolved)
                {
                    if(token.Content == "<")
                    {
                        brackedDepth++;
                    }
                    else if(token.Content == ">")
                    {
                        // Opérateur ">" à coup sûr.
                        if (brackedDepth == 0)
                            token.TkType = Token.TokenType.Operator;
                        else
                        {
                            brackedDepth--;
                            if (brackedDepth == 0)
                            {
                                // Examine 
                                token.TkType = Token.TokenType.SpecialChar;
                                markGroupAs(Token.TokenType.SpecialChar);
                            }
                        }
                    }
                }

                if(brackedDepth > 0)
                    currentGroup.Add(token);
            }

            return tokens;
        }

        static List<char> ignoredChars = new List<char>() {
            ' ', '\t', '\n'
        };

        static List<char> specialchars = new List<char>() {
            ',', ';', ':', '@', '$', '/', ' ', '\t', '\n', '{', '}', '(', ')', '[', ']', '~', '\'', '°', '@', '#', '<', '>'
        };

        /// <summary>
        /// Retourne vrai si le caractère passé en paramètre est un caractère ignoré.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        static bool isIgnoredChar(char chr)
        {
            return ignoredChars.Contains(chr);
        }
        /// <summary>
        /// Retourne vrai si le caractère passé en paramètre est un caractère spécial.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        static bool isSpecialChar(char chr)
        {
            return specialchars.Contains(chr);
        }

        static List<string> operators = new List<string> {
            "<=", ">=", "=", "==", "!=", "!", "&", "&&", "|", "||", "^", "*", "/", "+", "-", "*=", "+=", "-=", "/=", "%", "%=",
            "?", "."
        };
        /// <summary>
        /// Retourne vrai si le caractère chr est un opérateur connaissant la liste des caractères précédents.
        /// </summary>
        static bool isOperatorChar(char chr, List<char> previousChrs, bool isParsingOperator)
        {
            if (isParsingOperator)
            {
                if (previousChrs.Count >= 2)
                    return false;
                else if (previousChrs.Count == 1)
                {
                    return operators.Contains(previousChrs.First().ToString() + chr.ToString());
                }
                else if (previousChrs.Count == 0)
                {
                    return operators.Contains(chr.ToString());
                }
            }
            else
            {
                return operators.Contains(chr.ToString());
            }
            return true;
        }
    }
}
