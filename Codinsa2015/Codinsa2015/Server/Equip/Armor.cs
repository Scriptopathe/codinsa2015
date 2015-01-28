using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Equip
{
    /// <summary>
    /// Classe de base pour représenter une armure.
    /// </summary>
    public class Armor : PassiveEquipment
    {
        public Armor(Entities.EntityHero owner, PassiveEquipmentModel model)
            : base(owner, model) { }
    }
}
