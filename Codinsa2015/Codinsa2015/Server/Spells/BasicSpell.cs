using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Spells
{
    /// <summary>
    /// Représente un spell de base customisable.
    /// </summary>
    public class BasicSpell : Spell
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
        /// Prépare les paramètres du sort à partir des informations de ciblage.
        /// </summary>
        /// <param name="target"></param>
        public override void SetupParameters(SpellCastTargetInfo target)
        {
            base.SetupParameters(target);
            
            target.AlterationParameters.DashTargetPosition = target.TargetPosition;
            target.AlterationParameters.DashTargetEntity = GameServer.GetMap().GetEntityById(target.TargetId);
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
        /// Crée une nouvelle instance de BasicSpell.
        /// </summary>
        /// <param name="caster"></param>
        public BasicSpell(EntityBase caster,
            List<SpellDescription> levels, 
            string name)
        {
            SourceCaster = caster;
            Name = name;
            Levels = levels;
            CurrentCooldown = 0.0f;
        }
    }
}
