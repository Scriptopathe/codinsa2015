using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un littéral de type Int32.
    /// </summary>
    public class IntegerLiteral : Evaluable
    {
        /// <summary>
        /// Valeur du litéral.
        /// </summary>
        public int Value;
    }
}
