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
    public static class ArmorFactory
    {
        static int? LOWER = 0;
        static int? LOW = 1;
        static int? MEDIUM = 2;
        static int? HIGH = 3;
        static int? VERY_HIGH = 4;

        /// <summary>
        /// Génère les passifs des armures
        /// </summary>
        public static List<StateAlterationModel> Passives(int? bonusArmor, int? bonusRM, int? bonusHP, int? regenHP, int? bonusSpeed)
        {

            var cst = GameServer.GetScene().Constants.Equip;
            return new List<StateAlterationModel>()
            {
                // bonus armure (0)
                new StateAlterationModel()
                {
                    Type = StateAlterationType.ArmorBuff,
                    FlatValue = bonusArmor == null ? 0 : Math.Sign(bonusArmor.Value) * cst.ArmorBonusArmor[Math.Abs(bonusArmor.Value)]
                },
                // bonus de rm (1)
                new StateAlterationModel()
                {
                    Type = StateAlterationType.MagicResistBuff,
                    FlatValue = bonusRM == null ? 0 : Math.Sign(bonusRM.Value) * cst.ArmorBonusRM[Math.Abs(bonusRM.Value)]
                },
                // bonus de HP (2)
                new StateAlterationModel()
                {
                    Type = StateAlterationType.MaxHP,
                    FlatValue = bonusHP == null ? 0 : Math.Sign(bonusHP.Value) * cst.ArmorBonusHP[Math.Abs(bonusHP.Value)]
                },
                // bonus de regen hp (3)
                new StateAlterationModel()
                {
                    Type = StateAlterationType.Regen,
                    FlatValue = regenHP == null ? 0 : Math.Sign(regenHP.Value) * cst.ArmorBonusRegen[Math.Abs(regenHP.Value)]
                },
                // bonus de speed (4)
                new StateAlterationModel()
                {
                    Type = StateAlterationType.MoveSpeed,
                    FlatValue = bonusSpeed == null ? 0 : Math.Sign(bonusSpeed.Value) * cst.ArmorBonusSpeed[Math.Abs(bonusSpeed.Value)]
                },
            };
        }

        /// <summary>
        /// Décrit une robe d'archimage
        /// </summary>
        /// <returns></returns>
        public static PassiveEquipmentModel ArcmageArmor()
        {
            // Chargement des constantes
            var cst = GameServer.GetScene().Constants.Equip;
            // Armure
            PassiveEquipmentModel model = new Equip.PassiveEquipmentModel();
            model.Name = "Arcmage armor";

            // Décrit le niveau 1 de l'arme
            PassiveEquipmentUpgradeModel lvl1 = new Equip.PassiveEquipmentUpgradeModel()
            {
                Cost = cst.ArmorCost[LOW.Value],
                // Décrit tous les passifs conférés par l'arme.
                PassiveAlterations = Passives(null, HIGH, null, LOW, null),
            };

            PassiveEquipmentUpgradeModel lvl2 = lvl1.Copy();
            lvl2.Cost = cst.UpgradeCost1;
            lvl2.PassiveAlterations[1].FlatValue += 2 * cst.ArmorBonusRMStep;

            PassiveEquipmentUpgradeModel lvl3 = lvl2.Copy();
            lvl3.Cost = cst.UpgradeCost2;
            lvl3.PassiveAlterations[1].FlatValue += 2 * cst.ArmorBonusRMStep;

            model.Upgrades = new List<Equip.PassiveEquipmentUpgradeModel>()
            {
                lvl1, lvl2, lvl3
            };
            return model;
        }

        /// <summary>
        /// Décrit un harnois d'adamantine
        /// </summary>
        /// <returns></returns>
        public static PassiveEquipmentModel AdamantineHarness()
        {
            // Chargement des constantes
            var cst = GameServer.GetScene().Constants.Equip;
            // Armure
            PassiveEquipmentModel model = new Equip.PassiveEquipmentModel();
            model.Name = "Adamantine harness";

            // Décrit le niveau 1 de l'arme
            PassiveEquipmentUpgradeModel lvl1 = new Equip.PassiveEquipmentUpgradeModel()
            {
                Cost = cst.ArmorCost[LOW.Value],
                // Décrit tous les passifs conférés par l'arme.
                PassiveAlterations = Passives(VERY_HIGH, null, MEDIUM, null, -MEDIUM),
            };

            PassiveEquipmentUpgradeModel lvl2 = lvl1.Copy();
            lvl2.Cost = cst.UpgradeCost1;
            lvl2.PassiveAlterations[0].FlatValue += 4 * cst.ArmorBonusArmorStep;
            lvl2.PassiveAlterations[4].FlatValue += cst.ArmorBonusSpeedStep;

            PassiveEquipmentUpgradeModel lvl3 = lvl2.Copy();
            lvl3.Cost = cst.UpgradeCost2;
            lvl3.PassiveAlterations[0].FlatValue += 4 * cst.ArmorBonusArmorStep;
            lvl3.PassiveAlterations[4].FlatValue += cst.ArmorBonusSpeedStep;

            model.Upgrades = new List<Equip.PassiveEquipmentUpgradeModel>()
            {
                lvl1, lvl2, lvl3
            };
            return model;
        }
    
        /// <summary>
        /// Décrit une armure de sang
        /// </summary>
        /// <returns></returns>
        public static PassiveEquipmentModel BloodArmor()
        {
            // Chargement des constantes
            var cst = GameServer.GetScene().Constants.Equip;
            // Armure
            PassiveEquipmentModel model = new Equip.PassiveEquipmentModel();
            model.Name = "Blood Armor";

            // Décrit le niveau 1 de l'arme
            PassiveEquipmentUpgradeModel lvl1 = new Equip.PassiveEquipmentUpgradeModel()
            {
                Cost = cst.ArmorCost[LOW.Value],
                // Décrit tous les passifs conférés par l'arme.
                PassiveAlterations = Passives(null, null, HIGH, HIGH, null),
            };

            PassiveEquipmentUpgradeModel lvl2 = lvl1.Copy();
            lvl2.Cost = cst.UpgradeCost1;
            lvl2.PassiveAlterations[2].FlatValue += cst.ArmorBonusHPStep;
            lvl2.PassiveAlterations[3].FlatValue += cst.ArmorBonusRegenStep;

            PassiveEquipmentUpgradeModel lvl3 = lvl2.Copy();
            lvl3.Cost = cst.UpgradeCost2;
            lvl3.PassiveAlterations[2].FlatValue += cst.ArmorBonusHPStep;
            lvl3.PassiveAlterations[3].FlatValue += cst.ArmorBonusRegenStep;

            model.Upgrades = new List<Equip.PassiveEquipmentUpgradeModel>()
            {
                lvl1, lvl2, lvl3
            };
            return model;
        }

        /// <summary>
        /// Décrit un patchwork
        /// </summary>
        /// <returns></returns>
        public static PassiveEquipmentModel PatchworkArmor()
        {
            // Chargement des constantes
            var cst = GameServer.GetScene().Constants.Equip;
            // Armure
            PassiveEquipmentModel model = new Equip.PassiveEquipmentModel();
            model.Name = "Patchwork Armor";

            // Décrit le niveau 1 de l'arme
            PassiveEquipmentUpgradeModel lvl1 = new Equip.PassiveEquipmentUpgradeModel()
            {
                Cost = cst.ArmorCost[MEDIUM.Value],
                // Décrit tous les passifs conférés par l'arme.
                PassiveAlterations = Passives(MEDIUM, MEDIUM, null, MEDIUM, null),
            };

            PassiveEquipmentUpgradeModel lvl2 = lvl1.Copy();
            lvl2.Cost = cst.UpgradeCost1 + cst.UpgradeCostBonusIfExpensive;
            lvl2.PassiveAlterations[2].FlatValue += 2* cst.ArmorBonusHPStep;
            lvl2.PassiveAlterations[0].FlatValue += 2.5f * cst.ArmorBonusRegenStep;
            lvl2.PassiveAlterations[1].FlatValue += cst.ArmorBonusRegenStep;

            PassiveEquipmentUpgradeModel lvl3 = lvl2.Copy();
            lvl3.Cost = cst.UpgradeCost2 + cst.UpgradeCostBonusIfExpensive;
            lvl3.PassiveAlterations[2].FlatValue += 2 * cst.ArmorBonusHPStep;
            lvl3.PassiveAlterations[0].FlatValue += 2.5f * cst.ArmorBonusRegenStep;
            lvl3.PassiveAlterations[1].FlatValue += cst.ArmorBonusRegenStep;

            model.Upgrades = new List<Equip.PassiveEquipmentUpgradeModel>()
            {
                lvl1, lvl2, lvl3
            };
            return model;
        }

        /// <summary>
        /// Décrit une cotte à piquants
        /// </summary>
        /// <returns></returns>
        public static PassiveEquipmentModel PricklyCoat()
        {
            // Chargement des constantes
            var cst = GameServer.GetScene().Constants.Equip;
            // Armure
            PassiveEquipmentModel model = new Equip.PassiveEquipmentModel();
            model.Name = "Prickly Coat";

            // Décrit le niveau 1 de l'arme
            PassiveEquipmentUpgradeModel lvl1 = new Equip.PassiveEquipmentUpgradeModel()
            {
                Cost = cst.ArmorCost[LOW.Value],
                // Décrit tous les passifs conférés par l'arme.
                PassiveAlterations = Passives(HIGH, null, null, null, null),
            };

            PassiveEquipmentUpgradeModel lvl2 = lvl1.Copy();
            lvl2.Cost = cst.UpgradeCost1;
            lvl2.PassiveAlterations[2].FlatValue += 2 * cst.ArmorBonusHPStep;
            lvl2.PassiveAlterations[0].FlatValue += cst.ArmorBonusArmorStep;

            PassiveEquipmentUpgradeModel lvl3 = lvl2.Copy();
            lvl3.Cost = cst.UpgradeCost2;
            lvl3.PassiveAlterations[2].FlatValue += 2 * cst.ArmorBonusHPStep;
            lvl3.PassiveAlterations[0].FlatValue += cst.ArmorBonusArmorStep;

            model.Upgrades = new List<Equip.PassiveEquipmentUpgradeModel>()
            {
                lvl1, lvl2, lvl3
            };
            return model;
        }
    }
}
