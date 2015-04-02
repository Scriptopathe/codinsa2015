using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
using Codinsa2015.Server.Equip;
using Codinsa2015.Server.Balancing;
namespace Codinsa2015.Server.Equip
{
    public class PassiveEquipFactory
    {
        #region Helpers
        static float GetValue(float[] cst, int v)
        {
            return Math.Sign(v) * (v == 0 ? 0 : cst[Math.Abs(v) - 1]);
        }

        /// <summary>
        /// Représente un passifs variant avec les niveaux.
        /// </summary>
        public class Passive
        {
            public StateAlterationType Type { get; set; }
            public int[] Levels { get; set; }
            public Passive(StateAlterationType type, int[] levels)
            {
                Type = type;
                Levels = levels;
            }
        }

        /// <summary>
        /// Associe à chaque type d'altération son système d'équilibrage.
        /// </summary>
        public static Dictionary<StateAlterationType, IBalanceSystem<float>> BalanceSystems = new Dictionary<StateAlterationType, IBalanceSystem<float>>()
        {
            { StateAlterationType.ArmorBuff, GameServer.GetScene().Constants.Equip.EquipBonusArmor },
            { StateAlterationType.MagicResistBuff, GameServer.GetScene().Constants.Equip.EquipBonusMR },
            { StateAlterationType.AttackDamageBuff, GameServer.GetScene().Constants.Equip.EquipBonusAdFlat },
            { StateAlterationType.MagicDamageBuff, GameServer.GetScene().Constants.Equip.EquipBonusApFlat },
            { StateAlterationType.AttackSpeed, GameServer.GetScene().Constants.Equip.EquipBonusAttackSpeed },
            { StateAlterationType.Regen, GameServer.GetScene().Constants.Equip.EquipBonusRegen },
            { StateAlterationType.MaxHP, GameServer.GetScene().Constants.Equip.EquipBonusMaxHP },
            { StateAlterationType.MoveSpeed, GameServer.GetScene().Constants.Equip.EquipBonusMoveSpeed },
            { StateAlterationType.CDR, GameServer.GetScene().Constants.Equip.EquipBonusCDR },
            { StateAlterationType.Heal, GameServer.GetScene().Constants.Equip.EquipOnHitEffectFlatHeal}
        };

        /// <summary>
        /// Crée un équipement passif à partir d'une liste de passifs, d'une liste de prix et d'un type.
        /// </summary>
        public static PassiveEquipmentModel Create(string name, EquipmentType type, int[] prices, List<Passive> passives)
        {
            var cst = GameServer.GetScene().Constants.Equip;
            PassiveEquipmentModel b = new PassiveEquipmentModel();
            b.Name = name;
            b.Type = type;
            var passiveAlts = CreatePassives(passives);
            for(int i = 0; i < 3; i++)
            {
                PassiveEquipmentUpgradeModel upgrade = new PassiveEquipmentUpgradeModel();
                upgrade.Cost = cst.EquipCost[prices[i]];
                upgrade.PassiveAlterations = passiveAlts[i];
                b.Upgrades.Add(upgrade);
            }
            return b;
        }
        /// <summary>
        /// Crée une liste d'altération d'état par niveau à partir de passifs donnés.
        /// </summary>
        public static List<StateAlterationModel>[] CreatePassives(List<Passive> passives)
        {
            var cst = GameServer.GetScene().Constants.Equip;
            List<StateAlterationModel>[] levels = new List<StateAlterationModel>[3];
            for (int i = 0; i < 3; i++)
            {
                List<StateAlterationModel> alterations = new List<StateAlterationModel>();
                foreach (Passive passive in passives)
                {
                    if (passive.Levels[i] == 0)
                        continue;

                    alterations.Add(new StateAlterationModel()
                    {
                        Type = passive.Type,
                        FlatValue = BalanceSystems[passive.Type][passive.Levels[i]],
                    });
                }
                levels[i] = alterations;
            }
            return levels;
        }
        #endregion

        #region Boots
        /// <summary>
        /// Crée un équipement passif à partir de niveaux en move speed, armure, rm, regen et max hp.
        /// </summary>
        public static PassiveEquipmentModel CreateBoots(string name, 
            int[] prices,
            int[] moveSpeeds, int[] armor, int[] rm, int[] regen, int[] hp)
        {
            return Create(name, EquipmentType.Boots, prices, new List<Passive>()
                {
                    new Passive(StateAlterationType.MoveSpeed, moveSpeeds),
                    new Passive(StateAlterationType.ArmorBuff, armor),
                    new Passive(StateAlterationType.MagicResistBuff, rm),
                    new Passive(StateAlterationType.Regen, regen),
                    new Passive(StateAlterationType.MaxHP, hp)
                });
        }

        public static PassiveEquipmentModel LightShoes()
        {
            return CreateBoots("light-shoes",
                new int[] { 1, 2, 3 }, // prices,
                new int[] { 1, 3, 5}, // move speeds,
                new int[] { 0, 0, 0}, // armor
                new int[] { 0, 0, 0}, // rm
                new int[] { 0, 0, 1 }, // regen,
                new int[] { 0, 0, 1} // hp
                );
        }


        public static PassiveEquipmentModel Rangers()
        {
            return CreateBoots("rangers",
                new int[] { 1, 2, 3 }, // prices,
                new int[] { 1, 2, 3 }, // move speeds,
                new int[] { 2, 4, 5 }, // armor
                new int[] { 0, 0, 0 }, // rm
                new int[] { 0, 0, 0 }, // regen,
                new int[] { 0, 0, 0 } // hp
                );
        }

        public static PassiveEquipmentModel WhiteShoes()
        {
            return CreateBoots("white-shoes",
                new int[] { 1, 2, 3 }, // prices,
                new int[] { 1, 2, 4 }, // move speeds,
                new int[] { 1, 2, 4 }, // armor
                new int[] { 1, 2, 4 }, // rm
                new int[] { 0, 0, 0 }, // regen,
                new int[] { 0, 0, 0 } // hp
                );
        }

        public static PassiveEquipmentModel SteelChose()
        {
            return CreateBoots("steel-shoes",
                new int[] { 1, 2, 3 }, // prices,
                new int[] { -1, -1, 0 }, // move speeds,
                new int[] { 2, 4, 5 }, // armor
                new int[] { 2, 4, 5 }, // rm
                new int[] { 0, 0, 0 }, // regen,
                new int[] { 0, 0, 0 } // hp
                );
        }

        public static PassiveEquipmentModel LivingShoes()
        {
            return CreateBoots("living-shoes",
                new int[] { 1, 2, 3 }, // prices,
                new int[] { 1, 2, 3 }, // move speeds,
                new int[] { 1, 1, 2 }, // armor
                new int[] { 1, 1, 2 }, // rm
                new int[] { 1, 2, 4 }, // regen,
                new int[] { 0, 0, 1 } // hp
                );
        }
        #endregion
    }
}
