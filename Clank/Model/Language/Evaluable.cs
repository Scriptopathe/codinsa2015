using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente n'importe quel type d'expression évaluable.
    /// </summary>
    public abstract class Evaluable
    {
        /// <summary>
        /// Retourne le type de la variable.
        /// </summary>
        public virtual ClankTypeInstance Type { get; set; }

        /// <summary>
        /// Retourne un string représentant les types de la liste d'arguments passée en paramètre.
        /// Ex : (int, bool, string)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetArgTypesString(List<Evaluable> args)
        {
            string fullName = "(";
            foreach (Language.Evaluable arg in args)
            {
                string argName = arg.Type.GetFullName();
                fullName += argName + (arg == args.Last() ? "" : ", ");
            }
            fullName += ")";

            return fullName;
        }
    }
}
