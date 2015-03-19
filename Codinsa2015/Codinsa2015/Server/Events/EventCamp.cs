using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Events
{
    /// <summary>
    /// Représente un camp.
    /// Le comportement du camp est le suivant :
    ///     - au début, il est neutre : la première équipe qui tue le dernier monstre du camp 
    ///     s'octroie la possession de ce camp.
    ///     - le camp va alors faire spawn des entités qui vont push une lane de l'équipe qui
    ///     a la possession du camp..
    ///     - au bout d'un certain temps, le camp repawne, et peut être détruit par les 2 équipes.
    /// </summary>
    public class EventMonsterCamp : GameEvent
    {
        /// <summary>
        /// Position de l'event.
        /// </summary>
        Vector2 m_position;
        /// <summary>
        /// Offset déterminant le spawn des creeps.
        /// </summary>
        Vector2 m_creepSpawnOffset;
        /// <summary>
        /// Indique si le camp est actuellement dans l'état détruit.
        /// </summary>
        bool m_destroyed;
        /// <summary>
        /// Indique si l'équipe 1 a actuellement accès au timer de ce camp.
        /// </summary>
        bool m_team1Timer;
        /// <summary>
        /// Indique si l'équipe 2 a actuellement accès au timer de ce camp.
        /// </summary>
        bool m_team2Timer;
        /// <summary>
        /// Temps restant avant la réapparition du camp. (si m_destroyed vaut true).
        /// </summary>
        float m_respawnTimer;
        /// <summary>
        /// Temps restant avec le spawn du prochain creep.
        /// </summary>
        float m_creepSpawnTimer;
        /// <summary>
        /// Représente la team propriétaire du camp.
        /// </summary>
        Entities.EntityType m_teamOwner;
        /// <summary>
        /// Liste des monstres contrôlés par le camp.
        /// </summary>
        List<Entities.EntityCampMonster> m_monsters;
        /// <summary>
        /// Tueur du dernier monstre.
        /// </summary>
        Entities.EntityHero m_lastKiller;


        /// <summary>
        /// Obtient ou définit la position de l'évènement.
        /// </summary>
        public override Vector2 Position { get { return m_position; } set { m_position = value; } }

        /// <summary>
        /// Crée une nouvelle instance de EventCamp.
        /// </summary>
        public EventMonsterCamp()
        {

        }


        /// <summary>
        /// Initialise l'évènement.
        /// </summary>
        public override void Initialize()
        {
            m_position = new Vector2(30, 40);
            m_destroyed = true;
            m_team1Timer = true;
            m_team2Timer = true;
            m_teamOwner = 0;
            m_monsters = new List<Entities.EntityCampMonster>();
            m_lastKiller = null;
            m_creepSpawnOffset = new Vector2(0, 2);
            m_respawnTimer = 0; // GameServer.GetScene().Constants.Events.MonsterCamp.RespawnTimer;
        }

        /// <summary>
        /// Mets à jour l'évènement.
        /// </summary>
        public override void Update(GameTime time)
        {
            // Si le camp n'est pas détruit : on attend qu'il le soit.
            if (!m_destroyed)
            {
                // Si tous les monstres du camp sont tués, on donne l'ownership du camp 
                bool allDead = true;
                foreach (Entities.EntityCampMonster monster in m_monsters)
                {
                    allDead &= monster.IsDead;
                }

                if (allDead)
                {
                    if (m_lastKiller == null)
                        throw new Exception("Problème ?");

                    m_teamOwner = m_lastKiller.Type & Entities.EntityType.Teams;

                    // Distribution du timer à la team qui a tué le camp.
                    if (m_teamOwner == Entities.EntityType.Team1)
                        m_team1Timer = true;
                    else if (m_teamOwner == Entities.EntityType.Team2)
                        m_team2Timer = true;
                    // Attribution de la récompense au tueur
                    m_lastKiller.PA += GameServer.GetScene().Constants.Events.MonsterCamp.Reward;
                    // Distribution du timer aux équipes ayant la vision.
                    m_team1Timer |= GameServer.GetMap().Vision.HasVision(Entities.EntityType.Team1, m_position);
                    m_team2Timer |= GameServer.GetMap().Vision.HasVision(Entities.EntityType.Team2, m_position);
                    m_respawnTimer = GameServer.GetScene().Constants.Events.MonsterCamp.RespawnTimer;
                    m_destroyed = true;
                }
            }
            else
            {
                // Respawn du camp si le timer expire.
                m_respawnTimer -= (float)time.ElapsedGameTime.TotalSeconds;
                if (m_respawnTimer <= 0)
                {
                    SpawnCamp();
                    m_teamOwner = 0;
                    m_destroyed = false;
                }
            }

            // Si une team a l'ownership du camp, on fait spawn des sbires.
            if (m_teamOwner != 0)
            {
                m_creepSpawnTimer -= (float)time.ElapsedGameTime.TotalSeconds;
                if (m_creepSpawnTimer <= 0)
                {
                    m_creepSpawnTimer = GameServer.GetScene().Constants.Events.MonsterCamp.CreepSpawnInterval;
                    SpawnCreep();
                }
            }
          
        }

        /// <summary>
        /// Fait apparaître un creep.
        /// </summary>
        void SpawnCreep()
        {
            Entities.EntityCreep creep = new Entities.EntityCreep()
            {
                Type = Entities.EntityType.Creep | m_teamOwner,
                Row = 0,
                Position = m_position + m_creepSpawnOffset
            };
            GameServer.GetMap().AddEntity(creep);
        }

        /// <summary>
        /// Fait apparaître les monstres neutres.
        /// </summary>
        void SpawnCamp()
        {
            Vector2[] offsets = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(1, 0), 
                new Vector2(0, 1)
            };

            // Crée les 3 monstres du camp.
            m_monsters.Clear();
            for(int i = 0; i < 3; i++)
            {
                var monster = new Entities.EntityCampMonster(m_position + offsets[i])
                {
                    Type = Entities.EntityType.Monster,
                };
                m_monsters.Add(monster);
                GameServer.GetMap().AddEntity(monster);
                m_monsters[i].OnDie += EventCamp_OnDie;
            }
        }

        /// <summary>
        /// Se produit lorsqu'un des monstre du camp meurt.
        /// Mémorise le tueur du camp.
        /// </summary>
        void EventCamp_OnDie(Entities.EntityBase entity, Entities.EntityHero killer)
        {
            m_lastKiller = killer;
        }
    }
}
