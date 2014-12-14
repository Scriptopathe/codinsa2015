using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Equip
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
