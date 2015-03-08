using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Server.Particles
{
    /// <summary>
    /// Classe abstraite de base représentant une particule.
    /// </summary>
    public abstract class Particle
    {
        /// <summary>
        /// Mets à jour la particule.
        /// </summary>
        public abstract void Update(GameTime time);
        /// <summary>
        /// Dessine la particule à l'écran.
        /// </summary>
        /// <param name="batch"></param>
        public abstract void Draw(SpriteBatch batch, Vector2 viewportOffset, Vector2 scrollingOffset);
        /// <summary>
        /// Libère la mémoire utilisée par cette particule.
        /// </summary>
        public abstract void Dispose();
        /// <summary>
        /// Variable indiquant si la particule a été supprimée.
        /// </summary>
        public abstract bool IsDisposed { get; set; }
    }
}