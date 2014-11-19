using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Entities
{
    /// <summary>
    /// Représente un creep.
    /// </summary>
    public class EntitySpawner : EntityBase
    {
        #region Variables
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
            CreepsPerWave = 6;
            SpawnDecay = 0.1f;
            RowCount = 2;
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
                    Mobattack.GetScene().EventSheduler.Schedule(new Scheduler.ActionDelegate(() =>
                    {
                        EntityCreep creep = new EntityCreep()
                        {
                            Position = SpawnPosition,
                            Type = EntityType.Creep | (this.Type & (EntityType.Team1 | EntityType.Team2)),
                            Row = iref % RowCount,
                        };
                        Mobattack.GetMap().Entities.Add(creep.ID, creep);
                    }), decay);
                    decay += SpawnDecay;
                }
            }

            // Décrémente le timer d'apparition des vagues.
            m_timer -= (float)time.ElapsedGameTime.TotalSeconds;
        }



        #endregion

        
    }
}
