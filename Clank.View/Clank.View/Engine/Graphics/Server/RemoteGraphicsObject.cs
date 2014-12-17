using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Graphics.Server
{
    public class RemoteGraphicsObject
    {
        static int s_id = 0;

        /// <summary>
        /// Représente le serveur auquel cet objet appartient.
        /// </summary>
        public GraphicsServer Server { get; set; }
        /// <summary>
        /// Identificateur unique de l'objet.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Crée une nouvelle instance de GraphicsObject.
        /// </summary>
        /// <param name="server"></param>
        public RemoteGraphicsObject(GraphicsServer server)
        {
            Server = server;
            Server.SendCommand(new CommandCreateObject(this));
            ID = s_id++;
        }
    }
}
