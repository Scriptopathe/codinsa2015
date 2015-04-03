using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Spells
{
    /// <summary>
    /// Représente un spell utilisé par les armes.
    /// </summary>
    public class WeaponAttackSpell : Spell
    {
        /// <summary>
        /// Utilise le spell.
        /// </summary>
        /// <param name="target"></param>
        protected override void DoUseSpell(SpellCastTargetInfo target)
        {
            base.DoUseSpell(target);
            Spellcasts.SpellcastBase cast = new Spellcasts.SpellcastBase(this, target);
            GameServer.GetMap().AddSpellcast(cast);
        }

        /// <summary>
        /// Retourne le cooldown du sort.
        /// </summary>
        /// <returns></returns>
        protected override float GetUseCooldown()
        {
            return 0;
        }

        /// <summary>
        /// Crée une nouvelle instance de FireballSpell.
        /// </summary>
        /// <param name="caster"></param>
        public WeaponAttackSpell(EntityBase caster, SpellLevelDescription attack, Equip.WeaponEnchantModel enchant)
        {
            SourceCaster = caster;
            attack = attack.Copy();
            if(enchant.OnHitEffects != null)
                attack.OnHitEffects.AddRange(enchant.OnHitEffects);
            if(enchant.CastingEffects != null)
                attack.CastingTimeAlterations.AddRange(enchant.CastingEffects);
            Model = new SpellModel(new List<SpellLevelDescription>() 
            { 
                attack,
            }, "weapon-" + caster.ID);
            CurrentCooldown = 0.0f;
        }
    }
}
