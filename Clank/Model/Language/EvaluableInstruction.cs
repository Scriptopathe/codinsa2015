using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente une instruction qui ne contient qu'une valeur évaluable et ne fait rien.
    /// </summary>
    class EvaluableInstruction : Instruction
    {
        /// <summary>
        /// Valeur encapsulée par cette instruction.
        /// </summary>
        public Evaluable Value { get; set; }
    }
}
