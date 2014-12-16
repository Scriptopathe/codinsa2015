using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree.Instructions
{
    /// <summary>
    /// Représente une instruction For.
    /// </summary>
    class ForStatement : Instruction
    {
        #region Properties
        /// <summary>
        /// Initialisation de la boucle.
        /// </summary>
        public Instruction Initialisation
        {
            get;
            set;
        }
        /// <summary>
        /// Condition nécessaire pour effectuer un nouveau tour de boucle.
        /// </summary>
        public IGettable Condition
        {
            get;
            set;
        }
        /// <summary>
        /// Instruction appelée à la fin de chaque tour de boucle.
        /// </summary>
        public Instruction Update
        {
            get;
            set;
        }
        /// <summary>
        /// Block d'instructions appelé si la condition est remplie.
        /// </summary>
        public Block Block
        {
            get;
            set;
        }
        #endregion
        /// <summary>
        /// Retourne l'action correspondant à cette instruction.
        /// </summary>
        public override Action<Context> GetAction()
        {
            Action<Context> action = delegate(Context context)
            {
                var updateAction = Update.GetAction();
                for (Initialisation.GetAction()(context); (bool)Condition.GetValue(context); updateAction(context))
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
