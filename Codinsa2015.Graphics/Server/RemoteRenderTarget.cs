using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
namespace Codinsa2015.Graphics.Server
{
    /// <summary>
    /// Représente un RenderTarget distant.
    /// </summary>
    public class RemoteRenderTarget : RemoteTexture
    {
        [XmlElement("MHeight")]
        public int m_width;

        [XmlElement("MWidth")]
        public int m_height;
        /// <summary>
        /// Largeur du render target.
        /// </summary>
        public override int Width { get { return m_width; } set { m_width = value; } }
        /// <summary>
        /// Hauteur du render target.
        /// </summary>
        public override int Height { get { return m_height; } set { m_height = value; } }
        /// <summary>
        /// Usage du render target.
        /// </summary>
        public RenderTargetUsage Usage { get; set; }


        public RemoteRenderTarget() { }
        /// <summary>
        /// Crée une nouvelle instance de RenderTarget.
        /// </summary>
        public RemoteRenderTarget(GraphicsServer server, int width, int height, RenderTargetUsage usage)
            : base(server)
        {
            m_width = width;
            m_height = height;
            Usage = usage;
            Register();
        }

        /// <summary>
        /// Crée une nouvelle instance de RenderTarget.
        /// </summary>
        public RemoteRenderTarget(GraphicsServer server, int width, int height)
            : base(server)
        {
            m_width = width;
            m_height = height;
            Usage = RenderTargetUsage.DiscardContents;
        }
    }
}
