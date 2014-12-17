using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Graphics.Server
{
    /// <summary>
    /// Représente un effet distant.
    /// </summary>
    public class RemoteEffect : RemoteGraphicsObject
    {
        /// <summary>
        /// Nom de l'asset contenant l'effet.
        /// </summary>
        public string Filename { get; set; }

        public RemoteEffect(GraphicsServer server, string filename) : base(server) { Filename = filename; }
    }
}
