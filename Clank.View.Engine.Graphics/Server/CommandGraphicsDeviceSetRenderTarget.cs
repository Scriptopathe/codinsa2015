using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Graphics.Server
{
    /// <summary>
    /// Représente un appel à GraphicsDevice.SetRenderTarget().
    /// </summary>
    public class CommandGraphicsDeviceSetRenderTarget : Command
    {
        public RemoteRenderTarget RenderTarget { get; set; }

        public CommandGraphicsDeviceSetRenderTarget() { }
        public CommandGraphicsDeviceSetRenderTarget(RemoteRenderTarget target)
        {
            RenderTarget = target;
        }
    }
}
