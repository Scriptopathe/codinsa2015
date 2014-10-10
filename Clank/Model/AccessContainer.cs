using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model
{
    /// <summary>
    /// Regroupe un ensemble d'unités "access".
    /// </summary>
    public class AccessContainer
    {
        /// <summary>
        /// Déclarations contenues dans ce block access.
        /// </summary>
        public List<Language.FunctionDeclaration> Declarations { get; set; }
        /// <summary>
        /// Journal d'erreurs survenues lors du parsing.
        /// </summary>
        public Tools.EventLog ParsingLog { get; set; }
        
        
        /// <summary>
        /// Crée une nouvelle instance de AccessContainer.
        /// </summary>
        public AccessContainer()
        {
            ParsingLog = new Tools.EventLog();
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
                if(instruction is Language.NamedBlockDeclaration && ((Language.NamedBlockDeclaration)instruction).Name == Language.SemanticConstants.AccessBk)
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
                    // Vérifie que le type de retour de la fonction est public.
                    Language.FunctionDeclaration decl = (Language.FunctionDeclaration)instruction;
                    decl.Func.Owner = table.Types["State"];
                    if (!decl.Func.IsPublic)
                        ParsingLog.AddWarning("Les fonctions contenues dans le block access doivent être publiques.",
                            instruction.Line, instruction.Character, instruction.Source);
                    if (!decl.Func.ReturnType.BaseType.IsPublic)
                        ParsingLog.AddWarning("Les types de retour des déclaration de fonction doivent être des types publics. (donné : " + decl.Func.ReturnType.GetFullName() + ")",
                            instruction.Line, instruction.Character, instruction.Source);
                    
                    Declarations.Add((Language.FunctionDeclaration)instruction);
                }
                else
                {
                    ParsingLog.AddError("Instruction de type " + instruction.GetType().Name + " invalide dans un AccessBlock",
                        instruction.Line,
                        instruction.Character,
                        instruction.Source
                        );
                }
            }
        }

    }
}
