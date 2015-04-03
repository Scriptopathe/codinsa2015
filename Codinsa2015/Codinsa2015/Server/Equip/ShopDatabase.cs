using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
using Codinsa2015.Server.Spells;
namespace Codinsa2015.Server.Equip
{
    /// <summary>
    /// Représente une base de données contenant des instances d'armes, armures etc...
    /// </summary>
    public class ShopDatabase
    {
        /// <summary>
        /// Armes contenues dans la base de données.
        /// </summary>
        public List<WeaponModel> Weapons { get; set; }
        /// <summary>
        /// Enchantements contenus dans la base de données.
        /// </summary>
        public List<WeaponEnchantModel> Enchants { get; set; }
        /// <summary>
        /// Armures contenues dans la base de données.
        /// </summary>
        public List<PassiveEquipmentModel> Armors { get; set; }

        /// <summary>
        /// Bottes contenues dans la base de données.
        /// </summary>
        public List<PassiveEquipmentModel> Boots { get; set; }

        /// <summary>
        /// Obtient les différents types de consommables.
        /// </summary>
        public List<ConsummableModel> Consummables { get; set; }

        /// <summary>
        /// Obtient la liste des sorts existants.
        /// </summary>
        public List<Spells.SpellModel> Spells { get; set; }

        public Spells.SpellModel GetSpellById(int id)
        {
            var models = Spells.Where(new Func<SpellModel, bool>(model => model.ID == id));
            if (models.Count() != 0)
                return models.First();
            return null;
        }
        /// <summary>
        /// Crée une nouvelle instance de ShopDatabase vide.
        /// </summary>
        public ShopDatabase()
        {
            Weapons = new List<WeaponModel>();
            Enchants = new List<WeaponEnchantModel>();
            Armors = new List<PassiveEquipmentModel>();
            Boots = new List<PassiveEquipmentModel>();
            Consummables = new List<ConsummableModel>();
            Spells = new List<SpellModel>();
            // Bottes
            Boots.Add(PassiveEquipFactory.LightShoes());
            Boots.Add(PassiveEquipFactory.LivingShoes());
            Boots.Add(PassiveEquipFactory.SteelChose());
            Boots.Add(PassiveEquipFactory.WhiteShoes());
            Boots.Add(PassiveEquipFactory.Rangers());

            // Armures
            Armors.Add(ArmorFactory.AdamantineHarness());
            Armors.Add(ArmorFactory.ArcmageArmor());
            Armors.Add(ArmorFactory.BloodArmor());
            Armors.Add(ArmorFactory.PatchworkArmor());
            Armors.Add(ArmorFactory.PricklyCoat());

            // Enchantements
            Enchants.Add(EnchantFactory.Cold());
            Enchants.Add(EnchantFactory.Destroyer());
            Enchants.Add(EnchantFactory.Fury());
            Enchants.Add(EnchantFactory.Precision());
            Enchants.Add(EnchantFactory.Soften());
            Enchants.Add(EnchantFactory.Vampire());
            Enchants.Add(EnchantFactory.Runic());

            // Armes
            Weapons.Add(WeaponFactory.BaseWeapon());
            Weapons.Add(WeaponFactory.Spear());
            Weapons.Add(WeaponFactory.SwordAndShield());
            Weapons.Add(WeaponFactory.Cutlass());
            Weapons.Add(WeaponFactory.Hammer());

            // Sorts
            Spells.Add(SpellFactory.Go());
            Spells.Add(SpellFactory.BroForce());
            Spells.Add(SpellFactory.Bim());
            Spells.Add(SpellFactory.HoldOn());
            Spells.Add(SpellFactory.Kick());
            Spells.Add(SpellFactory.LaserBeam());
            Spells.Add(SpellFactory.MagicBeam());
            Spells.Add(SpellFactory.MaximumGravity());
            Spells.Add(SpellFactory.Meteor());
            Spells.Add(SpellFactory.Rage());
            Spells.Add(SpellFactory.Stasis());
            Spells.Add(SpellFactory.WarCry());
            // Consommables
            Equip.ConsummableModel unward = new Equip.ConsummableModel()
            {
                ConsummableType = Equip.ConsummableType.Unward,
                MaxStackSize = 2,
                Name = "unward",
                Price = 80,
            };
            Equip.ConsummableModel ward = new Equip.ConsummableModel()
            {
                ConsummableType = Equip.ConsummableType.Ward,
                MaxStackSize = 2,
                Name = "ward",
                Price = 80,
            };
            Equip.ConsummableModel empty = new Equip.ConsummableModel()
            {
                ConsummableType = Equip.ConsummableType.Empty,
                MaxStackSize = 2,
                Name = "empty",
                Price = 80
            };
            Consummables.Add(unward);
            Consummables.Add(ward);
            Consummables.Add(empty);

        }

        /// <summary>
        /// Obtient le modèle de consommables correspondant au type de consommable donné.
        /// </summary>
        public ConsummableModel GetConsummableModelByType(ConsummableType type)
        {
            foreach (ConsummableModel model in Consummables)
                if (model.ConsummableType == type)
                    return model;
            return null;
        }
        /// <summary>
        /// Charge une base de données depuis un fichier dont le chemin d'accès est passé en paramètre.
        /// </summary>
        /// <returns></returns>
        public static ShopDatabase Load(string file)
        {
            return Tools.Serializer.Deserialize<ShopDatabase>(System.IO.File.ReadAllText(file));
        }
        
        /// <summary>
        /// Sauvegarde la base de données dans le fichier dont le chemin d'accès est passé en paramètre.
        /// </summary>
        /// <param name="file"></param>
        public void Save(string file)
        {
            System.IO.File.WriteAllText(file, Tools.Serializer.Serialize<ShopDatabase>(this));
        }

        /// <summary>
        /// Obtient le modèle d'équipement d'id donné.
        /// </summary>
        public EquipmentModel GetEquipmentById(int id)
        {
            foreach (var e in Weapons) { if (id == e.ID) return e; }
            foreach (var e in Armors) { if (id == e.ID) return e; }
            foreach (var e in Boots) { if (id == e.ID) return e; }
            foreach (var e in Enchants) { if (id == e.ID) return e; }
            foreach (var e in Consummables) { if (id == e.ID) return e; }
            return null;
        }
    }
}
