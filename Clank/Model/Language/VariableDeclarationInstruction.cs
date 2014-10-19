using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente une déclaration de variable.
    /// </summary>
    public class VariableDeclarationInstruction : Instruction
    {
        /// <summary>
        /// Modificateurs de la déclaration.
        /// </summary>
        public List<string> Modifiers { get { return Var.Modifiers; } set { Var.Modifiers = value; } }
        /// <summary>
        /// Variable déclarée.
        /// </summary>
        public Variable Var { get; set; }

        /// <summary>
        /// Valeur indiquant si la déclaration représente une déclaration de variable d'instance.
        /// </summary>
        public bool IsInstanceVariable { get; set; }

        /// <summary>
        /// Retourne une valeur indiquant si cette déclaration est publique.
        /// </summary>
        public bool IsPublic { get { return Modifiers.Contains(Language.SemanticConstants.Public); } }

        /// <summary>
        /// Crée une nouvelle instance de VariableDeclarationInstruction.
        /// </summary>
        public VariableDeclarationInstruction()
        {
        }

        /// <summary>
        /// Retourne cette déclaration de variable sous forme de string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Var.Type.GetFullName() + " " + Var.Name;
        }
    }
}
