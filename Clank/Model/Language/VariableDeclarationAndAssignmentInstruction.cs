using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente une déclaration de variable.
    /// </summary>
    public class VariableDeclarationAndAssignmentInstruction : Instruction
    {
        /// <summary>
        /// Obtient l'instruction de déclaration de variable.
        /// </summary>
        public VariableDeclarationInstruction Declaration { get; set; }
        /// <summary>
        /// Obtient l'instruction d'affectation de variable.
        /// </summary>
        public AffectationInstruction Assignment { get; set; }

        /// <summary>
        /// Retourne cette déclaration de variable sous forme de string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Declaration.Var.Type.GetFullName() + " " + Assignment.ToString();
        }
    }
}
