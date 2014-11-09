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
                    decl.Func.Owner = table.Types[Language.SemanticConstants.StateClass];
                    if (!decl.Func.IsPublic)
                        ParsingLog.AddWarning("Les fonctions contenues dans le block access doivent être publiques.",
                            instruction.Line, instruction.Character, instruction.Source);
                    if (decl.Func.ReturnType.IsPrivateOrHasPrivateGenericArgs())
                        ParsingLog.AddWarning("Les types de retour des déclaration de fonction du bloc access doivent être des types publics" +
                            " et ne pas contenir de paramètre générique privé. (donné : "
                            + decl.Func.ReturnType.GetFullName() + ")",
                            instruction.Line, instruction.Character, instruction.Source);

                    // Vérification du return type.
                    List<string> outReasons;
                    if (!decl.Func.ReturnType.DoesSupportSerialization(out outReasons))
                    {
                        string error;
                        error = "Le type de retour de la fonction '" + decl.Func.GetFullName() + "' (" + decl.Func.ReturnType.GetFullName() +
                                ") ne prend pas en charge la sérialisation. Raisons : ";
                        error += Tools.StringUtils.Join(outReasons, ", ") + ".";
                        ParsingLog.AddWarning(error, decl.Line, decl.Character, decl.Source);
                    }
                    
                    // Vérification des types des arguments.
                    foreach(Language.FunctionArgument arg in decl.Func.Arguments)
                    {
                        if (!arg.ArgType.DoesSupportSerialization(out outReasons))
                        {
                            string error;
                            error = "Le type de l'argument '" + arg.ArgName + "' (" + arg.ArgType + ") de la fonction " + decl.Func.GetFullName() + 
                                    " ne prend pas en charge la sérialisation. Reasons : ";
         

                            error += Tools.StringUtils.Join(outReasons, ", ") + ".";
                            ParsingLog.AddWarning(error, decl.Line, decl.Character, decl.Source);
                        }
                    }
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
