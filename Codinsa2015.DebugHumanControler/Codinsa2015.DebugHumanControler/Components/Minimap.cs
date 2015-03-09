using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.Entities;
using Codinsa2015.Rendering;
namespace Codinsa2015.DebugHumanControler.Components
{
    /// <summary>
    /// Représente un composant capable d'afficher la minimap.
    /// </summary>
    public class Minimap
    {
        #region Variables
        bool m_isDirty;
        SpriteBatch m_minimapBatch;
        RenderTarget2D m_minimapTexture;
        MapRenderer m_map;
        #endregion 

        #region Properties
        /// <summary>
        /// Obtient ou définit la position de la minimap en pixels.
        /// </summary>
        public Vector2 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit la taille de la minimap en pixels.
        /// </summary>
        public Vector2 Size
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit le layer sur lequel est affichée la minimap.
        /// </summary>
        public float Z
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la minimap est visible.
        /// </summary>
        public bool Visible
        {
            get;
            set;
        }


        /// <summary>
        /// Obtient ou définit la map affichée.
        /// </summary>
        public Codinsa2015.Server.Map CurrentMap
        {
            get
            {
                return m_map.Map;
            }
            set
            {
                if (m_map.Map == value && m_minimapTexture != null)
                    return;

                m_map.Map = value;

                if (m_minimapTexture != null)
                    m_minimapTexture.Dispose();

                m_minimapTexture = new RenderTarget2D(Ressources.Device, value.Size.X, value.Size.Y, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

                m_map.Map.OnMapModified += m_map_OnMapModified;
            }
        }
        #endregion

        /// <summary>
        /// Crée une nouvelle instance de la minimap.
        /// </summary>
        public Minimap(MapRenderer maprdr)
        {
            m_map = maprdr;
            m_isDirty = true;
        }
        
        /// <summary>
        /// Charge les ressources dont a besoin la minimap.
        /// </summary>
        public void LoadContent()
        {
            m_minimapBatch = new SpriteBatch(Ressources.Device);
        }

        /// <summary>
        /// Dessine la minimap.
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            RenderTarget2D mainRenderTarget = m_map.SceneRenderer.MainRenderTarget;
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            int w = CurrentMap.Passability.GetLength(0);
            int h = CurrentMap.Passability.GetLength(1);
            int unitX = Math.Max(1, rect.Width / w);
            int unitY = Math.Max(1, rect.Height / h);

            if (m_isDirty)
            {
                // Si la taille de la map a a changé, on change la taille de la texture de la minimap.
                if (m_minimapTexture.Width != CurrentMap.Passability.GetLength(0) || m_minimapTexture.Height != CurrentMap.Passability.GetLength(1))
                {
                    m_minimapTexture.Dispose();
                    m_minimapTexture = new RenderTarget2D(Ressources.Device, CurrentMap.Size.X, CurrentMap.Size.Y, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);


                }

                m_minimapBatch.GraphicsDevice.SetRenderTarget(m_minimapTexture);
                m_minimapBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                m_minimapBatch.GraphicsDevice.Clear(Color.White);
                // Dessine la minimap sur la texture temporaire.
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        Color col = CurrentMap.Passability[x, y] ? Color.White : Color.Red;
                        col = new Color(col.R / 2, col.G / 2, col.B / 2);

                        m_minimapBatch.Draw(Ressources.DummyTexture,
                            new Rectangle(x, y, 1, 1),
                            null,
                            col,
                            0.0f,
                            Vector2.Zero, SpriteEffects.None,
                            Z);
                    }
                }
                m_minimapBatch.End();
                m_minimapBatch.GraphicsDevice.SetRenderTarget(mainRenderTarget);
                // Supprime le dirty bit.
                m_isDirty = false;
            }


            // Dessine la minimap
            batch.Draw(m_minimapTexture, rect, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, Z);

            // Dessine la vision sur la minimap
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (m_map.Map.Vision.HasVision(EntityType.Team1, new Vector2(x, y)))
                    {
                        Color col = new Color(255, 255, 255, 255);
                        batch.Draw(Ressources.DummyTexture,
                            new Rectangle((int)(rect.X + (x / (float)w) * rect.Width),
                            (int)(rect.Y + (y / (float)h) * rect.Height),
                            unitX,
                            unitY), null,
                            col,
                            0.0f,
                            Vector2.Zero, SpriteEffects.None,
                            Z + Rendering.GraphicsHelpers.Z.FrontStep);
                    }

                }
            }

            if (!Visible)
                return;

            // Dessine le rectangle indiquant quelle partie de la map est actuellement affichée à l'écran.
            Vector2 scrolling = m_map.ScrollingVector2;
            batch.Draw(Ressources.DummyTexture,
                new Rectangle((int)(rect.X + (m_map.ScrollingVector2.X / m_map.UnitSize / (float)w) * rect.Width),
                              (int)(rect.Y + (m_map.ScrollingVector2.Y / m_map.UnitSize / (float)h) * rect.Height),
                              (int)((m_map.Viewport.Width / (float)(w * m_map.UnitSize)) * rect.Width),
                              (int)((m_map.Viewport.Height / (float)(h * m_map.UnitSize)) * rect.Height)), null,
                              new Color(255, 255, 255, 60),
                              0.0f,
                              Vector2.Zero, SpriteEffects.None,
                              Z + Rendering.GraphicsHelpers.Z.FrontStep * 2);
        }

        /// <summary>
        /// Callback appelé lorsque la map a été modifiée.
        /// </summary>
        void m_map_OnMapModified()
        {
            m_isDirty = true;
        }
    }
}
