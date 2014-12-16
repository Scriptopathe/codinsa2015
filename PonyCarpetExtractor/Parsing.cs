using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor
{
    /// <summary>
    /// Classe utilisée pour rechercher des expressions dans des chaines
    /// de caractères.
    /// </summary>
    public class Parsing
    {
        #region General
        /// <summary>
        /// Retourne une liste de paramètres à partir d'une chaine de caractères.
        /// Ex :
        ///     entrée : "x, y, $p1.func($p2, 5)"
        ///     sortie : ["x", "y", "$p1.func($p2, 5)"]
        /// </summary>
        /// <returns></returns>
        public static string[] SeparateGroups(string paramStr, char separator, char opening, char closing)
        {
            if (!paramStr.Contains(separator))
                return new string[] { paramStr };
            // return paramStr.Split(',');
            List<string> output = new List<string>();
            int lastMatch = 0; // index de la dernière capture
            int lengh = 0;
            int parenthesisDepth = 0;
            bool inQuotes = false;
            bool lastIsDigit = false;
            bool lineCommented = false;

            if (separator == '.') // si c'est un point faut faire gaffe aux chiffres
            {
                for (int i = 0; i < paramStr.Count(); i++)
                {
                    // Comments the line until next line.
                    if (!inQuotes)
                    {
                        if (paramStr[i] == '/' && i != paramStr.Count() - 1 && paramStr[i + 1] == '/')
                        {
                            lineCommented = true;
                        }
                        else if (paramStr[i] == '\n')
                        {
                            lineCommented = false;
                        }
                    }
                    // If commented, ignores the end.
                    if (lineCommented)
                        continue;

                    if (paramStr[i] == separator && parenthesisDepth == 0 && !inQuotes)
                    {
                        if (!lastIsDigit || !(lastIsDigit && char.IsDigit(paramStr[i + 1])))
                        {
                            output.Add(paramStr.Substring(lastMatch, lengh));
                            lengh = -1;
                            lastMatch = i + 1;
                        }
                    }
                    else if (paramStr[i] == '"')
                    {
                        inQuotes = !inQuotes;
                    }
                    else if (paramStr[i] == opening)
                    {
                        parenthesisDepth++;
                    }
                    else if (paramStr[i] == closing)
                    {
                        parenthesisDepth--;
                    }
                    if (i == paramStr.Count() - 1)
                    {
                        output.Add(paramStr.Substring(lastMatch, lengh + 1));
                    }
                    if (char.IsDigit(paramStr[i]))
                        lastIsDigit = true;
                    lengh++;
                }
            }
            else
            {
                for (int i = 0; i < paramStr.Count(); i++)
                {
                    // Comments the line until next line.
                    if (!inQuotes)
                    {
                        if (paramStr[i] == '/' && i != paramStr.Count() - 1 && paramStr[i + 1] == '/')
                        {
                            lineCommented = true;
                        }
                        else if (paramStr[i] == '\n')
                        {
                            lineCommented = false;
                        }
                    }
                    // If commented, ignores the end.
                    if (lineCommented)
                        continue;

                    if (paramStr[i] == separator && parenthesisDepth == 0 && !inQuotes)
                    {
                        output.Add(paramStr.Substring(lastMatch, lengh));
                        lengh = -1;
                        lastMatch = i + 1;
                    }
                    else if (paramStr[i] == '"')
                    {
                        inQuotes = !inQuotes;
                    }
                    else if (paramStr[i] == opening)
                    {
                        parenthesisDepth++;
                    }
                    else if (paramStr[i] == closing)
                    {
                        parenthesisDepth--;
                    }
                    if (i == paramStr.Count() - 1)
                    {
                        output.Add(paramStr.Substring(lastMatch, lengh + 1));
                    }
                    lengh++;
                }
            }
            return output.ToArray();
        }
        #endregion

        #region Constant parsing
        /// <summary>
        /// Cherche parmi les types de base si le string donné correspond à ce type.
        /// Retourne le type valeur ainsi retrouvé.
        /// </summary>
        /// <returns></returns>
        public static object ParseBasicType(string arg)
        {
            // Si c'est un int
            int objInt;
            bool isInt = Int32.TryParse(arg, out objInt);
            if (isInt)
                return objInt;

            // Si c'est un float
            float objFloat;
            bool isFloat = float.TryParse(arg, out objFloat);
            if (isFloat)
                return objFloat;

            // Si c'est un bool
            bool objBool;
            bool isBool = bool.TryParse(arg, out objBool);
            if (isBool)
                return objBool;

            // Sinon c'est un string
            return arg;
            /*
            bool isString = arg.IndexOf('"') == 0 && arg.IndexOf('"', 1) == arg.Count() - 1;
            if (isString)
                return arg.Replace("\"", "");*/
            
            throw new InterpreterException("Impossible de reconnaître le type de " + arg);
        }
        #endregion

        #region Type parsing
        /// <summary>
        /// Donne les arguments génériques trouvés dans le nom du type donné
        /// sous forme de string.
        /// Retourne le nom du type sous forme de string comme premier object du tuple.
        /// </summary>
        /// <param name="typeName">Nom du type : Machin(t1, t3></param>
        /// <returns></returns>
        public static Tuple<string, string[]> TypeNameParseGenericArguments(string typeName)
        {
            typeName = typeName.Replace(" ", "");
            int firstBraceId = typeName.IndexOf('<')+1;
            int lastBraceId = 0;
            int depth = 0;
            for (int i = 0; i < typeName.Count(); i++)
            {
                if(typeName[i] == '<')
                    depth++;
                else if(typeName[i] == '>')
                {
                    if (depth == 1)
                        lastBraceId = i;
                    depth--;
                }

            }
            if (lastBraceId == 0)
                throw new InterpreterException("Nom du type invalide, '>' attendu");

            // Prends seulement l'intérieur des <>
            string typeList = typeName.Substring(firstBraceId, lastBraceId-firstBraceId);
            var rets = SeparateGroups(typeList, ',', '<', '>');
            var v = new Tuple<string, string[]>(typeName.Substring(0, firstBraceId - 1), rets);
            return new Tuple<string,string[]>(typeName.Substring(0, firstBraceId-1), rets);
        }
        /// <summary>
        /// Retourne les crochets (et ce qu'il y a à l'intérieur) situé après le nom du type.
        /// Ex : "machin[][]" => "[][]"
        ///    "machin" => ""
        /// </summary>
        /// <returns></returns>
        public static string TypeNameGetEndingArrayBrackets(string typeName)
        {
            int depth = 0;
            bool ok = false;
            int startIndex = -1;
            for (int i = 0; i < typeName.Count(); i++)
            {
                if (typeName[i] == '<')
                {
                    depth++; ok = true;
                }
                else if (typeName[i] == '>')
                    depth--;
                else if (depth == 0 && ok && typeName[i] == '[')
                {
                    startIndex = i;
                    break;
                }
            }
            if (startIndex == -1)
                return "";

            return typeName.Substring(startIndex);
        }
        #endregion
    }

}
