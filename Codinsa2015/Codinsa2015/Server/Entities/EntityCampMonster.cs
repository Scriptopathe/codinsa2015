using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Server.Entities
{
    /// <summary>
    /// Représente un monstre de camp.
    /// </summary>
    public class EntityCampMonster : EntityBase
    {
        /// <summary>
        /// Pourcentage de la range d'attaque à laquelle va s'approcher cette unité
        /// pour attaquer.
        /// </summary>
        const float AttackRangeApproach = 0.80f;
        #region Variables
        /// <summary>
        /// Référence vers l'entité ayant l'aggro de cette unité.
        /// </summary>
        EntityBase m_currentAgro;

        Vector2 m_currentAggroOldPos;
        /// <summary>
        /// Sort d'attaque de cette unité.
        /// </summary>
        Spells.Spell m_attackSpell;

        /// <summary>
        /// Range de l'unité, en unités métriques.
        /// </summary>
        float AttackRange { get; set; }
             

        Trajectory m_path;

        /// <summary>
        /// Indique si cette entité a stoppé sa course car elle était en range d'attaque 
        /// de l'entité qu'elle a aggro.
        /// </summary>
        bool m_stoppedAtAttackRange;
        #endregion

        #region Properties
        /// <summary>
        /// Position gardée par cette unité.
        /// </summary>
        public Vector2 GuardPosition { get; set; }
        /// <summary>
        /// Distance maximale à laquelle cette unité peut s'éloigner de GuardPosition.
        /// </summary>
        public float MaxMoveDistance { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de EntityCampMonster.
        /// </summary>
        public EntityCampMonster(Vector2 guardPosition)
            : base()
        {
            BaseArmor = GameServer.GetScene().Constants.CampMonsters.Armor;
            VisionRange = GameServer.GetScene().Constants.CampMonsters.VisionRange;
            MaxMoveDistance = GameServer.GetScene().Constants.CampMonsters.MaxMoveDistance;
            Type = EntityType.Monster;
            AttackRange = VisionRange /4;
            BaseAttackDamage = 60;
            BaseMagicResist = 40;
            BaseMaxHP = 100;
            HP = BaseMaxHP;
            BaseMoveSpeed = 2f;
            GuardPosition = guardPosition;
            Position = GuardPosition;
            m_attackSpell = new Spells.FireballSpell(this, 1.7f, EntityTypeRelative.Player);
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
            if (m_currentAgro != null)
            {
                m_path = new Trajectory(PathFinder.Astar(this.Position, m_currentAgro.Position));
            }
        }

        /// <summary>
        /// Fait avancer cette unité vers sa destination.
        /// </summary>
        void Travel(GameTime time)
        {
            // Si on a pas de trajectoire, on return
            if (m_path == null || m_path.TrajectoryUnits.Count == 0)
                return;

            // On met à jour la trajectoire.
            m_path.UpdateStep(Position, GetMoveSpeed(), time);
            Vector2 nextPosition = m_path.CurrentStep;


            // on s'arrête quand on est en range d'une tour / creep.
            float dstSqr = Vector2.DistanceSquared(m_path.LastPosition(), Position);
            float range = AttackRange * AttackRangeApproach;
            if (!IsAt(nextPosition))
            {
                if (m_currentAgro == null || dstSqr > range * range)
                {
                    Direction = nextPosition - Position;
                    MoveForward(time);
                    m_stoppedAtAttackRange = false;
                }
                else
                {
                    m_stoppedAtAttackRange = true;
                }
            }
            else
            {
                Position = nextPosition;
                m_stoppedAtAttackRange = false;
            }
             
        }

        /// <summary>
        /// Mets à jour l'aggro de la tour.
        /// </summary>
        void UpdateAggro(GameTime time)
        {
            EntityCollection entitiesInRange = GameServer.GetMap().Entities.GetAliveEntitiesInRange(this.Position, AttackRange);
            EntityBase oldAggro = m_currentAgro;

            if (m_currentAgro != null && (m_currentAgro.IsDead || !IsInVisionRange(m_currentAgro)))
            {
                m_currentAgro = null;
                m_path = new Trajectory(PathFinder.Astar(this.Position, this.GuardPosition)) { Offset = new Vector2(-0.5f, -0.5f) };
            }

            // Si pas d'aggro : on cherche le premier héros en range qui l'a attaqué.
            EntityBase aggressiveHero = GetRecentlyAgressiveEntities(0.2f).GetEntitiesByType(EntityType.Player).NearestFrom(this.Position);
            if (aggressiveHero != null)
                m_currentAgro = aggressiveHero;
            

            // Si l'aggro bouge, on recalcule l'A*.
            if (m_currentAgro != null && Vector2.DistanceSquared(m_currentAgro.Position, m_currentAggroOldPos) > 1 && 
                (m_path == null || m_stoppedAtAttackRange || IsAt(m_path.LastPosition()) ) )
            {
                m_currentAggroOldPos = m_currentAgro.Position;
                ComputePath();
            }

            // Si on s'éloigne trop de la position de garde, on lâche l'aggro, et on revient.
            if(m_currentAgro != null && Vector2.DistanceSquared(GuardPosition, Position) >= MaxMoveDistance * MaxMoveDistance)
            {
                m_currentAgro = null;
                m_path = new Trajectory(PathFinder.Astar(this.Position, this.GuardPosition)) { Offset = new Vector2(-0.5f, -0.5f) };
            }

            // Si on change d'aggro, on recalcule le chemin.
            if (m_currentAgro != oldAggro)
                ComputePath();
        }

        
        #endregion

        
    }
}
