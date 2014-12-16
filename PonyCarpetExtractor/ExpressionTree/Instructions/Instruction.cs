using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
namespace PonyCarpetExtractor.ExpressionTree.Instructions
{
    /// <summary>
    /// Représente une instruction.
    /// </summary>
    public abstract class Instruction
    {
        /// <summary>
        /// Retourne l'action correspondant à cette instruction.
        /// </summary>
        public abstract Action<Context> GetAction();
        /// <summary>
        /// Indique si un block dans l'instruction a effectué un retour.
        /// </summary>
        public bool HasReturned { get; set; }
        /// <summary>
        /// Donne la valeur du retour exécuté par l'instruction, si HasReturned vaut true.
        /// </summary>
        public object ReturnValue { get; set; }
    }
}
