using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Clank.View.Engine.Entities;
namespace Clank.View.Engine.Spells
{
    /// <summary>
    /// Représente un coup de tour.
    /// </summary>
    public class MovementSpeedBuffSpell : Spell
    {
        /// <summary>
        /// Utilise le spell
        /// </summary>
        /// <param name="target"></param>
        protected override void DoUseSpell(SpellCastTargetInfo target)
        {
            base.DoUseSpell(target);
            target.AlterationParameters.DashTargetEntity = SourceCaster;
            target.AlterationParameters.DashTargetDirection = SourceCaster.Position;
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
        /// Crée une nouvelle instance de ce spell.
        /// </summary>
        /// <param name="caster"></param>
        public MovementSpeedBuffSpell(EntityBase caster)
        {
            SourceCaster = caster;
            Name = "Run";
            Levels = new List<SpellDescription>() { new SpellDescription()
            {
                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.AllAlly,
                    AoeRadius = 0.1f,
                    Range = 6f,
                    Duration = 0.0f,
                    DieOnCollision = true,
                    Type = TargettingType.Targetted
                },
                BaseCooldown = 15f,
                CastingTime = 0.01f,
                CastingTimeAlteration = new StateAlterationModel() 
                {
                    Type = StateAlterationType.Root,
                    BaseDuration = 0.01f,
                },
                
                OnHitEffects = new List<StateAlterationModel>() { 
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.MoveSpeed,
                        BaseDuration = 10.0f,
                        FlatValue = 10.0f,
                        SourcePercentADValue = 0.80f,
                    },
                }
            }};
            CurrentCooldown = 0.0f;
        }
    }
}
