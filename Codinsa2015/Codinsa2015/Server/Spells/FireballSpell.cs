using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Spells
{
    /// <summary>
    /// Représente spell "boule de feu". (test).
    /// </summary>
    public class FireballSpell : Spell
    {
        /// <summary>
        /// Utilise le spell "Fireball".
        /// </summary>
        /// <param name="target"></param>
        protected override void DoUseSpell(SpellCastTargetInfo target)
        {
            base.DoUseSpell(target);
            Spellcasts.SpellcastBase fireball = new Spellcasts.SpellcastBase(this, target);
            GameServer.GetMap().AddSpellcast(fireball);
        }

        /// <summary>
        /// Retourne le cooldown du sort.
        /// </summary>
        /// <returns></returns>
        protected override float GetUseCooldown()
        {
            return base.GetUseCooldown();
        }

        /// <summary>
        /// Crée une nouvelle instance de FireballSpell.
        /// </summary>
        /// <param name="caster"></param>
        public FireballSpell(EntityBase caster,
            float cooldownBase=1.7f,
            float attackRange=6.0f,
            EntityTypeRelative allowedTargets = EntityTypeRelative.AllEnnemy | EntityTypeRelative.AllTargettableNeutral)
        {
            SourceCaster = caster;
            Model = new SpellModel(
            new List<SpellLevelDescription>() { new SpellLevelDescription()
            {
                
                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = allowedTargets,
                    AoeRadius = 0.3f,
                    Range = attackRange,
                    Duration = 0.6f,
                    DieOnCollision = true,
                    Type = TargettingType.Direction
                },
                BaseCooldown = cooldownBase,
                CastingTime = 0.01f,
                CastingTimeAlterations = new List<StateAlterationModel>() 
                {
                    new StateAlterationModel() 
                    {
                        Type = StateAlterationType.Root,
                        BaseDuration = 0.01f,
                    }
                },
                
                OnHitEffects = new List<StateAlterationModel>() { 
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.AttackDamage,
                        BaseDuration = 0.0f,
                        SourcePercentADValue = 1.0f,
                    },
                }
            }}, "Fireball");
            CurrentCooldown = 0.0f;
        }
    }
}
