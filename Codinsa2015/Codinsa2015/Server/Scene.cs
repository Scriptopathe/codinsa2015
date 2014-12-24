using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server;
using Codinsa2015.Server.Entities;
using Codinsa2015.Server.Gui;
using Codinsa2015.Server.Editor;
using Codinsa2015.Server.Particles;
using Codinsa2015.Graphics.Server;
using Codinsa2015.Graphics.Client;
namespace Codinsa2015.Server
{
    /// <summary>
    /// Représente un mode de fonctionnement de la scène.
    /// </summary>
    [Clank.ViewCreator.Enum("Représente un mode de fonctionnement de la scène.")]
    public enum SceneMode
    {
        Lobby,
        Pick,
        Game
    }
    /// <summary>
    /// Scène du moteur de jeu.
    /// </summary>
    public class Scene
    {
        #region Variables

        /// <summary>
        /// Queue de commandes reçues.
        /// </summary>
        Queue<Tuple<int, string>> m_commands;
        #endregion

        #region Properties
        /// <summary>
        /// Représente le mode de fonctionnement actuel de la scène.
        /// </summary>
        public SceneMode Mode { get; set; }
        /// <summary>
        /// Map sur laquelle se situe la scène.
        /// </summary>
        public Map Map { get; set; }
        /// <summary>
        /// Obtient le planificateur d'évènements de la scène.
        /// </summary>
        public Scheduler EventSheduler { get; private set; }

        /// <summary>
        /// Obtient une référence vers le conteneur de constantes du jeu.
        /// </summary>
        public GameConstants Constants { get; set; }

        /// <summary>
        /// Obtient une référence vers le système de récompenses.
        /// </summary>
        public RewardSystem RewardSystem { get; set; }

        /// <summary>
        /// Obtient une référence vers le serveur de commandes.
        /// </summary>
        public Net.CommandServer CommandServer { get; set; }

        /// <summary>
        /// Obtient un dictionnaire des contrôleurs des différents héros, indexés 
        /// par l'id du client qui les contrôle.
        /// </summary>
        public Dictionary<int, Server.Controlers.ControlerBase> Controlers { get; set; }

        /// <summary>
        /// Obtient ou définit une référence vers l'interpréteur de commandes de la scène.
        /// </summary>
        public PonyCarpetExtractor.Interpreter GameInterpreter { get; set; }

        /// <summary>
        /// Obtient le serveur graphique utilisé par ce contrôleur.
        /// </summary>
        public GraphicsServer GraphicsServer { get; set; }

        /// <summary>
        /// Render target principal.
        /// </summary>
        public RemoteRenderTarget MainRenderTarget { get; set; }

        /// <summary>
        /// Obtient une référence vers le contrôleur entrain d'effectuer des opérations.
        /// </summary>
        public Server.Controlers.ControlerBase CurrentControler { get; set; }

        /// <summary>
        /// Obtient une référence vers l'état du serveur.
        /// </summary>
        public Views.State State { get; set; }

        object m_controlerLock = new object();
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de Codinsa2015.Scene.
        /// </summary>
        public Scene() 
        {
            m_commands = new Queue<Tuple<int, string>>();
            Mode = SceneMode.Game;
        }

        /// <summary>
        /// Initialise les composants de la scène.
        /// </summary>
        public void Initialize()
        {
            // Serveur de commandes
            CommandServer = new Net.CommandServer();
            CommandServer.ClientConnected += CommandServer_ClientConnected;
            CommandServer.CommandReceived += CommandServer_CommandReceived;

            // Serveur graphique.
            GraphicsServer = new Graphics.Server.GraphicsServer(Graphics.Server.GraphicsServer.CommandExecutionMode.Immediate, GameServer.Instance.Content);

            // State
            State = new Views.State();

            // Controleurs.
            Controlers = new Dictionary<int, Server.Controlers.ControlerBase>();

            InitializeGameMode();
        }

