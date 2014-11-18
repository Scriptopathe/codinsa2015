using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Entities
{
    /// <summary>
    /// Classe de base pour les tours.
    /// </summary>
    public class EntityTower : EntityBase
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
        /// Range de la tour, en unités métriques.
        /// </summary>
        public float TowerRange { get; set; }

        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de EntityTower.
        /// </summary>
        public EntityTower() : base()
        {
            BaseArmor = 80;
            BaseAttackDamage = 900;
            BaseMagicResist = 40;
            BaseMaxHP = 400;
            HP = BaseMaxHP;
            TowerRange = 5.0f;

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
        }
        /// <summary>
        /// Attaque l'ennemi ayant l'agro.
        /// </summary>
        void Attack(GameTime time)
        {
            m_attackSpell.UpdateCooldown((float)time.ElapsedGameTime.TotalSeconds);
            if (m_currentAgro != null)
            {
                m_attackSpell.Use(new Spells.SpellCastTargetInfo()
                {
                    TargetDirection = m_currentAgro.Position - Position,
                    Type = Spells.TargettingType.Direction,
                });
            }
        }
        /// <summary>
        /// Mets à jour l'aggro de la tour.
        /// </summary>
        void UpdateAggro()
        {
            EntityCollection entitiesInRange = Mobattack.GetMap().Entities.GetAliveEntitiesInRange(this.Position, TowerRange);
            
            // Si la cible sort de l'aggro : on annule l'agro.
            if(m_currentAgro != null)
            {
                if(m_currentAgro.IsDead || Vector2.DistanceSquared(m_currentAgro.Position, Position) > TowerRange * TowerRange)
                {
                    m_currentAgro = null;
                }
            }

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

            if(m_currentAgro != null && !m_currentAgro.Type.HasFlag(EntityType.Player))
            {
                // Si la cible a déjà l'aggro sur quelqu'un et que ce n'est pas un héros,
                // on vérifie qu'un des alliés n'est pas attaqué par un héros adverse.
                // Si un allié est attaqué par un héros, la tour aggro le héros qui l'a attaquée.
                EntityType allyHeroType = EntityTypeConverter.ToAbsolute(EntityTypeRelative.AllyPlayer, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityType ennemyHeroType = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyPlayer, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityCollection allyHeroes = entitiesInRange.GetEntitiesByType(allyHeroType);

                // Pour tous les héros alliés, on regarde s'ils n'ont pas été attaqués recemment par des héros
                // ennemis.
                foreach(var kvp in allyHeroes)
                {
                    EntityBase allyHero = kvp.Value;
                    EntityCollection aggressiveHeros = allyHero.GetRecentlyAgressiveEntities().GetEntitiesByType(ennemyHeroType);
                    if (aggressiveHeros.Count != 0)
                    {
                        EntityBase aggressiveHero = aggressiveHeros.First().Value;
                        m_currentAgro = aggressiveHero;
                    }
                }
            }
        }
        #endregion

        
    }
}
