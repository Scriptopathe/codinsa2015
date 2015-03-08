using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Equip
{
    /// <summary>
    /// Représente un modèle d'upgrade d'un équipement passif.
    /// </summary>
    /// <summary>
    /// Représente un modèle upgrade d'arme.
    /// </summary>
    public class PassiveEquipmentUpgradeModel
    {
        /// <summary>
        /// Obtient les altérations d'état appliquées passivement par cet équipement.
        /// </summary>
        [Clank.ViewCreator.Export("List<StateAlterationModelView>", "Obtient les altérations d'état appliquées passivement par cet équipement.")]
        public List<Entities.StateAlterationModel> PassiveAlterations { get; set; }

        /// <summary>
        /// Obtient le coût de l'upgrade.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Obtient le coût de l'upgrade.")]
        public float Cost { get; set; }

        /// <summary>
        /// Crée une nouvelle instance de WeaponUpgrade.
        /// </summary>
        public PassiveEquipmentUpgradeModel()
        {
            PassiveAlterations = new List<Entities.StateAlterationModel>();
            Cost = 0;
        }

        /// <summary>
        /// Crée une nouvelle instance de Weapon upgrade initialisée avec
        /// les valeurs données.
        /// </summary>
        public PassiveEquipmentUpgradeModel(List<Entities.StateAlterationModel> passiveAlterations, float cost)
        {
            PassiveAlterations = passiveAlterations;
            Cost = cost;
        }
    }
    /// <summary>
    /// Représente un modèle d'équipement passif.
    /// </summary>
    public class PassiveEquipmentModel : EquipmentModel
    {
        #region Properties
        EquipmentType m_type;

        /// <summary>
        /// Type de l'équipement.
        /// </summary>
        public override EquipmentType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// Obtient ou définit la liste des upgrades de cet équipement.
        /// </summary>
        public List<PassiveEquipmentUpgradeModel> Upgrades { get; set; }

        #endregion
    }

    /// <summary>
    /// Représente un équipement passif (bottes, armure, etc...).
    /// </summary>
    public class PassiveEquipment
    {
        #region Variables
        PassiveEquipmentModel m_model;
        #endregion

        public PassiveEquipment(Entities.EntityHero owner, PassiveEquipmentModel model)
        {
            Owner = owner;
            Model = model;
        }
        /// <summary>
        /// Obtient ou définit le propriétaire de cet équipement.
        /// </summary>
        public Entities.EntityHero Owner { get; set; }
        /// <summary>
        /// Obtient le modèle de l'équipement actuel.
        /// </summary>
        public PassiveEquipmentModel Model
        {
            get 
            {
                return m_model;
            }
            set
            {
                m_model = value;
                //Owner.ApplyPassives(Type);
            }
        }

        /// <summary>
        /// Obtient le niveau de l'équipement actuel.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Représente le type d'équipement.
        /// </summary>
        public EquipmentType Type
        {
            get { return m_model.Type; }
        }

        /// <summary>
        /// Améliore l'équipement et obtient une valeur indiquant si l'opération
        /// a réussi.
        /// </summary>
        public bool Upgrade()
        {
            if (Level >= Model.Upgrades.Count - 1)
                return false;

            Level++;
            Owner.ApplyPassives(Type);
            return true;
        }

        /// <summary>
        /// Obtient les passifs procurés par cette arme et ses enchantements.
        /// </summary>
        public virtual List<Entities.StateAlterationModel> GetPassives()
        {
            List<Entities.StateAlterationModel> lst = new List<Entities.StateAlterationModel>();
            lst.AddRange(Model.Upgrades[Level].PassiveAlterations);
            return lst;
        }
    }
}
