using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente n'importe quel type d'expression évaluable.
    /// </summary>
    public abstract class Evaluable
    {
        /// <summary>
        /// Retourne le type de la variable.
        /// </summary>
        public virtual ClankTypeInstance Type { get; set; }
    }
}
