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
        public void Draw(SpriteBatch batch, Views.GenericShapeView shape, string name)
        {
            Point scroll = m_mapRenderer.Scrolling;
            Texture2D tex = Ressources.DummyTexture;
            switch(name)
            {
                case "Fireball":
                    tex = Ressources.Fireball;
                    break;
                default:
                    tex = Ressources.SpellZone;
                    break;
            }

            if (shape.ShapeType == Views.GenericShapeType.Circle)
            {
                batch.Draw(tex,
                    new Rectangle((int)(shape.Position.X * m_mapRenderer.UnitSize) - scroll.X,
                        (int)(shape.Position.Y * m_mapRenderer.UnitSize) - scroll.Y,
                        (int)(shape.Radius * 2 * m_mapRenderer.UnitSize),
                        (int)(shape.Radius * 2 * m_mapRenderer.UnitSize)),
                    null,
                    Color.Violet,
                    0.0f,
                    new Vector2(tex.Width / 2, tex.Height / 2),
                    SpriteEffects.None, 0.0f);
            }
            else
            {
                batch.Draw(tex,
                    new Rectangle((int)(shape.Position.X * m_mapRenderer.UnitSize) - scroll.X,
                        (int)(shape.Position.Y * m_mapRenderer.UnitSize) - scroll.Y,
                        (int)(shape.Size.X * m_mapRenderer.UnitSize),
                        (int)(shape.Size.Y * m_mapRenderer.UnitSize)),
                    null,
                    Color.Violet,
                    0.0f,
                    new Vector2(tex.Width / 2, tex.Height / 2),
                    SpriteEffects.None, 0.0f);
            }
        }
        #endregion
    }
}
