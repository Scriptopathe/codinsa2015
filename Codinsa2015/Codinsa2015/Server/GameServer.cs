using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace Codinsa2015.Server
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameServer
    {
        public static GameServer Instance;
        SpriteBatch m_spriteBatch;
        Scene m_scene;
        GameTime m_time;


        /// <summary>
        /// Obtient une référence vers le gestionnaire de contenus.
        /// </summary>
        public ContentManager Content
        {
            get;
            set;
        }


        /// <summary>
        /// Obtient une référence vers l'instance de GraphicsDevice utilisée par le serveur.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get;
            set;
        }
        /// <summary>
        /// Obtient le temps de jeu actuel.
        /// </summary>
        /// <returns></returns>
        public static GameTime GetTime()
        {
            return Instance.m_time;
        }
        

        /// <summary>
        /// Obtient la taille de l'écran.
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetScreenSize()
        {
            return new Vector2(1366, 768 + 100);//new Vector2(800, 600 + 100);//new Vector2(1366, 768 + 100);
        }

        /// <summary>
        /// Retourne la scène associée à ce jeu.
        /// </summary>
        /// <returns></returns>
        public static Scene GetScene()
        {
            return Instance.m_scene;
        }

        /// <summary>
        /// Retourne la map de la scène.
        /// </summary>
        /// <returns></returns>
        public static Server.Map GetMap()
        {
            return Instance.m_scene.Map;
        }

        /// <summary>
        /// Crée une nouvelle instance du serveur.
        /// </summary>
        /// <param name="content"></param>
        public GameServer()
        {
            Instance = this;
            m_scene = new Scene();
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public void Initialize(GraphicsDevice device, ContentManager content)
        {
            if (device == null || content == null)
                throw new ArgumentNullException();
            
            // Récupère la GraphicsDevice et le ContentManager.
            GraphicsDevice = device;
            Content = content;

            // Initialisation du module d'input.
            Server.Input.ModuleInit();
            // Initialisation de la scène, et chargement des contrôleurs.
            m_scene.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            Content = content;
            Content.RootDirectory = "Content";

            // Charge les ressources (doit être fait après l'appel à BindGraphicsClients()).
            Server.Ressources.LoadRessources(GraphicsDevice, Content);
            // Charge le contenu de la scène.
            m_scene.LoadContent();
            // Crée le batch principal.
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void UnloadContent()
        {
            m_scene.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            m_time = gameTime;

            // Mise à jour de la scène.
            m_scene.Update(gameTime);

            // Mets à jour l'input
            Input.Update();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            // Dessin de la scène.
            m_scene.Draw(gameTime, m_spriteBatch);
        }
    }
}
