using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Tokenizers
{
    /// <summary>
    /// Représente un jeton.
    /// </summary>
    public class Token
    {
        public enum TokenType
        {
            Name,
            Operator,
            SpecialChar,
            StringLiteral,
            NumberLiteral,
            BoolLiteral,
            Comment,
            Unresolved,
        }

        /// <summary>
        /// Contenu du jeton.
        /// </summary>
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// Type du jeton.
        /// </summary>
        public TokenType TkType
        {
            get;
            set;
        }

        /// <summary>
        /// Ligne ou a été rencontré le jeton.
        /// </summary>
        public int Line
        {
            get;
            set;
        }

        /// <summary>
        /// Index du dernier charactère du token dans la ligne d'où il a été extrait.
        /// </summary>
        public int Character
        {
            get;
            set;
        }

        /// <summary>
        /// Nom du fichier source duquel est extrait ce jeton.
        /// </summary>
        public string Source
        {
            get;
            set;
        }
        /// <summary>
        /// Crée une nouvelle instance de Token avec un Content vide et un Type par défaut.
        /// </summary>
        public Token() { }
        /// <summary>
        /// Crée une nouvelle instance de Token avec les paramètres passés en argument.
        /// </summary>
        public Token(string content, TokenType type, int line, int charIndex, string source)
        {
            Content = content;
            TkType = type;
            Line = line;
            Character = charIndex;
            Source = source;
        }

        public override string ToString()
        {
            return "<type=" + TkType.ToString() + "; content='" + Content + "'";
        }
    }
}
