using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree.Instructions
{
    /// <summary>
    /// Instruction de création de Patch.
    /// </summary>
    class PatchInstruction : Instruction
    {
        /// <summary>
        /// La clef de patch à faire correspondre.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Nom de la fonction à patcher.
        /// </summary>
        public string FuncName { get; set; }
        /// <summary>
        /// Liste d'instructions à ajouter au patch.
        /// </summary>
        public List<Instruction> Instructions { get; set;}
        /// <summary>
        /// Retourne l'action correspondant à cette instruction.
        /// </summary>
        public override Action<Context> GetAction()
        {
            return delegate(Context c)
            {
                Function func;
                if (c.LocalVariables.ContainsKey(FuncName) && c.LocalVariables[FuncName].Value is Function)
                    func = (Function)c.LocalVariables[FuncName].Value;
                else if (c.GlobalContext.Variables.ContainsKey(FuncName) && c.GlobalContext.Variables[FuncName].Value is Function)
                    func = (Function)c.GlobalContext.Variables[FuncName].Value;
                else
                    throw new Exception("The requested function " + FuncName + " does not exist in the current context");

                // Clefs prédéfinies.
                if (Key == "start")
                {
                    func.Body.Instructions.InsertRange(0, Instructions);
                    return;
                }
                else if (Key == "end")
                {
                    func.Body.Instructions.AddRange(Instructions);
                    return;
                }

                // On cherche la clef.
                int id = 0;
                bool keyFound = false;
                foreach (Instruction ins in func.Body.Instructions)
                {
                    if (ins is PatchkeyInstruction)
                    {
                        keyFound = true;
                        break;
                    }
                    id++;
                }

                // Si la clef est trouvée alors on ajoute les instruction du patch à l'endroit indiqué.
                if (keyFound)
                {
                    func.Body.Instructions.InsertRange(id, Instructions);
                }
                else
                {
                    throw new Exception("patchkey " + Key + " not found in function " + FuncName + ".");
                }
                
            };
        }
    }
}
