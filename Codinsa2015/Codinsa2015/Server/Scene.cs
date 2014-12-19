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
    /// Scène du moteur de jeu.
    /// </summary>
    public class Scene
    {
        #region Variables

        #endregion

        #region Properties
        /// <summary>
        /// Map sur laquelle se situe la scène.
        /// </summary>
        public Map Map { get; set; }
        /// <summary>
        /// Obtient le planificateur d'évènements de la scène.
        /// </summary>
        public Scheduler EventSheduler
        {
            get;
            private set;
        }


        /// <summary>
        /// Obtient une référence vers le conteneur de constantes du jeu.
        /// </summary>
        public GameConstants Constants
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient une référence vers le système de récompenses.
        /// </summary>
        public RewardSystem RewardSystem
        {
            get;
            set;
        }


        /// <summary>
        /// Obtient un dictionnaire des contrôleurs des différents héros, indexés 
        /// par l'id du client qui les contrôle.
        /// </summary>
        public Dictionary<int, Server.Controlers.ControlerBase> Controlers
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit une référence vers l'interpréteur de commandes de la scène.
        /// </summary>
        public PonyCarpetExtractor.Interpreter GameInterpreter
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient le serveur graphique utilisé par ce contrôleur.
        /// </summary>
        public GraphicsServer GraphicsServer
        {
            get;
            set;
        }

        /// <summary>
        /// Render target principal.
        /// </summary>
        public RemoteRenderTarget MainRenderTarget
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient une référence vers le contrôleur entrain d'effectuer des opérations.
        /// </summary>
        public Server.Controlers.ControlerBase CurrentControler
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de Codinsa2015.Scene.
        /// </summary>
        public Scene() { }

        /// <summary>
        /// Initialise les composants de la scène.
        /// </summary>
        public void Initialize()
        {
            // Initialise l'interpréteur de commandes.
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
            

            if(System.IO.File.Exists("constants.xml"))
            {
                Constants = GameConstants.LoadFromFile("constants.xml");
            }
            else
            {
                Constants = new GameConstants();
                Constants.Save("constants.xml");
            }

            Controlers = new Dictionary<int, Server.Controlers.ControlerBase>();

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

            // Serveur graphique.
            GraphicsServer = new Graphics.Server.GraphicsServer(Graphics.Server.GraphicsServer.CommandExecutionMode.Immediate, GameServer.Instance.Content);
        }

        /// <summary>
        /// Charge les contrôleurs du jeu.
        /// </summary>
        public void LoadControlers()
        {
            // Charge les contrôleurs.
            var humanControler = new Server.Controlers.HumanControler(Map.Heroes[0]);
            Controlers.Add(0, humanControler);
            GameInterpreter.MainContext.LocalVariables.Add("ctrl", new PonyCarpetExtractor.ExpressionTree.Mutable(humanControler));
        }
        /// <summary>
        /// Se produit lorsqu'une nouvelle map est chargée.
        /// </summary>
        public void LoadMap(Map map)
        {
            Map = map;
            Controlers.Clear();
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
            foreach (var kvp in Controlers) { CurrentControler = kvp.Value; kvp.Value.LoadContent(); }

            MainRenderTarget = new RemoteRenderTarget(GraphicsServer, (int)GameServer.GetScreenSize().X, (int)GameServer.GetScreenSize().Y, RenderTargetUsage.PreserveContents);
            
            Map.LoadContent();




        }

        /// <summary>
        /// Mets à jour la scène.
        /// </summary>
        public void Update(GameTime time)
        {
            // Mets à jour l'event scheduler.
            EventSheduler.Update(time);

            // Mets à jour la map
            Map.Update(time);

            // Mets à jour les contrôleurs
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
