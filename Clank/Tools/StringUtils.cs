using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Tools
{
    /// <summary>
    /// Contient des fonctions utilitaires pour opérer sur des string.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Découpe un string trop long en plusieurs lignes.
        /// </summary>
        public static string[] CutInLines(string s, int maxCharPerLine)
        {
            int charCount = 0;
            List<string> lines = new List<string>();
            List<string> currWords = new List<string>();
            string[] words = s.Split(' ');
            foreach(string word in words)
            {
                // Crée la ligne.
                if(charCount + word.Length > maxCharPerLine)
                {
                    lines.Add(Join(currWords, " "));

                    currWords.Clear();
                    charCount = 0;
                }

                // Mise à jour du nombre de chars
                charCount += word.Length;
                currWords.Add(word);
                
            }

            // Derniers éléments.
            if(currWords.Count != 0)
                lines.Add(Join(currWords, " "));

            return lines.ToArray();
        }

        /// <summary>
        /// Joint une série de strings à l'aide d'un séparateur.
        /// </summary>
        /// <returns></returns>
        public static string Join(IEnumerable<string> strings, string separator)
        {
            StringBuilder builder = new StringBuilder();
            foreach(string str in strings)
            {
                builder.Append(str +  (str == strings.Last() ? "" : separator));
            }
            return builder.ToString();
        }
        /// <summary>
        /// Retourne une version du string contenant les numéros de ligne.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string PutLineNumbers(string s)
        {
            string[] lines = s.Split('\n');
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                builder.AppendLine((i + 1).ToString() + ".\t" + lines[i]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Indentes toutes les lignes du string s passé en paramètre.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="spaces"></param>
        /// <returns></returns>
        public static string Indent(string s, int spaces=1)
        {
            string[] lines = s.Split('\n');
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim('\n', '\r', '\t', ' ').Length == 0)
                    continue;
                if(i != lines.Length - 1)
                    builder.Append("\t" + lines[i] + "\n");
                else
                    builder.Append("\t" + lines[i]);
            }
            // Dégueu.
            if(spaces > 1)
            {
                return Indent(builder.ToString(), spaces - 1);
            }
            return builder.ToString();
        }
        /// <summary>
        /// Trouve le string le plus ressemblant à str parmi les strings contenus dans strings.
        /// Seuls les strings dont le degré de ressemblance excède threshold (0-100) sont retenus.
        /// 
        /// Retourne true si un string convenable a été trouvé.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strings"></param>
        /// <param name="threshold"></param>
        /// <param name="nearest"></param>
        /// <returns></returns>
        public static bool FindNearest(string str, IEnumerable<string> strings, int threshold, out string nearest)
        {
            int maxSim = 0;
            nearest = "";
            foreach(string strCmp in strings)
            {
                int sim = GetSimilarity(str, strCmp);
                if(sim > threshold)
                {
                    maxSim = sim;
                    nearest = strCmp;
                }
            }
            return maxSim > 0;
        }

        /// <summary>
        /// Retourne le degré de ressemblance en pourcentage [0-100] d'un string a à un string b, en utilisant l'algorithme de 
        /// Levenshtein.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int GetSimilarity(string a, string b)
        {
            int[,] matrix = new int[a.Length + 1, b.Length + 1];
            for(int i = 0; i < a.Length+1;i++)
            {
                matrix[i, 0] = i;
            }

            for (int j = 0; j < b.Length + 1; j++)
            {
                matrix[0, j] = j;
            }

            // Calcul
            for(int x = 1; x < b.Length+1; x++)
            {
                for(int y = 1; y < a.Length+1;y++)
                {
                    int cout = 0;
                    if (a[y - 1] != b[x - 1])
                        cout++;
                    matrix[y, x] = Math.Min(cout + matrix[y - 1, x - 1], Math.Min(matrix[y - 1,x] + 1, matrix[y,x - 1] + 1));
                }
            }

            return 100 - ((100 * matrix[a.Length, b.Length]) / b.Length);
        }
    }
}
