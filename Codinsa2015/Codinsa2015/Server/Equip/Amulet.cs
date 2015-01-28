using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Equip
{
    /// <summary>
    /// Représente une amulette.
    /// </summary>
    public class Amulet : PassiveEquipment
    {
        public Amulet(Entities.EntityHero owner, PassiveEquipmentModel model) : base(owner, model) { }
    }
}
