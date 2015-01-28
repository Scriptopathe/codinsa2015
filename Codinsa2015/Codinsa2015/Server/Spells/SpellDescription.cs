using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Spells
{


    /// <summary>
    /// Décrit un spell ainsi que ses effets.
    /// 
    /// Une instance de SpellDescription est contenue dans chaque instance de SpellBase (et ses sous-classes),
    /// et permet aux IA de connaître l'effet du sort. 
    /// 
    /// Les valeurs dans SpellDescriptions sont aussi utilisées par les instances SpellCast pour 
    /// déterminer les effets appliqués par ce sort.
    /// 
    /// Ne contient pas d'information sur la source du sort.
    /// 
    /// Les valeurs dans un objet SpellDescription identifie à chaque fois UNE VERSION d'un sort
    /// (il peut y avoir un SpellDescription par niveau du sort pour chaque sort).
    /// </summary>
    public class SpellDescription
    {
        /// <summary>
        /// Cooldown de base du sort.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Cooldown de base du sort.")]
        public float BaseCooldown { get; set; }

        /// <summary>
        /// Casting time du sort. Pendant le casting time, l'altération de statut 
        /// CastingTimeAlteration est appliquée au casteur.
        /// --
        /// Si l'unité lançant le sort se fait interrompre pendant le casting time, le lancer du sort est annulé,
        /// et le sort est mis en cooldown.
        /// --
        /// Pour TargetType == TargettingType.Targetted
        /// Les effets à l'impact sont appliqués une fois le casting time terminé.
        /// Le casting time correspond donc au temps écoulé entre le lancer du sort, et l'arrivée des
        /// dégâts / effets.
        /// 
        /// Cela peut servir à simuler : un projectile targetté (casting time = temps entre l'envoi et l'arrivée du projectile)
        /// un arrêt pour caster un sort (casting time = temps de pose en utilisant un root comme casting time alteration).
        /// 
        /// 
        /// Pour TargetType == TargettingType.Position ou Direction
        /// Le projectile est lancé une fois le casting time terminé.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Casting time du sort.")]
        public float CastingTime { get; set; }

        /// <summary>
        /// Altération d'état appliquée pendant le casting time.
        /// </summary>
        [Clank.ViewCreator.Export("List<StateAlterationModelView>", "Altération d'état appliquée pendant le casting time.")]
        public List<StateAlterationModel> CastingTimeAlterations { get; set; }

        /// <summary>
        /// Manière dont le ciblage du sort est effectué.
        /// </summary>
        [Clank.ViewCreator.Export("SpellTargetInfoView", "Indique la manière dont le ciblage du sort est effectué.")]
        public SpellTargetInfo TargetType { get; set; }

        /// <summary>
        /// Effets à l'impact du sort.
        /// Ils sont appliqués une fois le casting time terminé.
        /// </summary>
        [Clank.ViewCreator.Export("List<StateAlterationModelView>", "Effets à l'impact du sort. Ils sont appliqués une fois le casting time terminé.")]
        public List<StateAlterationModel> OnHitEffects { get; set; }

        /// <summary>
        /// Retourne une vue représentant cette instance.
        /// </summary>
        /// <returns></returns>
        public Views.SpellDescriptionView ToView()
        {
            Views.SpellDescriptionView view = new Views.SpellDescriptionView();
            view.BaseCooldown = BaseCooldown;
            view.CastingTime = CastingTime;
            view.CastingTimeAlterations = new List<Views.StateAlterationModelView>();
            foreach(var alt in CastingTimeAlterations)
            {
                view.CastingTimeAlterations.Add(alt.ToView());
            }
            view.TargetType = TargetType.ToView();
            view.OnHitEffects = new List<Views.StateAlterationModelView>();
            foreach(var ohe in OnHitEffects)
            {
                view.OnHitEffects.Add(ohe.ToView());
            }
            return view;
        }

        /// <summary>
        /// Obtient une copue de cet objet.
        /// </summary>
        public SpellDescription Copy()
        {
            SpellDescription desc = new SpellDescription();
            desc.BaseCooldown = BaseCooldown;
            desc.CastingTime = CastingTime;

            desc.CastingTimeAlterations = new List<StateAlterationModel>();
            foreach(var effect in CastingTimeAlterations)
            {
                desc.CastingTimeAlterations.Add(effect.Copy());
            }

            desc.TargetType = TargetType;
            desc.OnHitEffects = new List<StateAlterationModel>();
            foreach(var effect in OnHitEffects)
            {
                desc.OnHitEffects.Add(effect.Copy());
            }
            return desc;
        }
    }
}
