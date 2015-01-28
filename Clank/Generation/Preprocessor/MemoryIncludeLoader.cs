using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Generation.Preprocessor
{
    /// <summary>
    /// Permet de charger des scripts depuis la mémoire.
    /// Les script disponibles pour les instructions include doivent être
    /// ajoutés à ce MemoryIncludeLoader en utilisant la fonction AddFile.
    /// </summary>
    public class MemoryIncludeLoader : IScriptIncludeLoader
    {
        Dictionary<string, string> m_scripts;

        public MemoryIncludeLoader() { m_scripts = new Dictionary<string, string>(); }
        /// <summary>
        /// Charge le script dont l'uri est passée en paramètre.
        /// </summary>
        /// <param name="uri">URI du fichier à charger.</param>
        /// <returns></returns>
        public string Load(string uri)
        {
            if (!m_scripts.ContainsKey(uri))
                throw new RessourceNotFoundException(uri);
            return m_scripts[uri];
        }

        /// <summary>
        /// Ajoute un fichier dans ce MemoryIncludeLoader.
        /// Tous les #include référençant l'uri donnée chargeront ce fichier.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        public void AddFile(string uri, string content)
        {
            m_scripts.Add(uri, content);
        }

        /// <summary>
        /// Supprime un fichier de la table de ce MemoryIncludeLoader.
        /// </summary>
        /// <param name="uri"></param>
        public void RemoveFile(string uri)
        {
            m_scripts.Remove(uri);
        }
    }
}
