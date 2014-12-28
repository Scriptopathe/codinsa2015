using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.Entities;
using Codinsa2015.Server.Spellcasts;
using System.IO;
using Codinsa2015.Graphics.Server;
namespace Codinsa2015.Server
{
    /// <summary>
    /// Classe représentant la map.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Représente une façon de voir la map.
        /// </summary>
        public class PointOfView
        {
            /// <summary>
            /// Obtient ou définit la / les teams pour lesquelles la vision sera affichée.
            /// </summary>
            public EntityType Teams;
            public Point Scrolling;
            public int UnitSize;
        }

        const int TILE_SCALE = 16;
        const int BLUR_PASSES = 0;

        #region Variables
        /// <summary>
        /// Représente les paramètres de point de vue de la map.
        /// </summary>
        PointOfView m_pointOfView;
        /// <summary>
        /// Passabilité de la map pour chaque case :
        /// - true veut dire passable
        /// - false veut dire non passable.
        /// </summary>
        bool[,] m_passability;
        /// <summary>
        /// Hashmap des entités présentes sur la map, indexées par leur ID.
        /// </summary>
        EntityCollection m_entities;
        /// <summary>
        /// Liste des sorts en cours d'activation sur la map.
        /// </summary>
        List<Spellcast> m_spellcasts;
        /// <summary>
        /// Rectangle de l'écran sur lequel sera dessinée la map.
        /// </summary>
        Rectangle m_viewport;
        /// <summary>
        /// Liste des entités à ajouter à la prochaine frame.
        /// </summary>
        EntityCollection m_entitiesAddList;

        /// <summary>
        /// Render Target des tiles
        /// </summary>
        RemoteRenderTarget m_tilesRenderTarget;
        /// <summary>
        /// Render target des entities.
        /// </summary>
        RemoteRenderTarget m_entitiesRenderTarget;
        RemoteRenderTarget m_tmpRenderTarget;
        RemoteRenderTarget m_tmpRenderTarget2;
        /// <summary>
        /// Effet de flou gaussien.
        /// </summary>
        GraphicsHelpers.GaussianBlur m_blur;
        #endregion

        #region Events
        public delegate void MapModifiedDelegate();
        bool __fireMapModifiedEvent;
        /// <summary>
        /// Event lancé lorsque la passabilité de la map est modifiée.
        /// </summary>
        public event MapModifiedDelegate OnMapModified;
        #endregion

        #region Graphics Properties

        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit le point de vue en cours de la map.
        /// </summary>
        public PointOfView Pov
        {
            get { return m_pointOfView; }
            set
            {
                m_pointOfView = value;
                Scrolling = m_pointOfView.Scrolling; // auto adjust scrolling
                SetupTileRenderTarget();
            }
        }
        /// <summary>
        /// Taille d'une unité métrique en pixels.
        /// </summary>
        public int UnitSize
        {
            get { return m_pointOfView.UnitSize; }
            set { m_pointOfView.UnitSize = value; SetupTileRenderTarget(); }
        }
        /// <summary>
        /// Retourne la liste des héros.
        /// </summary>
        public List<EntityHero> Heroes
        {
            get;
            set;
        }
        /// <summary>
        /// Obtient la taille de la map en cases.
        /// </summary>
        public Point Size
        {
            get
            {
                return new Point(m_passability.GetLength(0), m_passability.GetLength(1));
            }
        }
        /// <summary>
        /// Obtient la taille de la map en pixels.
        /// </summary>
        public Vector2 SizePixels
        {
            get
            {
                return new Vector2(m_passability.GetLength(0) * UnitSize, m_passability.GetLength(1) * UnitSize);
            }
        }
        /// <summary>
        /// Rectangle de l'écran sur lequel sera dessinée la map.
        /// </summary>
        public Rectangle Viewport
        {
            get { return m_viewport; }
            set { m_viewport = value; SetupRenderTargets(); }
        }

        /// <summary>
        /// Liste des sorts en cours d'activation sur la map.
        /// </summary>
        public List<Spellcast> Spellcasts
        {
            get { return m_spellcasts; }
            set { m_spellcasts = value; }
        }

