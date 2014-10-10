using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
namespace Clank.Server
{

    /// <summary>
    /// La classe Engine contient à la fois l'état du serveur, ainsi qu'une référence vers 
    /// le Serveur (qui s'occupe des échanges de données client/serveur).
    /// Cette classe fait la liason entre les messages entrants, et l'état du serveur.
    /// </summary>
    public class Engine
    {
        #region Variables
        /// <summary>
        /// Contient l'état du moteur.
        /// </summary>
        State m_state;
        /// <summary>
        /// Serveur permettant la communication avec les clients.
        /// </summary>
        Server m_server;
        /// <summary>
        /// Queue des messages à transmettre à l'état du moteur.
        /// </summary>
        Queue<Tuple<string, int>> m_messageQueue;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient une référence vers l'état du serveur.
        /// </summary>
        public State State
        {
            get { return m_state; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de Clank.Server.Engine.
        /// </summary>
        public Engine()
        {
            m_state = new State();
            m_server = new Server();
            m_messageQueue = new Queue<Tuple<string, int>>();
            m_server.MessageReceived += OnMessageReceived;
            m_server.EngineUpdate += UpdateState;
        }
        #region API
        /// <summary>
        /// Fait tourner le moteur de jeu sur une frame.
        /// </summary>
        public void RunOneFrame()
        {

        }
        #endregion

        #region Communication
        /// <summary>
        /// Initialise le serveur sur le port donné.
        /// </summary>
        /// <param name="port"></param>
        public void InitializeServer(int port)
        {
            m_server.Initialize(port);
        }
        /// <summary>
        /// Demande au serveur d'accepter les connexions clientes entrantes jusqu'à un 
        /// appel à StopWaitingForConnections.
        /// </summary>
        public void WaitForConnectionsAsync()
        {
            m_server.WaitForConnectionsAsync();
        }
        /// <summary>
        /// Demande au serveur d'arrêter la phase de recherche de connexions entrantes.
        /// </summary>
        public void StopWaitingForConnections()
        {
            m_server.StopWaitingForConnections();
        }
        /// <summary>
        /// Starts the server asynchronously.
        /// </summary>
        /// <param name="refreshRate">Fréquence à laquelle le serveur va envoyer des messages. (en Hz)</param>
        public void StartServerAsync(int refreshRate=10)
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                float durationMs = 1000.0f / refreshRate;
                
                while(true) // TODO : rajouter une condition d'arrêt.
                {
                    DateTime now = DateTime.Now;
                    m_server.RunOnFrame();
                    TimeSpan elapsed = DateTime.Now - now;
                    System.Threading.Thread.Sleep((int)Math.Max(1, (durationMs - elapsed.TotalMilliseconds)));
                }
            }));
            thread.Start();
        }

        object m_processingLock = new object();
        /// <summary>
        /// Effectue une mise à jour de l'état du moteur de jeu.
        /// </summary>
        void UpdateState()
        {
            while (m_messageQueue.Count != 0)
            {
                var item = m_messageQueue.Dequeue();
                string response = m_state.ProcessRequest(item.Item1, item.Item2);
                m_server.SendResponse(response, item.Item2);
            }
        }

        /// <summary>
        /// Méthode appelée lorsqu'un message est reçu par le serveur.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="clientId"></param>
        void OnMessageReceived(string msg, int clientId)
        {
            m_messageQueue.Enqueue(new Tuple<string, int>(msg, clientId));
        }
        #endregion
        #endregion
    }
}
