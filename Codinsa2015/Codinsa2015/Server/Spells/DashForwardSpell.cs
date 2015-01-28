using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Spells
{
    /// <summary>
    /// Représente un coup de tour.
    /// </summary>
    public class DashForwardSpell : Spell
    {
        /// <summary>
        /// Utilise le spell
        /// </summary>
        /// <param name="target"></param>
        protected override void DoUseSpell(SpellCastTargetInfo target)
        {
            base.DoUseSpell(target);

            // Vérification de range


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
        /// Crée une nouvelle instance de ce spell.
        /// </summary>
        /// <param name="caster"></param>
        public DashForwardSpell(EntityBase caster)
        {
            SourceCaster = caster;
            Name = "Dash";
            Levels = new List<SpellDescription>() { new SpellDescription()
            {
                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.Me,
                    Type = TargettingType.Targetted,
                },
                BaseCooldown = 0.5f,
                CastingTime = 0.05f,
                CastingTimeAlterations = new List<StateAlterationModel>() {
                    new StateAlterationModel() 
                    {
                        Type = StateAlterationType.Root,
                        BaseDuration = 0.05f,
                    }
                },
                
                OnHitEffects = new List<StateAlterationModel>() { 
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.Dash,
                        DashSpeed = 60,
                        DashGoThroughWall = true,
                        DashDirectionType = Entities.DashDirectionType.Direction,
                        BaseDuration = 0.075f,
                    },
                }
            }};
            CurrentCooldown = 0.0f;
        }
    }
}
