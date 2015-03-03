using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Equip
{
    public enum ShopTransactionResult
    {
        InvalidId,
        NotEnoughMoney,
        NotInShopRange,
        UnavailableItem,
        Success
    }
    /// <summary>
    /// Classe permettant la présentation de divers équipements.
    /// 
    /// Les shops sont chargés depuis des fichiers xml.
    /// TODO : buy and sell consummable.
    /// </summary>
    public class Shop
    {
        #region Variables
        List<EquipmentModel> m_availableEquip;
        /// <summary>
        /// Entité propriétaire du shop.
        /// </summary>
        EntityBase m_owner;
        /// <summary>
        /// Range au bout de laquelle les héros ne peuvent plus acheter ni revendre d'items au shop.
        /// </summary>
        float m_shopRange;
        #endregion

        /// <summary>
        /// Crée une nouvelle instance de shop, avec la liste des équipements donnée.
        /// </summary>
        public Shop(List<EquipmentModel> equip, EntityBase owner, float shopRange)
        {
            m_availableEquip = equip;
            m_owner = owner;
            m_shopRange = shopRange;
        }

        /// <summary>
        /// Vends l'équipement du type donné possédé par le héros pour un certain pourcentage
        /// de sa valeur d'origine.
        /// </summary>
        public ShopTransactionResult Sell(EntityHero hero, EquipmentType equipKind)
        {
            EquipmentModel model = null;
            switch(equipKind)
            {
                case EquipmentType.Amulet: model = (hero.Amulet == null ? null : hero.Amulet.Model); hero.Amulet = null; break;
                case EquipmentType.Armor: model = (hero.Armor == null ? null : hero.Armor.Model); hero.Armor = null; break;
                case EquipmentType.Boots: model = (hero.Boots == null ? null : hero.Boots.Model); hero.Boots = null; break;
                case EquipmentType.Weapon: model = (hero.Weapon == null ? null : hero.Weapon.Model); hero.Weapon = null; break;
                case EquipmentType.WeaponEnchant: return ShopTransactionResult.InvalidId;
                case EquipmentType.Consummable: return ShopTransactionResult.InvalidId;
            }

            // Vérifie que le héros possède un équipement.
            if (model == null)
                return ShopTransactionResult.InvalidId;

            // Vérifie la distance au shop.
            float dst = Vector2.Distance(hero.Position, m_owner.Position);
            if (dst > m_shopRange)
                return ShopTransactionResult.NotInShopRange;


            float factor = GameServer.GetScene().Constants.Structures.Shops.SellingPriceFactor;
            hero.PA += factor * model.Price;

            return ShopTransactionResult.Success;
        }
        /// <summary>
        /// Achète un objet d'id donné au shop.
        /// </summary>
        /// <param name="equipId"></param>
        /// <returns></returns>
        public ShopTransactionResult Purchase(EntityHero hero, int equipId)
        {
            EquipmentModel equip = GetEquipmentById(equipId);
            if (equip == null)
                return ShopTransactionResult.InvalidId;
            
            if (equip.Price > hero.PA)
                return ShopTransactionResult.NotEnoughMoney;

            float dst = Vector2.Distance(hero.Position, m_owner.Position);
            if (dst > m_shopRange)
                return ShopTransactionResult.NotInShopRange;

            hero.PA -= equip.Price;

            // Equipe l'arme au héros.
            switch(equip.Type)
            {
                case EquipmentType.Amulet: hero.Amulet = new Amulet(hero, (PassiveEquipmentModel)equip); break;
                case EquipmentType.Armor: hero.Armor = new Armor(hero, (PassiveEquipmentModel)equip); break;
                case EquipmentType.Boots: hero.Boots = new Boots(hero, (PassiveEquipmentModel)equip); break;
                case EquipmentType.Weapon: hero.Weapon = new Weapon(hero, (WeaponModel)equip); break;
            }

            return ShopTransactionResult.Success;
        }

        /// <summary>
        /// Obtient l'équipement d'id donné.
        /// </summary>
        public EquipmentModel GetEquipmentById(int equip)
        {
            foreach(EquipmentModel model in m_availableEquip)
            {
                if (model.ID == equip)
                    return model;
            }
            return null;
        }
        /// <summary>
        /// Retourne une liste d'elixirs disponibles pour le héros donné.
        /// </summary>
        /// <param name="hero"></param>
        /// <returns></returns>
        public List<EquipmentModel> GetConsummables(EntityHero hero)
        {
            return m_availableEquip.Where(new Func<EquipmentModel, bool>((EquipmentModel e) =>
            {
                return e.Type == EquipmentType.Consummable;
            })).ToList();
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

        /// <summary>
        /// Retourne une liste des amulettes disponibles pour le héros donné.
        /// </summary>
        public List<EquipmentModel> GetAmulets(EntityHero hero)
        {
            return m_availableEquip.Where(new Func<EquipmentModel, bool>((EquipmentModel e) =>
            {
                return e.Type == EquipmentType.Amulet;
            })).ToList();
        }
    }
}
