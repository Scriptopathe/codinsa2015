using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Clank.View.Engine.Spells
{

    /// <summary>
    /// Représente les différents types de targettings posssible.
    /// </summary>
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
        public TargettingType Type { get; set; }

        /// <summary>
        /// Range du sort. 
        /// Targetted : range à laquelle le sort peut être casté sur une unité.
        /// Direction : distance maximale parcourue par le projectile.
        /// Position  : distance maximale entre le lanceur de sort et la position du lancer.
        /// </summary>
        public float Range { get; set; }

        /// <summary>
        /// Durée en secondes que met le sort à atteindre la position donnée.
        /// Targetted : durée pour atteindre la cible à max range.
        /// Direction : durée pour atteindre la limite de range.
        /// Position  : durée de l'AOE.
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// Rayon du sort.
        /// Targetted : non utilisé.
        /// Direction : rayon de l'AOE (si IsAOE vaut true).
        /// Position  : rayon de l'AOE
        /// </summary>
        public float AoeRadius { get; set; }

        /// <summary>
        /// Obtient une valeur indiquant si le sort à un effet d'AOE à l'impact.
        /// </summary>
        public bool IsAOE { get { return AoeRadius > 0; } }

        /// <summary>
        /// Obtient une valeur indiquant si le sort est détruit lors d'une collision 
        /// avec une entité.
        /// </summary>
        public bool DieOnCollision { get; set; }
        
        /// <summary>
        /// Retourne le type de cibles pouvant être touchées par ce sort.
        /// 
        /// Certains sorts ne peuvent toucher que des alliés, d'autres n'affectent
        /// pas les structures etc...
        /// </summary>
        public Entities.EntityTypeRelative AllowedTargetTypes { get; set; }
    }

}
