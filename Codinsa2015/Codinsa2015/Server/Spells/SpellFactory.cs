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
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.MagicDamage,
                        SourcePercentAPValue = cst.ApDamageRatios[MEDIUM],
                        BaseDuration = 0.0f
                    },
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.MoveSpeed,
                        FlatValue = cst.MoveSpeedAlterations[LOWER],
                        BaseDuration = cst.MoveSpeedDurations[LOWER]
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


    }
}
