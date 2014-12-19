using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace Codinsa2015.Graphics.Server
{
    
    [XmlInclude(typeof(RemoteRenderTarget))]
    [XmlInclude(typeof(RemoteSpriteBatch))]
    [XmlInclude(typeof(RemoteSpriteFont))]
    [XmlInclude(typeof(RemoteTexture))]
    [XmlInclude(typeof(RemoteTexture2D))]
    public class RemoteGraphicsObject
    {
        static int s_id = 0;

        /// <summary>
        /// Représente le serveur auquel cet objet appartient.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public GraphicsServer Server { get; set; }
        /// <summary>
        /// Identificateur unique de l'objet.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Crée une nouvelle instance de GraphicsObject.
        /// </summary>
        /// <param name="server"></param>
        public RemoteGraphicsObject(GraphicsServer server, bool registerNow=true)
        {
            Server = server;
            ID = s_id++;
            if (registerNow)
                Register();
        }

        /// <summary>
        /// Constructeur utilisé par le sérialiseur XML.
        /// </summary>
        public RemoteGraphicsObject() { }

        /// <summary>
        /// Enregistre cet objet auprès du serveur graphique.
        /// </summary>
        protected void Register()
        {
            Server.SendCommand(new CommandCreateObject(this));
        }
        /// <summary>
        /// Demande au client de supprimer les ressources allouées par cet objet.
        /// </summary>
        public void Dispose()
        {
            Server.SendCommand(new CommandDisposeObject(this));   
        }
    }
}
