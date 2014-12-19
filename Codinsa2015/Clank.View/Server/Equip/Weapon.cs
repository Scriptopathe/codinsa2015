using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Equip
{
    /// <summary>
    /// Représente une arme.
    /// </summary>
    public class Weapon : Equipment
    {
        /// <summary>
        /// Obtient le spell d'auto-attaque que confère l'arme.
        /// </summary>
        /// <returns></returns>
        public virtual Spells.SpellDescription GetAttackSpell() { return null; }


        public override EquipmentType Type
        {
            get { return EquipmentType.Weapon; }
        }
    }
}
