using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Semantic
{

    public class DocumentationComment
    {
        /// <summary>
        /// Représente les différentes sections du commentaire.
        /// </summary>
        Dictionary<string, string> m_sections;


        /// <summary>
        /// Obtient le commentaire contenu dans la section donnée.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public string GetSection(string sectionName)
        {
            if (m_sections.ContainsKey(sectionName))
                return m_sections[sectionName];
            return "";
        }

        /// <summary>
        /// Obtient toutes les sections.
        /// </summary>
        public Dictionary<string, string> GetSections()
        {
            return m_sections;
        }

        /// <summary>
        /// Crée un nouveau commentaire de doc à partir de sections.
        /// </summary>
        /// <param name="sections"></param>
        public DocumentationComment(Dictionary<string, string> sections)
        {
            m_sections = sections;
        }
    }
    /// <summary>
    /// Permet le parsing de commentaires permettant la documentation de fonctions / classes / autres 
    /// sur un modèle ressemblant à java doc.
    /// </summary>
    public static class DocumentationParser
    {
        public const string DefaultSection = "brief";

        /// <summary>
        /// Parse un commentaire et retourne une instance de DocumentationComment.
        /// </summary>
        public static DocumentationComment Parse(string str)
        {
            Dictionary<string, string> sections = new Dictionary<string, string>();
            str = string.Join("\n", str.Split('\n').Select((string s) =>
            {
                return s.Trim('\r', '*', '\t', ' ');
            }));
            try
            {
                List<char> chars = new List<char>();
                bool isParsingSection = false;
                string sectionName = DefaultSection;
                foreach (char chr in str)
                {
                    // Si on est entrain de parser une section et qu'on trouve un espace
                    if (isParsingSection && chr == ' ')
                    {
                        if (chr == ' ')
                        {
                            sectionName = new string(chars.ToArray());
                            chars.Clear();
                            isParsingSection = false;
                        }
                    }
                    else if (!isParsingSection && chr == '@')
                    {
                        string currentSectionStr = new string(chars.ToArray());
                        chars.Clear();
                        if (!sections.ContainsKey(sectionName))
                            sections.Add(sectionName, currentSectionStr);
                        else
                            sections[sectionName] += currentSectionStr;
                        isParsingSection = true;
                    }
                    else
                        chars.Add(chr);
                }

                // Ajoute les derniers caractères à la dernière section.
                if (chars.Count != 0)
                {
                    sections.Add(sectionName, new string(chars.ToArray()));
                }

                return new DocumentationComment(sections);
            }
            catch(Exception e)
            {
                sections.Clear();
                sections.Add(DefaultSection, str);
                return new DocumentationComment(sections);
            }
        }
    }
}
