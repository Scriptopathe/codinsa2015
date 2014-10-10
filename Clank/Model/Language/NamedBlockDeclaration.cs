using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un bloc de code nommé.
    /// </summary>
    public class NamedBlockDeclaration : Instruction
    {
        /// <summary>
        /// Nom du bloc de code.
        /// </summary>
        public string Name;
        /// <summary>
        /// Instructions présentes dans le bloc de code.
        /// </summary>
        public List<Instruction> Instructions;


        /// <summary>
        /// Retourne le NamedBlock sous forme de string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "NamedBlock:" + this.Name;
        }
    }
}
