using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree.Instructions
{
    /// <summary>
    /// Modificateurs de blocks evaluables.
    /// </summary>
    public enum EvaluableBlockModifiers
    {
        None,
        Ref,
    }
    public enum PrimaryEvaluableBlockModifiers
    {
        Function,
        Lambda
    }
    /// <summary>
    /// Outil permettant l'évaluation d'une fonction comme une expression.
    /// </summary>
    public class EvaluableBlockExpression : IGettable
    {

        /// <summary>
        /// Le nom du block évaluable.
        /// S'il est spécifié, cela crée une variable contenant la fonction dans le contexte
        /// local.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Fonction liée à ce block évaluable.
        /// </summary>
        public Function Function
        {
            get;
            set;
        }
        /// <summary>
        /// Crée une nouvelle instance de EvaluableBlockExpression.
        /// </summary>
        public EvaluableBlockExpression()
        {
            
        }
        /// <summary>
        /// Implémentation de IGettable.GetValue.
        /// </summary>
        public object GetValue(Context context)
        {
            // Initialise le contexte de la fonction.
            FunctionDeclarationInstruction.InitializeFunctionContext(Function, context);

            // Si la fonction n'est pas anonyme, on la met dans le contexte local :
            if (Name != null)
            {
                // Ajoute la fonction au contexte de déclaration de la fonction.
                if (context.LocalVariables.ContainsKey(Name))
                    context.LocalVariables[Name] = new Mutable(Function);
                else
                    context.LocalVariables.Add(Name, new Mutable(Function));
            }

            return Function;
        }
    }
    /// <summary>
    /// Instruction de déclaration de fonction.
    /// Ajoute seulement la fonction au contexte local.
    /// </summary>
    public class FunctionDeclarationInstruction : Instruction
    {
        public delegate object FunctionDelegate(params object[] parameters);

        #region Properties
        /// <summary>
        /// Nom de la fonction.
        /// </summary>
        public string FunctionName
        {
            get;
            set;
        }
        /// <summary>
        /// Fonction déclarée par cette instruction.
        /// </summary>
        public Function Function
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        public FunctionDeclarationInstruction()
        {
            HasReturned = false;
            ReturnValue = null;
        }
        /// <summary>
        /// Initialise le contexte de la fonction, en fonction du contexte de déclaration context.
        /// </summary>
        public static void InitializeFunctionContext(Function func, Context context)
        {
            // -- Ajout des variables du contexte externe vers la fonction.
            // NE PAS OUBLIER : ici, il s'agit du contexte de DECLARATION de la fonction.
            if (func.PrimaryModifier == PrimaryEvaluableBlockModifiers.Function)
            {
                // Si c'est une fonction, on prend les VALEURS des variables contenues
                // dans le contexte extérieur.
                foreach (var variable in context.LocalVariables)
                {
                    // Priorité aux variables les plus internes.
                    if (!func.EmbeddedVariables.ContainsKey(variable.Key))
                        func.EmbeddedVariables.Add(variable.Key, variable.Value.Value);
                }
            }
            else if (func.PrimaryModifier == PrimaryEvaluableBlockModifiers.Lambda)
            {
                // Si c'est une lambda, on prend les REFERENCES des variables contenues
                // dans le contexte extérieur.
                foreach (var variable in context.LocalVariables)
                {
                    // Priorité aux variables les plus internes.
                    if (!func.EmbeddedVariables.ContainsKey(variable.Key))
                        func.EmbeddedVariables.Add(variable.Key, variable.Value);
                }
            }
            func.Body.Context.Include(context);
        }
        /// <summary>
        /// Retourne l'action correspondant à cette instruction.
        /// </summary>
        public override Action<Context> GetAction()
        {
            Action<Context> action = delegate(Context context)
            {
                // Initialise le contexte de la fonction.
                InitializeFunctionContext(Function, context);

                // Ajoute la fonction au contexte de déclaration de la fonction.
                if (context.LocalVariables.ContainsKey(FunctionName))
                    context.LocalVariables[FunctionName] = new Mutable(Function);
                else
                    context.LocalVariables.Add(FunctionName, new Mutable(Function));
                
            };
            return action;
        }
    }
}
