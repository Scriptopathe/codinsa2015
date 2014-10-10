using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Model
{
    /// <summary>
    /// Regroupe un ensemble d'unités "state".
    /// </summary>
    public class StateContainer
    {
        /// <summary>
        /// Contient les déclarations de variables contenues dans le block state.
        /// </summary>
        public Language.ClassDeclaration StateClass { get; set; }
        /// <summary>
        /// Déclarations de classes contenues dans container.
        /// </summary>
        public List<Language.ClassDeclaration> Classes { get; set; }
        /// <summary>
        /// Journal d'erreurs survenues lors du parsing.
        /// </summary>
        public Tools.Log ParsingLog { get; set; }


        /// <summary>
        /// Crée une nouvelle instance de StateContainer.
        /// </summary>
        public StateContainer()
        {
            ParsingLog = new Tools.Log();
            StateClass = new Language.ClassDeclaration() { Name = "State", Source = "generated" };// new Language.ClankType() { Name = "State" };
            Classes = new List<Language.ClassDeclaration>();
        }

        /// <summary>
        /// Ajoute des déclarations à ce Container à partir d'un block.
        /// </summary>
        /// <param name="block"></param>
        public void AddDeclarationsFromScript(Language.NamedBlockDeclaration block, Semantic.TypeTable types)
        {
            foreach (Language.Instruction instruction in block.Instructions)
            {
                if (instruction is Language.NamedBlockDeclaration && ((Language.NamedBlockDeclaration)instruction).Name == Language.SemanticConstants.StateBk)
                {
                    AddDeclarationsFromStateBlock((Language.NamedBlockDeclaration)instruction, types);
                }
            }
        }

        /// <summary>
        /// Ajoute des déclarations à ce Container à partir d'un block Access.
        /// </summary>
        /// <param name="block"></param>
        void AddDeclarationsFromStateBlock(Language.NamedBlockDeclaration block, Semantic.TypeTable types)
        {
            foreach (Language.Instruction instruction in block.Instructions)
            {
                if (instruction is Language.ClassDeclaration)
                {
                    Classes.Add((Language.ClassDeclaration)instruction);
                }
                else if(instruction is Language.VariableDeclarationInstruction)
                {
                    Language.VariableDeclarationInstruction variable = (Language.VariableDeclarationInstruction)instruction;
                    StateClass.Instructions.Add(variable);
                }
                else
                {
                    ParsingLog.AddError("Instruction de type " + instruction.GetType().Name + " invalide dans un StateBlock",
                        instruction.Line,
                        instruction.Character,
                        instruction.Source
                        );
                }
            }
        }
    }
}
