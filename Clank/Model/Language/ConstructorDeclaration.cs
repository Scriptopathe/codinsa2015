using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente une déclaration de constructeur.
    /// </summary>
    public class ConstructorDeclaration : Instruction
    {
        /// <summary>
        /// Représente la fonction déclarée.
        /// </summary>
        public Constructor Func { get; set; }

        /// <summary>
        /// Retourne un String représentant cette déclaration de fonction.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            foreach(FunctionArgument arg in Func.Arguments)
            {
                b.Append(arg.ArgType.GetFullName() + " " + arg.ArgName);
                if (arg != Func.Arguments.Last())
                    b.Append(",");
            }
            return Func.GetFullName() + "(" + b.ToString() + ")";
        }
    }
}
