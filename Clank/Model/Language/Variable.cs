using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Model.Language
{
    /// <summary>
    /// Représente une variable.
    /// </summary>
    public class Variable : Evaluable
    {
        /// <summary>
        /// Retourne le nom de la variable.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Retourne une instance de System.String représentant l'objet actuel.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Type.GetFullName() + " " + Name;
        }
    }
}
