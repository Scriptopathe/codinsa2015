using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente une déclaration de fonction.
    /// </summary>
    public class Function : Evaluable
    {
        /// <summary>
        /// Type de retour de la fonction.
        /// </summary>
        public ClankTypeInstance ReturnType { get; set; }
        /// <summary>
        /// Arguments de la fonction : type et nom.
        /// </summary>
        public List<FunctionArgument> Arguments { get; set; }
        /// <summary>
        /// Nom de la fonction.
        /// </summary>
        public string Name { get; set; }
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


            StringBuilder argTypes = new StringBuilder();
            argTypes.Append("(");
            foreach(FunctionArgument arg in Arguments)
            {
                argTypes.Append(arg.ArgType.GetFullName() + (arg == Arguments.Last() ? "" : ", "));
            }
            argTypes.Append(")");

            if (Owner == null)
                return Name + argTypes;
            else
                return Owner.GetFullName() + "." + Name + argTypes;
        }
        
        /// <summary>
        /// Valeur indiquant si la fonction est une fonction Macro, c'est à dire si elle fait partie du block macro,
        /// et doit être remplacée à la génération de code par une fonction avec un autre nom, et potentiellement des 
        /// arguments différents.
        /// </summary>
        public bool IsMacro { get; set; }
        /// <summary>
        /// Valeur indiquant si la fonction est statique.
        /// </summary>
        public bool IsStatic
        {
            get { return Modifiers.Contains(Language.SemanticConstants.Static); }
        }
        /// <summary>
        /// Valeur indiquant si la fonction est un constructeur d'instance.
        /// </summary>
        public bool IsConstructor
        {
            get { return Modifiers.Contains(Language.SemanticConstants.Constructor); }
        }

        /// <summary>
        /// Valeur indiquant si la fonction est publique.
        /// </summary>
        public bool IsPublic
        {
            get { return Modifiers.Contains(Language.SemanticConstants.Public) || IsMacro; }
        }

        /// <summary>
        /// Retourne une valeur indiquant si la fonction a un type de retour privé ou 
        /// des paramètres de types privés.
        /// </summary>
        /// <returns></returns>
        public bool HasPrivateReturnTypeOrParameterType()
        {
            if (ReturnType.IsPrivateOrHasPrivateGenericArgs())
                return true;
            foreach(FunctionArgument arg in Arguments)
            {
                if (arg.ArgType.IsPrivateOrHasPrivateGenericArgs())
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Retourne une version instanciée de cette fonction avec les types génériques donnés.
        /// </summary>
        public Function Instanciate(List<ClankTypeInstance> genArgs)
        {
            Function newFunc = new Function();
            newFunc.ReturnType = ReturnType.Instanciate(genArgs);
            newFunc.Name = Name;
            newFunc.Modifiers = Modifiers;
            newFunc.Code = Code;
            newFunc.Owner = Owner;
            newFunc.IsMacro = IsMacro;
            newFunc.Arguments = new List<FunctionArgument>();
            foreach(FunctionArgument arg in Arguments)
            {
                newFunc.Arguments.Add(new FunctionArgument() { ArgName = arg.ArgName, ArgType = arg.ArgType.Instanciate(genArgs) });
            }
            return newFunc;
        }

        /// <summary>
        /// Retourne une copie profonde de cette fonction.
        /// </summary>
        /// <returns></returns>
        public Function DeepCopy()
        {
            List<FunctionArgument> args = new List<FunctionArgument>();
            foreach(FunctionArgument arg in this.Arguments)
            {
                args.Add(new FunctionArgument() { ArgName = arg.ArgName, ArgType = arg.ArgType });
            }

            List<string> modifiers = new List<string>();
            foreach(string modifier in this.Modifiers)
            {
                modifiers.Add(modifier);
            }

            Function newFunc = new Function()
            {
                Name = this.Name,
                Owner = this.Owner,
                ReturnType = this.ReturnType,
                Type = this.Type,
                IsMacro = this.IsMacro,
                Modifiers = modifiers,
                Arguments = args,
                Code = this.Code
            };

            return newFunc;
        }

        
    }
}
