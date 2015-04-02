using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Equip
{
    /// <summary>
    /// Classe agglomérant le code de création des enchantements.
    /// </summary>
    public class EnchantFactory
    {


        /// <summary>
        /// Crée le passif du type donné ayant le niveau donné.
        /// </summary>
        public static StateAlterationModel CreatePassive(StateAlterationType type, int level, int durationLevel)
        {
            return new StateAlterationModel()
            {
                Type = type,
                FlatValue = PassiveEquipFactory.BalanceSystems[type][level],
                BaseDuration = GameServer.GetScene().Constants.Equip.EnchantOnHitEffectsDuration[durationLevel]
            };
        }

        public static WeaponEnchantModel Cold()
        {
            var cst = GameServer.GetScene().Constants.Equip;
            WeaponEnchantModel model = new WeaponEnchantModel();
            model.Price = cst.EquipCost[5];
            model.Name = "cold";
            model.OnHitEffects = new List<Entities.StateAlterationModel>()
            {
                CreatePassive(StateAlterationType.MoveSpeed, -2, 4),
            };
            return model;
        }

        public static WeaponEnchantModel Runic()
        {
            var cst = GameServer.GetScene().Constants.Equip;
            WeaponEnchantModel model = new WeaponEnchantModel();
            model.Price = cst.EquipCost[3];
            model.Name = "runic";
            model.PassiveEffects = new List<Entities.StateAlterationModel>()
            {
                CreatePassive(StateAlterationType.CDR, 3, 0),
            };
            return model;
        }


        public static WeaponEnchantModel Destroyer()
        {
            var cst = GameServer.GetScene().Constants.Equip;
            WeaponEnchantModel model = new WeaponEnchantModel();
            model.Price = cst.EquipCost[5];
            model.Name = "destroyer";
            model.OnHitEffects = new List<Entities.StateAlterationModel>()
            {
                CreatePassive(StateAlterationType.ArmorBuff, -3, 2),
            };
            return model;
        }

        public static WeaponEnchantModel Fury()
        {
            var cst = GameServer.GetScene().Constants.Equip;
            WeaponEnchantModel model = new WeaponEnchantModel();
            model.Price = cst.EquipCost[5];
            model.Name = "fury";
            model.CastingEffects = new List<Entities.StateAlterationModel>()
            {
                CreatePassive(StateAlterationType.ArmorBuff, -2, 4),
                CreatePassive(StateAlterationType.AttackSpeed, 4, 4),
                CreatePassive(StateAlterationType.MoveSpeed, 50, 4),
            };
            return model;
        }

        public static WeaponEnchantModel Soften()
        {
            var cst = GameServer.GetScene().Constants.Equip;
            WeaponEnchantModel model = new WeaponEnchantModel();
            model.Price = cst.EquipCost[5];
            model.Name = "soften";
            model.CastingEffects = new List<Entities.StateAlterationModel>()
            {
                CreatePassive(StateAlterationType.AttackDamageBuff, -2, 2),
                CreatePassive(StateAlterationType.MagicDamageBuff, -2, 2),
            };
            return model;
        }

        public static WeaponEnchantModel Vampire()
        {
            var cst = GameServer.GetScene().Constants.Equip;
            WeaponEnchantModel model = new WeaponEnchantModel();
            model.Price = cst.EquipCost[5];
            model.Name = "vampire";
            model.CastingEffects = new List<Entities.StateAlterationModel>()
            {
                CreatePassive(StateAlterationType.Heal, 4, 0),
            };
            return model;
        }

        public static WeaponEnchantModel Precision()
        {
            var cst = GameServer.GetScene().Constants.Equip;
            WeaponEnchantModel model = new WeaponEnchantModel();
            model.Price = cst.EquipCost[5];
            model.Name = "precision";
            model.CastingEffects = new List<Entities.StateAlterationModel>()
            {
                CreatePassive(StateAlterationType.AttackDamageBuff, 4, 3),
                CreatePassive(StateAlterationType.AttackSpeed, 3, 3)
            };
            return model;
        }
    }
}
