using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Clank.View.Engine;
using Clank.View.Engine.Entities;
using Clank.View.Engine.Gui;
using Clank.View.Engine.Editor;
using Clank.View.Engine.Particles;
namespace Clank.View
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
        /// Obtient ou définit le gestionnaire d'interface graphique.
        /// </summary>
        public GuiManager GuiManager
        {
            get;
            set;
        }
        /// <summary>
        /// Obtient ou définit une valeur indiquant si la map est en cours d'édition.
        /// </summary>
        public bool EditMode
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit le contrôleur permettant d'éditer la map.
        /// </summary>
        public MapEditorControler MapEditControler
        {
            get;
            set;
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
        /// Obtient une référence vers le gestionnaire de particules de la scène.
        /// </summary>
        public ParticleManager Particles
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient un dictionnaire des contrôleurs des différents héros, indexés 
        /// par l'id du client qui les contrôle.
        /// </summary>
        public Dictionary<int, Engine.Controlers.ControlerBase> Controlers
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de Clank.View.Scene.
        /// </summary>
        public Scene() { }

        /// <summary>
        /// Initialise les composants de la scène.
        /// </summary>
        public void Initialize()
        {
            if(System.IO.File.Exists("constants.xml"))
            {
                Constants = GameConstants.LoadFromFile("constants.xml");
            }
            else
            {
                Constants = new GameConstants();
                Constants.Save("constants.xml");
            }

            Controlers = new Dictionary<int, Engine.Controlers.ControlerBase>();
            Map = new Map();
            EventSheduler = new Scheduler();
            GuiManager = new GuiManager();
            MapEditControler = new MapEditorControler(Map);
            RewardSystem = new RewardSystem(Map.Heroes);
            Particles = new ParticleManager();

            // DEBUG
            Controlers.Add(0, new Engine.Controlers.HumanControler(Map.Heroes[0]));
        }
        /// <summary>
        /// Charge le contenu de la scène.
        /// </summary>
        public void LoadContent()
        {

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
            foreach (var kvp in Controlers) { kvp.Value.Update(time); }

            // Mets à jour les récompenses.
            RewardSystem.Update(time);

            // Mets à jour le contrôleur
            MapEditControler.Update(time);

            // Mets à jour la gui
            GuiManager.Update(time);

            // Mets à jour les particules.
            Particles.Update(time);

            
            // Mets à jour l'input
            Input.Update();
        }

        /// <summary>
        /// Dessine la scène.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="batch"></param>
        public void Draw(GameTime time, SpriteBatch batch)
        {
            Map.Draw(time, batch);
            batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            GuiManager.Draw(batch);
            MapEditControler.Draw(batch);
            Particles.Draw(batch, new Vector2(Map.Viewport.X, Map.Viewport.Y), Map.ScrollingVector2);
            foreach (var kvp in Controlers) { kvp.Value.Draw(batch, time); }
            batch.End();
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
