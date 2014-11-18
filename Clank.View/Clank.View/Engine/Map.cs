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
        public const int UnitSize = 16;

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

        #endregion

        #region Properties
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
            set { m_viewport = value; }
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
            set { m_passability = value; }
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
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de Map.
        /// </summary>
        public Map()
        {
            m_entities = new EntityCollection();
            m_spellcasts = new List<Spellcast>();
            m_viewport = new Rectangle(0, 0, 640, 480);
            m_scrolling = new Point();


            
            // DEBUG CODE
            m_passability = new bool[50, 50];
            __passabilityDrawRect(new Rectangle(1, 1, 40, 40), true);
            __passabilityDrawRect(new Rectangle(3, 4, 20, 2), false);

            
            m_entities.Add(0, new EntityBase() { Position = new Vector2(2, 2), Type = EntityType.Team1Player });
            m_entities.Add(1, new EntityBase() { Position = new Vector2(15, 10), Type = EntityType.Team2Player });
            m_entities.Add(2, new EntityTower() { Position = new Vector2(14, 20), Type = EntityType.Team2Tower });
            m_entities.Add(3, new EntityTower() { Position = new Vector2(21, 20), Type = EntityType.Team2Tower });
            m_entities.Add(4, new EntityTower() { Position = new Vector2(18, 32), Type = EntityType.Team2Tower });
            m_entities.Add(5, new EntityCreep() { Position = new Vector2(14, 2), Type = EntityType.Team1Creep });
            m_entities.Add(6, new EntityCreep() { Position = new Vector2(15, 2), Type = EntityType.Team1Creep });
            m_entities.Add(7, new EntityCreep() { Position = new Vector2(16, 2), Type = EntityType.Team1Creep });
            m_entities.Add(9, new EntityBase() { Position = new Vector2(21, 22), Type = EntityType.Team2Player });
            m_entities.Add(8, new EntitySpawner() { Position = new Vector2(13, 2), SpawnPosition = new Vector2(14, 2),  Type = EntityType.Team1Spawner });
            // ----

        }
        /// <summary>
        /// Mets à jour la map ainsi que les entités qu'elle contient.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
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
            m_scrolling.Y = Math.Max(0, Math.Min(m_passability.GetLength(1) * UnitSize - m_viewport.Width, m_scrolling.Y));
        }
        /// <summary>
        /// Dessine la map ainsi que les entités qu'elle contient.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="batch"></param>
        public void Draw(GameTime time, SpriteBatch batch)
        {


            // Dessin de debug de la map.
            // TODO ne dessiner que ce qu'il faut !
            for(int x = 0; x < m_passability.GetLength(0); x++)
            {
                for(int y = 0; y < m_passability.GetLength(1); y++)
                {
                    Point drawPos = new Point(m_viewport.X + x * UnitSize - Scrolling.X, m_viewport.Y + y * UnitSize - Scrolling.Y);
                    if(!m_passability[x, y])
                    {
                        batch.Draw(Ressources.DummyTexture, new Rectangle(drawPos.X, drawPos.Y, UnitSize, UnitSize), Color.Red);
                    }
                }
            }

            // Dessin des entités
            foreach (var kvp in m_entities) { kvp.Value.Draw(time, batch); }
            foreach (Spellcast cast in m_spellcasts) { cast.Draw(time, batch); }
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
            FileStream fs = new System.IO.FileStream("Content/map.txt", System.IO.FileMode.Create);
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
                if(entity.Type.HasFlag(EntityType.Struture))
                    writer.WriteLine(entity.Type.ToString() + " " + ((int)entity.Position.X).ToString() + " " + ((int)entity.Position.Y).ToString());
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
                        int sX = int.Parse(words[i + 1]);
                        int sY = int.Parse(words[i + 2]);
                        EntityBase newEntity = null;
                        switch (type & (EntityType.AllObjectives))
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
                    catch (System.ArgumentException e) { }
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
