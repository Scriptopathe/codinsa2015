using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clank.Core.Model.Language;
namespace Clank.Core.Generation.Languages
{
    /// <summary>
    /// Interface dont doivent hériter tous les générateurs dans les différents langages.
    /// </summary>
    public interface ILanguageGenerator
    {
        /// <summary>
        /// Génère les fichiers du projet.
        /// </summary>
        /// <returns></returns>
        List<OutputFile> GenerateProjectFiles(List<Instruction> instructions, string outputDirectory, bool isServer);
        /// <summary>
        /// Génère une instruction dans le langage cible.
        /// </summary>
        string GenerateInstruction(Model.Language.Instruction decl);
        /// <summary>
        /// Définit le projet contenant les informations nécessaires à la génération de code.
        /// </summary>
        void SetProject(Model.ProjectFile proj);
    }
}
