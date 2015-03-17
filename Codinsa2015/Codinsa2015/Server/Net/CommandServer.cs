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
        public delegate void ClientConnectedDelegate(int clientId, string nickname);
        public delegate void CommandReceivedDelegate(int clientId, byte[] command);

        /// <summary>
        /// Event lancé lorsqu'un client se connecte.
        /// </summary>
        public event ClientConnectedDelegate ClientConnected;
        /// <summary>
        /// Event lancé lorsqu'une commande est reçue.
        /// </summary>
        public event CommandReceivedDelegate CommandReceived;


        /// <summary>
        /// Représente l'encoding UTF8 sans BOM.
        /// </summary>
        public static Encoding UTF8 = new UTF8Encoding(false);
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
        Dictionary<int, byte[]> m_smallBuffer;
        Dictionary<int, byte[]> m_buffer;
        #endregion

        #region Properties
        public bool IsWaitingForConnections { get; set; }
        #endregion
        public CommandServer()
        {
            m_socketToIds = new Dictionary<Socket, int>();
            m_idToSocket = new Dictionary<int, Socket>();
            m_consecutiveTimeouts = new Dictionary<Socket, int>();
            m_smallBuffer = new Dictionary<int, byte[]>();
            m_buffer = new Dictionary<int, byte[]>();

            // Change la culture pour obtenir des floats au format en-us.
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
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
                    IsWaitingForConnections = true;
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
                            m_smallBuffer.Add(id, new byte[1]);
                            m_buffer.Add(id, new byte[512]);
                        }

                        string name = UTF8.GetString(Receive(id));
                        if (ClientConnected != null)
                            ClientConnected(id, name);

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
                finally
                {
                    IsWaitingForConnections = false;
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
            IsWaitingForConnections = false; // force
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
        public void Send(int clientId, byte[] data)
        {
            Send(m_idToSocket[clientId], data);
        }
        /// <summary>
        /// Envoie une commande dans le socket.
        /// </summary>
        /// <param name="data"></param>
        public void Send(Socket s, byte[] data)
        {
#if DEBUG
            string d = UTF8.GetString(data);
            string d2 = UTF8.GetString(UTF8.GetBytes(data.Length.ToString() + "\n"));
#endif
            try
            {
                s.Send(UTF8.GetBytes(data.Length.ToString() + "\n"));
                s.Send(data);
            }
            catch(SocketException e)
            {
                
            }
        }

        public byte[] Receive(int clientId) { return Receive(m_idToSocket[clientId], clientId); }
        /// <summary>
        /// Recoit une commande depuis le socket.
        /// </summary>
        /// <returns></returns>
        public byte[] Receive(Socket s, int clientId)
        {
            // Représente le caractère '\n'.
            byte last = UTF8.GetBytes(new char[] { '\n' })[0];

            // Récupère le nombre de données à lire
#if DEBUG
            List<byte> allBytes = new List<byte>();
#endif
            List<byte> dataBytes = new List<byte>();
            while (true)
            {
                int bytes = s.Receive(m_smallBuffer[clientId]);
                if (m_smallBuffer[clientId][0] == last)
                    break;
                dataBytes.Add(m_smallBuffer[clientId][0]);
#if DEBUG
                allBytes.Add(m_smallBuffer[clientId][0]);
#endif
            }


            int dataLength = int.Parse(UTF8.GetString(dataBytes.ToArray()));
            dataBytes.Clear();
            int totalBytes = 0;
            while (totalBytes < dataLength)
            {
                int bytes = s.Receive(m_buffer[clientId], Math.Min(dataLength - totalBytes, m_buffer[clientId].Length), SocketFlags.None);
                totalBytes += bytes;
                for (int i = 0; i < bytes; i++)
                {
                    dataBytes.Add(m_buffer[clientId][i]);
#if DEBUG
                    allBytes.Add(m_buffer[clientId][i]);
#endif
                }
            }

#if DEBUG
            string data = UTF8.GetString(allBytes.ToArray());

#endif
            return dataBytes.ToArray();
        }
        #endregion
    }
}
