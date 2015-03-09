using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Rendering
{
    /// <summary>
    /// Classe chargée du rendu de spellcasts.
    /// </summary>
    public class SpellcastRenderer
    {
        #region Variables
        MapRenderer m_mapRenderer;
        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de EntityRenderer associée au map renderer donné.
        /// </summary>
        /// <param name="m_mapRenderer"></param>
        public SpellcastRenderer(MapRenderer mapRenderer)
        {
            m_mapRenderer = mapRenderer;
        }

        /// <summary>
        /// Dessine le spellcast.
        /// </summary>
        public void Draw(SpriteBatch batch, Views.GenericShapeView shape)
        {
            Point scroll = m_mapRenderer.Scrolling;
            batch.Draw(Ressources.DummyTexture,
                new Rectangle((int)(shape.Position.X * m_mapRenderer.UnitSize) - scroll.X,
                    (int)(shape.Position.Y * m_mapRenderer.UnitSize) - scroll.Y,
                    (int)(shape.Radius * 2 * m_mapRenderer.UnitSize),
                    (int)(shape.Radius * 2 * m_mapRenderer.UnitSize)),
                null,
                Color.Violet,
                0.0f,
                new Vector2(shape.Radius * m_mapRenderer.UnitSize, shape.Radius * m_mapRenderer.UnitSize),
                SpriteEffects.None, 0.0f);
        }
        #endregion
    }
}
