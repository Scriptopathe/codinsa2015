using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
using Codinsa2015.Server.Equip;
namespace Codinsa2015.Server.Entities
{
    public class BootsFactory
    {
        public static float GetValue(float[] cst, int v)
        {
            return Math.Sign(v) * (v == 0 ? 0 : cst[Math.Abs(v) - 1]);
        }

        public static PassiveEquipmentModel Create(string name, 
            int[] prices,
            int[] moveSpeeds, int[] armor, int[] rm, int[] regen, int[] hp)
        {
            var cst = GameServer.GetScene().Constants.Equip;
            PassiveEquipmentModel b = new PassiveEquipmentModel();
            b.Name = name;
            b.Type = EquipmentType.Boots;
            for(int i  = 0; i < 3; i++)
            {
                b.Upgrades.Add(new PassiveEquipmentUpgradeModel()
                {
                    Cost = GetValue(cst.BootsPrices, prices[i]),

                    PassiveAlterations = new List<StateAlterationModel>()
                    {
                        new StateAlterationModel()
                        {
                            Type = StateAlterationType.MoveSpeed,
                            FlatValue = GetValue(cst.BootsMoveSpeed, moveSpeeds[i])
                        },
                        new StateAlterationModel()
                        {
                            Type = StateAlterationType.ArmorBuff,
                            FlatValue = GetValue(cst.BootsArmor, armor[i])
                        },
                        new StateAlterationModel()
                        {
                            Type = StateAlterationType.MagicResistBuff,
                            FlatValue = GetValue(cst.BootsRM, rm[i])
                        },
                        new StateAlterationModel()
                        {
                            Type = StateAlterationType.Regen,
                            FlatValue = GetValue(cst.BootsRegen, regen[i])
                        },
                        new StateAlterationModel()
                        {
                            Type = StateAlterationType.MaxHP,
                            FlatValue = GetValue(cst.BootsHP, hp[i])
                        },
                    },
                        
                });
            }

            return b;
        }

        public static PassiveEquipmentModel LightShoes()
        {
            return Create("light-shoes",
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
            return Create("rangers",
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
            return Create("white-shoes",
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
            return Create("steel-shoes",
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
            return Create("living-shoes",
                new int[] { 1, 2, 3 }, // prices,
                new int[] { 1, 2, 3 }, // move speeds,
                new int[] { 1, 1, 2 }, // armor
                new int[] { 1, 1, 2 }, // rm
                new int[] { 1, 2, 4 }, // regen,
                new int[] { 0, 0, 1 } // hp
                );
        }
    }
}
