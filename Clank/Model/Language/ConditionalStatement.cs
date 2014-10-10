using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un statement conditionnel (if, else, elsif, while).
    /// </summary>
    public class ConditionalStatement : Instruction
    {
        public static Dictionary<string, Type> StatementTypes = new Dictionary<string, Type>() {
            { "if", Type.If}, {"else", Type.Else}, {"elsif", Type.Elsif}, {"while", Type.While }
        };

        public enum Type { If, Else, Elsif, While };

        /// <summary>
        /// Type de statement représenté par cette instance.
        /// </summary>
        public Type StatementType { get; set; }
        /// <summary>
        /// Condition de passage pour ce bloc if.
        /// </summary>
        public Evaluable Condition { get; set; }

        /// <summary>
        /// Code contenu dans ce block if.
        /// </summary>
        public List<Instruction> Code { get; set; }
    }
}
