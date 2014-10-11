using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clank.IDE
{
    /// <summary>
    /// Représente les paramètres de compilation d'un projet.
    /// </summary>
    public class ProjectSettings
    {
        /// <summary>
        /// Fichier serveur cible pour la compilation.
        /// </summary>
        public Clank.Core.Generation.GenerationTarget ServerTarget { get; set; }
        /// <summary>
        /// Fichiers clients cibles pour la compilation.
        /// </summary>
        public List<Clank.Core.Generation.GenerationTarget> ClientTargets { get; set; }

        public ProjectSettings()
        {
            ClientTargets = new List<Core.Generation.GenerationTarget>();
        }
    }
}
