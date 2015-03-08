using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Display
{
    /// <summary>
    /// Classe contenant toutes les vues nécessaires pour que le renderer puisse effectuer un
    /// render de la map en mode Remote.
    /// </summary>
    public class MapStateView
    {
        public Views.MapView Map { get; set; }
        public List<Views.EntityBaseView> Entities { get; set; }
        public Views.VisionMapView VisionMap { get; set; }
        public List<Views.SpellcastBaseView> SpellCasts { get; set; }
    }
    /// <summary>
    /// Classe gérant le rendu d'une MapView.
    /// </summary>
    public class MapRenderer
    {
        #region Constants
        public bool SMOOTH_LIGHT = true;
        #endregion

        #region Variables
        /// <summary>
        /// Référence vers le renderer chargé de dessiner les entités.
        /// </summary>
        EntityRenderer m_entityRenderer;
        /// <summary>
        /// Référence vers le renderer chargé de dessiner les spell casts.
        /// </summary>
        SpellcastRenderer m_spellCastRenderer;
        /// <summary>
        /// Référence vers le renderer de la scène.
        /// </summary>
        SceneRenderer m_sceneRenderer;
        /// <summary>
        /// Render Target des tiles
        /// </summary>
        RenderTarget2D m_tilesRenderTarget;
        /// <summary>
        /// Render target des entities.
        /// </summary>
        RenderTarget2D m_entitiesRenderTarget;
        RenderTarget2D m_tmpRenderTarget;
        RenderTarget2D m_tmpRenderTarget2;
        /// <summary>
        /// Effet de flou gaussien.
        /// </summary>
        Codinsa2015.Server.GraphicsHelpers.GaussianBlur m_blur;
        #endregion

        #region properties
        /// <summary>
        /// Indique les équipes desquelles la vision doit être affichée.
        /// </summary>
        public Views.EntityType VisionDisplayed { get; set; }
        /// <summary>
        /// Taille d'une unité métrique en pixels.
        /// </summary>
        public int UnitSize { get; set; }
        /// <summary>
        /// Obtient le scrolling de la map (px).
        /// </summary>
        public Point Scrolling { get; set; }
        /// <summary>
        /// Valeur divisant la taille du render target sur lesquels seront dessinés les tiles, en vue
        /// d'un filtrage bilinéaire matériel lors de la remise à l'échelle.
        /// Une valeur + grande => tiles plus arrondis (et meilleures perfs)
        /// Valeur + petite     => tiles plus nets / carrés
        /// </summary>
        const int TILE_SCALE = 8;
        #endregion

        #region State
        /// <summary>
        /// Si en mode Remote => vue de la map qui sera dessinée.
        /// </summary>
        public MapStateView MapView { get; set; }
        /// <summary>
        /// Si le en mode Direct => map qui sera dessinée.
        /// </summary>
        public Codinsa2015.Server.Map Map { get; set; }

        /// <summary>
        /// Obtient la passabilité de la map.
        /// </summary>
        bool[,] Passability { 
            get
            {
                switch(m_sceneRenderer.Mode)
                {
                    case DataMode.Direct:
                        return Map.Passability;
                    case DataMode.Remote:
                        return MapView.Map.Passability;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la/les teams données ont la vision au point donné.
        /// </summary>
        /// <returns></returns>
        public bool HasVision(Views.EntityType teams, Vector2 position)
        {
            switch(m_sceneRenderer.Mode)
            {
                case DataMode.Direct:
                    return Map.Vision.HasVision((Server.Entities.EntityType)teams, position);
                case DataMode.Remote:
                    teams &= (Views.EntityType.Team1 | Views.EntityType.Team2);
                    return (MapView.VisionMap.Vision[(int)position.X, (int)position.Y] & (Views.VisionFlags)teams) != 0;
                default:
                    throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Obtient le rectangle sur lequel va être dessiné la map.
        /// </summary>
        public Rectangle Viewport
        {
            get { return m_sceneRenderer.Viewport; }
        }

        public Vector2 ScrollingVector2 
        { 
            get { return new Vector2(Scrolling.X, Scrolling.Y); } 
            set { Scrolling = new Point((int)value.X, (int)value.Y); } 
        }
        #endregion
        
        #region Methods

        /// <summary>
        /// Crée une nouvelle instance du map renderer.
        /// </summary>
        public MapRenderer(SceneRenderer sceneRenderer)
        {
            m_sceneRenderer = sceneRenderer;
            m_entityRenderer = new EntityRenderer(this);
            m_spellCastRenderer = new SpellcastRenderer(this);
        }

        #region Setup
        /// <summary>
        /// Crée les render targets.
        /// </summary>
        void SetupRenderTargets()
        {
            if (m_entitiesRenderTarget != null)
            {
                m_entitiesRenderTarget.Dispose();
                m_tilesRenderTarget.Dispose();
            }
            m_entitiesRenderTarget = new RenderTarget2D(Ressources.Device, Viewport.Width, Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            m_tmpRenderTarget = new RenderTarget2D(Ressources.Device, Viewport.Width, Viewport.Height);
            m_tmpRenderTarget2 = new RenderTarget2D(Ressources.Device, Viewport.Width, Viewport.Height);
            SetupTileRenderTarget();
            m_blur.ComputeOffsets(Viewport.Width, Viewport.Height);
        }

        Dictionary<Point, RenderTarget2D> m_tilesRenderTargets = new Dictionary<Point, RenderTarget2D>();
        /// <summary>
        /// Crée le render target des tiles (s'adapted à la taille des cases).
        /// </summary>
        void SetupTileRenderTarget()
        {
            Point resolution = new Point(Viewport.Width / TILE_SCALE, Viewport.Height / TILE_SCALE);
            if (m_tilesRenderTargets.ContainsKey(resolution))
                m_tilesRenderTarget = m_tilesRenderTargets[resolution];
            else
            {
                m_tilesRenderTarget = new RenderTarget2D(Ressources.Device, resolution.X, resolution.Y, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                m_tilesRenderTargets.Add(resolution, m_tilesRenderTarget);
            }
        }
        #endregion


        Vector2 __oldScroll;
        float __oldUnitSize;
        float __xShaderTime;
        public void Draw(GameTime time, SpriteBatch batch, RenderTarget2D output)
        {
            batch.GraphicsDevice.SetRenderTarget(m_tilesRenderTarget);
            // batch.GraphicsDevice.Clear(Color.Transparent);
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            
            // Dessin de debug de la map.
            int beginX = Math.Max(0, Scrolling.X / UnitSize);
            int beginY = Math.Max(0, Scrolling.Y / UnitSize);
            int endX = Math.Min(beginX + (Viewport.Width / UnitSize + 1), Passability.GetLength(0));
            int endY = Math.Min(beginY + (Viewport.Height / UnitSize + 1), Passability.GetLength(1));

            int smoothAlpha = (__oldScroll == ScrollingVector2 && UnitSize == __oldUnitSize) ? 25 : 255;
            bool[,] passability = Passability;
            for (int x = beginX; x < endX; x++)
            {
                for (int y = beginY; y < endY; y++)
                {
                    Point drawPos = new Point(x * UnitSize - Scrolling.X, y * UnitSize - Scrolling.Y);
                    int r = passability[x, y] ? 0 : 255;
                    int b = HasVision(VisionDisplayed, new Vector2(x, y)) ? 255 : 0;
                    int a = SMOOTH_LIGHT ? smoothAlpha : 255;
                    batch.Draw(Ressources.DummyTexture, new Rectangle(drawPos.X / TILE_SCALE, drawPos.Y / TILE_SCALE, UnitSize / TILE_SCALE, UnitSize / TILE_SCALE), new Color(r, 0, b, a));

                }
            }
            batch.End();

            // Dessin des entités
            batch.GraphicsDevice.SetRenderTarget(m_entitiesRenderTarget);
            batch.GraphicsDevice.Clear(Color.Transparent);
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            switch(m_sceneRenderer.Mode)
            {
                case DataMode.Direct:
                    foreach (var kvp in Map.Entities) 
                    {
                        m_entityRenderer.Draw(time, batch, kvp.Value.Position, (Views.EntityType)kvp.Value.Type);
                    }
                    foreach (var cast in Map.Spellcasts) 
                    {
                        float radius = 0.0f;
                        Vector2 size = Vector2.Zero;
                        Codinsa2015.Server.Shapes.CircleShape circ = cast.GetShape() as Codinsa2015.Server.Shapes.CircleShape;
                        Codinsa2015.Server.Shapes.RectangleShape rect = cast.GetShape() as Codinsa2015.Server.Shapes.RectangleShape;
                        if (circ != null) radius = circ.Radius;
                        if (rect != null) size = new Vector2(rect.Width, rect.Height);

                        m_spellCastRenderer.Draw(
                            batch,
                            new Views.GenericShapeView()
                            {
                                Position = cast.GetShape().Position,
                                Radius = radius,
                                Size = size,
                                ShapeType = (circ == null) ? Views.GenericShapeType.Rectangle : Views.GenericShapeType.Circle
                            });
                    }
                    break;
                case DataMode.Remote:
                    foreach(var entity in MapView.Entities)
                    {
                        m_entityRenderer.Draw(time, batch, entity.Position, entity.Type);
                    }
                    foreach(var cast in MapView.SpellCasts)
                    {
                        m_spellCastRenderer.Draw(batch, cast.Shape);
                    }
                    break;
            }
            batch.End();


            // Blur
            /*for (int i = 0; i < BLUR_PASSES; i++)
            {
                m_blur.PerformGaussianBlur(m_tilesRenderTarget, m_tmpRenderTarget, m_tmpRenderTarget2, batch);
                m_blur.PerformGaussianBlur(m_tmpRenderTarget2, m_tmpRenderTarget, m_tilesRenderTarget, batch);
            }*/

            // Dessin du tout
            batch.GraphicsDevice.SetRenderTarget(output);
            batch.GraphicsDevice.Clear(Color.DarkGray);
            Ressources.MapEffect.Parameters["xSourceTexture"].SetValue(m_tilesRenderTarget);
            Ressources.MapEffect.Parameters["scrolling"].SetValue(new Vector2(Scrolling.X / (float)Viewport.Width, Scrolling.Y / (float)Viewport.Height));
            Ressources.MapEffect.Parameters["xPixelSize"].SetValue(new Vector2(1.0f / Viewport.Width, 1.0f / Viewport.Height));
            Ressources.MapEffect.Parameters["xUnitSize"].SetValue(UnitSize);
            __xShaderTime += 0.0005f;
            Ressources.MapEffect.Parameters["xTime"].SetValue(__xShaderTime);

            // Dessine les tiles avec le shader
            batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, Ressources.MapEffect);
            batch.Draw(m_tilesRenderTarget, Viewport, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, Codinsa2015.Server.GraphicsHelpers.Z.Map);
            batch.End();

            // Dessine les entités.
            batch.Begin();
            batch.Draw(m_entitiesRenderTarget, Viewport, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, Codinsa2015.Server.GraphicsHelpers.Z.Entities);
            batch.End();



            __oldScroll = ScrollingVector2;
            __oldUnitSize = UnitSize;
        }

        #endregion
    }
}
