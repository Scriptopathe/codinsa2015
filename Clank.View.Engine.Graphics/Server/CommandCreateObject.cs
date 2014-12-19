using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Graphics.Server
{
    /// <summary>
    /// Représente la construction / chargement d'un objet graphique (texture, effect etc...).
    /// </summary>
    public class CommandCreateObject : Command
    {
        public RemoteGraphicsObject GraphicsObject { get; set; }
        public CommandCreateObject() { }
        public CommandCreateObject(RemoteGraphicsObject obj)
        {
            GraphicsObject = obj;
        }
    }
}
