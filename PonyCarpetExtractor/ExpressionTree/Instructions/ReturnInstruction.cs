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
    public class ReturnInstruction : Instruction
    {
        /// <summary>
        /// Expression dont on retournera la valeur.
        /// </summary>
        public IGettable Expression
        {
            get;
            set;
        }
        /// <summary>
        /// Retourne l'action correspondant à cette instruction.
        /// </summary>
        public override Action<Context> GetAction()
        {
            return delegate(Context c)
            {
                if (Expression != null)
                    ReturnValue = Expression.GetValue(c);
                else
                    ReturnValue = null;
            };
        }
    }
}
