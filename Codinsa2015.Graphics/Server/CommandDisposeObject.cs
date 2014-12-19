using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Graphics.Server
{
    /// <summary>
    /// Commande de suppression d'un objet graphique
    /// </summary>
    public class CommandDisposeObject : Command
    {
        public RemoteGraphicsObject GraphicsObject { get; set; }
        public CommandDisposeObject() { }
        public CommandDisposeObject(RemoteGraphicsObject obj)
        {
            GraphicsObject = obj;
        }
    }
}
