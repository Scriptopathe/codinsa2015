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
        /// Dessine l'entité donnée en utilisant les primitives fournies
        /// par le batch donné.
        /// </summary>
        public virtual void Draw(SpriteBatch batch, GameTime time, Server.Entities.EntityBase entity)
        {
            // Extrait des informations utiles de l'entité.
            Vector2 entityPosition = entity.Position;
            Views.EntityType type = (Views.EntityType)entity.Type;
            Server.Entities.EntityHero entityAsHero = entity as Server.Entities.EntityHero;
            Views.EntityHeroRole role = entityAsHero != null ? (Views.EntityHeroRole)entityAsHero.Role : 0;
            Point scroll = m_mapRenderer.Scrolling;
            Point drawPos = new Point((int)(entityPosition.X * m_mapRenderer.UnitSize) - scroll.X, (int)(entityPosition.Y * m_mapRenderer.UnitSize) - scroll.Y);

            // Si l'entité est hors du champ de vision : on la clip.
            if (drawPos.X > m_mapRenderer.Viewport.Right || drawPos.Y > m_mapRenderer.Viewport.Bottom
                || drawPos.X < m_mapRenderer.Viewport.Left - m_mapRenderer.UnitSize || drawPos.Y < m_mapRenderer.Viewport.Top - m_mapRenderer.UnitSize)
                return;


            Color col = Color.White;
            Texture2D tex = Ressources.DummyTexture;
            float sx = 1;
            float sy = 1; // scale X, Y
            bool blue = type.HasFlag(Views.EntityType.Team1);

            switch(type & (Views.EntityType.Teams ^ (Views.EntityType.All)))
            {
                // Creep
                case Views.EntityType.Creep:
                    sx = 1;
                    sy = 1;
                    tex = blue ? Ressources.BlueCreep : Ressources.RedCreep;
                    break;
                // Player
                case Views.EntityType.Player:
                    sx = 1;
                    sy = 1;
                    switch (role)
                    {
                        case Views.EntityHeroRole.Fighter:
                            tex = blue ? Ressources.BlueFighter : Ressources.RedFighter;
                            break;
                        case Views.EntityHeroRole.Mage:
                            tex = blue ? Ressources.BlueMage : Ressources.RedMage;
                            break;
                        case Views.EntityHeroRole.Tank:
                            tex = blue ? Ressources.BlueTank : Ressources.RedTank;
                            break;
                    }
                    break;
                // Tower
                case Views.EntityType.Tower:
                    sx = 1;
                    sy = 2;
                    tex = blue ? Ressources.BlueTower : Ressources.RedTower;
                    break;
                // Checkpoint
                case Views.EntityType.Checkpoint:
                    sx = 0.25f; sy = 0.25f; col = blue ? Color.Blue : Color.Red;
                    break;
            }


            // Rectangle de dessin de l'entité
            Rectangle drawRect = new Rectangle(drawPos.X, drawPos.Y, (int)(m_mapRenderer.UnitSize * sx), (int)(m_mapRenderer.UnitSize * sy));


            // 0 back, 1 front
            float entityZ = 0.25f + ((drawPos.Y - tex.Height) / m_mapRenderer.SceneRenderer.MainRenderTarget.Height) / 2;


            // Dessin de la jauge
            int totalLength = (int)(drawRect.Width)*3/4;
            int totalH = 6;
            Vector2 gaugeOrigin = new Vector2(0, 0);
            float max = (entity.GetMaxHP() + entity.ShieldPoints);
            float percent = entity.HP / max;
            float shieldPercent = entity.ShieldPoints / max;

            int offsetY = -drawRect.Height;
            int offsetX = -drawRect.Width / 2;

            Rectangle gaugeRect = new Rectangle(drawPos.X +  offsetX + (drawRect.Width - totalLength) / 2,
                drawPos.Y + offsetY - 10,
                totalLength,
                totalH);
            float gaugeZ = 0.25f + ((gaugeRect.Y - tex.Height) / m_mapRenderer.SceneRenderer.MainRenderTarget.Height) / 2;
            Color gaugeColor = blue ? Color.Blue : Color.Red;
            // Jauge vide
            batch.Draw(Ressources.LifebarEmpty,
                gaugeRect,
                null,
                gaugeColor,
                0, gaugeOrigin, SpriteEffects.None, gaugeZ);

            // Vie
            gaugeRect.Width = (int)(totalLength * percent);
            batch.Draw(Ressources.LifebarFull,
                gaugeRect,
                null,
                gaugeColor,
                0, gaugeOrigin, SpriteEffects.None, gaugeZ + 0.00001f);

            if (entity.ShieldPoints > 0)
            {
                // Shield
                gaugeRect.X += (int)(totalLength * percent);
                gaugeRect.Width = (int)(gaugeRect.Width * shieldPercent);
                batch.Draw(Ressources.LifebarFull,
                    gaugeRect,
                    null,
                    Color.White,
                    0, gaugeOrigin, SpriteEffects.None, gaugeZ + 0.00002f);
            }
            
            // Entité
            col.A = (byte)((m_mapRenderer.HasVision((type & Views.EntityType.Teams) ^ Views.EntityType.Teams, entityPosition)) ? 255 : 220);
            if (entity.IsStealthed)
                col.A = 120;

            

            // Dessin de l'entité
            batch.Draw(tex,
                drawRect, 
                null,
                col,
                __angle,
                new Vector2(tex.Width/2, tex.Height),
                SpriteEffects.None,
                entityZ);

            // Roots
            if (entity.IsSilenced)
            {
                int us = m_mapRenderer.UnitSize;
                Rectangle drect = new Rectangle(gaugeRect.X + us, gaugeRect.Y, us/2, us/2);
                batch.Draw(Ressources.SilenceIcon,
                    drect,
                    null,
                    Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, entityZ);
            }
            if(entity.IsBlind)
            {
                int us = m_mapRenderer.UnitSize;
                Rectangle drect = new Rectangle(gaugeRect.X - us, gaugeRect.Y, us / 2, us / 2);
                batch.Draw(Ressources.BlindIcon,
                    drect,
                    null,
                    Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, entityZ);
            }
        }
        /// <summary>
        /// Dessine l'entité à la position donnée.
        /// 
        /// Cette méthode doit être réécrite pour chaque type d'entité.
        /// </summary>
        /// <param name="time">Temps de jeu.</param>
        /// <param name="batch">Batch sur lequel dessiner.</param>
        /// <param name="drawPos">Position à laquelle dessiner l'unité.</param>
        public virtual void Draw(SpriteBatch batch, GameTime time, Vector2 entityPosition, Views.EntityType type, Views.EntityHeroRole role)
        {
            /*
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
            else if(type.HasFlag(Views.EntityType.Player))
            {
                bool blue = type.HasFlag(Views.EntityType.Team1);
                switch(role)
                {
                    case Views.EntityHeroRole.Fighter:
                        tex = blue ? Ressources.BlueFighter : Ressources.RedFighter;
                        break;
                    case Views.EntityHeroRole.Mage:
                        tex = blue ? Ressources.BlueMage : Ressources.RedMage;
                        break;
                    case Views.EntityHeroRole.Tank:
                        tex = blue ? Ressources.BlueTank : Ressources.RedTank;
                        break;
                }
                sx *= 2;
                sy *= 2;
                col = Color.White;
            }
            col.A = (byte)((m_mapRenderer.HasVision((type & Views.EntityType.Teams) ^ Views.EntityType.Teams, entityPosition)) ? 255 : 220);
            batch.Draw(tex,
                new Rectangle(drawPos.X, drawPos.Y, sx, sy), null, col, __angle, new Vector2(s, s), SpriteEffects.None, 0.0f);*/
        }
        #endregion
    }
}
