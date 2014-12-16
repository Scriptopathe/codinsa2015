using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree.Instructions
{
    /// <summary>
    /// Instruction permettant de charger une assembly.
    /// </summary>
    public class UseNamespaceInstruction : Instruction
    {
        /// <summary>
        /// Nom du namespace qui devra être chargé.
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// Retourne l'action correspondant à cette instruction.
        /// </summary>
        public override Action<Context> GetAction()
        {
            Action<Context> action = delegate(Context c)
            {
                if(!c.GlobalContext.LoadedNamespaces.Contains(Namespace))
                    c.GlobalContext.LoadedNamespaces.Add(Namespace);
            };
            return action;
        }
        /// <summary>
        /// Constructeur.
        /// </summary>
        public UseNamespaceInstruction(string @namespace)
        {
            Namespace = @namespace;
        }
    }
}