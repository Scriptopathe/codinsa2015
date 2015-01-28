using System;
using System.Collections.Generic;
using System.Linq;
using Codinsa2015.Server;
using Codinsa2015.Graphics.Server;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace Codinsa2015
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameClient : Microsoft.Xna.Framework.Game
    {
        public static GameClient Instance;
        GraphicsDeviceManager m_graphics;
        GameServer m_server;



        public GameClient()
        {
            Instance = this;
            m_graphics = new GraphicsDeviceManager(this);
            m_server = new GameServer();

            Content.RootDirectory = "Content";
            m_graphics.PreferredBackBufferWidth = (int)GameServer.GetScreenSize().X;
            m_graphics.PreferredBackBufferHeight = (int)GameServer.GetScreenSize().Y;
            m_graphics.SynchronizeWithVerticalRetrace = false;
            m_graphics.IsFullScreen = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            m_server.Initialize(GraphicsDevice, Content);
            m_graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            m_server.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            m_server.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Mise à jour du serveur.
            m_server.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Dessine le contenu du serveur.
            m_server.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
