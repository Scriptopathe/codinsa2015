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
            target.AlterationParameters.DashTargetEntity = SourceCaster;
            target.AlterationParameters.DashTargetPosition = SourceCaster.Position;
            Spellcasts.SpellcastBase fireball = new Spellcasts.SpellcastBase(this, target);
            Mobattack.GetMap().AddSpellcast(fireball);
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
            Levels = new List<SpellDescription>() { new SpellDescription()
            {
                
                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.AllEnnemy,
                    AoeRadius = 0.3f,
                    Range = 6f,
                    Duration = 0.6f,
                    DieOnCollision = true,
                    Type = TargettingType.Direction
                },
                BaseCooldown = 0.5f,
                CastingTime = 0.01f,
                CastingTimeAlteration = new StateAlterationModel() 
                {
                    Type = StateAlterationType.Root,
                    BaseDuration = 0.01f,
                },
                
                OnHitEffects = new List<StateAlterationModel>() { 
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.AttackDamage,
                        BaseDuration = 0.0f,
                        FlatValue = 100.0f,
                        SourcePercentADValue = 0.0f,
                    },
                    /*new StateAlterationModel()
                    {
                        Type = StateAlterationType.Dash,
                        DashSpeed = 2.0f,
                        DashDirectionType = DashDirectionType.TowardsEntity,
                        Duration = 0.2f
                    }*/
                }
            }};
            CurrentCooldown = 0.0f;
        }
    }
}
