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

namespace Clank.View
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Mobattack : Microsoft.Xna.Framework.Game
    {
        public static Mobattack Instance;
        GraphicsDeviceManager m_graphics;
        SpriteBatch m_spriteBatch;
        Scene m_scene;

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
        public static Engine.Map GetMap()
        {
            return Instance.m_scene.Map;
        }

        public Mobattack()
        {
            Instance = this;
            m_graphics = new GraphicsDeviceManager(this);
            m_scene = new Scene();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Engine.Input.ModuleInit();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            Engine.Ressources.LoadRessources(Content);
            m_scene.LoadContent();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            // TODO: Add your update logic here
            m_scene.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            m_spriteBatch.Begin();
            m_scene.Draw(gameTime, m_spriteBatch);
            m_spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
