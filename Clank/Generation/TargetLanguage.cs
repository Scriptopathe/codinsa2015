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
        /// Nom du dossier dans lequel mettre les fichiers en sortie.
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// Crée une nouvelle instance de GenerationTarget.
        /// </summary>
        /// <param name="identifier"></param>
        public GenerationTarget(string identifier, string outputFilename)
        {
            LanguageIdentifier = identifier;
            OutputDirectory = outputFilename;
        }
        public GenerationTarget()
        {
            LanguageIdentifier = "";
            OutputDirectory = "";
        }
        /// <summary>
        /// Crée une nouvelle cible de génération à partir d'un string au format:
        /// Identifier:filename
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static GenerationTarget FromString(string str)
        {
            if (str.Contains(":"))
            {
                string[] parts = str.Split(':');
                GenerationTarget target = new GenerationTarget();
                target.LanguageIdentifier = parts[0];
                target.OutputDirectory = parts[1].Replace("\r", "").Replace("\n", "").Trim('"');
                return target;
            }
            else
                return new GenerationTarget();
        }

        /// <summary>
        /// Convertit cette cible de génération en string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (LanguageIdentifier == "" && OutputDirectory == "")
                return "";
            return LanguageIdentifier.ToString() + ":\"" + OutputDirectory.ToString() + "\"";
        }

        /// <summary>
        /// Convertit une liste de GenerationTarget en string.
        /// </summary>
        public static string TargetsToString(List<GenerationTarget> targets, char separator = '\n')
        {
            StringBuilder builer = new StringBuilder();
            foreach(var target in targets)
            {
                builer.Append(target.ToString() + (target == targets.Last() ? "" : separator.ToString()));
            }
            return builer.ToString();
        }

        /// <summary>
        /// Convertit un string au format :
        /// LanguageIdentifier:Filename|LanguageIdentifier:Filename
        /// en une liste de GenerationTarget.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<GenerationTarget> TargetsFromString(string str, char separator='\n')
        {
            List<GenerationTarget> targets = new List<GenerationTarget>();
            string[] parts = str.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string part in parts)
            {
                targets.Add(FromString(part));
            }
            return targets;
        }
    }
}
