using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Model
{
    /// <summary>
    /// Regroupe un ensemble d'unités "access".
    /// </summary>
    public class WriteContainer
    {
        /// <summary>
        /// Déclarations contenues dans ce block access.
        /// </summary>
        public List<Language.FunctionDeclaration> Declarations { get; set; }
        /// <summary>
        /// Journal d'erreurs survenues lors du parsing.
        /// </summary>
        public Tools.Log ParsingLog { get; set; }
        
        
        /// <summary>
        /// Crée une nouvelle instance de WriteContainer.
        /// </summary>
        public WriteContainer()
        {
            ParsingLog = new Tools.Log();
            Declarations = new List<Language.FunctionDeclaration>();
        }

        /// <summary>
        /// Ajoute des déclarations à ce Container à partir d'un block.
        /// </summary>
        /// <param name="block"></param>
        public void AddDeclarationsFromScript(Language.NamedBlockDeclaration block, Semantic.TypeTable table)
        {
            foreach(Language.Instruction instruction in block.Instructions)
            {
                if(instruction is Language.NamedBlockDeclaration && ((Language.NamedBlockDeclaration)instruction).Name == Language.SemanticConstants.WriteBk)
                {
                    AddDeclarationsFromAccessBlock((Language.NamedBlockDeclaration)instruction, table);
                }
            }
        }

        /// <summary>
        /// Ajoute des déclarations à ce Container à partir d'un block Access.
        /// </summary>
        /// <param name="block"></param>
        void AddDeclarationsFromAccessBlock(Language.NamedBlockDeclaration block, Semantic.TypeTable table)
        {
            foreach (Language.Instruction instruction in block.Instructions)
            {
                if(instruction is Language.FunctionDeclaration)
                {
                    Language.FunctionDeclaration decl = (Language.FunctionDeclaration)instruction;
                    decl.Func.Owner = table.Types["State"];
                    Declarations.Add(decl);
                }
                else
                {
                    ParsingLog.AddError("Instruction de type " + instruction.GetType().Name + " invalide dans un block write. Attendu : déclaration de fonction",
                        instruction.Line,
                        instruction.Character,
                        instruction.Source
                        );
                }
            }
        }

    }
}
