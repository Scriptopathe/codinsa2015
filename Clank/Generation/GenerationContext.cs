using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Generation
{
    /// <summary>
    /// Représente un contexte de génération.
    /// </summary>
    public class GenerationContext
    {
        /// <summary>
        /// Représente les types disponibles dans le contexte.
        /// </summary>
        public Model.Semantic.TypeTable Types { get; set; }

    }
}
