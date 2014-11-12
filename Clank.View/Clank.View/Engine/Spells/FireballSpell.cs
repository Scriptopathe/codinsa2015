using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Clank.View.Engine.Entities;
namespace Clank.View.Engine.Spells
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
        public FireballSpell(EntityBase caster)
        {
            SourceCaster = caster;
            Description = new SpellDescription()
            {
                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.EnnemyPlayer,
                    Radius = 0.3f,
                    Range = 4f,
                    Type = TargettingType.Direction
                },

                BaseCooldown = 1.0f,
                CastingTime = 0.1f,
                CastingTimeAlteration = new StateAlterationModel() 
                {
                    Type = StateAlterationType.Root,
                    Duration = 0.1f,
                },

                OnHitEffects = new StateAlterationModel()
                {
                    Type = StateAlterationType.AttackDamage | StateAlterationType.Root,
                    Duration = 1.0f,
                    FlatValue = 100.0f,
                    SourcePercentADValue = 100.0f
                }
            };
            CurrentCooldown = 0.0f;
        }
    }
}
