using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
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
        /// Modificateurs de la variable.
        /// </summary>
        public List<string> Modifiers { get; set; }

        /// <summary>
        /// Obtient une valeur indiquant si cette variable est publique (dans le cas d'une variable d'instance).
        /// </summary>
        public bool IsPublic
        {
            get { return Modifiers.Contains(SemanticConstants.Public); }
        }

        public Variable() { Modifiers = new List<string>(); }
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
