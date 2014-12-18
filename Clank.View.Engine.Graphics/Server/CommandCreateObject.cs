using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Graphics.Server
{
    /// <summary>
    /// Représente la construction / chargement d'un objet graphique (texture, effect etc...).
    /// </summary>
    class CommandCreateObject : Command
    {
        public RemoteGraphicsObject GraphicsObject { get; set; }

        public CommandCreateObject(RemoteGraphicsObject obj)
        {
            GraphicsObject = obj;
        }
    }
}
