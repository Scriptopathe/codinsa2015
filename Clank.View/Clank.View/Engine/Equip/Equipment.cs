using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Equip
{
    public enum EquipmentType
    {
        Armor,
        Weapon,
        Boots,
    }
    /// <summary>
    /// Classe de base pour représenter une armure.
    /// </summary>
    public abstract class Equipment
    {
        static int s_currentId = 0;

        /// <summary>
        /// Obtient ou définit la liste des altérations d'état données par cette
        /// armure.
        /// </summary>
        public List<Entities.StateAlterationModel> Alterations { get; set; }

        /// <summary>
        /// Nom de l'armure.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Prix de l'armure en points d'amélioration.
        /// </summary>
        public float Price { get; set; }

        /// <summary>
        /// Identifiant unique de l'armure.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Obtient le type d'équipement de cet équipement.
        /// </summary>
        public abstract EquipmentType Type { get; }

        /// <summary>
        /// Obtient la liste des évolutions possibles de l'armure.
        /// </summary>
        public List<int> AvailableEvolutions { get; set; }

        /// <summary>
        /// Crée une nouvelle instance d'armor.
        /// </summary>
        public Equipment()
        {
            Alterations = new List<Entities.StateAlterationModel>();
            Name = "";
            ID = s_currentId++;
            AvailableEvolutions = new List<int>();
        }
    }
}
