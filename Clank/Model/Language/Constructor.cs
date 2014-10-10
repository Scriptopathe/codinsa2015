using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un constructeur d'instance.
    /// </summary>
    public class Constructor
    {
        /// <summary>
        /// Arguments de la fonction : type et nom.
        /// </summary>
        public List<FunctionArgument> Arguments { get; set; }
        /// <summary>
        /// Retourne la liste des modificateurs de la fonction.
        /// </summary>
        public List<string> Modifiers { get; set; }
        /// <summary>
        /// Retourne les instructions contenues dans cette fonction.
        /// </summary>
        public List<Instruction> Code { get; set; }
        /// <summary>
        /// Représente le type propriétaire de la fonction.
        /// </summary>
        public ClankType Owner { get; set; }
        /// <summary>
        /// Nom par lequel se référrer à cette fonction dans une table de fonctions.
        /// </summary>
        public string GetFullName()
        {
            return Owner.GetFullName() + ".new";
        }

        public Constructor()
        {
            Arguments = new List<FunctionArgument>();
            Modifiers = new List<string>();
            Code = new List<Instruction>();
        }
        /// <summary>
        /// Retourne une version instanciée de ce constructeur avec les types génériques donnés.
        /// </summary>
        public Constructor Instanciate(List<ClankTypeInstance> genArgs)
        {
            Constructor newFunc = new Constructor();
            newFunc.Modifiers = Modifiers;
            newFunc.Code = Code;
            newFunc.Owner = Owner;
            newFunc.Arguments = new List<FunctionArgument>();
            foreach(FunctionArgument arg in Arguments)
            {
                newFunc.Arguments.Add(new FunctionArgument() { ArgName = arg.ArgName, ArgType = arg.ArgType.Instanciate(genArgs) });
            }
            return newFunc;
        }
    }
}
