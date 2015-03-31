using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Server.Entities
{
    /// <summary>
    /// Représente un creep.
    /// </summary>
    public class EntityCreep : EntityBase
    {
        static Random s_random = new Random();
        /// <summary>
        /// Pourcentage de la range d'attaque à laquelle va s'approcher cette unité
        /// pour attaquer.
        /// </summary>
        const float AttackRangeApproach = 0.80f;
        /// <summary>
        /// Délai moyen (+- 10%) après lequel l'aggro des creeps sera mise à jour.
        /// </summary>
        const float AggroUpdateDelay = 0.100f;
        /// <summary>
        /// Pourcentage d'altération random possible sur le délai de mise à jour de l'aggro.
        /// </summary>
        const float AggroUpdateDelayMargin = 0.100f;
        
        #region Variables
        /// <summary>
        /// Référence vers l'entité ayant l'aggro de la tour.
        /// </summary>
        EntityBase m_currentAgro;
        Vector2 m_currentAggroOldPos;
        /// <summary>
        /// Sort d'attaque de la tour.
        /// </summary>
        Spells.Spell m_attackSpell;

        /// <summary>
        /// Range du creep, en unités métriques.
        /// </summary>
        float AttackRange { get; set; }

        /// <summary>
        /// Obtient la rangée de checkpoints que devra suivre ce creep.
        /// </summary>
        public int Row { get; set; }
        

        Trajectory m_path;

        /// <summary>
        /// Checkpoint actuel de la trajectoire prévue du creep.
        /// </summary>
        int m_currentCheckpointId;

        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de EntityTower.
        /// </summary>
        public EntityCreep() : base()
        {
            LoadEntityConstants(GameServer.GetScene().Constants.Creeps);
            AttackRange = GameServer.GetScene().Constants.Creeps.AttackRange;
            HP = BaseMaxHP;
            m_currentCheckpointId = -1;
            m_attackSpell = new Spells.FireballSpell(this);
            Type |= EntityType.Creep;
        }

        /// <summary>
        /// Mets à jour la tour.
        /// </summary>
        protected override void DoUpdate(GameTime time)
        {
            base.DoUpdate(time);
            UpdateAggro(time);
            Attack(time);
            Travel(time);
        }
        
        /// <summary>
        /// Applique le buff du big boss à ce creep.
        /// </summary>
        public void ApplyBossBuff()
        {
            LoadEntityConstants(GameServer.GetScene().Constants.BuffedCreeps);
            AttackRange = GameServer.GetScene().Constants.BuffedCreeps.AttackRange;
        }

        /// <summary>
        /// Attaque l'ennemi ayant l'agro.
        /// </summary>
        void Attack(GameTime time)
        {
            m_attackSpell.UpdateCooldown((float)time.ElapsedGameTime.TotalSeconds);
            if (m_currentAgro != null && !m_currentAgro.Type.HasFlag(EntityType.Checkpoint))
            {
                float dstSqr = Vector2.DistanceSquared(m_currentAgro.Position, Position);
                if(dstSqr <= AttackRange * AttackRange )
                {
                    m_attackSpell.Use(new Spells.SpellCastTargetInfo()
                    {
                        TargetDirection = m_currentAgro.Position - Position,
                        Type = Spells.TargettingType.Direction,
                    });
                }

            }
        }

        /// <summary>
        /// Calcule la trajectoire de ce creep.
        /// </summary>
        void ComputePath()
        {
            if(m_currentAgro != null)
            {
                m_path = new Trajectory(PathFinder.Astar(this.Position, m_currentAgro.Position));
                if(m_path.TrajectoryUnits.Count <= 0)
                {
                    string ah = "5";
                }
            }
        }

        /// <summary>
        /// Fait avancer ce creep vers sa destination.
        /// </summary>
        void Travel(GameTime time)
        {
            // Si on a pas de trajectoire, on return
            if (m_path == null || m_path.TrajectoryUnits.Count == 0 || m_currentAgro == null)
                return;

            // On mets à jour la trajectoire.
            m_path.UpdateStep(Position, GetMoveSpeed(), time);
            m_path.Offset = (Row - 1) * new Vector2(0.25f, 0.25f);
            Vector2 nextPosition = m_path.CurrentStep;

            // Si on est trop près d'un sbire, on s'arrête
            EntityType allyCreepsType = EntityTypeConverter.ToAbsolute(EntityTypeRelative.AllyCreep, Type);
            EntityCollection allyCreeps = GameServer.GetMap().Entities.GetEntitiesByType(allyCreepsType);
            foreach(var kvp in allyCreeps)
            {
                EntityCreep entity = (EntityCreep)kvp.Value;
                if (entity == this || entity.Row != this.Row)
                    continue;

                // Ce creep est trop près, on s'arrête s'il a le même aggro et est plus proche.
                if(Vector2.DistanceSquared(entity.Position, Position) <= 4.0f)
                {
                    if (entity.ID < this.ID)
                        return;
                    
                }
            }

            // on s'arrête quand on est en range d'une tour / creep.
            float dstSqr = Vector2.DistanceSquared(m_path.LastPosition(), Position);
            float range = AttackRange * AttackRangeApproach;
            if (m_currentAgro.Type.HasFlag(EntityType.Checkpoint) || dstSqr > range * range)
            {
                Direction = nextPosition - Position;
                MoveForward(time);
            }
        }

        float __updateAgroDelay;
        /// <summary>
        /// Mets à jour l'aggro du creep.
        /// </summary>
        void UpdateAggro(GameTime time)
        {
            // Vérification du délai d'update
            // Ce délai est mis en place pour des questions de performances.
            __updateAgroDelay -= (float)time.ElapsedGameTime.TotalSeconds;
            if (__updateAgroDelay > 0)
            {
                return;
            }

            // Mise à jour du délai d'update
            float margin = AggroUpdateDelay * AggroUpdateDelayMargin;
            __updateAgroDelay = AggroUpdateDelay + ((float)s_random.NextDouble() - 0.5f) * margin;


            // Commencement de l'update.
            EntityCollection entitiesInRange = GameServer.GetMap().Entities.GetAliveEntitiesInRange(this.Position, AttackRange).GetEntitiesInSight(Type & EntityType.Teams);
            EntityBase oldAggro = m_currentAgro;

            if (m_currentAgro != null && (m_currentAgro.IsDead || !HasSightOn(m_currentAgro)))
                m_currentAgro = null;

            // Si la creep n'a pas d'aggro : on cherche la première unité creep en range
            EntityType ennemyCreepType = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyCreep, this.Type & (EntityType.Team1 | EntityType.Team2));
            EntityBase nearestEnnemyCreep = entitiesInRange.GetEntitiesByType(ennemyCreepType).NearestFrom(this.Position);
            if(nearestEnnemyCreep != null)
                m_currentAgro = nearestEnnemyCreep;
            
            // S'il n'y a pas de creep ennemi en range, on regarde si il y a une tour en range.
            EntityType ennemyTower = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyTower, this.Type & (EntityType.Team1 | EntityType.Team2));
            EntityBase nearestTower = entitiesInRange.GetEntitiesByType(ennemyTower).NearestFrom(this.Position);
            if(nearestTower != null)
                m_currentAgro = nearestTower;

            // Idole
            EntityType ennemyIdol = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyIdol, this.Type & (EntityType.Team1 | EntityType.Team2));
            EntityBase nearestIdol = entitiesInRange.GetEntitiesByType(ennemyIdol).NearestFrom(this.Position);
            if (nearestIdol != null)
                m_currentAgro = nearestIdol;

            // Si on n'en trouve pas : on cherche le premier héros en range.
            EntityType ennemyHero = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyPlayer, this.Type & (EntityType.Team1 | EntityType.Team2));
            EntityBase nearestEnnemyHero = entitiesInRange.GetEntitiesByType(ennemyHero).NearestFrom(this.Position);
            if(nearestEnnemyHero != null)
                m_currentAgro = nearestEnnemyHero;

            // Avance au checkpoint suivant si on a atteint le précédent ou qu'on a rien trouvé à aggro.
            if (m_currentAgro == null || (m_currentAgro.Type.HasFlag(EntityType.Checkpoint) && HasReachedPosition(m_currentAgro.Position, time, GetMoveSpeed())))
            {
                //bool __debugPositionReached = HasReachedPosition(m_currentAgro.Position, time, GetMoveSpeed());
                EntityType allycp = EntityTypeConverter.ToAbsolute(EntityTypeRelative.AllyCheckpoint, this.Type);
                EntityCollection checkpoints = GameServer.GetMap().Entities.GetEntitiesByType(allycp);

                // Obtient le checkpoint suivant le plus proche.
                float minDistanceSqr = float.MaxValue;
                EntityBase next = m_currentAgro;
                foreach (var kvp in checkpoints)
                {
                    EntityCheckpoint cp = (EntityCheckpoint)kvp.Value;

                    // On ne considère pas les checkpoints qui ne sont pas dans notre rangée.
                    if (cp.CheckpointRow != Row)
                        continue;

                    // On prend le + proche des checkpoints suivants.
                    float dstSqr = Vector2.DistanceSquared(cp.Position, Position);
                    if ( (cp.CheckpointID > m_currentCheckpointId  ||
                        (m_currentAgro == null && cp.CheckpointID >= m_currentCheckpointId)) &&
                        dstSqr < minDistanceSqr)
                    {
                        minDistanceSqr = dstSqr;
                        next = cp;
                    }
                }
                if (next != null)
                    m_currentCheckpointId = ((EntityCheckpoint)next).CheckpointID;
                m_currentAgro = next;
            }
            

            // Si l'aggro bouge, on recalcule l'A*.
            if (m_currentAgro != null && Vector2.DistanceSquared(m_currentAgro.Position, m_currentAggroOldPos) > 1)
            {
                m_currentAggroOldPos = m_currentAgro.Position;
                ComputePath();
            }

            // Si on change d'aggro, retourne le chemin vers la tour la plus proche.
            if (m_currentAgro != oldAggro)
                ComputePath();


        }
        #endregion

        
    }
}
