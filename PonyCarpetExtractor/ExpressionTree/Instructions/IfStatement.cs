using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree.Instructions
{
    /// <summary>
    /// Représente une structure de contrôle de la forme :
    /// If then {block}
    /// Else if then {block}
    /// Else then {block}
    /// </summary>
    public class IfStatement : Instruction
    {
        #region Variables
        /// <summary>
        /// Blocks conditionnels correspondant chacun à un couple condition/instruction.
        /// Le premier correspond au "if", les suivants au "else if".
        /// </summary>
        public List<ConditionalBlock> Blocks
        {
            get;
            set;
        }
        /// <summary>
        /// Block exécuté si aucune des conditions n'est remplie.
        /// Vaut null si aucun block "else" n'est déclaré.
        /// </summary>
        public Block ElseBlock
        {
            get;
            set;
        }

        /// <summary>
        /// Constructeur du IfStatement.
        /// </summary>
        public IfStatement()
        {
            Blocks = new List<ConditionalBlock>();
            HasReturned = false;
            ReturnValue = null;
        }
        #endregion

        #region Instruction implementation
        /// <summary>
        /// Retourne l'action correspondant à cette instruction.
        /// </summary>
        public override Action<Context> GetAction()
        {
            Action<Context> action = delegate(Context c)
            {
                bool broken = false;
                foreach (ConditionalBlock block in Blocks)
                {
                    object value = block.Condition.GetValue(c);
                    // Vérifie que la valeur soit bien un booléen.
                    if(!(value is bool))
                        throw new InterpreterException("L'expression de condition dans un contrôle \"If\" doit être un booléen");
                    
                    // Exécute l'action.
                    if ((bool)value)
                    {
                        ReturnValue = block.Block.Execute(c);
                        HasReturned = block.Block.HasReturned;
                        broken = true;
                        break;
                    }
                }
                // Si aucune autre condition n'a été remplie, on exécute le block else.
                if (!broken && ElseBlock != null)
                {
                    ReturnValue = ElseBlock.Execute(c);
                    HasReturned = ElseBlock.HasReturned;
                }
            };
            return action;
        }
        #endregion
    }

    /// <summary>
    /// Représente un block conditionnel.
    /// </summary>
    public class ConditionalBlock
    {
        /// <summary>
        /// Block d'instructions à exécuter si la condition nécessaire est remplie.
        /// </summary>
        public Block Block
        {
            get;
            set;
        }
        /// <summary>
        /// Condition nécessaire à l'exécution du block.
        /// </summary>
        public IGettable Condition
        {
            get;
            set;
        }
        /// <summary>
        /// Crée un nouveau block conditionnel.
        /// </summary>
        public ConditionalBlock()
        {

        }
    }
}
