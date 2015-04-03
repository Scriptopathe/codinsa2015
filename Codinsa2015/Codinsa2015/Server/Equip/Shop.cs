using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Equip
{
    [Clank.ViewCreator.Enum("Représente les résultats de transaction possibles après un achat / vente.")]
    public enum ShopTransactionResult
    {
        ItemDoesNotExist,
        ItemIsNotAConsummable,
        NoItemToSell,
        NotEnoughMoney,
        NotInShopRange,
        UnavailableItem,
        ProvidedSlotDoesNotExist,
        NoSlotAvailableOnHero,
        EnchantForNoWeapon,
        StackOverflow,
        Success,
        AlreadyMaxLevel,
    }
    /// <summary>
    /// Classe permettant la présentation de divers équipements.
    /// </summary>
    public class Shop
    {
        #region Variables
        ShopDatabase m_database;
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
        public Shop(ShopDatabase db, EntityBase owner, float shopRange)
        {
            m_database = db;
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

            // Vérifie la distance au shop.
            float dst = Vector2.Distance(hero.Position, m_owner.Position);
            if (dst > m_shopRange)
                return ShopTransactionResult.NotInShopRange;

            switch(equipKind)
            {
                //case EquipmentType.Amulet: model = (hero.Amulet == null ? null : hero.Amulet.Model); hero.Amulet = null; break;
                case EquipmentType.Armor: model = (hero.Armor == null ? null : hero.Armor.Model); hero.Armor = null; break;
                case EquipmentType.Boots: model = (hero.Boots == null ? null : hero.Boots.Model); hero.Boots = null; break;
                case EquipmentType.Weapon: model = (hero.Weapon == null ? null : hero.Weapon.Model); hero.Weapon = null; break;
                case EquipmentType.WeaponEnchant:
                    if (hero.Weapon == null)
                        model = null;
                    else if (hero.Weapon.Enchant == null)
                        model = null;
                    else
                        model = hero.Weapon.Enchant;
                    if(model != null)
                    {
                        hero.Weapon.Enchant = null;
                    }
                    break;
                case EquipmentType.Consummable: return ShopTransactionResult.ItemDoesNotExist;
            }

            // Vérifie que le héros possède un équipement.
            if (model == null)
                return ShopTransactionResult.NoItemToSell;



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
                return ShopTransactionResult.ItemDoesNotExist;
            
            if (equip.Price > hero.PA)
                return ShopTransactionResult.NotEnoughMoney;

            float dst = Vector2.Distance(hero.Position, m_owner.Position);
            if (dst > m_shopRange)
                return ShopTransactionResult.NotInShopRange;


            // Equipe l'arme au héros.
            switch(equip.Type)
            {
                /*case EquipmentType.Amulet:
                    if (hero.Amulet != null)
                        return ShopTransactionResult.NoSlotAvailableOnHero;
                    hero.Amulet = new Amulet(hero, (PassiveEquipmentModel)equip); 
                    break;*/
                case EquipmentType.Armor:
                    if (hero.Armor != null)
                        return ShopTransactionResult.NoSlotAvailableOnHero;
                    hero.Armor = new Armor(hero, (PassiveEquipmentModel)equip); 
                    break;
                case EquipmentType.Boots:
                    if (hero.Boots != null)
                        return ShopTransactionResult.NoSlotAvailableOnHero;
                    hero.Boots = new Boots(hero, (PassiveEquipmentModel)equip);
                    break;
                case EquipmentType.Weapon:
                    if (hero.Weapon != null)
                        return ShopTransactionResult.NoSlotAvailableOnHero;
                    hero.Weapon = new Weapon(hero, (WeaponModel)equip);
                    break;
                case EquipmentType.WeaponEnchant:
                    if(hero.Weapon == null)
                    {
                        return ShopTransactionResult.EnchantForNoWeapon;
                    }
                    if (hero.Weapon.Enchant != null)
                        return ShopTransactionResult.NoSlotAvailableOnHero;

                    hero.Weapon.Enchant = (WeaponEnchantModel)equip;
                    break;
            }
            
            hero.PA -= equip.Price;
            return ShopTransactionResult.Success;
        }




        /// <summary>
        /// Vends un consommable dans le slot donné du héros donné.
        /// </summary>
        public ShopTransactionResult SellConsummable(EntityHero hero, int slot)
        {
            // Vérifie la distance au shop.
            float dst = Vector2.Distance(hero.Position, m_owner.Position);
            if (dst > m_shopRange)
                return ShopTransactionResult.NotInShopRange;

            // Slot out of range : erreur
            if (slot < 0 || slot >= hero.Consummables.Length)
                return ShopTransactionResult.ProvidedSlotDoesNotExist;

            // Consommable vide ?
            bool emptySlot = hero.Consummables[slot].Model.ConsummableType == ConsummableType.Empty;
            if (emptySlot)
                return ShopTransactionResult.NoItemToSell;

            if (hero.Consummables[slot].Count <= 0)
                return ShopTransactionResult.NoItemToSell;

            ConsummableModel model = hero.Consummables[slot].Model;
            hero.PA += model.Price;
            hero.Consummables[slot].Count--;
            return ShopTransactionResult.Success;
        }

        /// <summary>
        /// Achète un consommable pour le héros donné, et le place dans le slot donné.
        /// </summary>
        public ShopTransactionResult PurchaseConsummable(EntityHero hero, int consummableId, int slot)
        {
            EquipmentModel equip = GetEquipmentById(consummableId);
            if (equip == null || equip.Type != EquipmentType.Consummable)
                return ShopTransactionResult.ItemDoesNotExist;
            ConsummableModel model = (ConsummableModel)equip;
            if (equip.Price > hero.PA)
                return ShopTransactionResult.NotEnoughMoney;

            float dst = Vector2.Distance(hero.Position, m_owner.Position);
            if (dst > m_shopRange)
                return ShopTransactionResult.NotInShopRange;
            
            // Slot out of range : erreur
            if(slot < 0 || slot >= hero.Consummables.Length)
                return ShopTransactionResult.ProvidedSlotDoesNotExist;
            bool emptySlot = hero.Consummables[slot].Model.ConsummableType == ConsummableType.Empty;

            // Si un consommable d'un autre type est dans le slot, erreur
            if (!emptySlot && (hero.Consummables[slot].Model.ConsummableType != model.ConsummableType))
                return ShopTransactionResult.NoSlotAvailableOnHero;

            // Dépassement de la stack du consommable : erreur
            if (hero.Consummables[slot].Count >= hero.Consummables[slot].Model.MaxStackSize)
                return ShopTransactionResult.StackOverflow;

            // Achat !!
            hero.Consummables[slot].Model = model;
            hero.Consummables[slot].Count++;
            hero.PA -= equip.Price;
            return ShopTransactionResult.Success;
        }
        /// <summary>
        /// Obtient l'équipement d'id donné.
        /// </summary>
        public EquipmentModel GetEquipmentById(int equip)
        {
            return m_database.GetEquipmentById(equip);
        }
        /// <summary>
        /// Retourne une liste d'enchantements disponibles pour le héros donné.
        /// </summary>
        public List<EquipmentModel> GetEnchants(EntityHero hero)
        {
            List<EquipmentModel> items = new List<EquipmentModel>();
            foreach (var item in m_database.Enchants)
            {
                // TODO : un filtre éventuel.
                items.Add(item);
            }
            return items;
        }
        /// <summary>
        /// Retourne une liste d'elixirs disponibles pour le héros donné.
        /// </summary>
        public List<EquipmentModel> GetConsummables(EntityHero hero)
        {
            List<EquipmentModel> items = new List<EquipmentModel>();
            foreach(var item in m_database.Consummables)
            {
                // TODO : un filtre éventuel.
                items.Add(item);
            }
            return items;
        }

        /// <summary>
        /// Retourne une liste d'armes disponibles pour le héros donné.
        /// </summary>
        public List<EquipmentModel> GetWeapons(EntityHero hero)
        {
            List<EquipmentModel> items = new List<EquipmentModel>();
            foreach (var item in m_database.Weapons)
            {
                // TODO : un filtre éventuel.
                items.Add(item);
            }
            return items;
        }

        /// <summary>
        /// Retourne une liste d'armures disponibles pour le héros donné.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<EquipmentModel> GetArmors(EntityHero hero)
        {
            List<EquipmentModel> items = new List<EquipmentModel>();
            foreach (var item in m_database.Armors)
            {
                // TODO : un filtre éventuel.
                items.Add(item);
            }
            return items;
        }

        /// <summary>
        /// Retourne une liste de bottes disponibles pour le héros donné.
        /// </summary>
        public List<EquipmentModel> GetBoots(EntityHero hero)
        {
            List<EquipmentModel> items = new List<EquipmentModel>();
            foreach (var item in m_database.Boots)
            {
                // TODO : un filtre éventuel.
                items.Add(item);
            }
            return items;
        }

        #region Upgrades
        /// <summary>
        /// Upgrade l'équipement du type donné.
        /// </summary>
        public ShopTransactionResult UpgradeEquip(EntityHero hero, EquipmentType type)
        {
            switch(type)
            {
                case EquipmentType.Armor:
                    return UpgradeArmor(hero);
                case EquipmentType.Weapon:
                    return UpgradeWeapon(hero);
                case EquipmentType.Boots:
                    return UpgradeBoots(hero);
            }
            return ShopTransactionResult.UnavailableItem;
        }
        /// <summary>
        /// Demande au shop de procéder si possible à l'upgrade de l'arme du héros donné.
        /// </summary>
        public ShopTransactionResult UpgradeWeapon(EntityHero hero)
        {
            // Vérifie la distance au shop.
            float dst = Vector2.Distance(hero.Position, m_owner.Position);
            if (dst > m_shopRange)
                return ShopTransactionResult.NotInShopRange;

            // Le héros a-t-il une arme ?
            if (hero.Weapon == null)
                return ShopTransactionResult.UnavailableItem;

            // Vérification du niveau.
            int nextLevel = hero.Weapon.Level + 1;
            if (nextLevel >= hero.Weapon.Model.Upgrades.Count)
            {
                return ShopTransactionResult.AlreadyMaxLevel;
            }

            // Vérification de l'argent disponible.
            float price = hero.Weapon.Model.Upgrades[nextLevel].Cost;
            if (price > hero.PA)
                return ShopTransactionResult.NotEnoughMoney;

            // Si c'est bon : on procède.
            hero.PA -= price;

            hero.Weapon.Upgrade();

            return ShopTransactionResult.Success;
        }
        /// <summary>
        /// Demande au shop de procéder si possible à l'upgrade des bottes du héros donné.
        /// </summary>
        public ShopTransactionResult UpgradeBoots(EntityHero hero)
        {
            // Vérifie la distance au shop.
            float dst = Vector2.Distance(hero.Position, m_owner.Position);
            if (dst > m_shopRange)
                return ShopTransactionResult.NotInShopRange;

            // Le héros a-t-il des bottes ?
            if (hero.Boots == null)
                return ShopTransactionResult.UnavailableItem;
            // Vérification du niveau.
            int nextLevel = hero.Boots.Level + 1;
            if (nextLevel >= hero.Boots.Model.Upgrades.Count)
            {
                return ShopTransactionResult.AlreadyMaxLevel;
            }

            // Vérification de l'argent disponible.
            float price = hero.Boots.Model.Upgrades[nextLevel].Cost;
            if (price > hero.PA)
                return ShopTransactionResult.NotEnoughMoney;

            // Si c'est bon : on procède.
            hero.PA -= price;

            hero.Boots.Upgrade();

            return ShopTransactionResult.Success;
        }

        /// <summary>
        /// Demande au shop de procéder si possible à l'upgrade de l'armure du héros donné.
        /// </summary>
        public ShopTransactionResult UpgradeArmor(EntityHero hero)
        {
            // Vérifie la distance au shop.
            float dst = Vector2.Distance(hero.Position, m_owner.Position);
            if (dst > m_shopRange)
                return ShopTransactionResult.NotInShopRange;

            // Le héros a-t-il une armure ?
            if (hero.Armor == null)
                return ShopTransactionResult.UnavailableItem;
            // Vérification du niveau.
            int nextLevel = hero.Armor.Level + 1;
            if (nextLevel >= hero.Armor.Model.Upgrades.Count)
            {
                return ShopTransactionResult.AlreadyMaxLevel;
            }

            // Vérification de l'argent disponible.
            float price = hero.Armor.Model.Upgrades[nextLevel].Cost;
            if (price > hero.PA)
                return ShopTransactionResult.NotEnoughMoney;

            // Si c'est bon : on procède.
            hero.PA -= price;

            hero.Armor.Upgrade();

            return ShopTransactionResult.Success;
        }
        #endregion
    }
}
