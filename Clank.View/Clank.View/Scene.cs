using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Clank.View.Engine;
using Clank.View.Engine.Gui;
using Clank.View.Engine.Editor;
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
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de Clank.View.Scene.
        /// </summary>
        public Scene()
        {
            Map = new Map();
            EventSheduler = new Scheduler();
            GuiManager = new GuiManager();
            MapEditControler = new MapEditorControler(Map);
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

            // Mets à jour le contrôleur
            MapEditControler.Update(time);

            // Mets à jour la gui
            GuiManager.Update(time);


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
            GuiManager.Draw(batch);
            MapEditControler.Draw(batch);
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
