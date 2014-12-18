using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Graphics.Server
{
    /// <summary>
    /// Représente une texture se trouvant potentiellement sur une autre machine,
    /// et manipulée par un autre programme.
    /// </summary>
    public abstract class RemoteTexture : RemoteGraphicsObject
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public RemoteTexture(GraphicsServer server) : base(server, false)
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
        public Texture2D UnderlyingTexture { get; set; }
        /// <summary>
        /// Largeur de la texture.
        /// </summary>
        public override int Width
        {
            get { return UnderlyingTexture.Width; }
        }
        
        /// <summary>
        /// Hauteur de la texture.
        /// </summary>
        public override int Height
        {
            get { return UnderlyingTexture.Height; }
        }

        public RemoteTexture2D(GraphicsServer server, string filename) : base(server)
        {
            Filename = filename;
            UnderlyingTexture = server.Content.Load<Texture2D>(filename);
            Register();
        }
    }
}
