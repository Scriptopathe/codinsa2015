using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Tokenizers
{

    /// <summary>
    /// Classe permettant de faciliter le parsing de strings.
    /// </summary>
    public class Lexer
    {
        string m_string;
        int m_position;
        int m_line;
        int m_character;
        public enum GroupType
        {
            SingleLineComment   = 0x01,
            MultilineComment    = 0x02,
            String              = 0x04
        }

        public char EscapeChar { get; set; }
        public string StringDelimiter { get; set; }
        public string SingleLineComment { get; set; }
        public string MultilineCommentStart { get; set; }
        public string MultilineCommentEnd { get; set; }
        
        /// <summary>
        /// Crée une nouvelle instance du lexer pour le string donné.
        /// </summary>
        public Lexer(string str)
        {
            m_string = str;
            SingleLineComment = "//";
            MultilineCommentStart = "/*";
            MultilineCommentEnd = "*/";
            StringDelimiter = "\"";
            EscapeChar = '\\';
        }

        /// <summary>
        /// Obtient la colonne de caractère actuelle.
        /// </summary>
        /// <returns></returns>
        public int GetCharacterIndex()
        {
            return m_character;
        }

        /// <summary>
        /// Obtient la ligne actuelle.
        /// </summary>
        /// <returns></returns>
        public int GetLine()
        {
            return m_line;
        }

        /// <summary>
        /// Obtient une valeur indiquant si on se trouve à la fin du fichier.
        /// </summary>
        /// <returns></returns>
        public bool IsOEF()
        {
            return m_position >= m_string.Length;
        }

        /// <summary>
        /// Obtient le caractère actuel.
        /// </summary>
        /// <returns></returns>
        public char GetCurrentChar()
        {
            return Get(m_position);
        }

        /// <summary>
        /// Avance d'un caractère la position du lexer.
        /// </summary>
        public void Next()
        {
            m_character++;
            if(GetCurrentChar() == '\n')
            {
                m_line++;
                m_character = 0;
            }
            m_position++;
        }
        /// <summary>
        /// Avance d'un nombre donné de caractères la position du lexer.
        /// </summary>
        public void Next(int n)
        {
            for(int i = 0; i < n; i++)
            {
                Next();
            }
        }
        /// <summary>
        /// Obtient le caractère à la position donnée en effectuant une vérification des bords.
        /// Retourne un caractère nul si la fin du string est atteinte.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        char Get(int position)
        {
            if (position >= m_string.Length)
                return '\0';

            return m_string[position];
        }

        /// <summary>
        /// Obtient une valeur indiquant si les prochains caractères matchent le pattern donné.
        /// <param name="updateStreamPosition">Si true, la position du flux est mise à jour pour être la position de fin du pattern.</param>
        /// </summary>
        public bool MatchNext(string pattern, bool updateStreamPosition=true)
        {
            for(int i = 0; i < pattern.Length; i++)
            {
                char current = Get(m_position + i);
                if (pattern[i] != current)
                    return false;
            }
            if (updateStreamPosition)
                Next(pattern.Length);
            return true;
        }
        /// <summary>
        /// Obtient une valeur indiquant si les prochains caractères matchent le pattern donné.
        /// </summary>
        public bool MatchNext(string pattern, int position)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                char current = Get(position + i);
                if (pattern[i] != current)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Obtient une valeur indiquant si un string a pu être parsé.
        /// Le string parsé sera copié dans content le cas échéant.
        /// </summary>
        /// <param name="updateStreamPosition">Si true, la position du flux est mise à jour pour être la position de fin du pattern.</param>
        /// <returns></returns>
        public bool MatchString(out string content, bool updateStreamPosition=true)
        {
            bool value =  MatchGroup(StringDelimiter, StringDelimiter, EscapeChar, out content);
            if (value && updateStreamPosition)
                Next(StringDelimiter.Length * 2 + content.Length);

            return value;
        }

        /// <summary>
        /// Obtient une valeur indiquant si un commentaire a pu être parsé.
        /// Le string parsé sera copié dans content le cas échéant.
        /// </summary>
        /// <param name="updateStreamPosition">Si true, la position du flux est mise à jour pour être la position de fin du pattern.</param>
        /// <returns></returns>
        public bool MatchComment(out string content, bool updateStreamPosition = true)
        {
            bool value = MatchGroup(SingleLineComment, "\n", '\0', out content);
            if (value)
            {
                if(updateStreamPosition)
                    Next(SingleLineComment.Length + content.Length + 1);
                return true;
            }

            value = MatchGroup(MultilineCommentStart, MultilineCommentEnd, '\0', out content);
            if (value && updateStreamPosition)
                Next(MultilineCommentStart.Length + content.Length + MultilineCommentEnd.Length);

            return value;
        }


        /// <summary>
        /// Obtient une valeur indiquant si un qui démarre du jeton 
        /// </summary>
        /// <param name="groupStart">Chaine marquant le début du groupe.</param>
        /// <param name="groupEnd">Chaine marquant la fin du groupe.</param>
        /// <param name="escapeCharacter">Caractère pouvant échapper une fin de groupe.</param>
        /// <param name="content">Contenu récupéré à l'intérieur du groupe.</param>
        /// <returns></returns>
        public bool MatchGroup(string groupStart, string groupEnd, char escapeCharacter, out string content)
        {
            content = "";
            // Vérifie qu'on remplisse le critère d'ouverture.
            if(!MatchNext(groupStart, false))
                return false;

            bool escape = false;
            int firstPos = m_position + groupStart.Length;
            int pos = m_position + groupStart.Length;
            List<char> chars = new List<char>();
            while(Get(pos) != '\0')
            {
                char next = Get(pos);
                escape = false;
                // Si on trouve un caractère d'échappement : on le note.
                if(next == escapeCharacter && escapeCharacter != '\0')
                {
                    chars.Add(next);
                    escape = true;
                }
                // Si on trouve une marque de fermeture et qu'on a pas eu d'escape char avant :
                else if(MatchNext(groupEnd, pos) && !escape)
                {
                    // On récupère le contenu du groupe.
                    content = new string(chars.ToArray());
                    return true;
                }
                else
                {
                    chars.Add(next);
                }
                pos++;
            }

            return false;
        }
    }
}
