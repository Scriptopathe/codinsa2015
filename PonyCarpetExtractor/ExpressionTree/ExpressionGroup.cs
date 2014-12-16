using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree
{
    /// <summary>
    /// Un sous-groupe d'expression est un ensemble 
    /// Operande+Operande, où les opérandes sont des objets
    /// pouvant retourner une value (IGettable)
    /// Décomposition de l'exemple :
    /// gr1 = (4+5)
    /// gr2 = 6*machin.cst
    /// gr3 = gr1+gr2
    /// 
    /// Les Operandes peuvent elles mêmes être des ExpressionSubGroup.
    /// </summary>
    public class ExpressionGroup : IGettable
    {
        /// <summary>
        /// Opérande 2 passée en argument à l'opérateur.
        /// </summary>
        public IGettable Operand1
        {
            get;
            set;
        }
        /// <summary>
        /// Opérande 2 passée en argument à l'opérateur.
        /// </summary>
        public IGettable Operand2
        {
            get;
            set;
        }
        /// <summary>
        /// Opérateur qui prend en argument 1 ou deux opérandes.
        /// </summary>
        public Operator Operator
        {
            get;
            set;
        }
        /// <summary>
        /// Retourne la valeur de l'opération.
        /// </summary>
        /// <returns></returns>
        public object GetValue(Context context)
        {
            // Si l'opérateur vaut null, par convention, la seconde opérande vaut null.
            // La valeur à retourner est celle de la première opérande.
            if (Operator == null)
                return Operand1.GetValue(context);
            var operand1 = Operand1.GetValue(context);
            var operand2 = Operand2.GetValue(context);
            object value =  Operator.Operation(operand1, operand2);
            return value;
        }
        /// <summary>
        /// Crée un nouveau groupe d'expression.
        /// </summary>
        public ExpressionGroup()
        {

        }
        /// <summary>
        /// Crée un nouveau groupe d'expression.
        /// </summary>
        /// <param name="operand1">Première opérande</param>
        /// <param name="_operator">Opérateur</param>
        /// <param name="operand2">Seconde opérande.</param>
        public ExpressionGroup(IGettable operand1, Operator _operator, IGettable operand2)
        {
            Operand2 = operand2;
            Operand1 = operand1;
            Operator = _operator;
        }
    }
}
