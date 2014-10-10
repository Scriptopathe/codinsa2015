using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Model.Language
{
    /// <summary>
    /// Représente une déclaration d'enum.
    /// </summary>
    public class EnumDeclaration : Instruction
    {
        /// <summary>
        /// Membres de l'enum.
        /// </summary>
        public List<string> Members { get; set; }
        /// <summary>
        /// Membre de l'enum.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Crée une nouvelle instance de EnumDeclaration.
        /// </summary>
        public EnumDeclaration()
        {
            Members = new List<string>();
        }
    }
}
