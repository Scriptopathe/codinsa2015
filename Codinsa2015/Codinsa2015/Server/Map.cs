using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.Entities;
using Codinsa2015.Server.Spellcasts;
using System.IO;

using Codinsa2015.Server.Events;
namespace Codinsa2015.Server
{
    /// <summary>
    /// Classe représentant la map.
    /// </summary>
    public class Map
    {

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
        /// Dictionnaire contenant les différents évènements du jeu, addressés par leur type.
        /// </summary>
        Dictionary<EventId, GameEvent> m_events;

        /// <summary>
        /// Rectangle de l'écran sur lequel sera dessinée la map.
        /// </summary>
        Rectangle m_viewport;
        /// <summary>
        /// Liste des entités à ajouter à la prochaine frame.
        /// </summary>
        EntityCollection m_entitiesAddList;

        /// <summary>
        /// Indique si les évents doivent être initialisés à la prochaine update.
        /// </summary>
        bool m_refreshEvents;
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
        /// Dictionnaire contenant les différents évènements du jeu, addressés par leur type.
        /// </summary>
        public Dictionary<EventId, GameEvent> Events
        {
            get { return m_events; }
            set { m_events = value; }
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
            set { m_passability = value; Vision.SetSize(new Point(this.Passability.GetLength(0), this.Passability.GetLength(1))); }
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

        
        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de Map.
        /// </summary>
        public Map()
        {
            m_entities = new EntityCollection();
            m_entitiesAddList = new EntityCollection();
            m_spellcasts = new List<Spellcast>();
            m_events = new Dictionary<EventId, GameEvent>();

            // DEBUG CODE
            m_passability = new bool[50, 50];
            __passabilityDrawRect(new Rectangle(1, 1, 40, 40), true);
            __passabilityDrawRect(new Rectangle(3, 4, 20, 2), false);

            // TODO : chargement des évents.
            CreateDefaultEvents();
            CreateDebugEvents();

            // Vision
            Vision = new VisionMap(this);

            // Ajout des héros
            Heroes = new List<EntityHero>();
        }

        /// <summary>
        /// Ajoute les évènements toujours présents sur la map.
        /// </summary>
        void CreateDefaultEvents()
        {
            m_events.Add(EventId.Resurrector, new Events.PlayerResurrectorEvent());
        }
        /// <summary>
        /// Crée et ajoute les évènements de debug de la map.
        /// </summary>
        void CreateDebugEvents()
        {
            m_events.Add(EventId.Camp1, new Events.EventMonsterCamp());
            m_events.Add(EventId.Miniboss2, new Events.EventMiniboss());
        }

        /// <summary>
        /// Initialise les évents de la map une fois que tout le reste est prêt.
        /// </summary>
        public void Initialize()
        {
            // Initialisation des évents
            foreach (var kvp in m_events)
                kvp.Value.Initialize();
        }
        /// <summary>
        /// Mets à jour la map ainsi que les entités qu'elle contient.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            // Mise à jour de la vision.
            Vision.Update(time, Entities);

            // Mise à jour des entités
            List<int> entitiesToDelete = new List<int>();
            List<Spellcast> spellcastsToDelete = new List<Spellcast>();

            // Ajoute les entités en attente.
            while (m_entitiesAddList.Count != 0)
            {
                var first = m_entitiesAddList.First();
                m_entities.Add(first.Key, first.Value);
                m_entitiesAddList.Remove(first.Key);
            }

            // Refresh les events
            // Certains events sont dépendant d'entités, il faut donc attendre qu'elles soient ajoutées au jeu.
            // C'est pourquoi ce code est ici.
            if(m_refreshEvents)
            {
                foreach(var kvp in m_events)
                {
                    kvp.Value.Initialize();
                }
                m_refreshEvents = false;
            }

            // Mets à jour tous les spells lancés.
            foreach (Spellcast cast in m_spellcasts)
            {
                cast.Update(time);
                if (cast.IsDisposing)
                {
                    spellcastsToDelete.Add(cast);
                }
            }



            // Mets à jour tous les évènements.
            foreach (var kvp in m_events)
                kvp.Value.Update(time);

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

            // Cherche les spawners des 2 équipes
            EntityCollection spawners = map.Entities.GetEntitiesByType(EntityType.HeroSpawner);
            EntityCollection newEntities = new EntityCollection();
            
            // Supprime les entités non-héros
            foreach(var entity in Entities)
            {
                if (entity.Value.Type.HasFlag(EntityType.Player))
                {
                    newEntities.Add(entity.Key, entity.Value);

                    // Positionne les héros sur les spawners.
                    var spawnerCol = spawners.GetEntitiesByType(entity.Value.Type & EntityType.Teams);
                    if(spawnerCol.Count > 0)
                    {
                        EntityBase spawner = spawnerCol.First().Value;
                        entity.Value.Position = spawner.Position;
                    }
                }
            }

            foreach(var entity in map.Entities)
            {
                newEntities.Add(entity.Key, entity.Value);
            }

            // Ajoute les évents.
            m_events.Clear();

            CreateDefaultEvents();
            foreach(var kvp in map.Events)
            {
                if(!m_events.ContainsKey(kvp.Key) && kvp.Value != null)
                    m_events.Add(kvp.Key, kvp.Value);
            }

            Entities = newEntities;
            Vision.SetMap(this);
            m_refreshEvents = true;
            if(OnMapModified != null)
                OnMapModified();
        }
        #endregion
    }


}


