using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree.Instructions
{
    /// <summary>
    /// Représente une instruction d'appel de méthode.
    /// </summary>
    public class MethodCallInstruction : Instruction
    {
        #region Properties
        SubExpression m_methodExpression;
        /// <summary>
        /// Expression représentant l'appel de méthode.
        /// </summary>
        public SubExpression MethodExpression
        {
            get { return m_methodExpression; }
            set
            {
                if (value.Parts.Last().SubExpType != SubExpressionPart.ExpTypes.Method)
                    throw new Exception("Pas une méthode");
                m_methodExpression = value;
            }
        }

        #endregion
        /// <summary>
        /// Crée une nouvelle instruction d'appel de méthode.
        /// </summary>
        public MethodCallInstruction()
        {
            HasReturned = false;
            ReturnValue = null;
        }
        /// <summary>
        /// Crée une nouvelle instruction d'appel de méthode.
        /// </summary>
        public MethodCallInstruction(SubExpression methodExpr)
        {
            MethodExpression = methodExpr;
        }
        /// <summary>
        /// Retourne l'action correspondant à cette instruction.
        /// </summary>
        public override Action<Context> GetAction()
        {
            return delegate(Context c)
            {
                MethodExpression.GetValue(c);
            };
        }

    }
}
