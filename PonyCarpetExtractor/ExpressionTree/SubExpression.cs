using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree
{
    public class SubExpression : IGettable, ISettable
    {
        #region Variables
        /// <summary>
        /// Parties de l'expression.
        /// Ce sont les parties ordonnées qui sont, dans la syntaxe, 
        /// séparées par des points :
        /// </summary>
        public List<SubExpressionPart> Parts
        {
            get;
            set;
        }
        #endregion
        /* --------------------------------------------------------------------
         * Static constructors
         * Regroupe des constructeurs statiques simples à utiliser.
         * -------------------------------------------------------------------*/
        #region Static constructors
        public static SubExpression Variable(string name)
        {
            return new SubExpression(new SubExpressionPart(name, SubExpressionPart.ExpTypes.Variable));
        }
        public static SubExpression Function(string name)
        {
            return new SubExpression(new SubExpressionPart(name, SubExpressionPart.ExpTypes.Method));
        }
        public static SubExpression Constant(string valueStr)
        {
            return new SubExpression(new SubExpressionPart(valueStr, 
                SubExpressionPart.ExpTypes.ConstantObject));
        }
        public static SubExpression Type(string name)
        {
            return new SubExpression(
                new SubExpressionPart(name,
                    SubExpressionPart.ExpTypes.ConstantTypeName));
        }
        public static SubExpression NewObj(string typename)
        {
            return new SubExpression(
                new SubExpressionPart(typename,
                    SubExpressionPart.ExpTypes.NewObject));
        }
        public static SubExpression NewObj(string typename, IGettable[] parameters)
        {
            return new SubExpression(
                new SubExpressionPart(typename,
                    SubExpressionPart.ExpTypes.NewObject,
                    parameters.ToList()));
        }
        public static SubExpression NewDelegateObj()
        {
            throw new Exception();
        }
        public static SubExpression Event(string varName, string evtName)
        {
            return new SubExpression(
                new SubExpressionPart(varName, SubExpressionPart.ExpTypes.Variable),
                new SubExpressionPart(evtName, SubExpressionPart.ExpTypes.Event));
        }
        #endregion
        #region Methods
        /// <summary>
        /// Constructeur de la sous expression. N'itialise pas le membre "Parts"
        /// </summary>
        public SubExpression()
        {
            
        }
        /// <summary>
        /// Crée une sous expression à partir des parties de sous expression données.
        /// </summary>
        /// <param name="parts">Parties de sous expressions à partir des quelles créer la sous expression.</param>
        public SubExpression(params SubExpressionPart[] parts)
        {
            Parts = parts.ToList();
        }
        /// <summary>
        /// Retourne la valeur de cette sous expression.
        /// </summary>
        /// <returns></returns>
        public object GetValue(Context context)
        {
            var first = Parts.First();
            object obj = first.GetObjectUnboundValue(context);
            if (Parts.Count > 1)
            {
                for (int i = 1; i < Parts.Count; i++)
                {
                    SubExpressionPart part = Parts[i];
                    obj = part.GetObjectBoundValue(context, obj);
                }
            }
            return obj;
        }
        /// <summary>
        /// Modifie la valeur de cette sous expression.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        public void SetValue(Context context, dynamic value)
        {
            var first = Parts.First();
            // Vérifions que la dernière partie est accessible en écriture.
            if (Parts.Last().IsSettable)
            {
                if (Parts.Count > 2)
                {
                    object obj = first.GetObjectUnboundValue(context);
                    int i;
                    for (i = 1; i < Parts.Count - 1; i++)
                    {
                        SubExpressionPart part = Parts[i];
                        obj = part.GetObjectBoundValue(context, obj);
                    }
                    Parts[i].SetObjectBoundValue(context, obj, value);
                }
                else if (Parts.Count == 2)
                {
                    // Optimisation "force brute"
                    object obj = first.GetObjectUnboundValue(context);
                    Parts[1].SetObjectBoundValue(context, obj, value);
                }
                else if (Parts.Count == 1)
                {
                    // On modifie une valeur du contexte.
                    first.SetObjectUnboundValue(context, value);
                }
            }
            else
            {
                throw new InterpreterException(String.Format("L'expression {0}, n'est pas accessible en écriture",
                    Parts.Last().ToString()));
            }
        }
        #endregion
    }
}