        /// <summary>
        /// Initialise le mode de jeu de la scène.
        /// </summary>
        void InitializeGameMode()
        {
            // Initialise l'interpréteur de commandes.
            InitializeInterpreter();

            // Charge les constantes du jeu.
            LoadConstants();


            // Création de la map.
            Map = new Map();

            // Création de l'event scheduler.
            EventSheduler = new Scheduler();

            // Création du système de récompenses.
            RewardSystem = new RewardSystem(Map.Heroes);

            // Charge les variables dans l'interpréteur.
            GameInterpreter.MainContext.LocalVariables.Add("map", new PonyCarpetExtractor.ExpressionTree.Mutable(GameServer.GetMap()));
            GameInterpreter.MainContext.LocalVariables.Add("scene", new PonyCarpetExtractor.ExpressionTree.Mutable(GameServer.GetScene()));
            GameInterpreter.Eval("print = function(arg) { Interpreter.Puts(arg); };");
        }

        /// <summary>
        /// Charge les constantes de jeu.
        /// </summary>
        void LoadConstants()
        {
            if (System.IO.File.Exists("constants.xml"))
            {
                Constants = GameConstants.LoadFromFile("constants.xml");
            }
            else
            {
                Constants = new GameConstants();
                Constants.Save("constants.xml");
            }
        }

        /// <summary>
        /// Initialise l'interpreteur de commandes.
        /// </summary>
        void InitializeInterpreter()
        {
            GameInterpreter = new PonyCarpetExtractor.Interpreter();

            GameInterpreter.MainContext.GlobalContext.LoadedAssemblies = new Dictionary<string, System.Reflection.Assembly>() 
            {
                { System.Reflection.Assembly.GetExecutingAssembly().FullName, System.Reflection.Assembly.GetExecutingAssembly()},
                { System.Reflection.Assembly.GetAssembly(typeof(Vector2)).FullName, System.Reflection.Assembly.GetAssembly(typeof(Vector2))},
                { System.Reflection.Assembly.GetAssembly(typeof(System.Windows.Forms.Form)).FullName, System.Reflection.Assembly.GetAssembly(typeof(System.Windows.Forms.Form)) }
            };

            GameInterpreter.MainContext.GlobalContext.LoadedNamespaces = new List<string>() 
            {
                "System", 
                "System.Collections.Generic",
                "Microsoft.Xna.Framework",
                "Clank",
                "Codinsa2015.Engine",
                "Codinsa2015.Engine.Entities",
                "Codinsa2015.Engine.Equip",
                "Codinsa2015.Engine.Controlers",
                "Codinsa2015.Engine.Graphics",
                "Codinsa2015.Engine.Spells",
                "Codinsa2015.Engine.Spellcasts",
                "Codinsa2015.Engine.Spells",
                "Codinsa2015.Engine.Views",
                "System.Windows.Forms"
            };

        }

        /// <summary>
        /// Callback appelé lorsqu'une commande est envoyée au serveur par un client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        void CommandServer_CommandReceived(int clientId, string command)
        {
            m_commands.Enqueue(new Tuple<int, string>(clientId, command));
        }

        /// <summary>
        /// Callback appelé lorsqu'un client se connecte au serveur.
        /// </summary>
        /// <param name="clientId"></param>
        void CommandServer_ClientConnected(int clientId)
        {
            EntityHero hero = new EntityHero() { Type = EntityType.Team2 | EntityType.Player };
            Map.Heroes.Add(hero); 

            lock(m_controlerLock)
                Controlers.Add(clientId, new Controlers.IAControler(hero));
            Map.Entities.Add(hero.ID, hero);
            // DEBUG
            CommandServer.StopWaitingForConnections();
            CommandServer.Start();
        }

