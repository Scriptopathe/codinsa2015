using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Rendering.Particles
{
    /// <summary>
    /// Classe représentant une particule gérant les effets et affichant une texture.
    /// </summary>
    public class ParticleTexture : ParticleBase
    {

        #region Variables

        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit la texture affiché par la particule.
        /// </summary>
        public Texture2D Texture
        {
            get;
            set;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de ParticleText.
        /// </summary>
        public ParticleTexture(ParticleManager mgr)
            : base(mgr)
        {

        }
        /// <summary>
        /// Mets à jour la particule.
        /// </summary>
        public override void Update(GameTime time)
        {
            base.Update(time);
        }
        /// <summary>
        /// Dessine la particule à l'écran.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch, Vector2 viewportOffset, Vector2 scrollingOffset)
        {

            batch.Draw(Texture, this.CurrentPosition, CurrentColor);
        }
        /// <summary>
        /// Libère la mémoire utilisée par cette particule.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }
        /// <summary>
        /// Variable indiquant si la particule a été supprimée.
        /// </summary>
        public override bool IsDisposed
        {
            get;
            set;
        }
        #endregion
    }
}