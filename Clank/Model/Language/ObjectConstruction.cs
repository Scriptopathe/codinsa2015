using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un appel de constructeur d'un objet.
    /// </summary>
    public class ObjectConstruction
    {
        /// <summary>
        /// Type de l'objet à construire.
        /// </summary>
        public ClankTypeInstance FunctionName { get; set; }
        /// <summary>
        /// Arguments passés au constructeur.
        /// </summary>
        public List<Evaluable> Arguments { get; set; }
    }
}
