using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree.Instructions
{
    class PatchkeyInstruction : Instruction
    {
        /// <summary>
        /// Clef de patch.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Crée une nouvelle instance de PatchKeyInstruction.
        /// </summary>
        public PatchkeyInstruction()
        {
            Key = "";
        }
        /// <summary>
        /// Retourne l'action correspondant à cette instruction.
        /// </summary>
        public override Action<Context> GetAction()
        {
            return null;
        }
    }
}
