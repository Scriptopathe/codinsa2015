using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente une déclaration de fonction.
    /// </summary>
    public class FunctionDeclaration : Instruction
    {
        /// <summary>
        /// Représente la fonction déclarée.
        /// </summary>
        public Function Func { get; set; }
        /// <summary>
        /// Retourne les instructions contenues dans cette fonction.
        /// </summary>
        public List<Instruction> Code { get; set; }

        /// <summary>
        /// Commentaire de doc de la fonction.
        /// </summary>
        public Semantic.DocumentationComment DocComment { get; set; }

        /// <summary>
        /// Retourne un String représentant cette déclaration de fonction.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            foreach(FunctionArgument arg in Func.Arguments)
            {
                b.Append(arg.ArgType.GetFullName() + " " + arg.ArgName);
                if (arg != Func.Arguments.Last())
                    b.Append(",");
            }
            return Func.ReturnType.GetFullName() + " " + Func.Name + "(" + b.ToString() + ")";
        }

        /// <summary>
        /// Retourne une copie superficielle de cet objet et une copie profonde de la fonction encapsulée.
        /// </summary>
        /// <returns></returns>
        public FunctionDeclaration FuncCopy()
        {
            FunctionDeclaration func = new FunctionDeclaration();
            func.Func = Func.DeepCopy();
            func.Character = Character;
            func.Line = Line;
            func.Source = Source;
            func.Code = Code;
            func.Comment = Comment;
            return func;
        }
    }
}
