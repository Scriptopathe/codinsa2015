using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Model.Language
{
    /// <summary>
    /// Représente une instruction return en language Clank.
    /// </summary>
    public class ReturnInstruction : Instruction
    {
        /// <summary>
        /// Valeur retournée par cette instruction return.
        /// </summary>
        public Evaluable Value { get; set; }
    }
}
