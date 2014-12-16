using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PonyCarpetExtractor.ExpressionTree.Instructions;
namespace PonyCarpetExtractor.ExpressionTree
{
    /// <summary>
    /// Représente un block d'instructions.
    /// </summary>
    public class Block
    {
        #region Properties
        /// <summary>
        /// Contexte local du bloc.
        /// </summary>
        public Context Context
        {
            get;
            set;
        }
        /// <summary>
        /// Instructions contenues dans ce block.
        /// </summary>
        public List<Instruction> Instructions
        {
            get;
            set;
        }
        /// <summary>
        /// Indique si l'instruction return a été utilisée dans ce block.
        /// </summary>
        public bool HasReturned
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée un nouveau block dont le contexte est créé
        /// dans un contexte global donné.
        /// La liste d'instructions n'est pas initialisée.
        /// </summary>
        /// <param name="c"></param>
        public Block(GlobalContext c)
        {
            Context = new Context(c);
            HasReturned = false;
        }
        /// <summary>
        /// Crée un nouveau block, mais ne lui crée pas de contexte.
        /// La liste d'instructions n'est pas initialisée.
        /// </summary>
        public Block()
        {

        }
        /// <summary>
        /// Exécute le block, et retourne une valeur.
        /// </summary>
        /// <returns></returns>
        public object Execute(Context externalContext)
        {
            return Execute(externalContext, null, null, null);
        }
        /// <summary>
        /// Exécute le block, et retourne une valeur.
        /// Des arguments peuvent êtres passés, et seront transformés en
        /// variables pour le contexte.
        /// </summary>
        public object Execute(Context externalContext, List<string> argumentNames, object[] arguments, 
            Dictionary<string, object> embeddedVariables)
        {
            return Execute(externalContext, argumentNames, arguments, embeddedVariables, true);
        }
        /// <summary>
        /// Exécute le block, et retourne une valeur.
        /// Des arguments peuvent êtres passés, et seront transformés en
        /// variables pour le contexte.
        /// CopyContext indique si une copie du contexte doit être faite pour l'exécution
        /// (utile dans le cas d'appel récursif à une fonction).
        /// </summary>
        /// <returns></returns>
        public object Execute(Context externalContext, List<string> argumentNames,
            object[] arguments, 
            Dictionary<string, object> embeddedVariables,
            bool copyContext)
        {
            // Contexte externe au moment de l'appel (priorité minimale).
            if (copyContext)
            {
                // Fait une copie du contexte externe, afin de ne pas le modifier.
                if (Context == externalContext)
                    Context = externalContext.Copy();

                // Effectue un reset du context, et inclue le contexte externe dans
                // ce contexte.
                Context.Reset();
                if (externalContext != null)
                    Context.Include(externalContext);
            }
            else
                Context = externalContext;
            

            // On effectue une copie du contexte de la fonction, afin que chaque appel
            // ait son propre contexte (pas optimisé).
            /*Context = Context.Copy();
            Context.Include(externalContext);*/
            HasReturned = false;




            if (argumentNames != null && arguments != null)
            {
                // Arguments en priorité ++
                if (arguments.Count() != argumentNames.Count)
                    throw new InterpreterException("Mauvais nombre d'arguments pour l'appel à la fonction");

                // Ajout des arguments au contexte.
                for (int i = 0; i < arguments.Count(); i++)
                {
                    if(!Context.LocalVariables.ContainsKey(argumentNames[i]))
                        Context.LocalVariables.Add(argumentNames[i], new Mutable(arguments[i]));
                    else
                        Context.LocalVariables[argumentNames[i]] =  new Mutable(arguments[i]);
                }
            }

            // Variables imbriquées (au moment de la déclaration) en priorité +++
            if (embeddedVariables != null && embeddedVariables.Count != 0)
            {
                foreach (KeyValuePair<string, object> kvp in embeddedVariables)
                {
                    // Si kvp.Value est mutable, on conserve sa référence.
                    // Sinon, on crée une référence mutable locale pour la valeur de la variable
                    Mutable val = (Mutable)(kvp.Value is Mutable ? kvp.Value : new Mutable(kvp.Value));
                    if (!Context.LocalVariables.ContainsKey(kvp.Key))
                        Context.LocalVariables.Add(kvp.Key, val);
                    else
                        Context.LocalVariables[kvp.Key] =  val;
                }
            }


            // ExternalContext peut être nul.
            foreach (Instruction inst in Instructions)
            {
                // Exécute l'instruction
                var action = inst.GetAction();
                if(action != null)
                    action(Context);

                // Prise en charge du return.
                if (inst is ReturnInstruction || inst.HasReturned)
                {
                    HasReturned = true;
                    return inst.ReturnValue;
                }
            }
            return null;
        }
        #endregion
    }
}
