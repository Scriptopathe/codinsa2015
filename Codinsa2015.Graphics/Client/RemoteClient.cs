using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace Codinsa2015.Graphics.Client
{
    /// <summary>
    /// Représente un client graphique distant.
    /// </summary>
    public class RemoteClient : GraphicsClient
    {
        Socket m_clientSocket;
        public static int __DEBUG_PORT = 2000;
        /// <summary>
        /// Initialise le serveur graphique : attend la connexion d'un client.
        /// </summary>
        public void Initialize()
        {
            Socket servSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            servSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), __DEBUG_PORT));
            servSocket.Listen(1);
            m_clientSocket = servSocket.Accept();
            servSocket.Close();
        }

        /// <summary>
        /// Envoie la commande au client graphique distant.
        /// </summary>
        /// <param name="command"></param>
        public override void ProcessCommand(Server.Command command)
        {
            m_clientSocket.Send(Encoding.UTF8.GetBytes(Tools.Serializer.Serialize<Server.Command>(command)));
        }
    }
}
