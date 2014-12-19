using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Graphics.Server
{
    /// <summary>
    /// Commande indiquant au client graphique que la frame en cours de dessin est terminée.
    /// </summary>
    public class CommandEndFrame : Command
    {
        public CommandEndFrame() { }
    }
}
