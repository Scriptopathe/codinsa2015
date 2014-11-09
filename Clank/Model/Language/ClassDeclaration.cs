using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente une déclaration de classe.
    /// </summary>
    public class ClassDeclaration : Instruction
    {
        /// <summary>
        /// Représente le nom de la classe dont cette classe hérite.
        /// </summary>
        public string InheritsFrom { get; set; }
        /// <summary>
        /// Nom de la classe.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Retourne la liste des modificateurs de la classe.
        /// </summary>
        public List<string> Modifiers { get; set; }
        /// <summary>
        /// Nom des paramètres génériques, s'ils existent.
        /// </summary>
        public List<string> GenericParameters { get; set; }
        /// <summary>
        /// Instructions (déclarations en particulier) contenues dans cette classe.
        /// </summary>
        public List<Instruction> Instructions { get; set; }
        /// <summary>
        /// Préfixe à placer devant ce nom de classe pour le retrouver une TypeTable.
        /// </summary>
        public string ContextPrefix { get; set; }

        /// <summary>
        /// Crée et retourne une copie superficielle de cette déclaration de classe.
        /// </summary>
        /// <returns></returns>
        public ClassDeclaration Copy()
        {
            ClassDeclaration newClass = new ClassDeclaration();
            newClass.Name = Name;
            newClass.Modifiers = Modifiers;
            newClass.GenericParameters = GenericParameters;
            newClass.Instructions = new List<Instruction>();
            foreach (Instruction ins in Instructions) { newClass.Instructions.Add(ins); };
            newClass.ContextPrefix = ContextPrefix;
            newClass.Line = Line;
            newClass.Character = Character;
            newClass.Source = Source;
            return newClass;
        }

        public ClassDeclaration()
        {
            Modifiers = new List<string>();
            Instructions = new List<Instruction>();
            GenericParameters = new List<string>();
        }
        /// <summary>
        /// Retourne cette déclaration de classe sous la forme d'un string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder genParametersStr = new StringBuilder();
            genParametersStr.Append("<");
            foreach(string genParam in GenericParameters)
            {
                genParametersStr.Append(genParam);
                if (genParam != GenericParameters.Last())
                    genParametersStr.Append(",");
            }
            genParametersStr.Append(">");

            StringBuilder modifiersStr = new StringBuilder();
            foreach(string modifier in Modifiers)
            {
                modifiersStr.Append(modifier);
                modifiersStr.Append(" ");
            }

            return modifiersStr.ToString() + Name + genParametersStr.ToString();
        }

        /// <summary>
        /// Retourne le nom complet du type permettant d'y accéder dans la table des types.
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            return ContextPrefix + Name;
        }
    }
}
