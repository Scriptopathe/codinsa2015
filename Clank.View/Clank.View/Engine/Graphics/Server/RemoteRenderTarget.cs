using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Graphics.Server
{
    /// <summary>
    /// Représente un RenderTarget distant.
    /// </summary>
    public class RemoteRenderTarget : RemoteTexture
    {
        int m_width;
        int m_height;
        /// <summary>
        /// Largeur du render target.
        /// </summary>
        public override int Width { get { return m_width; } }
        /// <summary>
        /// Hauteur du render target.
        /// </summary>
        public override int Height { get { return m_height; } }
        /// <summary>
        /// Usage du render target.
        /// </summary>
        public RenderTargetUsage Usage { get; private set; }

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
