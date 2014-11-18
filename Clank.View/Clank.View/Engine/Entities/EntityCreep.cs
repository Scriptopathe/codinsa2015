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
    public class EntityCreep : EntityBase
    {
        #region Variables
        /// <summary>
        /// Référence vers l'entité ayant l'aggro de la tour.
        /// </summary>
        EntityBase m_currentAgro;

        /// <summary>
        /// Sort d'attaque de la tour.
        /// </summary>
        Spells.Spell m_attackSpell;

        /// <summary>
        /// Range du creep, en unités métriques.
        /// </summary>
        float Range { get; set; }

        
        Trajectory m_path;
        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de EntityTower.
        /// </summary>
        public EntityCreep() : base()
        {
            BaseArmor = 40;
            BaseAttackDamage = 60;
            BaseMagicResist = 40;
            BaseMaxHP = 100;
            HP = BaseMaxHP;
            Range = 3.0f;
            BaseMoveSpeed = 5f;
            m_attackSpell = new Spells.FireballSpell(this);
        }

        /// <summary>
        /// Mets à jour la tour.
        /// </summary>
        protected override void DoUpdate(GameTime time)
        {
            base.DoUpdate(time);
            UpdateAggro();
            Attack(time);
            Travel(time);
        }
        /// <summary>
        /// Attaque l'ennemi ayant l'agro.
        /// </summary>
        void Attack(GameTime time)
        {
            m_attackSpell.UpdateCooldown((float)time.ElapsedGameTime.TotalSeconds);
            if (m_currentAgro != null && Vector2.DistanceSquared(m_currentAgro.Position, Position) <= Range * Range)
            {
                m_attackSpell.Use(new Spells.SpellCastTargetInfo()
                {
                    TargetDirection = m_currentAgro.Position - Position,
                    Type = Spells.TargettingType.Direction,
                });
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
                if(m_path.TrajectoryUnits.Count <= 1)
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

            Vector2 nextPosition = m_path.CurrentStep;

            // Si on est trop près d'un sbire, on s'arrête
            EntityType allyCreepsType = EntityTypeConverter.ToAbsolute(EntityTypeRelative.AllyCreep, Type);
            EntityCollection allyCreeps = Mobattack.GetMap().Entities.GetEntitiesByType(allyCreepsType);
            foreach(var kvp in allyCreeps)
            {
                EntityCreep entity = (EntityCreep)kvp.Value;
                if (entity == this)
                    continue;

                // Ce creep est trop près, on s'arrête s'il a le même aggro et est plus proche.
                if(Vector2.DistanceSquared(entity.Position, Position) <= 1)
                {
                    if(entity.m_currentAgro == m_currentAgro)
                    {
                        // Si l'autre entité est + proche, on s'arrête.
                        if (Vector2.DistanceSquared(m_currentAgro.Position, Position) > Vector2.DistanceSquared(entity.m_currentAgro.Position, entity.Position))
                            return;
                    }
                }
            }

            // on s'arrête quand on est en range d'une tour
            if (Vector2.DistanceSquared(m_path.LastPosition(), Position) >= Range * Range)
            {
                Direction = nextPosition - Position;
                MoveForward(time);
            }
        }
        /// <summary>
        /// Mets à jour l'aggro de la tour.
        /// </summary>
        void UpdateAggro()
        {
            EntityCollection entitiesInRange = Mobattack.GetMap().Entities.GetAliveEntitiesInRange(this.Position, Range);
            EntityBase oldAggro = m_currentAgro;

            // Si l'entité qui avait l'aggro meurt, on la remplace
            if (m_currentAgro != null && m_currentAgro.IsDead)
                m_currentAgro = null;

            // Si la tour n'a pas d'aggro : on cherche la première unité creep en range
            if(m_currentAgro == null)
            {
                EntityType ennemyCreep = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyCreep, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityBase nearestEnnemyCreep = entitiesInRange.GetEntitiesByType(ennemyCreep).NearestFrom(this.Position);
                m_currentAgro = nearestEnnemyCreep;
            }

            // Si on n'en trouve pas : on cherche le premier héros en range.
            if(m_currentAgro == null)
            {
                EntityType ennemyHero = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyPlayer, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityCollection ennemyHeroes = entitiesInRange.GetEntitiesByType(ennemyHero);
                EntityBase nearestEnnemyHero = ennemyHeroes.NearestFrom(this.Position);
                m_currentAgro = nearestEnnemyHero;
            }

            // Si on n'en trouve toujours pas, on cherche la tour ennemie la plus proche.
            if(m_currentAgro == null)
            {
                EntityType ennemyTower = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyTower, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityBase nearestTower = Mobattack.GetMap().Entities.GetEntitiesByType(ennemyTower).NearestFrom(this.Position);
                m_currentAgro = nearestTower;
            }

            // Puis l'inhibiteur
            if (m_currentAgro == null)
            {
                EntityType ennemyIdol = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyInhibitor, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityBase nearest = Mobattack.GetMap().Entities.GetEntitiesByType(ennemyIdol).NearestFrom(this.Position);
                m_currentAgro = nearest;
            }

            // Puis l'idole
            if (m_currentAgro == null)
            {
                EntityType ennemyIdol = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyIdol, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityBase nearest = Mobattack.GetMap().Entities.GetEntitiesByType(ennemyIdol).NearestFrom(this.Position);
                m_currentAgro = nearest;
            }


            if(m_currentAgro != null)
            {
                // Si la cible a déjà l'aggro sur quelqu'un, on vérifie qu'un des alliés n'est
                // pas attaqué.
                // Si un allié est attaqué, la tour aggro le héros qui l'a attaquée.
                EntityType allyHeroType = EntityTypeConverter.ToAbsolute(EntityTypeRelative.AllyPlayer, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityType ennemyHeroType = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyPlayer, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityCollection allyHeroes = entitiesInRange.GetEntitiesByType(allyHeroType);

                foreach(var kvp in allyHeroes)
                {
                    EntityBase allyHero = kvp.Value;
                    // On regarde si un des héros alliés subit des dégâts
                    List<StateAlteration> alterations = allyHero.StateAlterations.GetInteractionsByType(StateAlterationType.AttackDamage | StateAlterationType.TrueDamage);
                    if(alterations.Count != 0)
                    {
                        // On vérifie que ces dégâts proviennent d'un héros ennemi.
                        foreach(StateAlteration alteration in alterations)
                        {
                            if (alteration.Source.Type.HasFlag(ennemyHeroType))
                                m_currentAgro = alteration.Source;
                        }
                    }
                }
            }

            // Si on change d'aggro, retourne le chemin vers la tour la plus proche.
            if (m_currentAgro != oldAggro)
                ComputePath();
        }
        #endregion

        
    }
}
