using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Graphics.Engine
{
    /// <summary>
    /// Représente un appel à GraphicsDevice.SetRenderTarget().
    /// </summary>
    public class CommandGraphicsDeviceSetRenderTarget
    {
        public RemoteRenderTarget RenderTarget { get; set; }
        public CommandGraphicsDeviceSetRenderTarget(RemoteRenderTarget target)
        {
            RenderTarget = target;
        }
    }
}
