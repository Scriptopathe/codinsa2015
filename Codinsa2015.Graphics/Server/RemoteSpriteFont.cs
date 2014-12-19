using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Graphics.Server
{
    /// <summary>
    /// Représente une instance de SpriteFont.
    /// </summary>
    public class RemoteSpriteFont : RemoteGraphicsObject
    {
        public string Filename { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        public SpriteFont Font { get; set; }

        public RemoteSpriteFont() { }
        public RemoteSpriteFont(GraphicsServer server, string filename) : base(server, false)
        {
            Filename = filename;
            Font = server.Content.Load<SpriteFont>(filename);
            Register();
        }
        public Vector2 MeasureString(string s) { return Font.MeasureString(s); }
    }
}
