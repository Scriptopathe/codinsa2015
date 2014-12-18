using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Graphics.Server
{
    /// <summary>
    /// Commande de suppression d'un objet graphique
    /// </summary>
    class CommandDisposeObject : Command
    {
        public RemoteGraphicsObject GraphicsObject { get; set; }

        public CommandDisposeObject(RemoteGraphicsObject obj)
        {
            GraphicsObject = obj;
        }
    }
}
