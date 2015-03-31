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
        public const int LOWER = 0;
        public const int LOW = 1;
        public const int MEDIUM = 2;
        public const int HIGH = 3;

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
                Cost = cst.UpgradeCost1,
                // Décrit tous les passifs conférés par l'arme.
                PassiveAlterations = new List<StateAlterationModel>()
                {
                    new StateAlterationModel()
                    {
                        BaseDuration = 1.0f,
                        Type = StateAlterationType.AttackSpeed,
                        FlatValue = 1
                    }
                },

                // Décrit le spell d'attaque de l'arme.
                Description = new Spells.SpellDescription()
                {
                    // Pour les armes : le cd doit être de 1.
                    BaseCooldown = 1.0f,

                    // Casting de l'arme : permet de rooter le héros qui attaque pendant son 
                    // auto attaque.
                    CastingTime = 0.01f,
                    CastingTimeAlterations = new List<StateAlterationModel>()
                    {
                        new StateAlterationModel()
                        {
                            BaseDuration = 0.01f,
                            Type = StateAlterationType.Root
                        }
                    },

                    // Mode de visée de l'arme : ne varie généralement pas.
                    TargetType = new Spells.SpellTargetInfo()
                    {
                        Type = Spells.TargettingType.Targetted,
                        AllowedTargetTypes = EntityTypeRelative.AllEnnemy | EntityTypeRelative.AllTargettableNeutral,
                        Range = 8,
                        DieOnCollision = true,
                        Duration = 1,
                        AoeRadius = 0.3f
                    },

                    // Effets à l'impact : en général des dégâts plus ou moins élevés.
                    OnHitEffects = new List<StateAlterationModel>()
                    {
                        new StateAlterationModel()
                        {
                            BaseDuration = 0,
                            FlatValue = cst.WeaponFlatAD[LOWER], // dégâts de base de l'arme.
                            SourcePercentADValue = cst.WeaponScalingAD[MEDIUM], // dégâts en fonction de l'AD du héros.
                            Type = StateAlterationType.AttackDamage,
                        }
                    },
                }
            };

            // Niveau 2 de l'arme
            WeaponUpgradeModel lvl2 = lvl1.Copy();
            lvl2.Cost = cst.UpgradeCost2; 
            lvl2.Description.OnHitEffects.First().FlatValue = cst.WeaponFlatAD[LOW]; // + de dégâts mdr

            model.Upgrades = new List<Equip.WeaponUpgradeModel>()
            {
                lvl1, lvl2,
            };
            return model;
        }
    }
}
