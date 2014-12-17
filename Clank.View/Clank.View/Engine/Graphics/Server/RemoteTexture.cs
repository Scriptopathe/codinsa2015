using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Graphics.Server
{
    /// <summary>
    /// Représente une texture se trouvant potentiellement sur une autre machine,
    /// et manipulée par un autre programme.
    /// </summary>
    public class RemoteTexture : RemoteGraphicsObject
    {
        public RemoteTexture(GraphicsServer server) : base(server)
        {

        }
    }

    /// <summary>
    /// Représente une texture se trouvant potentiellement sur une autre machine, 
    /// et chargée à partir du disque via un fichier.
    /// </summary>
    public class RemoteTexture2D : RemoteTexture
    {
        public string Filename { get; set; }

        public RemoteTexture2D(GraphicsServer server, string filename) : base(server)
        {
            Filename = filename;
        }
    }
}
