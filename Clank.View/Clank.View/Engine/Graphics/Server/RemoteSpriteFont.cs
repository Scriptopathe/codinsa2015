using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Graphics.Server
{
    /// <summary>
    /// Représente une instance de SpriteFont.
    /// </summary>
    public class RemoteSpriteFont : RemoteGraphicsObject
    {
        public string Filename { get; set; }
        public RemoteSpriteFont(GraphicsServer server, string filename) : base(server)
        {
            Filename = filename;
        }
    }
}
