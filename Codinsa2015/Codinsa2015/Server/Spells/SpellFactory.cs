using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Spells
{
    /// <summary>
    /// Classe chargée de la génération de spells.
    /// </summary>
    public static class SpellFactory
    {
        public const int LOWER = 0;
        public const int LOW = 1;
        public const int MEDIUM = 2;
        public const int HIGH = 3;

        #region Dégâts
        /// <summary>
        /// Spell "magic-beam"
        /// Un projectile autoguidé à courte portée lancé régulièrement
        /// 
        /// </summary>
        public static Spell MagicBeam(EntityBase caster)
        {
            var cst = GameServer.GetScene().Constants.ActiveSpells;
            SpellDescription lvl1 = new SpellDescription()
            {
                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.AllTargettableNeutral |
                        EntityTypeRelative.EnnemyCreep | EntityTypeRelative.EnnemyPlayer,
                    AoeRadius = cst.Aoes[LOW],
                    DieOnCollision = true,
                    Range = cst.Ranges[LOW],
                    Type = TargettingType.Targetted,
                    Duration = cst.Ranges[HIGH] / cst.ProjectileSpeed[HIGH]
                },
                BaseCooldown = cst.CDs[LOWER],
                OnHitEffects = new List<StateAlterationModel>()
                {
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.MagicDamage,
                        SourcePercentAPValue = cst.ApDamageRatios[LOW],
                        BaseDuration = 0.0f
                    },
                }
            };
            SpellDescription lvl2 = lvl1.Copy();
            // Slow mini durée mini
            lvl2.OnHitEffects.Add(new StateAlterationModel()
            {
                Type = StateAlterationType.MoveSpeed,
                FlatValue = -cst.MoveSpeedAlterations[LOWER],
                BaseDuration = cst.ResistAlterationDuration[LOWER]
            });

            SpellDescription lvl3 = lvl2.Copy();
            // buff de dégâts magiques court
            lvl3.CastingTimeAlterations.Add(new StateAlterationModel()
                {
                    Type = StateAlterationType.MagicDamageBuff,
                    FlatValue = cst.FlatADAPBonuses[LOW],
                    BaseDuration = cst.ADAPBonusesDurations[LOW]
                });

            Spell spell = new BasicSpell(caster,
                new List<SpellDescription>() { lvl1, lvl2, lvl3 },
                "magic-beam");

            return spell;
        }

        /// <summary>
        /// Spell laser beam.
        /// Envoie un projectile en ligne droite infligeant des dégâts au premier ennemi touché.
        /// </summary>
        public static Spell LaserBeam(EntityBase caster)
        {
            var cst = GameServer.GetScene().Constants.ActiveSpells;
            SpellDescription lvl1 = new SpellDescription()
            {
                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.AllTargettableNeutral |
                        EntityTypeRelative.EnnemyCreep | EntityTypeRelative.EnnemyPlayer,
                    AoeRadius = cst.Aoes[LOW],
                    DieOnCollision = true,
                    Range = cst.Ranges[HIGH],
                    Type = TargettingType.Direction,
                    Duration = cst.Ranges[HIGH] / cst.ProjectileSpeed[HIGH]
                },
                BaseCooldown = cst.CDs[HIGH],
                OnHitEffects = new List<StateAlterationModel>()
                {
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.MagicDamage,
                        SourcePercentAPValue = cst.ApDamageRatios[HIGH],
                        BaseDuration = 0.0f
                    },
                }
            };
            SpellDescription lvl2 = lvl1.Copy();
            lvl2.OnHitEffects.Add(new StateAlterationModel()
                {
                    Type = StateAlterationType.MagicDamageBuff,
                    FlatValue = cst.MrAlterations[LOW],
                    BaseDuration = cst.ResistAlterationDuration[HIGH]
                });

            SpellDescription lvl3 = lvl2.Copy();
            lvl3.OnHitEffects.Add(new StateAlterationModel()
            {
                Type = StateAlterationType.Blind,
                BaseDuration = cst.BlindDurations[MEDIUM],
            });

            Spell spell = new BasicSpell(caster,
                new List<SpellDescription>() { lvl1, lvl2, lvl3 },
                "laser-beam");

            return spell;
        }

        /// <summary>
        /// Spell météore.
        /// Inflige des dégâts en AOE via un gros caillou qui tombe du ciel.
        /// </summary>
        public static Spell Meteor(EntityBase caster)
        {
            var cst = GameServer.GetScene().Constants.ActiveSpells;
            SpellDescription lvl1 = new SpellDescription()
            {
                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.AllTargettableNeutral |
                        EntityTypeRelative.EnnemyCreep | EntityTypeRelative.EnnemyPlayer,
                    AoeRadius = cst.Aoes[MEDIUM],
                    DieOnCollision = false,
                    Range = cst.Ranges[MEDIUM],
                    Type = TargettingType.Position,
                    Duration = cst.AoeDurations[LOW]
                },
                BaseCooldown = cst.CDs[MEDIUM],
                OnHitEffects = new List<StateAlterationModel>()
                {
                    // medium damage
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.MagicDamage,
                        SourcePercentAPValue = cst.ApDamageRatios[MEDIUM],
                        BaseDuration = 0.0f
                    },
                    // slow
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.MoveSpeed,
                        FlatValue = -cst.MoveSpeedAlterations[LOWER],
                        BaseDuration = cst.MoveSpeedDurations[LOW]
                    }
                }
            };
            SpellDescription lvl2 = lvl1.Copy();
            lvl2.OnHitEffects[0].FlatValue = cst.ApDamageFlat[MEDIUM];

            SpellDescription lvl3 = lvl2.Copy();
            lvl3.OnHitEffects[1] = new StateAlterationModel()
            {
                Type = StateAlterationType.Stun,
                BaseDuration = cst.StunDurations[LOW],
            };

            Spell spell = new BasicSpell(caster,
                new List<SpellDescription>() {lvl1, lvl2, lvl3},
                "meteor");

            return spell;
        }

        #endregion

        #region Supporting
        /// <summary>
        /// Sort "hold-on".
        /// Soigne légérement un allié, et augmente son armure pendant un court moment.
        /// </summary>
        public static Spell HoldOn(EntityBase caster)
        {
            var cst = GameServer.GetScene().Constants.ActiveSpells;
            SpellDescription lvl1 = new SpellDescription()
            {
                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.AllyPlayer,
                    AoeRadius = cst.Aoes[LOWER],
                    DieOnCollision = true,
                    Range = cst.Ranges[LOW],
                    Type = TargettingType.Targetted,
                    Duration = 0
                },
                BaseCooldown = cst.CDs[MEDIUM],
                OnHitEffects = new List<StateAlterationModel>()
                {
                    // medium heal
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.Heal,
                        SourcePercentAPValue = cst.HealApRatio[MEDIUM],
                        BaseDuration = 0.0f
                    },
                }
            };
            SpellDescription lvl2 = lvl1.Copy();
            // Bonus d'armure moyen durée faible
            lvl2.OnHitEffects.Add(new StateAlterationModel()
                {
                    Type = StateAlterationType.ArmorBuff,
                    FlatValue = cst.ArmorAlterations[MEDIUM],
                    BaseDuration = cst.ResistAlterationDuration[LOW]
                });

            SpellDescription lvl3 = lvl2.Copy();
            // Bonus de dégâts moyens durée faible
            lvl3.OnHitEffects.Add(new StateAlterationModel()
            {
                Type = StateAlterationType.AttackDamageBuff,
                FlatValue = cst.FlatADAPBonuses[MEDIUM],
                BaseDuration = cst.ADAPBonusesDurations[LOW],
            });

            Spell spell = new BasicSpell(caster,
                new List<SpellDescription>() { lvl1, lvl2, lvl3 },
                "hold on!");

            return spell;
        }

        /// <summary>
        /// Sort "go".
        /// Accélère les alliés proches (et confifère une immunité courte au CC au lvl 3).
        /// </summary>
        public static Spell Go(EntityBase caster)
        {
            var cst = GameServer.GetScene().Constants.ActiveSpells;
            SpellDescription lvl1 = new SpellDescription()
            {
                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.AllAlly,
                    AoeRadius = cst.Aoes[MEDIUM],
                    DieOnCollision = false,
                    Range = cst.Ranges[LOWER],
                    Type = TargettingType.Position,
                    Duration = cst.AoeDurations[LOWER]
                },
                BaseCooldown = cst.CDs[MEDIUM],
                OnHitEffects = new List<StateAlterationModel>()
                {
                    // bonus de MS faible, durée moyenne
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.MoveSpeed,
                        FlatValue = cst.MoveSpeedAlterations[LOW],
                        BaseDuration = cst.MoveSpeedDurations[MEDIUM],
                    },
                }
            };
            SpellDescription lvl2 = lvl1.Copy();
            // bonus de MS moyen, durée moyenne
            lvl2.OnHitEffects[0].FlatValue = cst.MoveSpeedAlterations[MEDIUM];

            SpellDescription lvl3 = lvl2.Copy();
            // Bonus de dégâts moyens durée faible
            lvl3.OnHitEffects.Add(new StateAlterationModel()
            {
                Type = StateAlterationType.ControlImmune,
                BaseDuration = cst.StunDurations[LOWER],
            });

            Spell spell = new BasicSpell(caster,
                new List<SpellDescription>() { lvl1, lvl2, lvl3 },
                "go!");

            return spell;
        }

        /// <summary>
        /// Sort bro-force.
        /// Dashe vers un allié et lui octroie un bouclier pendant un cours instant.
        /// </summary>
        public static Spell BroForce(EntityBase caster)
        {
            var cst = GameServer.GetScene().Constants.ActiveSpells;
            SpellDescription lvl1 = new SpellDescription()
            {
                CastingTime = 0.2f,
                CastingTimeAlterations = new List<StateAlterationModel>()
                {
                    // Dash
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.Dash,
                        DashDirType = DashDirectionType.TowardsEntity,
                        DashSpeed = 40f,
                    }
                },
                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.AllAlly,
                    AoeRadius = cst.Aoes[LOWER],
                    DieOnCollision = false,
                    Range = cst.Ranges[MEDIUM],
                    Type = TargettingType.Targetted,
                    Duration = 0
                },
                BaseCooldown = cst.CDs[HIGH],
                OnHitEffects = new List<StateAlterationModel>()
                {
                    // bouclier faible, durée moyenne
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.Shield,
                        SourcePercentAPValue = cst.ShieldApRatio[LOW],
                        BaseDuration = cst.ShieldDuration[MEDIUM],
                    },
                }
            };
            SpellDescription lvl2 = lvl1.Copy();
            // bonus de MS moyen, durée moyenne
            lvl2.OnHitEffects[0].SourcePercentAPValue = cst.ShieldApRatio[MEDIUM];
            lvl2.OnHitEffects.Add(new StateAlterationModel()
            {
                // bonus d'armure moyen, durée moyenne
                Type = StateAlterationType.ArmorBuff,
                FlatValue = cst.ArmorAlterations[MEDIUM],
                BaseDuration = cst.ShieldDuration[MEDIUM] // même durée que le shield
            });
            SpellDescription lvl3 = lvl2.Copy();
            lvl3.OnHitEffects[0].SourcePercentAPValue = cst.ShieldApRatio[HIGH];
            // Bouclier grand, bonus move speed moyen court.
            lvl3.OnHitEffects.Add(new StateAlterationModel()
            {
                Type = StateAlterationType.MoveSpeed,
                FlatValue = cst.MoveSpeedAlterations[MEDIUM],
                BaseDuration = cst.MoveSpeedDurations[LOW]
            });

            Spell spell = new BasicSpell(caster,
                new List<SpellDescription>() { lvl1, lvl2, lvl3 },
                "bro-force");

            return spell;
        }


        /// <summary>
        /// Sort war-cry.
        /// Augmente les dégâts des alliés pendant un cours instant.
        /// </summary>
        public static Spell WarCry(EntityBase caster)
        {
            var cst = GameServer.GetScene().Constants.ActiveSpells;
            SpellDescription lvl1 = new SpellDescription()
            {

                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.AllyPlayer,
                    AoeRadius = cst.Aoes[MEDIUM],
                    DieOnCollision = false,
                    Range = cst.Ranges[MEDIUM],
                    Type = TargettingType.Position,
                    Duration = cst.AoeDurations[LOW]
                },
                BaseCooldown = cst.CDs[HIGH],
                OnHitEffects = new List<StateAlterationModel>()
                {
                    // bonus d'ad moyen
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.AttackDamageBuff,
                        FlatValue = cst.AdDamageFlat[MEDIUM],
                        BaseDuration = cst.ADAPBonusesDurations[LOW],
                    },                    
                    // bonus d'ap moyen
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.MagicDamageBuff,
                        FlatValue = cst.ApDamageFlat[MEDIUM],
                        BaseDuration = cst.ADAPBonusesDurations[LOW],
                    },
                }
            };
            SpellDescription lvl2 = lvl1.Copy();
            // bonus de d'ad et ap forts
            lvl2.OnHitEffects[0].FlatValue = cst.AdDamageFlat[HIGH];
            lvl2.OnHitEffects[1].FlatValue = cst.ApDamageFlat[HIGH];

            SpellDescription lvl3 = lvl2.Copy();

            // bonus move speed moyen court.
            lvl3.OnHitEffects.Add(new StateAlterationModel()
            {
                Type = StateAlterationType.MoveSpeed,
                FlatValue = cst.MoveSpeedAlterations[MEDIUM],
                BaseDuration = cst.MoveSpeedDurations[LOW]
            });
            // bonus attack speed moyen court.
            lvl3.OnHitEffects.Add(new StateAlterationModel()
            {
                Type = StateAlterationType.AttackSpeed,
                FlatValue = cst.AttackSpeedBonuses[MEDIUM],
                BaseDuration = cst.AttackSpeedBonusesDurations[LOW]
            });
            Spell spell = new BasicSpell(caster,
                new List<SpellDescription>() { lvl1, lvl2, lvl3 },
                "bro-force");

            return spell;
        }
        #endregion

        #region Spells de CC
        /// <summary>
        /// Sort bim!.
        /// Stunne un ennemi pendant une courte durée.
        /// </summary>
        public static Spell Bim(EntityBase caster)
        {
            var cst = GameServer.GetScene().Constants.ActiveSpells;
            SpellDescription lvl1 = new SpellDescription()
            {

                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.EnnemyPlayer |
                            EntityTypeRelative.EnnemyCreep | EntityTypeRelative.AllTargettableNeutral,
                    AoeRadius = cst.Aoes[LOWER],
                    DieOnCollision = false,
                    Range = cst.Ranges[MEDIUM],
                    Type = TargettingType.Direction,
                    Duration = 0
                },
                BaseCooldown = cst.CDs[HIGH],
                OnHitEffects = new List<StateAlterationModel>()
                {
                    // stun durée moyenne
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.Stun,
                        BaseDuration = cst.StunDurations[MEDIUM],
                    },
                }
            };
            SpellDescription lvl2 = lvl1.Copy();
            // dégâts de sorts faibles
            lvl2.OnHitEffects.Add(new StateAlterationModel()
            {
                Type = StateAlterationType.MagicDamage,
                BaseDuration = 0,
                FlatValue = cst.ApDamageFlat[LOWER],
                SourcePercentAPValue = cst.ApDamageRatios[LOW]
            });

            SpellDescription lvl3 = lvl2.Copy();
            lvl3.TargetType.DieOnCollision = false;

            Spell spell = new BasicSpell(caster,
                new List<SpellDescription>() { lvl1, lvl2, lvl3 },
                "bim!");

            return spell;
        }
        /// <summary>
        /// Sort Kick !
        /// Projette un ennemi en arrière.
        /// </summary>
        public static Spell Kick(EntityBase caster)
        {
            var cst = GameServer.GetScene().Constants.ActiveSpells;
            SpellDescription lvl1 = new SpellDescription()
            {

                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.EnnemyPlayer |
                                         EntityTypeRelative.EnnemyCreep,
                    AoeRadius = cst.Aoes[LOWER],
                    DieOnCollision = false,
                    Range = cst.Ranges[LOWER],
                    Type = TargettingType.Targetted,
                    Duration = 0
                },
                BaseCooldown = cst.CDs[HIGH],
                OnHitEffects = new List<StateAlterationModel>()
                {
                    // Kick en arrière
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.Dash,
                        DashDirType = DashDirectionType.BackwardsCaster,
                        DashSpeed = 40,
                        BaseDuration = 0.15f,
                    },
                    // Dégâts moyens
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.MagicDamage,
                        SourcePercentAPValue = cst.ApDamageRatios[MEDIUM]
                    }
                }
            };
            SpellDescription lvl2 = lvl1.Copy();
            // Ralentissement grand, durée faible.
            lvl2.OnHitEffects.Add(new StateAlterationModel()
            {
                Type = StateAlterationType.MoveSpeed,
                BaseDuration = cst.MoveSpeedDurations[LOW],
                FlatValue = -cst.MoveSpeedAlterations[HIGH],
            });

            SpellDescription lvl3 = lvl2.Copy();
            // Baisse d'armure moyenne, durée moyenne.
            lvl3.OnHitEffects.Add(new StateAlterationModel()
            {
                Type = StateAlterationType.ArmorBuff,
                BaseDuration = cst.ResistAlterationDuration[MEDIUM],
                FlatValue = -cst.ArmorAlterations[MEDIUM]
            });

            Spell spell = new BasicSpell(caster,
                new List<SpellDescription>() { lvl1, lvl2, lvl3 },
                "kick");

            return spell;
        }
        #endregion
    }
}
