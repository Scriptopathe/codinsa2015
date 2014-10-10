using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un littéral de type bool.
    /// </summary>
    public class BoolLiteral : Evaluable
    {
        /// <summary>
        /// Valeur du litéral.
        /// </summary>
        public bool Value;
    }
}
