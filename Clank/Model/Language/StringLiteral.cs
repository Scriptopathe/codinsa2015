using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un littéral de type string.
    /// </summary>
    public class StringLiteral : Evaluable
    {
        /// <summary>
        /// Valeur du litéral.
        /// </summary>
        public string Value;
    }
}
