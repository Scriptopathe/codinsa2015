using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Model.Language
{
    /// <summary>
    /// Représente une déclaration de variable.
    /// </summary>
    public class VariableDeclarationInstruction : Instruction
    {
        /// <summary>
        /// Modificateurs de la déclaration.
        /// </summary>
        public List<string> Modifiers { get; set; }
        /// <summary>
        /// Variable déclarée.
        /// </summary>
        public Variable Var { get; set; }

        /// <summary>
        /// Valeur indiquant si la déclaration représente une déclaration de variable d'instance.
        /// </summary>
        public bool IsInstanceVariable { get; set; }

        /// <summary>
        /// Crée une nouvelle instance de VariableDeclarationInstruction.
        /// </summary>
        public VariableDeclarationInstruction()
        {
            Modifiers = new List<string>();
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
