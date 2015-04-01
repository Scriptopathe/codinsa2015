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
        public const int LOWER = 0;
        public const int LOW = 1;
        public const int MEDIUM = 2;
        public const int HIGH = 3;

        /// <summary>
        /// Génère les passifs des armures
        /// </summary>
        public static List<StateAlterationModel> Passives(int bonusArmor, int bonusRM, int bonusHP, int regenHP, int bonusSpeed)
        {

            var cst = GameServer.GetScene().Constants.Equip;
            return new List<StateAlterationModel>()
            {
                // bonus armure
                new StateAlterationModel()
                {
                    Type = StateAlterationType.ArmorBuff,
                    FlatValue = bonusArmor == 0 ? bonusArmor : cst.ArmorBonusArmor[bonusArmor]
                },
                // bonus de rm
                new StateAlterationModel()
                {
                    Type = StateAlterationType.MagicResistBuff,
                    FlatValue = bonusRM == 0 ? bonusRM : cst.ArmorBonusRM[bonusRM]
                },
                // bonus de HP
                new StateAlterationModel()
                {
                    Type = StateAlterationType.MaxHP,
                    FlatValue = bonusHP == 0 ? bonusHP : cst.ArmorBonusHP[bonusHP]
                },
                // bonus de regen hp
                new StateAlterationModel()
                {
                    Type = StateAlterationType.Regen,
                    FlatValue = regenHP == 0 ? regenHP : cst.ArmorBonusRegen[regenHP]
                },
                // bonus de speed
                new StateAlterationModel()
                {
                    Type = StateAlterationType.MoveSpeed,
                    FlatValue = bonusSpeed == 0 ? bonusSpeed : cst.ArmorBonusSpeed[bonusSpeed]
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
                Cost = 0,
                // Décrit tous les passifs conférés par l'arme.
                PassiveAlterations = Passives(0, HIGH, 0, LOW, 0),
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
    }
}
