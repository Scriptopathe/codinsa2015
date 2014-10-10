﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Generation.Preprocessor
{
    /// <summary>
    /// Permet de charger des scripts depuis le système de fichiers.
    /// </summary>
    public class FileIncludeLoader : IScriptIncludeLoader
    {
        public FileIncludeLoader() { }
        /// <summary>
        /// Charge le script dont l'uri est passée en paramètre.
        /// </summary>
        /// <param name="uri">URI du fichier à charger.</param>
        /// <returns></returns>
        public string Load(string uri)
        {
            return System.IO.File.ReadAllText(uri);
        }
    }
}
