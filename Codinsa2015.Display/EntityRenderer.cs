using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Rendering
{
    /// <summary>
    /// Classe chargée de faire le rendu d'entités.
    /// </summary>
    public class EntityRenderer
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
        public EntityRenderer(MapRenderer mapRenderer)
        {
            m_mapRenderer = mapRenderer;
        }


        protected float __angle;
        /// <summary>
        /// Dessine l'entité à la position donnée.
        /// 
        /// Cette méthode doit être réécrite pour chaque type d'entité.
        /// </summary>
        /// <param name="time">Temps de jeu.</param>
        /// <param name="batch">Batch sur lequel dessiner.</param>
        /// <param name="drawPos">Position à laquelle dessiner l'unité.</param>
        public virtual void Draw(SpriteBatch batch, GameTime time, Vector2 entityPosition,
            Views.EntityType type)
        {
            Point scroll = m_mapRenderer.Scrolling;
            Point drawPos = new Point((int)(entityPosition.X * m_mapRenderer.UnitSize) - scroll.X, (int)(entityPosition.Y * m_mapRenderer.UnitSize) - scroll.Y);

            if (drawPos.X > m_mapRenderer.Viewport.Right || drawPos.Y > m_mapRenderer.Viewport.Bottom
                || drawPos.X < m_mapRenderer.Viewport.Left - m_mapRenderer.UnitSize || drawPos.Y < m_mapRenderer.Viewport.Top - m_mapRenderer.UnitSize)
                return;

            Color col;
            if (type.HasFlag(Views.EntityType.Team1))
                col = Color.Blue;
            else if (type.HasFlag(Views.EntityType.Team2))
                col = Color.Red;
            else
                col = Color.White;

            Texture2D tex = Ressources.DummyTexture;
            if (type.HasFlag(Views.EntityType.Tower))
            {
                tex = (type & Views.EntityType.Teams) == Views.EntityType.Team1 ? Ressources.BlueTower : Ressources.RedTower;
                col = Color.White;
            }
            else if (type.HasFlag(Views.EntityType.Spawner))
                tex = Ressources.TextBox;
            else if (type.HasFlag(Views.EntityType.WardPlacement))
                tex = Ressources.SelectMark;

            int s = m_mapRenderer.UnitSize / 2;
            int sx = m_mapRenderer.UnitSize / 2;
            int sy = m_mapRenderer.UnitSize / 2;
            if (type.HasFlag(Views.EntityType.Checkpoint))
            {
                sx /= 4;
                sy /= 4;
            }
            else if(type.HasFlag(Views.EntityType.Tower))
            {
                sy *= 4;
                sx *= 2;
            }
            col.A = (byte)((m_mapRenderer.HasVision((type & Views.EntityType.Teams) ^ Views.EntityType.Teams, entityPosition)) ? 255 : 150);
            batch.Draw(tex,
                new Rectangle(drawPos.X, drawPos.Y, sx, sy), null, col, __angle, new Vector2(s, s), SpriteEffects.None, 0.0f);
        }
        #endregion
    }
}
