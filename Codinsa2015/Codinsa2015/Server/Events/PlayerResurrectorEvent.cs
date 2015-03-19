using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Events
{
    /// <summary>
    /// Evènement chargé de résusciter les héros morts après un certain délai.
    /// </summary>
    public class PlayerResurrectorEvent : GameEvent
    {

        class TimerRef
        {
            public float Value { get; set; }
            public TimerRef(float value) { Value = value; }
        }
        /// <summary>
        /// Héros présents sur la map.
        /// </summary>
        List<EntityHero> m_heroes;
        /// <summary>
        /// Timers de résurrection pour chaque héros mort.
        /// </summary>
        Dictionary<EntityHero, TimerRef> m_resurrectTimers;
        /// <summary>
        /// Crée une nouvelle instance de PlayerResurrectorEvent.
        /// </summary>
        /// <param name="heroes"></param>
        public PlayerResurrectorEvent()
        {
            
        }

        /// <summary>
        /// Initialise le Resurrector.
        /// </summary>
        public override void Initialize()
        {
            m_resurrectTimers = new Dictionary<EntityHero, TimerRef>();
            m_heroes = GameServer.GetMap().Heroes;
            foreach (var hero in m_heroes)
            {
                hero.OnDie += hero_OnDie;
            }
        }

        /// <summary>
        /// Callback appelé lorsqu'un héros meurt.
        /// </summary>
        /// <param name="killer"></param>
        void hero_OnDie(EntityBase entity, EntityHero killer)
        {
            m_resurrectTimers.Add((EntityHero)entity, ComputeDeathTimer());
        }

        /// <summary>
        /// Mets à jour le Resurrector.
        /// </summary>
        public override void Update(GameTime time)
        {
            List<EntityHero> resurrected = new List<EntityHero>();
            foreach(var kvp in m_resurrectTimers)
            {
                kvp.Value.Value -= (float)(time.ElapsedGameTime.TotalSeconds);
                if(kvp.Value.Value <= 0)
                {
                    kvp.Key.Resurrect();
                    resurrected.Add(kvp.Key);
                    
                    // Trouve le spawner de l'entité :
                    var spawners = GameServer.GetMap().Entities.GetEntitiesByType(EntityType.HeroSpawner | (kvp.Key.Type & EntityType.Teams));
                    if (spawners.Count != 1)
                        throw new Exceptions.IdiotProgrammerException("Il doit exister un unique spawner par équipe pour faire fonctionner le resurrector.");
                    EntityBase spawner = spawners.First().Value;
                    kvp.Key.Position = spawner.Position;
                    GameServer.GetMap().AddEntity(kvp.Key);
                }
            }

            // Supprime les timers pour les joueurs ressuscités.
            foreach(EntityHero hero in resurrected)
            {
                m_resurrectTimers.Remove(hero);
            }
        }

        /// <summary>
        /// Calcule et retourne le timer de mort des héros.
        /// </summary>
        TimerRef ComputeDeathTimer()
        {
            return new TimerRef(10);
        }


        /// <summary>
        /// Position de l'event (n'a pas de sens pour cet event ci).
        /// </summary>
        public override Vector2 Position
        {
            get
            {
                return Vector2.Zero;
            }
            set
            {

            }
        }
    }
}
