using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un littéral de type Float32.
    /// </summary>
    public class FloatLiteral : Evaluable
    {
        /// <summary>
        /// Valeur du litéral.
        /// </summary>
        public float Value;
    }
}
