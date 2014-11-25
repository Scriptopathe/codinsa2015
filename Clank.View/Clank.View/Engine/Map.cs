using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Clank.View.Engine.Entities;
using Clank.View.Engine.Spellcasts;
using System.IO;
namespace Clank.View.Engine
{
    /// <summary>
    /// Classe représentant la map.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Taille d'une unité métrique (= un case) en pixels.
        /// </summary>
        public static int UnitSize = 32;

        #region Variables
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
        /// Scrolling de la map (en px).
        /// </summary>
        Point m_scrolling;


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
        Graphics.GaussianBlur m_blur;
        #endregion

        #region Properties
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
            get { return m_scrolling; }
            set { m_scrolling = value; }
        }

        /// <summary>
        /// Scrolling de la map (en px).
        /// </summary>
        public Vector2 ScrollingVector2
        {
            get { return new Vector2(m_scrolling.X, m_scrolling.Y); }
            set { m_scrolling = new Point((int)value.X, (int)value.Y); }
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

            m_entitiesRenderTarget = new RenderTarget2D(Mobattack.Instance.GraphicsDevice, Viewport.Width, Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24, 1, RenderTargetUsage.PreserveContents);
            m_tilesRenderTarget = new RenderTarget2D(Mobattack.Instance.GraphicsDevice, Viewport.Width, Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24, 1, RenderTargetUsage.PreserveContents);
            m_tmpRenderTarget = new RenderTarget2D(Mobattack.Instance.GraphicsDevice, Viewport.Width, Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            m_tmpRenderTarget2 = new RenderTarget2D(Mobattack.Instance.GraphicsDevice, Viewport.Width, Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);

            m_blur.ComputeOffsets(Viewport.Width, Viewport.Height);
        }

        public const bool SMOOTH_LIGHT = true;
        /// <summary>
        /// Dessine la map ainsi que les entités qu'elle contient.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="batch"></param>
        public void Draw(GameTime time, SpriteBatch batch)
        {
            Mobattack.Instance.GraphicsDevice.SetRenderTarget(m_tilesRenderTarget);
            // batch.GraphicsDevice.Clear(Color.Transparent);
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            // Dessin de debug de la map.
            int beginX = m_scrolling.X / UnitSize;
            int beginY = m_scrolling.Y / UnitSize;
            int endX = Math.Min(beginX + (Viewport.Width / UnitSize + 1), m_passability.GetLength(0));
            int endY = Math.Min(beginY + (Viewport.Height / UnitSize + 1), m_passability.GetLength(1));
            for (int x = beginX; x < endX; x++)
            {
                for (int y = beginY; y < endY; y++)
                {
                    Point drawPos = new Point(x * UnitSize - Scrolling.X, y * UnitSize - Scrolling.Y);
                    int r = m_passability[x, y] ? 0 : 255;
                    int b = Vision.HasVision(EntityType.Team1, new Vector2(x, y)) ? 255 : 0;
                    int a = SMOOTH_LIGHT ? 100 : 255;
                    batch.Draw(Ressources.DummyTexture, new Rectangle(drawPos.X, drawPos.Y, UnitSize, UnitSize), new Color(r, 0, b, a));
                    
                }
            }
            batch.End();

            // Dessin des entités
            Mobattack.Instance.GraphicsDevice.SetRenderTarget(m_entitiesRenderTarget);
            batch.GraphicsDevice.Clear(Color.Transparent);
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            foreach (var kvp in m_entities) { kvp.Value.Draw(time, batch); }
            foreach (Spellcast cast in m_spellcasts) { cast.Draw(time, batch); }
            batch.End();


            // Blur
            const int passes = 1;
            for (int i = 0; i < passes; i++)
            {
                m_blur.PerformGaussianBlur(m_tilesRenderTarget, m_tmpRenderTarget, m_tmpRenderTarget2, batch);
                m_blur.PerformGaussianBlur(m_tmpRenderTarget2, m_tmpRenderTarget, m_tilesRenderTarget, batch);
            }

            // Dessin du tout
            Mobattack.Instance.GraphicsDevice.SetRenderTarget(null);
            batch.GraphicsDevice.Clear(Color.LightBlue);
            Ressources.MapEffect.Parameters["xSourceTexture"].SetValue(m_tilesRenderTarget);
            Ressources.MapEffect.Parameters["scrolling"].SetValue(new Vector2(Scrolling.X / (float)Viewport.Width, Scrolling.Y / (float)Viewport.Height));
            batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, Ressources.MapEffect);
            batch.Draw(m_tilesRenderTarget, Viewport, Color.White);
            batch.End();

            batch.Begin();
            batch.Draw(m_entitiesRenderTarget, Viewport, Color.White);
            batch.End();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de Map.
        /// </summary>
        public Map()
        {
            m_entities = new EntityCollection();
            m_spellcasts = new List<Spellcast>();


            // Initialise l'effet de blur.
            m_blur = new Graphics.GaussianBlur(Mobattack.Instance);
            m_blur.ComputeKernel(4, 2);

            // Création du viewport
            Viewport = new Rectangle(0, 25, (int)Mobattack.GetScreenSize().X, (int)Mobattack.GetScreenSize().Y - 125);
            m_scrolling = new Point();
            

            
            // DEBUG CODE
            m_passability = new bool[50, 50];
            __passabilityDrawRect(new Rectangle(1, 1, 40, 40), true);
            __passabilityDrawRect(new Rectangle(3, 4, 20, 2), false);

            
            m_entities.Add(0, new EntityHero() { Position = new Vector2(2, 2), Type = EntityType.Team1Player, Role = EntityHeroRole.Fighter , BaseMaxHP = 50000, HP = 50000});
            m_entities.Add(1, new EntityHero() { Position = new Vector2(15, 10), Type = EntityType.Team2Player });
            m_entities.Add(2, new EntityTower() { Position = new Vector2(14, 20), Type = EntityType.Team2Tower });
            m_entities.Add(3, new EntityTower() { Position = new Vector2(21, 20), Type = EntityType.Team2Tower });
            m_entities.Add(4, new EntityTower() { Position = new Vector2(18, 32), Type = EntityType.Team2Tower });
            m_entities.Add(5, new EntityCreep() { Position = new Vector2(14, 2), Type = EntityType.Team1Creep });
            m_entities.Add(6, new EntityCreep() { Position = new Vector2(15, 2), Type = EntityType.Team1Creep });
            m_entities.Add(7, new EntityCreep() { Position = new Vector2(16, 2), Type = EntityType.Team1Creep });
            m_entities.Add(9, new EntityBase() { Position = new Vector2(21, 22), Type = EntityType.Team2Player });
            m_entities.Add(8, new EntitySpawner() { Position = new Vector2(13, 2), SpawnPosition = new Vector2(14, 2),  Type = EntityType.Team1Spawner });
            // ----

            Vision = new VisionMap(this);

            // Ajout des héros
            Heroes = new List<EntityHero>();
            Heroes.Add((EntityHero)m_entities[0]);
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

            UpdateScrolling();

        }

        
        /// <summary>
        /// Mise à jour du scrolling de la map.
        /// </summary>
        void UpdateScrolling()
        {
            // Scrolling
            int speed = 16;
            if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad4))
                m_scrolling.X -= speed;
            else if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad6))
                m_scrolling.X += speed;
            if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad8))
                m_scrolling.Y -= speed;
            else if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad2))
                m_scrolling.Y += speed;

            m_scrolling.X = Math.Max(0, Math.Min(m_passability.GetLength(0) * UnitSize - m_viewport.Width, m_scrolling.X));
            m_scrolling.Y = Math.Max(0, Math.Min(m_passability.GetLength(1) * UnitSize - m_viewport.Height, m_scrolling.Y));
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
            // screenSpacePos = 
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
        /// Retourne la passabilité de la map à la position donnée en unités métriques.
        /// </summary>
        public bool GetPassabilityAt(float x, float y)
        {
            if (x < 0 || y < 0 || x >= m_passability.GetLength(0) || y >= m_passability.GetLength(1))
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

            m_passability[(int)x, (int)y] = value;
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

        #region Save Load
        /// <summary>
        /// Sauvegarde la map.
        /// </summary>
        public void Save()
        {
            FileStream fs = new System.IO.FileStream(Ressources.MapFilename, System.IO.FileMode.Create);
            StreamWriter writer = new StreamWriter(fs);
            writer.WriteLine("size " + Size.X.ToString() + " " + Size.Y.ToString());
            writer.WriteLine("map ");
            for(int y = 0; y < Size.Y; y++)
            {
                for(int x = 0; x < Size.X; x++)
                {
                    writer.Write(GetPassabilityAt(x, y) ? "1" : "0");
                }
                writer.WriteLine();
            }

            // Ecrit les entités
            foreach(EntityBase entity in m_entities.Values)
            {
                string x = entity.Position.X.ToString();
                string y = entity.Position.Y.ToString();
                if(entity.Type.HasFlag(EntityType.Structure))
                    writer.WriteLine(entity.Type.ToString() + " " + x.ToString() + " " + y.ToString());
                else if(entity.Type.HasFlag(EntityType.Checkpoint))
                {
                    EntityCheckpoint cp = (EntityCheckpoint)entity;
                    writer.WriteLine(entity.Type.ToString() + " " + x.ToString() + " " + y.ToString() + " " + 
                        cp.CheckpointRow + " " + cp.CheckpointID);
                }
                    
            }

            writer.Flush();
            writer.Close();
            fs.Close();
        }

        /// <summary>
        /// Crée une nouvelle map depuis un fichier.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Map FromFile(string path)
        {
            string[] words = File.ReadAllText(path).Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Point size = new Point(10, 10);
            bool[,] pass = new bool[10, 10];
            EntityCollection newEntities = new EntityCollection();
            
            for(int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                if(word == "size")
                {
                    int sX = int.Parse(words[i + 1]);
                    int sY = int.Parse(words[i + 2]);
                    size = new Point(sX, sY);
                    pass = new bool[sX, sY];
                    i += 2;
                } 
                else if(word == "map")
                {
                    i++;

                    for(int y = 0; y < size.Y; y++)
                    {
                        for(int x = 0; x < size.X; x++)
                        {
                            pass[x, y] = words[i][x] == '1' ? true : false;
                        }
                        i++;
                    }

                    i--;
                }
                else
                {
                    try
                    {
                        // Entitié
                        EntityType type = (EntityType)Enum.Parse(typeof(EntityType), word);
                        float sX = float.Parse(words[i + 1]);
                        float sY = float.Parse(words[i + 2]);
                        EntityBase newEntity = null;
                        switch (type & (EntityType.AllSaved))
                        {
                            case EntityType.Tower:
                                newEntity = new EntityTower()
                                {
                                    Position = new Vector2(sX, sY),
                                    Type = type
                                };
                                break;

                            case EntityType.Spawner:
                                newEntity = new EntitySpawner()
                                {
                                    Position = new Vector2(sX, sY),
                                    SpawnPosition = new Vector2(sX, sY),
                                    Type = type
                                };
                                break;
                            case EntityType.Checkpoint:
                                int row = int.Parse(words[i + 3]);
                                int id = int.Parse(words[i + 4]);
                                i += 2;
                                newEntity = new EntityCheckpoint()
                                {
                                    Position = new Vector2(sX, sY),
                                    Type = type,
                                    CheckpointRow = row,
                                    CheckpointID = id
                                };
                                break;
                            case EntityType.Inhibitor:
                                throw new NotImplementedException();
                                break;
                            case EntityType.Miniboss:
                                throw new NotImplementedException();
                                break;
                            case EntityType.Idol:
                                throw new NotImplementedException();
                                break;
                            case EntityType.Boss:
                                throw new NotImplementedException();
                                break;
                        }

                        i += 2;
                        newEntities.Add(newEntity.ID, newEntity);
                    }
                    catch (System.ArgumentException) { }
                }
            }

            Map map = new Map() { Entities = newEntities, Passability = pass };

            EntityBase dummyPlayer = new EntityBase() { Position = new Vector2(2, 2), Type = EntityType.Team1Player };
            map.Entities.Add(dummyPlayer.ID, dummyPlayer);
            return map;
        }
        #endregion

    }


}
