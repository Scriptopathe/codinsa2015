using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Graphics.Engine
{
    /// <summary>
    /// Représente un RenderTarget distant.
    /// </summary>
    public class RemoteRenderTarget : RemoteTexture
    {
        /// <summary>
        /// Largeur du render target.
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// Hauteur du render target.
        /// </summary>
        public int Height { get; private set; }
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
            Width = width;
            Height = height;
            Usage = usage;
        }
    }
}