        /// <summary>
        /// Envoie la queue de commandes au serveur.
        /// </summary>
        void CommandServer_Update()
        {
            while(m_commands.Count != 0)
            {
                var tup = m_commands.Dequeue();
                CommandServer.Send(tup.Item1, State.ProcessRequest(tup.Item2, tup.Item1));
            }
        }
        /// <summary>
        /// Charge les contrôleurs du jeu.
        /// </summary>
        public void LoadControlers()
        {
            // Charge les contrôleurs.
            var humanControler = new Server.Controlers.HumanControler(Map.Heroes[0]);
            Controlers.Add(1, humanControler);
            GameInterpreter.MainContext.LocalVariables.Add("ctrl", new PonyCarpetExtractor.ExpressionTree.Mutable(humanControler));
            
            // DEBUG
            CommandServer.WaitForConnectionsAsync(Codinsa2015.Graphics.Client.RemoteClient.__DEBUG_PORT, "127.0.0.1");
        }

        /// <summary>
        /// Se produit lorsqu'une nouvelle map est chargée.
        /// </summary>
        public void LoadMap(Map map)
        {
            Map = map;
            Controlers.Clear();

            // DEBUG
            Controlers.Add(0, new Server.Controlers.HumanControler(Map.Heroes[0]));
            GameInterpreter.MainContext.LocalVariables["map"] = new PonyCarpetExtractor.ExpressionTree.Mutable(GameServer.GetMap());
            RewardSystem = new RewardSystem(Map.Heroes);
        }

        /// <summary>
        /// Lie les clients graphiques des contrôleurs au serveur graphique.
        /// </summary>
        public void BindGraphicsClients()
        {
            foreach(var kvp in Controlers) { CurrentControler = kvp.Value; kvp.Value.BindGraphicsClient(GraphicsServer); };
        }

        /// <summary>
        /// Charge le contenu de la scène.
        /// </summary>
        public void LoadContent()
        {
            // Charge le content des contrôleurs.
            lock(m_controlerLock)
                foreach (var kvp in Controlers) { CurrentControler = kvp.Value; kvp.Value.LoadContent(); }

            MainRenderTarget = new RemoteRenderTarget(GraphicsServer, (int)GameServer.GetScreenSize().X, (int)GameServer.GetScreenSize().Y, RenderTargetUsage.PreserveContents);
            Map.LoadContent();
        }

        /// <summary>
        /// Mets à jour la scène.
        /// </summary>
        public void Update(GameTime time)
        {
            switch(Mode)
            {
                case SceneMode.Game:
                    UpdateGameMode(time);
                    break;
            }
        }

        /// <summary>
        /// Mets à jour la scène en mode "Game".
        /// </summary>
        public void UpdateGameMode(GameTime time)
        {
            // Mets à jour l'event scheduler.
            EventSheduler.Update(time);

            // Mise à jour du serveur de commandes.
            CommandServer_Update();

            // Mets à jour la map
            Map.Update(time);

            // Mets à jour les contrôleurs
            lock (m_controlerLock)
                foreach (var kvp in Controlers) { CurrentControler = kvp.Value; kvp.Value.Update(time); }

            // Mets à jour les récompenses.
            RewardSystem.Update(time);

            // Mets à jour l'input
            Input.Update();
        }

        /// <summary>
        /// Dessine la scène.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="batch"></param>
        public void Draw(GameTime time, RemoteSpriteBatch batch)
        {
            switch(Mode)
            {
                case SceneMode.Game:
                    DrawGameMode(time, batch);
                    break;
            }
        }

        /// <summary>
        /// Dessine la scène en mode "Game".
        /// </summary>
        /// <param name="time"></param>
        /// <param name="batch"></param>
        void DrawGameMode(GameTime time, RemoteSpriteBatch batch)
        {

            lock (m_controlerLock)
                foreach (var kvp in Controlers) { CurrentControler = kvp.Value; kvp.Value.Draw(batch, time); GraphicsServer.Flush(); }
        }
        /// <summary>
        /// Supprime les ressources allouées.
        /// </summary>
        public void Dispose()
        {
            
        }
        #endregion
    }
}
