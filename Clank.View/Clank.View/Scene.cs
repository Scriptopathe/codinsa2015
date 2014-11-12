using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Clank.View.Engine;
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
        #endregion
        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de Clank.View.Scene.
        /// </summary>
        public Scene()
        {
            Map = new Map();
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
            Map.Update(time);
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
