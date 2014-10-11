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
        public GenerationTarget()
        {
            LanguageIdentifier = "";
            OutputFilename = "";
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
                target.OutputFilename = parts[1].Trim('"');
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
            if (LanguageIdentifier == "" && OutputFilename == "")
                return "";
            return LanguageIdentifier.ToString() + ":\"" + OutputFilename.ToString() + "\"";
        }

        /// <summary>
        /// Convertit une liste de GenerationTarget en string.
        /// </summary>
        public static string TargetsToString(List<GenerationTarget> targets)
        {
            StringBuilder builer = new StringBuilder();
            foreach(var target in targets)
            {
                builer.Append(target.ToString() + (target == targets.Last() ? "" : "|"));
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
        public static List<GenerationTarget> TargetsFromString(string str)
        {
            List<GenerationTarget> targets = new List<GenerationTarget>();
            string[] parts = str.Split('|');
            foreach(string part in parts)
            {
                targets.Add(FromString(part));
            }
            return targets;
        }
    }
}
