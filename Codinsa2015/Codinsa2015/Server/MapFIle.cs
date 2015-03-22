using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
using Codinsa2015.Server.Events;
using System.IO;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server
{
    /// <summary>
    /// Représente un fichier map pouvant être chargé.
    /// </summary>
    public class MapFile
    {
        public bool[,] Passability { get; set; }
        public EntityCollection Entities { get; set; }
        public Dictionary<EventId, GameEvent> Events { get; set; }
        
        /// <summary>
        /// Sauvegarde la map passée en paramètre dans le fichier map par défaut.
        /// </summary>
        /// <param name="map"></param>
        public static void Save(Map map)
        {
            Save(map, Ressources.MapFilename);
        }
        /// <summary>
        /// Sauvegarde la map passée en paramètre dans le fichier dont le nom est passé en paramètre.
        /// </summary>
        /// <param name="map"></param>
        public static void Save(Map map, string filename)
        {
        
            FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create);
            StreamWriter writer = new StreamWriter(fs);
            writer.WriteLine("size " + map.Size.X.ToString() + " " + map.Size.Y.ToString());
            writer.WriteLine("map ");
            for (int y = 0; y < map.Size.Y; y++)
            {
                for (int x = 0; x < map.Size.X; x++)
                {
                    writer.Write(map.GetPassabilityAt(x, y) ? "1" : "0");
                }
                writer.WriteLine();
            }

            // Ecrit les entités
            foreach (EntityBase entity in map.Entities.Values)
            {
                string x = entity.Position.X.ToString();
                string y = entity.Position.Y.ToString();
                if(entity.Type.HasFlag(EntityType.Structure) | entity.Type.HasFlag(EntityType.WardPlacement))
                    writer.WriteLine(entity.Type.ToString() + " " + x.ToString() + " " + y.ToString());
                else if(entity.Type.HasFlag(EntityType.Checkpoint))
                {
                    EntityCheckpoint cp = (EntityCheckpoint)entity;
                    writer.WriteLine(entity.Type.ToString() + " " + x.ToString() + " " + y.ToString() + " " + 
                        cp.CheckpointRow + " " + cp.CheckpointID);
                }
                    
            }

            // Ecrit les évents.
            foreach(var kvp in map.Events)
            {
                GameEvent evt = kvp.Value;
                string x = evt.Position.X.ToString();
                string y = evt.Position.Y.ToString();
                writer.WriteLine(kvp.Key.ToString() + " " + x + " " + y);
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
        public static MapFile FromFile(string path)
        {
            string[] words = File.ReadAllText(path).Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Point size = new Point(10, 10);
            bool[,] pass = new bool[10, 10];
            EntityCollection newEntities = new EntityCollection();
            Dictionary<EventId, GameEvent> newEvents = new Dictionary<EventId, GameEvent>();
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                if (word == "size")
                {
                    int sX = int.Parse(words[i + 1]);
                    int sY = int.Parse(words[i + 2]);
                    size = new Point(sX, sY);
                    pass = new bool[sX, sY];
                    i += 2;
                }
                else if (word == "map")
                {
                    i++;

                    for (int y = 0; y < size.Y; y++)
                    {
                        for (int x = 0; x < size.X; x++)
                        {
                            pass[x, y] = words[i][x] == '1' ? true : false;
                        }
                        i++;
                    }

                    i--;
                }
                else
                {
                    #region Entities
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

                            case EntityType.WardPlacement:
                                newEntity = new EntityWardPlacement()
                                {
                                    Position = new Vector2(sX, sY),
                                    Type = type,
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

                        newEntities.Add(newEntity.ID, newEntity);
                        i += 2;
                    }
                    catch (System.ArgumentException) { }
                    #endregion



                    #region Events
                    try
                    {
                        // Entitié
                        EventId type = (EventId)Enum.Parse(typeof(EventId), word);
                        float sX = float.Parse(words[i + 1]);
                        float sY = float.Parse(words[i + 2]);
                        GameEvent newEvent = null;
                        switch (type)
                        {
                            case EventId.Camp4:
                            case EventId.Camp3:
                            case EventId.Camp2:
                            case EventId.Camp1:
                                newEvent = new EventMonsterCamp() { Position = new Vector2(sX, sY) };
                                break;
                            case EventId.MinibossWest:
                            case EventId.MinibossEast:
                                newEvent = new EventMiniboss() { Position = new Vector2(sX, sY) };
                                break;
                            case EventId.Bigboss:
                                newEvent = new EventBigBoss() { Position = new Vector2(sX, sY) };
                                break;
                        }

                        newEvents.Add(type, newEvent);
                        i += 2;
                    }
                    catch (System.ArgumentException) { }
                    #endregion
                }
            }

            MapFile map = new MapFile() { Entities = newEntities, Passability = pass, Events = newEvents };
            return map;
        }
    }
}