        /// <summary>
        /// Liste des entités présentes sur la map.
        /// </summary>
        public EntityCollection Entities
        {
            get { return m_entities; }
            set { m_entities = value; }
        }
        /// <summary>
        /// Passabilité de la map pour chaque case :
        /// - true veut dire passable
        /// - false veut dire non passable.
        /// </summary>
        [Clank.ViewCreator.Export("Matrix<bool>", "Tableau de passabilité de la map. true : passable, false : non passable.")]
        public bool[,] Passability
        {
            get { return m_passability; }
            set { m_passability = value; Vision.TheMap = this; }
        }
        /// <summary>
        /// Scrolling de la map (en px).
        /// </summary>
        public Point Scrolling
        {
            get { return m_pointOfView.Scrolling; }
            set 
            {
                Vector2 valueVect = new Vector2(value.X, value.Y);
                Vector2 pos = Vector2.Max(new Vector2(0, 0), Vector2.Min(valueVect, SizePixels - new Vector2(Viewport.Width, Viewport.Height)));
                m_pointOfView.Scrolling = new Point((int)pos.X, (int)pos.Y);
            }
        }

        /// <summary>
        /// Scrolling de la map (en px).
        /// </summary>
        public Vector2 ScrollingVector2
        {
            get { return new Vector2(Scrolling.X, Scrolling.Y); }
            set { Scrolling = new Point((int)value.X, (int)value.Y); }
        }

        /// <summary>
        /// Obtient ou définit la carte de vision de cette map.
        /// </summary>
        public VisionMap Vision
        {
            get;
            set;
        }
        
        #endregion

        
        #region Graphics
        /// <summary>
        /// Crée les render targets.
        /// </summary>
        void SetupRenderTargets()
        {
            if(m_entitiesRenderTarget != null)
            {
                m_entitiesRenderTarget.Dispose();
                m_tilesRenderTarget.Dispose();
            }
            m_entitiesRenderTarget = new RemoteRenderTarget(GameServer.GetScene().GraphicsServer, Viewport.Width, Viewport.Height, RenderTargetUsage.PreserveContents);
            m_tmpRenderTarget = new RemoteRenderTarget(GameServer.GetScene().GraphicsServer, Viewport.Width, Viewport.Height);
            m_tmpRenderTarget2 = new RemoteRenderTarget(GameServer.GetScene().GraphicsServer, Viewport.Width, Viewport.Height);
            SetupTileRenderTarget();
            m_blur.ComputeOffsets(Viewport.Width, Viewport.Height);
        }

