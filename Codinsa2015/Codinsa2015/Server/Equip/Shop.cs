using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Equip
{
    
    /// <summary>
    /// Classe permettant la présentation de divers équipements.
    /// 
    /// Les shops sont chargés depuis des fichiers xml.
    /// </summary>
    public class Shop
    {
        #region Variables
        List<EquipmentModel> m_availableEquip;
        List<Consummable> m_availableConsummables;
        #endregion

        /// <summary>
        /// Crée une nouvelle instance de shop, avec la liste des équipements donnée.
        /// </summary>
        public Shop(List<EquipmentModel> equip, List<Consummable> consummables)
        {
            m_availableEquip = equip;
            m_availableConsummables = consummables;
        }

        /// <summary>
        /// Retourne une liste d'elixirs disponibles pour le héros donné.
        /// </summary>
        /// <param name="hero"></param>
        /// <returns></returns>
        public List<Consummable> GetConsummables(EntityHero hero)
        {
            return m_availableConsummables;
        }

        /// <summary>
        /// Retourne une liste d'armes disponibles pour le héros donné.
        /// </summary>
        public List<EquipmentModel> GetWeapons(EntityHero hero)
        {
            return m_availableEquip.Where(new Func<EquipmentModel, bool>((EquipmentModel e) =>
            {
                return e.Type == EquipmentType.Weapon;
            })).ToList();
        }

        /// <summary>
        /// Retourne une liste d'armures disponibles pour le héros donné.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<EquipmentModel> GetArmors(EntityHero hero)
        {
            return m_availableEquip.Where(new Func<EquipmentModel, bool>((EquipmentModel e) =>
            {
                return e.Type == EquipmentType.Armor;
            })).ToList();
        }

        /// <summary>
        /// Retourne une liste de bottes disponibles pour le héros donné.
        /// </summary>
        public List<EquipmentModel> GetBoots(EntityHero hero)
        {
            return m_availableEquip.Where(new Func<EquipmentModel, bool>((EquipmentModel e) =>
            {
                return e.Type == EquipmentType.Boots;
            })).ToList();
        }

    }
}
