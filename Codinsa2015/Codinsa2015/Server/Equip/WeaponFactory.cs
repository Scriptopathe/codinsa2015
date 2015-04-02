using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Spells;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Equip
{
  
    /// <summary>
    /// Classe statique regroupant le code de création des diverses armes.
    /// </summary>
    public static class WeaponFactory
    {
        public const int LOWER = 1;
        public const int LOW = 2;
        public const int MEDIUM = 3;
        public const int HIGH = 4;

        /// <summary>
        /// Casting time alteration commune à toutes les armes.
        /// </summary>
        /// <returns></returns>
        public static List<StateAlterationModel> CastingTimeAlteration = new List<StateAlterationModel>() {
            new StateAlterationModel()
            {
                BaseDuration = 0.1f,
                Type = StateAlterationType.Root
            }
        };
        
        /// <summary>
        /// Target info commun à toutes les armes.
        /// </summary>
        public static SpellTargetInfo TargetInfo(int range)
        {
            return new Spells.SpellTargetInfo()
            {
                Type = Spells.TargettingType.Targetted,
                AllowedTargetTypes = EntityTypeRelative.AllEnnemy | EntityTypeRelative.AllTargettableNeutral,
                Range = GameServer.GetScene().Constants.Equip.EquipRange[range],
                DieOnCollision = true,
                Duration = 0.2f,
                AoeRadius = 0.3f
            };
        }

        /// <summary>
        /// Génère les passifs d'AS et d'AD flat.
        /// </summary>
        public static List<StateAlterationModel> Passives(int attackSpeed, int bonusAd)
        {

            var cst = GameServer.GetScene().Constants.Equip;
            return new List<StateAlterationModel>()
            {
                // vitesse d'attaque
                new StateAlterationModel()
                {
                    BaseDuration = 1.0f,
                    Type = StateAlterationType.AttackSpeed,
                    FlatValue = cst.EquipBonusAttackSpeed[attackSpeed]
                },
                // bonus d'ad
                new StateAlterationModel()
                {
                    BaseDuration = 1.0f,
                    Type = StateAlterationType.AttackDamageBuff,
                    FlatValue = cst.EquipBonusAdFlat[bonusAd]
                }
            };
        }

        /// <summary>
        /// Génère une altération de on-hit damage avec les dégâts flats et ratios ad donnés.
        /// </summary>
        public static StateAlterationModel OnHitDamages(int flatDmg, int adRatio)
        {
            var cst = GameServer.GetScene().Constants.Equip;
            return new StateAlterationModel()
            {
                BaseDuration = 0,
                FlatValue = cst.EquipAdFlatOnHit[flatDmg], // dégâts de base de l'arme.
                SourcePercentADValue = cst.EquipBonusSourcePercentAd[adRatio], // dégâts en fonction de l'AD du héros.
                Type = StateAlterationType.AttackDamage,
            };
        }

        /// <summary>
        /// Décrit une arme de base.
        /// </summary>
        /// <returns></returns>
        public static WeaponModel BaseWeapon()
        {
            // Chargement des constantes
            var cst = GameServer.GetScene().Constants.Equip;
            // Arme
            WeaponModel model = new Equip.WeaponModel();
            model.Name = "Arme de base";

            // Décrit le niveau 1 de l'arme
            WeaponUpgradeModel lvl1 = new Equip.WeaponUpgradeModel()
            {
                Cost = 0,
                // Décrit tous les passifs conférés par l'arme.
                PassiveAlterations = Passives(LOW, LOW),

                // Décrit le spell d'attaque de l'arme.
                Description = new Spells.SpellDescription()
                {
                    // Pour les armes : le cd doit être de 1.
                    BaseCooldown = 1.0f,

                    // Casting de l'arme : permet de rooter le héros qui attaque pendant son 
                    // auto attaque.
                    CastingTime = 0.01f,
                    CastingTimeAlterations = CastingTimeAlteration,

                    // Mode de visée de l'arme : ne varie généralement pas.
                    TargetType = TargetInfo(LOW),

                    // Effets à l'impact : en général des dégâts plus ou moins élevés.
                    OnHitEffects = new List<StateAlterationModel>()
                    {
                        OnHitDamages(LOW, LOW)
                    },
                }
            };

            model.Upgrades = new List<Equip.WeaponUpgradeModel>()
            {
                lvl1,
            };
            return model;
        }


        /// <summary>
        /// Arme spear.
        /// </summary>
        /// <returns></returns>
        public static WeaponModel Spear()
        {
            // Chargement des constantes
            var cst = GameServer.GetScene().Constants.Equip;
            // Arme
            WeaponModel model = new Equip.WeaponModel();
            model.Name = "Spear";

            // Décrit le niveau 1 de l'arme
            WeaponUpgradeModel lvl1 = new Equip.WeaponUpgradeModel()
            {
                // ++ Coût
                Cost = cst.EquipCost[LOW],

                // Décrit le spell d'attaque de l'arme.
                Description = new Spells.SpellDescription()
                {
                    // Pour les armes : le cd doit être de 1.
                    BaseCooldown = 1.0f,

                    // Casting de l'arme : permet de rooter le héros qui attaque pendant son 
                    // auto attaque.
                    CastingTime = 0.01f,
                    CastingTimeAlterations = CastingTimeAlteration,

                    // ++ Range
                    TargetType = TargetInfo(HIGH),

                    // Effets à l'impact : en général des dégâts plus ou moins élevés.
                    OnHitEffects = new List<StateAlterationModel>()
                    {
                        // ++ Damage flat, Ad ratio
                        OnHitDamages(MEDIUM, MEDIUM)
                    },
                },
                // ++ Vitesse d'attaque / bonus d'ad
                PassiveAlterations = Passives(MEDIUM, MEDIUM),
            };

            // Niveau 2 de l'arme
            WeaponUpgradeModel lvl2 = lvl1.Copy();
            lvl2.Cost = cst.UpgradeCost1;
            lvl2.PassiveAlterations[1].FlatValue += cst.UpgradeAdBonus[LOW];
            lvl2.PassiveAlterations[0].FlatValue += cst.UpgradeAsBonus[LOW];

            // Niveau 3 de l'arme
            WeaponUpgradeModel lvl3 = lvl2.Copy();
            lvl3.Cost = cst.UpgradeCost2;
            lvl3.PassiveAlterations[1].FlatValue += cst.UpgradeAdBonus[LOW];
            lvl3.PassiveAlterations[0].FlatValue += cst.UpgradeAsBonus[LOW];
            model.Upgrades = new List<Equip.WeaponUpgradeModel>()
            {
                lvl1, lvl2, lvl3
            };
            return model;
        }

        /// <summary>
        /// SwordAndShield.
        /// </summary>
        public static WeaponModel SwordAndShield()
        {
            // Chargement des constantes
            var cst = GameServer.GetScene().Constants.Equip;
            // Arme
            WeaponModel model = new Equip.WeaponModel();
            model.Name = "SwordAndShield";

            // Décrit le niveau 1 de l'arme
            WeaponUpgradeModel lvl1 = new Equip.WeaponUpgradeModel()
            {
                // ++ Coût
                Cost = cst.EquipCost[LOW],

                // Décrit le spell d'attaque de l'arme.
                Description = new Spells.SpellDescription()
                {
                    // Pour les armes : le cd doit être de 1.
                    BaseCooldown = 1.0f,

                    // Casting de l'arme : permet de rooter le héros qui attaque pendant son 
                    // auto attaque.
                    CastingTime = 0.01f,
                    CastingTimeAlterations = CastingTimeAlteration,

                    // ++ Range
                    TargetType = TargetInfo(MEDIUM),

                    // Effets à l'impact : en général des dégâts plus ou moins élevés.
                    OnHitEffects = new List<StateAlterationModel>()
                    {
                        // ++ Damage flat, Ad ratio
                        OnHitDamages(LOW, MEDIUM)
                    },
                },
                // ++ Vitesse d'attaque / bonus d'ad
                PassiveAlterations = Passives(LOW, MEDIUM),
            };
            lvl1.PassiveAlterations.Add(new StateAlterationModel()
            {
                Type = StateAlterationType.ArmorBuff,
                FlatValue = GameServer.GetScene().Constants.ActiveSpells.ArmorAlterations[MEDIUM],
                BaseDuration = 1.0f,
            });
            // Niveau 2 de l'arme
            WeaponUpgradeModel lvl2 = lvl1.Copy();
            lvl2.Cost = cst.UpgradeCost1;
            lvl2.PassiveAlterations[1].FlatValue += cst.UpgradeAdBonus[MEDIUM]; 
            lvl2.PassiveAlterations[0].FlatValue += cst.UpgradeAsBonus[LOW]; 
            
            // Niveau 3 de l'arme
            WeaponUpgradeModel lvl3 = lvl2.Copy();
            lvl3.Cost = cst.UpgradeCost2;
            lvl3.PassiveAlterations[1].FlatValue += cst.UpgradeAdBonus[MEDIUM];
            lvl3.PassiveAlterations[0].FlatValue += cst.UpgradeAsBonus[LOW];
            lvl3.PassiveAlterations[2].FlatValue = GameServer.GetScene().Constants.ActiveSpells.ArmorAlterations[HIGH];
            model.Upgrades = new List<Equip.WeaponUpgradeModel>()
            {
                lvl1, lvl2, lvl3
            };
            return model;
        }

        /// <summary>
        /// Hammer.
        /// </summary>
        public static WeaponModel Hammer()
        {
            // Chargement des constantes
            var cst = GameServer.GetScene().Constants.Equip;
            // Arme
            WeaponModel model = new Equip.WeaponModel();
            model.Name = "Hammer";

            // Décrit le niveau 1 de l'arme
            WeaponUpgradeModel lvl1 = new Equip.WeaponUpgradeModel()
            {
                // ++ Coût
                Cost = cst.EquipCost[LOW],

                // Décrit le spell d'attaque de l'arme.
                Description = new Spells.SpellDescription()
                {
                    // Pour les armes : le cd doit être de 1.
                    BaseCooldown = 1.0f,

                    // Casting de l'arme : permet de rooter le héros qui attaque pendant son 
                    // auto attaque.
                    CastingTime = 0.01f,
                    CastingTimeAlterations = CastingTimeAlteration,

                    // ++ Range
                    TargetType = TargetInfo(LOW),

                    // Effets à l'impact : en général des dégâts plus ou moins élevés.
                    OnHitEffects = new List<StateAlterationModel>()
                    {
                        // ++ Damage flat, Ad ratio
                        OnHitDamages(HIGH, LOW)
                    },
                },
                // ++ Vitesse d'attaque / bonus d'ad
                PassiveAlterations = Passives(MEDIUM, HIGH),
            };
            // Niveau 2 de l'arme
            WeaponUpgradeModel lvl2 = lvl1.Copy();
            lvl2.Cost = cst.UpgradeCost1;
            lvl2.PassiveAlterations[1].FlatValue += cst.UpgradeAdBonus[MEDIUM];
            lvl2.PassiveAlterations[0].FlatValue += cst.UpgradeAsBonus[MEDIUM];

            // Niveau 3 de l'arme
            WeaponUpgradeModel lvl3 = lvl2.Copy();
            lvl3.Cost = cst.UpgradeCost2;
            lvl3.PassiveAlterations[1].FlatValue += cst.UpgradeAdBonus[MEDIUM];
            lvl3.PassiveAlterations[0].FlatValue += cst.UpgradeAsBonus[MEDIUM];
            model.Upgrades = new List<Equip.WeaponUpgradeModel>()
            {
                lvl1, lvl2, lvl3
            };
            return model;
        }

        /// <summary>
        /// Cutlass.
        /// </summary>
        public static WeaponModel Cutlass()
        {
            // Chargement des constantes
            var cst = GameServer.GetScene().Constants.Equip;
            // Arme
            WeaponModel model = new Equip.WeaponModel();
            model.Name = "Cutlass";

            // Décrit le niveau 1 de l'arme
            WeaponUpgradeModel lvl1 = new Equip.WeaponUpgradeModel()
            {
                // ++ Coût
                Cost = cst.EquipCost[LOW],

                // Décrit le spell d'attaque de l'arme.
                Description = new Spells.SpellDescription()
                {
                    // Pour les armes : le cd doit être de 1.
                    BaseCooldown = 1.0f,

                    // Casting de l'arme : permet de rooter le héros qui attaque pendant son 
                    // auto attaque.
                    CastingTime = 0.01f,
                    CastingTimeAlterations = CastingTimeAlteration,

                    // ++ Range
                    TargetType = TargetInfo(LOW),

                    // Effets à l'impact : en général des dégâts plus ou moins élevés.
                    OnHitEffects = new List<StateAlterationModel>()
                    {
                        // ++ Damage flat, Ad ratio
                        OnHitDamages(MEDIUM, HIGH)
                    },
                },
                // ++ Vitesse d'attaque / bonus d'ad
                PassiveAlterations = Passives(HIGH, MEDIUM),
            };
            lvl1.PassiveAlterations.Add(new StateAlterationModel()
            {
                Type = StateAlterationType.MoveSpeed,
                FlatValue = GameServer.GetScene().Constants.ActiveSpells.MoveSpeedAlterations[LOWER],
                BaseDuration = 0.25f,
            });
            // Niveau 2 de l'arme
            WeaponUpgradeModel lvl2 = lvl1.Copy();
            lvl2.Cost = cst.UpgradeCost1;
            lvl2.PassiveAlterations[1].FlatValue += cst.UpgradeAdBonus[MEDIUM];
            lvl2.PassiveAlterations[0].FlatValue += cst.UpgradeAsBonus[MEDIUM];
            // Niveau 3 de l'arme
            WeaponUpgradeModel lvl3 = lvl2.Copy();
            lvl3.Cost = cst.UpgradeCost2;
            lvl3.PassiveAlterations[1].FlatValue += cst.UpgradeAdBonus[MEDIUM];
            lvl3.PassiveAlterations[0].FlatValue += cst.UpgradeAsBonus[MEDIUM];
            model.Upgrades = new List<Equip.WeaponUpgradeModel>()
            {
                lvl1, lvl2, lvl3
            };
            return model;
        }
    }


}
