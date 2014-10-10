using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Generation
{
    /// <summary>
    /// Représente une cible de génération.
    /// </summary>
    public class GenerationTarget
    {
        /// <summary>
        /// String identifiant le language.
        /// </summary>
        public string LanguageIdentifier { get; set; }
        /// <summary>
        /// Nom du fichier de sortie.
        /// </summary>
        public string OutputFilename { get; set; }
        /// <summary>
        /// Crée une nouvelle instance de GenerationTarget.
        /// </summary>
        /// <param name="identifier"></param>
        public GenerationTarget(string identifier, string outputFilename)
        {
            LanguageIdentifier = identifier;
            OutputFilename = outputFilename;
        }
    }
}
