using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente une instruction d'affectation.
    /// </summary>
    public class AffectationInstruction : Instruction
    {
        /// <summary>
        /// Représente l'expression qui contient la left value et right value.
        /// </summary>
        public BinaryExpressionGroup Expression { get; set; }
        
        /// <summary>
        /// Retourne une instance de System.String représentant l'objet actuel.
        /// </summary>
        public override string ToString()
        {
            return Expression.Operand1.ToString() + " = " + Expression.Operand2.ToString();
        }
    }
}
