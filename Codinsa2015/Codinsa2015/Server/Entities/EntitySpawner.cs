using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Server.Entities
{
    /// <summary>
    /// Représente un spawner de creeps.
    /// </summary>
    public class EntitySpawner : EntityBase
    {
        #region Variables
        /// <summary>
        /// Si supérieur à 0, représente le temps restant du buff du big boss sur les creeps.
        /// </summary>
        float m_bossBuffTimer;
        float m_timer;
        /// <summary>
        /// Intervalle de temps en secondes entre l'apparition de 2 vagues de
        /// creeps.
        /// </summary>
        public float SpawnInterval { get; set; }
        /// <summary>
        /// Nombre de creeps apparaissant à chaque vague.
        /// </summary>
        public int CreepsPerWave { get; set; }
        /// <summary>
        /// Spawn position of the creeps.
        /// </summary>
        public Vector2 SpawnPosition { get; set; }
        /// <summary>
        /// Délai entre le spawn de 2 creeps.
        /// </summary>
        public float SpawnDecay { get; set; }

        /// <summary>
        /// Nombre de colonnes de creeps.
        /// </summary>
        public int RowCount { get; set; }
        #endregion

        #region Properties

        /// <summary>
        /// Obtient une valeur indiquant si les creeps spawnés par cette entités
        /// seront renforcés du buff du big boss.
        /// </summary>
        public bool HasBossBuff
        {
            get
            {
                return m_timer > 0;
            }
        }


        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de EntityTower.
        /// </summary>
        public EntitySpawner() : base()
        {
            BaseArmor = 40;
            BaseAttackDamage = 60;
            BaseMagicResist = 40;
            BaseMaxHP = 1200;
            HP = BaseMaxHP;
            BaseMoveSpeed = 0f;
            SpawnInterval = 30;
            CreepsPerWave = 0; // 6
            SpawnDecay = 0.1f;
            RowCount = 3;
            Type |= EntityType.Spawner;
        }

        /// <summary>
        /// Mets à jour la tour.
        /// </summary>
        protected override void DoUpdate(GameTime time)
        {
            base.DoUpdate(time);
            if(m_timer <= 0)
            {
                m_timer = SpawnInterval;
                float decay = 0;
                for(int i = 0; i < CreepsPerWave; i++)
                {
                    int iref = i;
                    GameServer.GetScene().EventSheduler.Schedule(new Scheduler.ActionDelegate(() =>
                    {
                        EntityCreep creep = new EntityCreep()
                        {
                            Position = SpawnPosition,
                            Type = EntityType.Creep | (this.Type & (EntityType.Team1 | EntityType.Team2)),
                            Row = iref % RowCount,
                        };
                        
                        if(HasBossBuff)
                        {
                            creep.ApplyBossBuff();
                        }

                        GameServer.GetMap().Entities.Add(creep.ID, creep);
                    }), decay);
                    decay += SpawnDecay;
                }
            }

            // Décrémente le timer d'apparition des vagues.
            m_timer -= (float)time.ElapsedGameTime.TotalSeconds;

            if(m_bossBuffTimer > 0)
                m_bossBuffTimer -= (float)time.ElapsedGameTime.TotalSeconds;
        }


        /// <summary>
        /// Applique le buff du big boss sur les creeps pendant la durée
        /// précisée en secondes.
        /// </summary>
        /// <param name="duration"></param>
        public void BuffCreeps(float duration)
        {
            m_bossBuffTimer = duration;
        }
        #endregion

        
    }
}
