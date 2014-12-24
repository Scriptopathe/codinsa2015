using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace Codinsa2015.Server.Net
{
    /// <summary>
    /// Contient un ensemble de fonction permettant au serveur d'envoyer des données vers
    /// un socket.
    /// </summary>
    public class CommandServer
    {
        #region Delegates / events
        public delegate void ClientConnectedDelegate(int clientId);
        public delegate void CommandReceivedDelegate(int clientId, string command);

        /// <summary>
        /// Event lancé lorsqu'un client se connecte.
        /// </summary>
        public event ClientConnectedDelegate ClientConnected;
        /// <summary>
        /// Event lancé lorsqu'une commande est reçue.
        /// </summary>
        public event CommandReceivedDelegate CommandReceived;
        #endregion

        #region Variables
        /// <summary>
        /// Socket utilisé pour accepter les connexions entrantes.
        /// </summary>
        Socket m_listenSocket;
        /// <summary>
        /// Sockets client -> id du socket.
        /// </summary>
        Dictionary<Socket, int> m_socketToIds;
        /// <summary>
        /// Id du socket -> socket client.
        /// </summary>
        Dictionary<int, Socket> m_idToSocket;
        /// <summary>
        /// Contains the number of consecutive timeouts for each socket.
        /// </summary>
        Dictionary<Socket, int> m_consecutiveTimeouts;


        object m_clientSocketsLock = new object();
        static byte[] s_smallBuffer = new byte[1];
        static byte[] s_buffer = new byte[512];
        #endregion

        public CommandServer()
        {
            m_socketToIds = new Dictionary<Socket, int>();
            m_idToSocket = new Dictionary<int, Socket>();
            m_consecutiveTimeouts = new Dictionary<Socket, int>();
        }

        #region Wait for connections
        /// <summary>
        /// Accepts incoming connexions until a call to StopWaitingForConnections is made.
        /// </summary>
        public void WaitForConnectionsAsync(int port, string address)
        {
            m_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_listenSocket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Thread thread = new Thread(new ThreadStart(() =>
            {

                try
                {
                    m_listenSocket.Listen(10);
                    int id = 0;
                    while (true)
                    {
                        Socket sock = m_listenSocket.Accept();
                        lock (m_clientSocketsLock)
                        {
                            m_socketToIds.Add(sock, id);
                            m_idToSocket.Add(id, sock);
                            m_consecutiveTimeouts.Add(sock, 0);
                        }

                        if (ClientConnected != null)
                            ClientConnected(id);

                        id++;
                    }

                }
                catch(ObjectDisposedException)
                {

                }
                catch (SocketException)
                {
                    // Ce bloc est éxécuté quand StopWaitingForConnections est appelé.
                    // Console.WriteLine("Exception : " + e.Message);
                }
            }));
            thread.Start();
        }

        /// <summary>
        /// Stops the server from waiting for incoming connexions.
        /// </summary>
        public void StopWaitingForConnections()
        {
            m_listenSocket.Close();
        }
        #endregion

        #region Run
        /// <summary>
        /// Démarre un thread par client, qui envoie / reçoit des commandes via un event.
        /// </summary>
        public void Start()
        {
            foreach(var kvp in m_idToSocket)
            {
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    while(true)
                    {
                        try
                        {
                            CommandReceived(kvp.Key, Receive(kvp.Key));
                        }
                        catch(SocketException)
                        {
                            // le client a planté.
                            break;
                        }
                    }
                }));
                thread.Start();
            }
        }
        #endregion

        #region Send / Receive
        public void Send(int clientId, string data)
        {
            Send(m_idToSocket[clientId], data);
        }
        /// <summary>
        /// Envoie une commande dans le socket.
        /// </summary>
        /// <param name="data"></param>
        public void Send(Socket s, string data)
        {
            try
            {
                s.Send(Encoding.UTF8.GetBytes(data.Length.ToString() + "\n" + data));
            }
            catch(SocketException e)
            {
                
            }
        }

        public string Receive(int clientId) { return Receive(m_idToSocket[clientId]); }
        /// <summary>
        /// Recoit une commande depuis le socket.
        /// </summary>
        /// <returns></returns>
        public string Receive(Socket s)
        {
            // Représente le caractère '\n'.
            byte last = Encoding.UTF8.GetBytes(new char[] { '\n' })[0];

            // Récupère le nombre de données à lire
            string dataLengthStr = "";
            while (true)
            {
                int bytes = s.Receive(s_smallBuffer);
                if (s_smallBuffer[0] == last)
                    break;
                dataLengthStr += Encoding.UTF8.GetString(s_smallBuffer);
            }


            string data = "";
            int dataLength = int.Parse(dataLengthStr);
            int totalBytes = 0;
            while (totalBytes < dataLength)
            {
                int bytes = s.Receive(s_buffer, Math.Min(dataLength - totalBytes, s_buffer.Length), SocketFlags.None);
                totalBytes += bytes;
                data += Encoding.UTF8.GetString(s_buffer, 0, bytes);
            }

            return data;
        }
        #endregion
    }
}
