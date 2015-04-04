using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Server.Entities
{
    /// <summary>
    /// Représente la mining farm.
    /// Tuer la mining farm rapporte :
    /// des points d’amélioration pour tout l’équipe
    /// la réapparition du bâtiment d’unité de spawn s’il est détruit,
    /// ou la création de sbires beaucoup plus puissants.
    /// </summary>
    public class EntityMiningFarm : EntityBase
    {
        #region Variables
        /// <summary>
        /// Référence vers l'entité ayant l'aggro du boss.
        /// </summary>
        EntityBase m_currentAgro;

        /// <summary>
        /// Sort d'attaque du boss.
        /// </summary>
        Spells.Spell m_attackSpell;

        /// <summary>
        /// Range d'attaque, en unités métriques.
        /// </summary>
        public float AttackRange { get; set; }

        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de EntityBigBoss.
        /// </summary>
        public EntityMiningFarm()
            : base()
        {
            BaseArmor = 80;
            BaseAttackDamage = 900;
            BaseMagicResist = 40;
            BaseMaxHP = 400;
            HP = BaseMaxHP;
            VisionRange = 6.0f;
            AttackRange = VisionRange;
            Type |= EntityType.MiningFarm;
            m_attackSpell = new Spells.FireballSpell(this);
        }

        /// <summary>
        /// Mets à jour cette entité.
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
        /// Mets à jour l'aggro du big boss.
        /// </summary>
        void UpdateAggro()
        {
            EntityCollection entitiesInRange = GameServer.GetMap().Entities.GetAliveEntitiesInRange(this.Position, AttackRange);

            // Si la cible sort de l'aggro : on annule l'agro.
            if (m_currentAgro != null)
            {
                if (m_currentAgro.IsDead || Vector2.DistanceSquared(m_currentAgro.Position, Position) > AttackRange * AttackRange)
                {
                    m_currentAgro = null;
                }
            }

            // Si la tour n'a pas d'aggro : on cherche la première unité Virus en range
            if (m_currentAgro == null)
            {
                EntityType ennemyVirus = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyVirus, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityBase nearestEnnemyVirus = entitiesInRange.GetEntitiesByType(ennemyVirus).NearestFrom(this.Position);
                m_currentAgro = nearestEnnemyVirus;
            }
            // Si on n'en trouve pas : on cherche le premier héros en range.
            if (m_currentAgro == null)
            {
                EntityType ennemyHero = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyPlayer, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityCollection ennemyHeroes = entitiesInRange.GetEntitiesByType(ennemyHero);
                EntityBase nearestEnnemyHero = ennemyHeroes.NearestFrom(this.Position);
                m_currentAgro = nearestEnnemyHero;
            }

            if (m_currentAgro != null && !m_currentAgro.Type.HasFlag(EntityType.Player))
            {
                // Si la cible a déjà l'aggro sur quelqu'un et que ce n'est pas un héros,
                // on vérifie qu'un des alliés n'est pas attaqué par un héros adverse.
                // Si un allié est attaqué par un héros, la tour aggro le héros qui l'a attaquée.
                EntityType allyHeroType = EntityTypeConverter.ToAbsolute(EntityTypeRelative.AllyPlayer, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityType ennemyHeroType = EntityTypeConverter.ToAbsolute(EntityTypeRelative.EnnemyPlayer, this.Type & (EntityType.Team1 | EntityType.Team2));
                EntityCollection allyHeroes = entitiesInRange.GetEntitiesByType(allyHeroType);

                // Pour tous les héros alliés, on regarde s'ils n'ont pas été attaqués recemment par des héros
                // ennemis.
                foreach (var kvp in allyHeroes)
                {
                    EntityBase allyHero = kvp.Value;
                    EntityCollection aggressiveHeros = allyHero.GetRecentlyAgressiveEntities(1.0f).GetEntitiesByType(ennemyHeroType);
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
