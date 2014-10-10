using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente une expression composée d'un opérateur et de deux opérandes.
    /// </summary>
    public class BinaryExpressionGroup : Evaluable
    {
        /// <summary>
        /// Représente l'opérande 1 de l'expression.
        /// </summary>
        public Evaluable Operand1 { get; set; }
        /// <summary>
        /// Représente l'opérande 2 de l'expression.
        /// </summary>
        public Evaluable Operand2 { get; set; }
        /// <summary>
        /// Représente l'opérateur de l'expression.
        /// </summary>
        public Operator Operator { get; set; }
    }
}
