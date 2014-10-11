using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Generation.Preprocessor
{
    /// <summary>
    /// Interface décrivant des objets capables de charger un script à partir d'une 
    /// uri.
    /// Permet au générateur de pouvoir charger des fichier depuis différentes sources (mémoire, disque, etc...)
    /// </summary>
    public interface IScriptIncludeLoader
    {
        /// <summary>
        /// Charge le script dont l'uri est passée en paramètre.
        /// </summary>
        /// <param name="uri">URI du fichier à charger.</param>
        /// <returns></returns>
        string Load(string uri);
    }

    public class RessourceNotFoundException : Exception
    {
        public string Ressource {get;set;}
        public RessourceNotFoundException() : base() { }
        public RessourceNotFoundException(string file) : base() { Ressource = file; }
    }
}
