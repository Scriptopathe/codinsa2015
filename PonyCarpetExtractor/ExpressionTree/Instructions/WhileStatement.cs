using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree.Instructions
{
    /// <summary>
    /// Représente une boucle while
    /// </summary>
    public class WhileStatement : Instruction
    {
        #region Properties
        /// <summary>
        /// Block de code contenu dans la boucle.
        /// </summary>
        public Block Block
        {
            get;
            set;
        }
        /// <summary>
        /// Condition nécessaire à l'exécution du block.
        /// </summary>
        public IGettable Condition
        {
            get;
            set;
        }
        #endregion
        /// <summary>
        /// Crée une nouvelle boucle while.
        /// </summary>
        public WhileStatement()
        {
            
        }
        /// <summary>
        /// Retourne l'action correspondant à cette instruction.
        /// </summary>
        public override Action<Context> GetAction()
        {
            Action<Context> action = delegate(Context context)
            {
                while ((bool)Condition.GetValue(context))
                {
                    object val = Block.Execute(context);
                    // Si le block a effectué return :
                    if (Block.HasReturned)
                    {
                        HasReturned = true;
                        ReturnValue = val;
                    }
                }
            };
            return action;
        }
    }
}
