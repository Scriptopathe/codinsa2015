using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree.Instructions
{
    /// <summary>
    /// Instruction permettant de charger une assembly.
    /// </summary>
    class LoadAssemblyInstruction : Instruction
    {
        /// <summary>
        /// Nom de l'assembly qui devra être chargé.
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// Retourne l'action correspondant à cette instruction.
        /// </summary>
        public override Action<Context> GetAction()
        {
            Action<Context> action = delegate(Context c)
            {
                if(!c.GlobalContext.LoadedAssemblies.ContainsKey(AssemblyName))
                    c.GlobalContext.LoadedAssemblies.Add(AssemblyName,
                        System.Reflection.Assembly.LoadWithPartialName(AssemblyName));
            };
            return action;
        }
        /// <summary>
        /// Crée une instance de LoadAssemblyInstruction.
        /// </summary>
        public LoadAssemblyInstruction(string assemblyName)
        {
            AssemblyName = assemblyName;
        }
    }
}
