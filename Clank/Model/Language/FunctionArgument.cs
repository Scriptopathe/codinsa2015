using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un argument pouvant être passé à une fonction.
    /// </summary>
    public class FunctionArgument
    {
        /// <summary>
        /// Type de l'argument.
        /// </summary>
        public ClankTypeInstance ArgType { get; set; }
        /// <summary>
        /// Nom donné à l'argument de la fonction.
        /// </summary>
        public string ArgName { get; set; }
    }
}
