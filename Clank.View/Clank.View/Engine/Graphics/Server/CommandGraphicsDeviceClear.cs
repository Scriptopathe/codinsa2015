using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Clank.View.Engine.Graphics.Server
{
    /// <summary>
    /// Représente un appel à GraphicsDevice.Clear().
    /// </summary>
    public class CommandGraphicsDeviceClear : Command
    {
        public Color Color { get; set; }

        public CommandGraphicsDeviceClear(Color color)
        {
            Color = color;
        }
    }
}
