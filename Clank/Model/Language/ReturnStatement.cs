using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un statement "return".
    /// </summary>
    public class ReturnStatement : Instruction
    {
        /// <summary>
        /// Représente le jeton évaluable retourné par ce statement.
        /// </summary>
        public Evaluable Returned { get; set; }
    }
}
