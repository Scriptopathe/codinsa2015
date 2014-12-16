using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PonyCarpetExtractor.ExpressionTree.Instructions;
namespace PonyCarpetExtractor.ExpressionTree
{
    public class Function
    {
        #region Properties
        /// <summary>
        /// Modificateur primaire de la fonction.
        /// </summary>
        public PrimaryEvaluableBlockModifiers PrimaryModifier
        {
            get;
            set;
        }
        /// <summary>
        /// Modificateurs secondaires de la fonction.
        /// </summary>
        public EvaluableBlockModifiers Modifiers
        {
            get;
            set;
        }
        /// <summary>
        /// Liste des paramètres de cette fonction.
        /// </summary>
        public List<string> ArgumentNames
        {
            get;
            set;
        }
        /// <summary>
        /// Corps de la fonction.
        /// </summary>
        public Block Body
        {
            get;
            set;
        }
        /// <summary>
        /// Liste des variables de la portée de la fonction parente 
        /// dont les valeurs sont conservées au moment de la création
        /// de la fonction (et non au moment de l'appel).
        /// </summary>
        public Dictionary<string, object> EmbeddedVariables
        {
            get;
            set;
        }
        #endregion

        #region Operators
        public static Function operator +(Function f1, Function f2)
        {
            // Ajoute deux fonctions.
            // Ouais c'est possible :D*
            // TODO
            Function newFunc = new Function();
            Function[] funcs = new Function[] { f1, f2 };
            if (f1.PrimaryModifier != f2.PrimaryModifier)
                throw new InterpreterException("Unable to add two functions with different primary modifiers");
            newFunc.PrimaryModifier = f1.PrimaryModifier;
            foreach (Function f in funcs)
            {
                foreach (string argName in f.ArgumentNames)
                {
                    newFunc.ArgumentNames.Add(argName);
                }
                foreach (var kvp in f.EmbeddedVariables)
                {
                    if(!newFunc.EmbeddedVariables.ContainsKey(kvp.Key))
                        newFunc.EmbeddedVariables.Add(kvp.Key, kvp.Value);
                }
                f.Body.Context.GlobalContext = f1.Body.Context.GlobalContext;

            }
            newFunc.Body = new Block(f1.Body.Context.GlobalContext);
            ReturnInstruction ret = new ReturnInstruction();
            // Expression : f1(*args) + f2(*args2);
            ret.Expression = new PonyCarpetExtractor.ExpressionTree.ExpressionGroup();
            
            return newFunc;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de la fonction.
        /// </summary>
        public Function()
        {
            EmbeddedVariables = new Dictionary<string, object>();
        }
        /// <summary>
        /// Appelle la méthode à l'aide des paramètres spécifiés, en incluant
        /// dans le block le contexte externe passé en argument.
        /// </summary>
        /// <returns></returns>
        public object Call(object[] arguments, Context externalContext)
        {
            // FIXME DEBUG
            object val = Body.Execute(externalContext, ArgumentNames, arguments, EmbeddedVariables);
            return val;
        }
        /// <summary>
        /// Appelle la méthode à l'aide des paramètres spécifiés.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object Call(object[] arguments)
        {
            return Call(arguments, null);
        }
        #endregion
    }
}
