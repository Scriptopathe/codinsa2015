using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clank.View.Engine.Entities;
namespace Clank.View.Engine.Equip
{
    /// <summary>
    /// Classe permettant la présentation de divers équipements.
    /// 
    /// Les shops sont chargés depuis des fichiers xml.
    /// </summary>
    public class Shop
    {
        #region Variables
        List<Equipment> m_availableEquip;
        List<Consummable> m_availableConsummables;
        #endregion

        /// <summary>
        /// Crée une nouvelle instance de shop, avec la liste des équipements donnée.
        /// </summary>
        public Shop(List<Equipment> equip, List<Consummable> consummables)
        {
            m_availableEquip = equip;
            m_availableConsummables = consummables;
        }

        /// <summary>
        /// Retourne une liste d'elixirs disponibles pour le héros donné.
        /// </summary>
        /// <param name="hero"></param>
        /// <returns></returns>
        public List<Consummable> GetElixirs(EntityHero hero)
        {
            return m_availableConsummables;
        }

        /// <summary>
        /// Retourne une liste d'armes disponibles pour le héros donné.
        /// </summary>
        public List<Equipment> GetWeapons(EntityHero hero)
        {
            return m_availableEquip.Where(new Func<Equipment, bool>((Equipment e) =>
            {
                return e.Type == EquipmentType.Weapon;
            })).ToList();
        }
        /// <summary>
        /// Retourne une liste d'armures disponibles pour le héros donné.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<Equipment> GetArmors(EntityHero hero)
        {

            return m_availableEquip.Where(new Func<Equipment, bool>((Equipment e) =>
            {
                return e.Type == EquipmentType.Armor;
            })).ToList();
        }

        /// <summary>
        /// Retourne une liste de bottes disponibles pour le héros donné.
        /// </summary>
        public List<Equipment> GetBoots(EntityHero hero)
        {
            return m_availableEquip.Where(new Func<Equipment, bool>((Equipment e) =>
            {
                return e.Type == EquipmentType.Boots;
            })).ToList();
        }
        

    }
}
