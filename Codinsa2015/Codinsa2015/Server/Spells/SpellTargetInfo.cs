using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Spells
{

    /// <summary>
    /// Représente les différents types de targettings possibles.
    /// </summary>
    [Clank.ViewCreator.Enum("Représente les différents types de targettings possibles.")]
    public enum TargettingType
    {
        Targetted   = 0x01,                 // ciblé sur un allié / ennemi / monstre neutre
        Position    = 0x02,                 // ciblé sur une position   (projectile venant du ciel)
        Direction   = 0x04                  // ciblé sur une direction. (projectile)
    }

    /// <summary>
    /// Représente la manière dont un sort est ciblé.
    /// Il contient des informations sur le type de targetting (TargettingType), 
    /// et sur la range du sort, sa durée, etc...
    /// </summary>
    public class SpellTargetInfo
    {
        /// <summary>
        /// Type de ciblage du sort.
        /// </summary>
        [Clank.ViewCreator.Export("TargettingType", "Type de ciblage du sort.")]
        public TargettingType Type { get; set; }

        /// <summary>
        /// Range du sort. 
        /// Targetted : range à laquelle le sort peut être casté sur une unité.
        /// Direction : distance maximale parcourue par le projectile.
        /// Position  : distance maximale entre le lanceur de sort et la position du lancer.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Range du sort en unités métriques.")]
        public float Range { get; set; }

        /// <summary>
        /// Durée en secondes que met le sort à atteindre la position donnée.
        /// Targetted : durée pour atteindre la cible à max range.
        /// Direction : durée pour atteindre la limite de range.
        /// Position  : durée de l'AOE.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Durée en secondes que met le sort à atteindre la position donnée.")]
        public float Duration { get; set; }

        /// <summary>
        /// Rayon du sort.
        /// Targetted : non utilisé.
        /// Direction : rayon de l'AOE (si IsAOE vaut true).
        /// Position  : rayon de l'AOE
        /// </summary>
        [Clank.ViewCreator.Export("float", "Rayon du sort. (non utilisé pour les sort targetted)")]
        public float AoeRadius { get; set; }

        /// <summary>
        /// Obtient une valeur indiquant si le sort à un effet d'AOE à l'impact.
        /// </summary>
        public bool IsAOE { get { return AoeRadius > 0; } }

        /// <summary>
        /// Obtient une valeur indiquant si le sort est détruit lors d'une collision 
        /// avec une entité.
        /// </summary>
        [Clank.ViewCreator.Export("bool", "Obtient une valeur indiquant si le sort est détruit lors d'une collision avec une entité")]
        public bool DieOnCollision { get; set; }
        
        /// <summary>
        /// Retourne le type de cibles pouvant être touchées par ce sort.
        /// 
        /// Certains sorts ne peuvent toucher que des alliés, d'autres n'affectent
        /// pas les structures etc...
        /// </summary>
        [Clank.ViewCreator.Export("EntityTypeRelative", "Retourne le type de cibles pouvant être touchées par ce sort.")]
        public Entities.EntityTypeRelative AllowedTargetTypes { get; set; }

        /// <summary>
        /// Retourne une view représentant cette instance.
        /// </summary>
        public Views.SpellTargetInfoView ToView()
        {
            Views.SpellTargetInfoView view = new Views.SpellTargetInfoView();
            view.AllowedTargetTypes = (Views.EntityTypeRelative)AllowedTargetTypes;
            view.AoeRadius = AoeRadius;
            view.DieOnCollision = DieOnCollision;
            view.Duration = Duration;
            view.Range = Range;
            view.Type = (Views.TargettingType)Type;
            return view;
        }
    }

}