        Dictionary<Point, RemoteRenderTarget> m_tilesRenderTargets = new Dictionary<Point, RemoteRenderTarget>();
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
                m_tilesRenderTarget = new RemoteRenderTarget(GameServer.GetScene().GraphicsServer, resolution.X, resolution.Y, RenderTargetUsage.PreserveContents);
                m_tilesRenderTargets.Add(resolution, m_tilesRenderTarget);
            }
        }

        public const bool SMOOTH_LIGHT = true;
        float __xShaderTime = 0.0f;
        Vector2 __oldScroll;
        int __oldUnitSize;
        /// <summary>
        /// Dessine la map ainsi que les entités qu'elle contient.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="batch"></param>
        public void Draw(GameTime time, RemoteSpriteBatch batch)
        {
            batch.GraphicsDevice.SetRenderTarget(m_tilesRenderTarget);
            // batch.GraphicsDevice.Clear(Color.Transparent);
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            // Dessin de debug de la map.
            int beginX = Math.Max(0, Scrolling.X / UnitSize);
            int beginY = Math.Max(0, Scrolling.Y / UnitSize);
            int endX = Math.Min(beginX + (Viewport.Width / UnitSize + 1), m_passability.GetLength(0));
            int endY = Math.Min(beginY + (Viewport.Height / UnitSize + 1), m_passability.GetLength(1));

            int smoothAlpha = (__oldScroll == ScrollingVector2  && UnitSize == __oldUnitSize) ? 25 : 255;
            for (int x = beginX; x < endX; x++)
            {
                for (int y = beginY; y < endY; y++)
                {
                    Point drawPos = new Point(x * UnitSize - Scrolling.X, y * UnitSize - Scrolling.Y);
                    int r = m_passability[x, y] ? 0 : 255;
                    int b = Vision.HasVision(Pov.Teams, new Vector2(x, y)) ? 255 : 0;
                    int a = SMOOTH_LIGHT ? smoothAlpha : 255;
                    batch.Draw(Ressources.DummyTexture, new Rectangle(drawPos.X / TILE_SCALE, drawPos.Y / TILE_SCALE, UnitSize / TILE_SCALE, UnitSize / TILE_SCALE), new Color(r, 0, b, a));
                    
                }
            }
            batch.End();

            // Dessin des entités
            batch.GraphicsDevice.SetRenderTarget(m_entitiesRenderTarget);
            batch.GraphicsDevice.Clear(Color.Transparent);
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            foreach (var kvp in m_entities) { kvp.Value.Draw(time, batch); }
            foreach (Spellcast cast in m_spellcasts) { cast.Draw(time, batch); }
            batch.End();


            // Blur
            /*for (int i = 0; i < BLUR_PASSES; i++)
            {
                m_blur.PerformGaussianBlur(m_tilesRenderTarget, m_tmpRenderTarget, m_tmpRenderTarget2, batch);
                m_blur.PerformGaussianBlur(m_tmpRenderTarget2, m_tmpRenderTarget, m_tilesRenderTarget, batch);
            }*/

            // Dessin du tout
            batch.GraphicsDevice.SetRenderTarget(GameServer.GetScene().MainRenderTarget);
            batch.GraphicsDevice.Clear(Color.DarkGray);
            Ressources.MapEffect.Parameters["xSourceTexture"].SetValue(m_tilesRenderTarget);
            Ressources.MapEffect.Parameters["scrolling"].SetValue(new Vector2(Scrolling.X / (float)Viewport.Width, Scrolling.Y / (float)Viewport.Height));
            Ressources.MapEffect.Parameters["xPixelSize"].SetValue(new Vector2(1.0f / Viewport.Width, 1.0f / Viewport.Height));
            Ressources.MapEffect.Parameters["xUnitSize"].SetValue(UnitSize);
            __xShaderTime += 0.0005f;
            Ressources.MapEffect.Parameters["xTime"].SetValue(__xShaderTime);

            // Dessine les tiles avec le shader
            batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, Ressources.MapEffect);
            batch.Draw(m_tilesRenderTarget, Viewport, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.Map);
            batch.End();

            // Dessine les entités.
            batch.Begin();
            batch.Draw(m_entitiesRenderTarget, Viewport, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.Entities);
            batch.End();



            __oldScroll = ScrollingVector2;
            __oldUnitSize = UnitSize;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de Map.
        /// </summary>
        public Map()
        {
            m_entities = new EntityCollection();
            m_entitiesAddList = new EntityCollection();
            m_spellcasts = new List<Spellcast>();
            m_pointOfView = new PointOfView();
            m_pointOfView.UnitSize = 32;
            m_pointOfView.Teams = EntityType.Team1;           
            
            // DEBUG CODE
            m_passability = new bool[50, 50];
            __passabilityDrawRect(new Rectangle(1, 1, 40, 40), true);
            __passabilityDrawRect(new Rectangle(3, 4, 20, 2), false);


            // Vision
            Vision = new VisionMap(this);

            // Ajout des héros
            Heroes = new List<EntityHero>();
        }

        /// <summary>
        /// Charge les ressources graphiques dont a besoin la map.
        /// </summary>
        public void LoadContent()
        {
            // Initialise l'effet de blur.
            m_blur = new GraphicsHelpers.GaussianBlur(GameServer.GetScene().GraphicsServer);
            m_blur.ComputeKernel(4, 2);


            // Création du viewport
            Viewport = new Rectangle(0, 25, (int)GameServer.GetScreenSize().X, (int)GameServer.GetScreenSize().Y - 125);
            Scrolling = new Point();
        }
        /// <summary>
        /// Mets à jour la map ainsi que les entités qu'elle contient.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            // Mise à jour de la vision.
            Vision.Update(time);

            // Mise à jour des entités
            List<int> entitiesToDelete = new List<int>();
            List<Spellcast> spellcastsToDelete = new List<Spellcast>();

            // Mets à jour tous les spells lancés.
            foreach (Spellcast cast in m_spellcasts)
            {
                cast.Update(time);
                if (cast.IsDisposing)
                {
                    spellcastsToDelete.Add(cast);
                }
            }

            // Ajoute les entités en attente.
            while(m_entitiesAddList.Count != 0)
            {
                var first = m_entitiesAddList.First();
                m_entities.Add(first.Key, first.Value);
                m_entitiesAddList.Remove(first.Key);
            }

            // Mets à jour les entités.
            foreach (var kvp in m_entities) 
            { 
                if (kvp.Value.IsDisposing) 
                { 
                    entitiesToDelete.Add(kvp.Key);
                }
                else
                {
                    // Vérifie les collisions spells / entités.
                    foreach(Spellcast cast in m_spellcasts)
                    {
                        if (cast.GetShape().Intersects(kvp.Value.Shape))
                            cast.OnCollide(kvp.Value);
                    }
                }
                kvp.Value.Update(time);
            }



            // Suppression des entités marquées Disposed
            foreach (int key in entitiesToDelete) 
            {
                m_entities[key].Dispose();
                m_entities.Remove(key); 
            }
            foreach (Spellcast cast in spellcastsToDelete)
            { 
                cast.Dispose();
                m_spellcasts.Remove(cast); 
            }


            // Lance l'évènement indiquant que la map a été modifiée.
            if(__fireMapModifiedEvent)
            {
                if (OnMapModified != null)
                    OnMapModified();
                __fireMapModifiedEvent = false;
            }
        }

        
        #endregion
        
        #region Positions
        /// <summary>
        /// Obtient la position dans l'espace de l'écran à partir d'une position
        /// en unités métriques. 
        /// (prends en compte viewport, scrolling, scaling).
        /// </summary>
        /// <returns></returns>
        public Vector2 ToScreenSpace(Vector2 mapPos)
        {
            return mapPos * UnitSize - ScrollingVector2 + new Vector2(Viewport.X, Viewport.Y);
        }

        /// <summary>
        /// Obtient la position dans l'espace de la map (unités métriques) à
        /// partir d'une position à l'écran.
        /// </summary>
        /// <param name="screenSpacePos"></param>
        /// <returns></returns>
        public Vector2 ToMapSpace(Vector2 screenSpacePos)
        {
            return (screenSpacePos + ScrollingVector2 - new Vector2(Viewport.X, Viewport.Y)) / UnitSize;
        }
        #endregion
        
        #region API
        /// <summary>
        /// Ajoute une entité à la map.
        /// </summary>
        public void AddEntity(EntityBase entity)
        {
            m_entitiesAddList.Add(entity.ID, entity);
        }
        /// <summary>
        /// Retourne la passabilité de la map à la position donnée en unités métriques.
        /// </summary>
        public bool GetPassabilityAt(float x, float y)
        {
            if (x < 0 || y < 0 || x >= m_passability.GetLength(0) || y >= m_passability.GetLength(1) || float.IsNaN(x) || float.IsNaN(y))
                return false;

            return m_passability[(int)x, (int)y];
        }

        /// <summary>
        /// Positionne le bit de passabilité à la position (x, y) à la valeur donnée.
        /// </summary>
        public void SetPassabilityAt(float x, float y, bool value)
        {
            if (x < 0 || y < 0 || x >= m_passability.GetLength(0) || y >= m_passability.GetLength(1))
                return;
            int ix = (int)x;
            int iy = (int)y;

            __fireMapModifiedEvent |= m_passability[ix, iy] != value;
            m_passability[ix, iy] = value;
        }

        public void SetPassabilityRect(int x, int y, int w, int h, bool value)
        {
            for (int i = x; i < x + w; i++)
                for (int j = y; j < y + h; j++)
                    SetPassabilityAt(i, j, value);
        }
        /// <summary>
        /// Retourne la passabilité de la map à la position donnée en unités métriques.
        /// </summary>
        public bool GetPassabilityAt(Vector2 position)
        {
            return GetPassabilityAt(position.X, position.Y);
        }
        /// <summary>
        /// Positionne le bit de passabilité à la position (x, y) à la valeur donnée.
        /// </summary>
        public void SetPassabilityAt(Vector2 position, bool value)
        {
            SetPassabilityAt(position.X, position.Y, value);
        }
        /// <summary>
        /// Retourne l'entité ayant l'id donné.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityBase GetEntityById(int id)
        {
            if (!m_entities.ContainsKey(id))
                return null;
            return m_entities[id];
        }

        /// <summary>
        /// Ajoute un Spellcast sur la map.
        /// </summary>
        public void AddSpellcast(Spellcast cast)
        {
            m_spellcasts.Add(cast);
        }

        #endregion

        #region DEBUG
        void __passabilityDrawRect(Rectangle rect, bool value)
        {
            for(int x = rect.Left; x < rect.Right; x++)
            {
                for(int y = rect.Top; y < rect.Bottom; y++)
                {
                    m_passability[x, y] = value;
                }
            }
        }
        #endregion

        #region Load
        /// <summary>
        /// Charge la map passée en paramètre.
        /// </summary>
        /// <param name="map"></param>
        public void Load(MapFile map)
        {
            Passability = map.Passability;

            EntityCollection newEntities = new EntityCollection();
            
            // Supprime les entités non-héros
            foreach(var entity in Entities)
            {
                if (entity.Value.Type.HasFlag(EntityType.Player))
                    newEntities.Add(entity.Key, entity.Value);
            }

            foreach(var entity in map.Entities)
            {
                newEntities.Add(entity.Key, entity.Value);
            }

            Entities = newEntities;
            Vision.TheMap = this;
            if(OnMapModified != null)
                OnMapModified();
        }
        #endregion


        #region View

        #endregion
    }


}


