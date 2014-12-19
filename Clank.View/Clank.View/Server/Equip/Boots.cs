using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Equip
{
    /// <summary>
    /// Représente une paire de bottes.
    /// </summary>
    public class Boots : Equipment
    {
        public override EquipmentType Type
        {
            get { return EquipmentType.Boots; }
        }
    }
}
