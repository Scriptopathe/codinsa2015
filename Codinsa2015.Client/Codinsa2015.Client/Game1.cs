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
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Xml.Serialization;
namespace Codinsa2015.Client
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Codinsa2015.Graphics.Client.IntegratedClient client;
        Socket m_socket;
        byte[] buffer;
        XmlSerializer serializer = new XmlSerializer(typeof(Graphics.Server.Command));
        public static Vector2 GetScreenSize()
        {
            return new Vector2(1366, 768 + 100);
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = (int)GetScreenSize().X;
            graphics.PreferredBackBufferHeight = (int)GetScreenSize().Y;
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
            
            base.Initialize();
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Graphics.Client.RemoteClient.__DEBUG_PORT));
            buffer = new byte[512];
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            client = new Graphics.Client.IntegratedClient(GraphicsDevice, Content);
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

            base.Update(gameTime);
        }

        List<Graphics.Server.Command> RetrieveCommand()
        {
            DateTime t = DateTime.Now;
            List<Graphics.Server.Command> cmds = new List<Graphics.Server.Command>();

            string var = "";
            while(!var.EndsWith("</Command>"))
            {
                int bytes = m_socket.Receive(buffer);
                var += System.Text.Encoding.UTF8.GetString(buffer, 0, bytes);
            }

            string[] vars = var.Split(new string[] { "<?xml version=\"1.0\"?>" }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach(string v in vars)
            {
                StringReader reader = new StringReader("<?xml version=\"1.0\"?>" + v);
                Graphics.Server.Command cmd = (Graphics.Server.Command)serializer.Deserialize(reader);
                cmds.Add(cmd);
            }

            double elapsed = (t - DateTime.Now).TotalSeconds;
            return cmds;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            List<Graphics.Server.Command> cmd;
            bool ended = false;
            while(!ended)
            {
                cmd = RetrieveCommand();
                foreach (Graphics.Server.Command c in cmd)
                {
                    if (c is Graphics.Server.CommandEndFrame)
                    {
                        ended = true;
                        break;
                    }
                    else
                        client.ProcessCommand(c);
                }
            }

            base.Draw(gameTime);
        }
    }
}
