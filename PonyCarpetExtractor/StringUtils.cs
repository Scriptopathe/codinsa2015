using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor
{
    /// <summary>
    /// Classe contenant des outils pour manipuler les string.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Capitalize la première lettre d'une chaine.
        /// </summary>
        /// <returns></returns>
        public static string Capitalize(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (value.Length == 0)
                return value;

            StringBuilder result = new StringBuilder(value);
            result[0] = char.ToUpper(result[0]);
            return result.ToString();
        }
    }
}
