using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
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
        /// Crée une nouvelle instance de ShopDatabase vide.
        /// </summary>
        public ShopDatabase()
        {
            Weapons = new List<WeaponModel>();
            Enchants = new List<WeaponEnchantModel>();
            Armors = new List<PassiveEquipmentModel>();
            Boots = new List<PassiveEquipmentModel>();
            Consummables = new List<ConsummableModel>();


            // Bottes
            Equip.PassiveEquipmentModel baseBoots = new Equip.PassiveEquipmentModel();
            baseBoots.Type = Equip.EquipmentType.Boots;
            baseBoots.Upgrades = new List<Equip.PassiveEquipmentUpgradeModel>() { 
                    new Equip.PassiveEquipmentUpgradeModel(
                        new List<StateAlterationModel>() 
                        {
                            
                            new StateAlterationModel()
                            {
                                Type = StateAlterationType.MoveSpeed,
                                BaseDuration = 1.0f,
                                FlatValue = 5f,
                            }
                        },
                        0)
                };
            Boots.Add(baseBoots);

            // Armure
            Equip.PassiveEquipmentModel baseArmor = new Equip.PassiveEquipmentModel();
            baseArmor.Type = Equip.EquipmentType.Armor;
            baseArmor.Upgrades = new List<Equip.PassiveEquipmentUpgradeModel>() { 
                    new Equip.PassiveEquipmentUpgradeModel(
                        new List<StateAlterationModel>() { },
                        0)
                };
            Armors.Add(baseArmor);

            // Arme
            Equip.WeaponModel model = new Equip.WeaponModel();
            model.Name = "Arme de base";
            model.Price = 0;
            model.Upgrades = new List<Equip.WeaponUpgradeModel>()
                {
                    new Equip.WeaponUpgradeModel()
                    {
                        Cost = 400,
                        PassiveAlterations = new List<StateAlterationModel>()
                        {
                            new StateAlterationModel()
                            {
                                BaseDuration = 1.0f,
                                Type = StateAlterationType.AttackSpeed,
                                FlatValue = 1
                            }
                        },
                        Description = new Spells.SpellDescription()
                        {
                            BaseCooldown = 8.0f,
                            CastingTime = 0.1f,
                            CastingTimeAlterations = new List<StateAlterationModel>()
                            {
                                new StateAlterationModel()
                                {
                                    BaseDuration = 0.01f,
                                    Type = StateAlterationType.Root
                                }
                            },
                            
                            TargetType = new Spells.SpellTargetInfo()
                            {
                                Type = Spells.TargettingType.Targetted,
                                AllowedTargetTypes = EntityTypeRelative.AllEnnemy | EntityTypeRelative.AllTargettableNeutral,
                                Range = 8,
                                DieOnCollision = true,
                                Duration = 1,
                                AoeRadius = 0.3f
                            },
                            OnHitEffects = new List<StateAlterationModel>()
                            {
                                new StateAlterationModel()
                                {
                                    BaseDuration = 0,
                                    FlatValue = 10,
                                    SourcePercentADValue = 0.50f,
                                    Type = StateAlterationType.AttackDamage,
                                }
                            },
                           
                        }
                    }
                };

            Equip.WeaponEnchantModel enchant = new Equip.WeaponEnchantModel()
            {
                Name = "base",
                Price = 0,

            };

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
            Weapons.Add(model);
            Enchants.Add(enchant);
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
    }
}
