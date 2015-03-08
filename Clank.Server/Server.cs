using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace Clank.Server
{
    /// <summary>
    /// Représente le serveur.
    /// 
    /// Différents types de synchro :
    ///     - Update du serveur chaque X ms
    ///     - Attente des clients (avec timeout).
    ///     
    /// </summary>
    public class Server
    {
        #region Delegates / Events
        public delegate void MessageEventHandler(string msg, int clientId);
        public delegate void UpdateEventHandler();
        /// <summary>
        /// Event fired when a message from a client is received by the server.
        /// </summary>
        public event MessageEventHandler MessageReceived;
        #endregion

        #region Variable
        Socket m_listenSocket;
        object m_clientSocketsLock = new object();
        List<Socket> m_clientSockets;
        Dictionary<Socket, int> m_socketToIds;
        Dictionary<int, Socket> m_idToSocket;

        /// <summary>
        /// Contains the number of consecutive timeouts for each socket.
        /// </summary>
        Dictionary<Socket, int> m_consecutiveTimeouts;
        #endregion

        #region Properties
        /// <summary>
        /// Timeout for the reception of data from one client.
        /// </summary>
        public int ReceiveTimeout { get; set; }
        /// <summary>
        /// Gets or sets a value indicating if the server should disconnect clients when they
        /// timeout many times in a row.
        /// </summary>
        public bool DisconnectClientOnConsecutiveTimeouts { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the server with a port number.
        /// </summary>
        public void Initialize(int port)
        {
            m_clientSockets = new List<Socket>();
            m_socketToIds = new Dictionary<Socket, int>();
            m_idToSocket = new Dictionary<int, Socket>();
            m_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_listenSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
            m_consecutiveTimeouts = new Dictionary<Socket, int>();
            ReceiveTimeout = 2000;
        }

        /// <summary>
        /// Accepts incoming connexions until a call to StopWaitingForConnections is made.
        /// </summary>
        public void WaitForConnectionsAsync()
        {
            
            Thread thread = new Thread(new ThreadStart(() =>
            {
                
                try
                {
                    m_listenSocket.Listen(10);
                    int id = 0;
                    while (true)
                    {
                        Socket sock = m_listenSocket.Accept();
                        Console.WriteLine("Client connected. Id: " + id);
                        lock (m_clientSocketsLock)
                        {
                            m_clientSockets.Add(sock);
                            m_socketToIds.Add(sock, id);
                            m_idToSocket.Add(id, sock);
                            m_consecutiveTimeouts.Add(sock, 0);
                        }

                        id++;
                    }

                }
                catch(SocketException e)
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

        /// <summary>
        /// Runs one server frame.
        /// </summary>
        public void RunOnFrame()
        {
            // List of invalid sockets.
            List<Socket> invalidSockets = new List<Socket>();

            foreach(Socket sock in m_clientSockets)
            {
                // Récupération de la commande.
                string command = String.Empty;
                try
                {
                    Console.WriteLine("Retrieving command from client " + m_socketToIds[sock]);
                    command = Receive(sock, ReceiveTimeout);
                    Console.WriteLine("Command: \"" + command + "\" retrieved.");
                    m_consecutiveTimeouts[sock] = 0;
                }
                catch (SocketException e) 
                {
                    if (e.SocketErrorCode != SocketError.TimedOut)
                        invalidSockets.Add(sock);
                    else if(DisconnectClientOnConsecutiveTimeouts)
                    {
                        // If there are too many consecutive timeouts, the client may have entered
                        // infinite loop.
                        m_consecutiveTimeouts[sock]++;
                        if (m_consecutiveTimeouts[sock] == 5)
                            invalidSockets.Add(sock);
                    }
                }

                // Executes the command.
                if (command != String.Empty)
                    MessageReceived(command, m_socketToIds[sock]);
            }
            
            // Deletes invalid sockets.
            foreach (Socket sock in invalidSockets)
            {
                // Closes the socket
                try { sock.Close(); }
                catch { }

                m_clientSockets.Remove(sock);
                Console.Write("Client n°" + m_socketToIds[sock] + " crashed or went into an infinite loop.");
            }

            // Fires engine update
            EngineUpdate();
        }

        /// <summary>
        /// Send a response to the given client.
        /// </summary>
        /// <param name="response">Response to give to the client.</param>
        /// <param name="clientId">Client's id.</param>
        public void SendResponse(string response, int clientId)
        {
            Socket sock = m_idToSocket[clientId];
            sock.Send(Encoding.UTF8.GetBytes(response));
        }

        /// <summary>
        /// Receives a string from the given socket.
        /// </summary>
        /// <returns></returns>
        string Receive(Socket sock, int timeout)
        {
            sock.ReceiveTimeout = timeout;
            int bytesReceived = 0;
            byte[] buff = new byte[4096];
            StringBuilder builder = new StringBuilder();
            do
            {
                bytesReceived = sock.Receive(buff);
                builder.Append(Encoding.UTF8.GetString(buff, 0, bytesReceived));
            } while (bytesReceived == buff.Length);
            return builder.ToString();
        }

        #endregion
    }
}
