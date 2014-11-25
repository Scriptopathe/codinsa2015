using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Particles
{
    /// <summary>
    /// Classe représentant une particule gérant les effets et affichant une texture.
    /// </summary>
    public class ParticleAnimation : ParticleBase
    {
        static Random s_rand = new Random();

        #region Variables
        private int m_counter;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit le charset d'animation de la texture.
        /// </summary>
        public Graphics.Charset Charset
        {
            get;
            set;
        }

        public int AnimationFrames
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de ParticleText.
        /// </summary>
        public ParticleAnimation(GameTime time, Vector2 startPos, Vector2 endPos)
            : base()
        {
            StartPosition = startPos;
            CurrentPosition = startPos;
            FadeBaseColor = new Color(255, 255, 255, 255);
            FadeInStartColor = new Color(255, 255, 255, 255);
            FadeOutEndColor = new Color(255, 255, 255, 255);
            FadeInDelay = 0.0f;
            FadeInDuration = 0.2f;
            FadeOutDuration = 0.2f;
            CurrentColor = FadeInStartColor;
            AnimationFrames = 8;
            MoveFunction = ParticleBase.MoveLine(endPos);
        }

        /// <summary>
        /// Mets à jour la particule.
        /// </summary>
        public override void Update(GameTime time)
        {
            // Compteur d'animation.
            m_counter++;
            if (m_counter >= Charset.WalkFrames * 8)
            {
                m_counter = 0;
            }


            base.Update(time);
        }

        /// <summary>
        /// Dessine la particule à l'écran.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch, Vector2 viewportOffset, Vector2 scrollingOffset)
        {
            Rectangle srcTile = Charset.GetWalkSrcRect(m_counter / 8);
            batch.Draw(Charset.Texture, this.CurrentPosition, srcTile, CurrentColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, Graphics.Z.Particles);
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
