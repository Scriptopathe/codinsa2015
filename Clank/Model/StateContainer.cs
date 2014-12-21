using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model
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
        public Tools.EventLog ParsingLog { get; set; }

        /// <summary>
        /// Crée une nouvelle instance de StateContainer.
        /// </summary>
        public StateContainer()
        {
            ParsingLog = new Tools.EventLog();
            StateClass = new Language.ClassDeclaration() { Name = Language.SemanticConstants.StateClass, Source = "generated" };// new Language.ClankType() { Name = "State" };
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
                    PerformSerializableChecking((Language.ClassDeclaration)instruction, types);
                    Classes.Add((Language.ClassDeclaration)instruction);
                }
                else if(instruction is Language.EnumDeclaration)
                {
                    StateClass.Instructions.Add(instruction);
                }
                else if(instruction is Language.VariableDeclarationInstruction)
                {
                    Language.VariableDeclarationInstruction variable = (Language.VariableDeclarationInstruction)instruction;
                    StateClass.Instructions.Add(variable);
                }
                else if (instruction is Language.PlaceholderInstruction) { }
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

        /// <summary>
        /// Vérifie que la classe passée en paramètre respecte les conditions pour être marquée sérializable
        /// (si elle l'est).
        /// </summary>
        void PerformSerializableChecking(Language.ClassDeclaration decl, Semantic.TypeTable types)
        {
            Language.ClankType type = types.Types[decl.Name];
            if (!type.SupportSerialization)
                return;

            bool hasError = false;
            string error = "La classe '" + decl.Name + "' est marquée serializable mais contient des variables non serializables : ";
            // Vérifie que les types des variables d'instances sont tous "serializable".
            foreach(var kvp in type.InstanceVariables)
            {
                string name = kvp.Key;
                Language.Variable variable = kvp.Value;
                // Si la variable a des paramètres génériqu
                if(variable.Type.BaseType.HasGenericParameterTypeMembers())
                {
                    /*if(!variable.Type.BaseType.SupportSerializationAsGeneric)
                    {
                        hasError = true;
                        error += variable.ToString() + " ne supporte pas la sérialisation en tant que param générique, ";
                    }*/
                    // On ne peut pas check maintenant, il faut connaître les params génériques.
                }
                else if(!(variable.Type.BaseType is Language.GenericParameterType))
                {
                    List<string> dummy;
                    if(!variable.Type.DoesSupportSerialization(out dummy))
                    {
                        hasError = true;
                        error += variable.ToString() + "ne supporte pas la sérialisation, ";
                    }
                }
            }
            
            if(hasError)
            {
                error = error.TrimEnd(',', ' ');
                ParsingLog.AddWarning(error, decl.Line, decl.Character, decl.Source);
            }
        }
    }
}
