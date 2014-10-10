using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un nom de type.
    /// Les noms de types sont typés "Type".
    /// </summary>
    public class Typename : Evaluable
    {
        /// <summary>
        /// Type représenté par ce typename.
        /// </summary>
        public Language.ClankTypeInstance Name { get; set; }

        /// <summary>
        /// Retourne une instance de System.String représentant l'objet actuel.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Type.GetFullName() + " " + Name.ToString();
        }
    }
}
