using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Server.Entities
{
    /// <summary>
    /// Représente un spawner de Virus.
    /// </summary>
    public class EntitySpawner : EntityBase
    {
        #region Variables
        /// <summary>
        /// Si supérieur à 0, représente le temps restant du buff du big boss sur les Virus.
        /// </summary>
        float m_bossBuffTimer;
        float m_respawnTimer;
        float m_waveTimer;
        /// <summary>
        /// Intervalle de temps en secondes entre l'apparition de 2 vagues de
        /// Virus.
        /// </summary>
        public float SpawnInterval { get; set; }
        /// <summary>
        /// Nombre de Virus apparaissant à chaque vague.
        /// </summary>
        public int VirusPerWave { get; set; }
        /// <summary>
        /// Spawn position of the Virus.
        /// </summary>
        public Vector2 SpawnPosition { get; set; }
        /// <summary>
        /// Délai entre le spawn de 2 Virus.
        /// </summary>
        public float SpawnDecay { get; set; }

        /// <summary>
        /// Nombre de colonnes de Virus.
        /// </summary>
        public int RowCount { get; set; }
        #endregion

        #region Properties

        /// <summary>
        /// Obtient une valeur indiquant si les Virus spawnés par cette entités
        /// seront renforcés du buff du big boss.
        /// </summary>
        public bool HasBossBuff
        {
            get
            {
                return m_waveTimer > 0;
            }
        }

        /// <summary>
        /// Le spawner est immunisé au dégâts si il est désactivé.
        /// </summary>
        public override bool IsDamageImmune
        {
            get
            {
                if (m_respawnTimer > 0)
                    return true;
                return base.IsDamageImmune;
            }
        }

        /// <summary>
        /// Désactive le timer.
        /// </summary>
        public override void Die()
        {
            m_respawnTimer = GameServer.GetScene().Constants.Structures.Spawners.RespawnTimer;
            m_waveTimer = 0;
            m_bossBuffTimer = 0;
            HP = GetMaxHP();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de EntityTower.
        /// </summary>
        public EntitySpawner() : base()
        {
            var cst = GameServer.GetScene().Constants.Structures.Spawners;
            RowCount = cst.Rows;
            SpawnDecay = cst.ViruspawnDelay;
            VirusPerWave = cst.VirusPerWave;
            SpawnInterval = cst.WavesInterval;
            Type |= EntityType.Spawner;
        }

        /// <summary>
        /// Mets à jour la tour.
        /// </summary>
        protected override void DoUpdate(GameTime time)
        {
            base.DoUpdate(time);

            // Temps d'attente avant de respawn.
            if(m_respawnTimer > 0)
            {
                m_respawnTimer -= (float)time.ElapsedGameTime.TotalSeconds;
                return;
            }

            // Si on doit spawn une vague
            if(m_waveTimer <= 0)
            {
                m_waveTimer = SpawnInterval;
                float decay = 0;
                for(int i = 0; i < VirusPerWave; i++)
                {
                    int iref = i;
                    var DatacenterCandidates = GameServer.GetMap().Entities.GetEntitiesByType((this.Type & EntityType.Teams) | EntityType.Datacenter);
                    if (DatacenterCandidates.Count > 0)
                    {
                        var Datacenter = DatacenterCandidates.First().Value;
                        SpawnPosition = Datacenter.Position;
                    }

                    GameServer.GetScene().EventSheduler.Schedule(new Scheduler.ActionDelegate(() =>
                    {
                        EntityVirus Virus = new EntityVirus()
                        {
                            Position = SpawnPosition,
                            Type = EntityType.Virus | (this.Type & (EntityType.Team1 | EntityType.Team2)),
                            Row = iref % RowCount,
                        };
                        
                        if(HasBossBuff)
                        {
                            Virus.ApplyBossBuff();
                        }

                        GameServer.GetMap().Entities.Add(Virus.ID, Virus);
                    }), decay);
                    decay += SpawnDecay;
                }
            }

            // Décrémente le timer d'apparition des vagues.
            m_waveTimer -= (float)time.ElapsedGameTime.TotalSeconds;

            if(m_bossBuffTimer > 0)
                m_bossBuffTimer -= (float)time.ElapsedGameTime.TotalSeconds;
        }


        /// <summary>
        /// Applique le buff du big boss sur les Virus pendant la durée
        /// précisée en secondes.
        /// </summary>
        /// <param name="duration"></param>
        public void BuffVirus(float duration)
        {
            m_bossBuffTimer = duration;
            m_respawnTimer = 0;
        }
        #endregion

        
    }
}
