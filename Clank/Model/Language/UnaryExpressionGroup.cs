using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente une expression composée d'un opérateur et d'une opérande.
    /// </summary>
    public class UnaryExpressionGroup : Evaluable
    {
        /// <summary>
        /// Représente l'opérande de l'expression.
        /// </summary>
        public Evaluable Operand { get; set; }
        /// <summary>
        /// Représente l'opérateur de l'expression.
        /// </summary>
        public Operator Operator { get; set; }
    }
}
