using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Events
{
    /// <summary>
    /// Représente un camp de miniboss.
    /// Le miniboss accorde la vision sur une large zone de la map une fois tué pendant
    /// une période de temps limitée.
    /// Après cette période de temps, le mini-boss réapparait.
    /// </summary>
    public class EventMiniboss : GameEvent
    {
        /// <summary>
        /// Position de l'event.
        /// </summary>
        Vector2 m_position;
        /// <summary>
        /// Indique si le miniboss est actuellement dans l'état détruit.
        /// </summary>
        bool m_destroyed;
        /// <summary>
        /// Indique si l'équipe 1 a actuellement accès au timer de respawn du miniboss.
        /// </summary>
        bool m_team1Timer;
        /// <summary>
        /// Indique si l'équipe 2 a actuellement accès au timer de respawn du miniboss.
        /// </summary>
        bool m_team2Timer;
        /// <summary>
        /// Temps restant avant la réapparition du camp. (si m_destroyed vaut true).
        /// </summary>
        float m_respawnTimer;
        /// <summary>
        /// Représente la team propriétaire du camp.
        /// </summary>
        Entities.EntityType m_teamOwner;
        /// <summary>
        /// Monstre contrôlé par l'évènement.
        /// </summary>
        Entities.EntityMiniboss m_miniboss;
        /// <summary>
        /// Dernier tueur du miniboss.
        /// </summary>
        Entities.EntityHero m_lastKiller;
        /// <summary>
        /// Représente la ward libérée une fois le camp tué.
        /// </summary>
        Entities.EntityWard m_ward;
        /// <summary>
        /// Crée une nouvelle instance de EventMiniboss.
        /// </summary>
        public EventMiniboss()
        {

        }


        /// <summary>
        /// Initialise l'évènement.
        /// </summary>
        public override void Initialize()
        {
            m_position = new Vector2(40, 40);
            m_destroyed = true;
            m_team1Timer = true;
            m_team2Timer = true;
            m_teamOwner = 0;
            m_lastKiller = null;
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
                bool allDead = m_miniboss.IsDead;

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
                    m_lastKiller.PA += GameServer.GetScene().Constants.Events.MinibossCamp.Reward;

                    // Distribution du timer aux équipes ayant la vision.
                    m_team1Timer |= GameServer.GetMap().Vision.HasVision(Entities.EntityType.Team1, m_position);
                    m_team2Timer |= GameServer.GetMap().Vision.HasVision(Entities.EntityType.Team2, m_position);
                    m_respawnTimer = GameServer.GetScene().Constants.Events.MinibossCamp.RespawnTimer;
                    m_destroyed = true;
                    SpawnWard();
                }
            }
            else
            {
                // Respawn du camp si le timer expire.
                m_respawnTimer -= (float)time.ElapsedGameTime.TotalSeconds;
                if (m_respawnTimer <= 0)
                {
                    if(m_ward != null)
                        m_ward.Die();
                    SpawnCamp();
                    m_teamOwner = 0;
                    m_destroyed = false;
                }
            }


        }

        /// <summary>
        /// Fait apparaître la ward décernée au tueur du mini-boss.
        /// </summary>
        void SpawnWard()
        {
            m_ward = new Entities.EntityWard()
            {
                Type = Entities.EntityType.Ward | m_teamOwner,
                Position = m_position,
            };
            GameServer.GetMap().AddEntity(m_ward);
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
            m_miniboss = new Entities.EntityMiniboss(m_position);
            m_miniboss.OnDie += EventCamp_OnDie;
            GameServer.GetMap().AddEntity(m_miniboss);
        }

        /// <summary>
        /// Se produit lorsque le miniboss est tué.
        /// </summary>
        void EventCamp_OnDie(Entities.EntityBase entity, Entities.EntityHero killer)
        {
            m_lastKiller = killer;
        }
    }
}
