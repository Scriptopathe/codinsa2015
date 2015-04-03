using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Equip
{
    [Clank.ViewCreator.Enum("Enumère les différents types d'équipement.")]
    public enum EquipmentType
    {
        Consummable,
        Armor,
        Weapon,
        WeaponEnchant,
        Boots,
        //Amulet,
    }
    /// <summary>
    /// Classe de base pour représenter un modèle d'équipement.
    /// </summary>
    public abstract class EquipmentModel
    {
        static int s_currentId = 0;

        /// <summary>
        /// Nom de l'équipement.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Prix de l'équipement en points d'amélioration.
        /// </summary>
        public virtual float Price { get; set; }

        /// <summary>
        /// Identifiant unique de l'armure.
        /// </summary>
        [Clank.ViewCreator.Access("int", "ID de l'équipement")]
        public int ID { get; set; }

        /// <summary>
        /// Obtient le type d'équipement de cet équipement.
        /// </summary>
        public abstract EquipmentType Type { get; set; }
        /// <summary>
        /// Crée une nouvelle instance d'armor.
        /// </summary>
        public EquipmentModel()
        {
            Name = "";
            ID = s_currentId++;
        }
    }
}
