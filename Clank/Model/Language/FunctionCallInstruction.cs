using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un appel de fonction dans le langage Clank.Core.
    /// </summary>
    public class FunctionCallInstruction : Instruction
    {
        /// <summary>
        /// Représente l'appel de fonction encapsulé par cette instruction.
        /// </summary>
        public FunctionCall Call { get; set; }
    }
}
