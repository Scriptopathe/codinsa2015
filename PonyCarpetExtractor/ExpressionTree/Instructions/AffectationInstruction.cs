using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq.Expressions;
namespace PonyCarpetExtractor.ExpressionTree.Instructions
{
    // KOMENFAIR : un truc dans le block du genre "SetNext" qui change l'instruction pointée.
    // + champ statique pour la récupérer.
    // Le setNext doit être  appelé depuis l'IL.
    // le champ statique aussi.
    /// <summary>
    /// Instruction d'affectation :
    /// </summary>
    public class AffectationInstruction : Instruction
    {
        /// <summary>
        /// Membre dont la valeur va être modifiée dans l'affectation.
        /// </summary>
        public ISettable LeftMember
        {
            get;
            set;
        }
        /// <summary>
        /// Membre dont la valeur va être affectée au membre de gauche.
        /// </summary>
        public IGettable RightMember
        {
            get;
            set;
        }
        /// <summary>
        /// Constructeur de l'instruction.
        /// </summary>
        public AffectationInstruction()
        {
            HasReturned = false;
            ReturnValue = null;
        }
        /// <summary>
        /// Retourne l'action à exécuter par cette instruction.
        /// </summary>
        /// <returns></returns>
        public override Action<Context> GetAction()
        {
            Action<Context> action = delegate(Context c)
            {
                LeftMember.SetValue(c, RightMember.GetValue(c));
            };
            return action;
        }
        
    }
}
