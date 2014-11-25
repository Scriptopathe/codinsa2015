using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Clank.View.Engine;
using Clank.View.Engine.Entities;
namespace Clank.View.Engine
{

    /// <summary>
    /// Système de récompenses : distribue des récompenses aux joueurs en fonction de leurs
    /// actions.
    /// </summary>
    public class RewardSystem
    {
        const float ASSIST_MEMORY = 30;
        class EntityMemoryRemainingTime
        {
            public float RemainingTime {get;set;}
            public EntityMemoryRemainingTime(float time)
            {
                RemainingTime = time;
            }
        }
        /// <summary>
        /// Représente une mémoire contenant des entités pendant un temps déterminé,
        /// et qui supprime ces entités automatiquement une fois ce temps écoulé.
        /// </summary>
        class EntityMemory : Dictionary<EntityBase, EntityMemoryRemainingTime>
        {
            /// <summary>
            /// Mets à jour la mémoire.
            /// </summary>
            public void UpdateMemory(GameTime time)
            {
                
                List<EntityBase> toDelete = new List<EntityBase>();
                foreach(var kvp in this)
                {
                    kvp.Value.RemainingTime = (float)time.ElapsedGameTime.TotalSeconds;
                    if(kvp.Value.RemainingTime < 0)
                        toDelete.Add(kvp.Key);
                }

                foreach(EntityBase del in toDelete) { this.Remove(del); }
            }
        }

        /// <summary>
        /// Représente un dictionnaire qui a chaque héros associe une liste d'entités
        /// l'ayant aidée par des buffs etc;..
        /// </summary>
        Dictionary<EntityHero, EntityMemory> m_assistants;
        /// <summary>
        /// Représente un dictionnaire qui à chaque héros associe une liste d'entités
        /// l'ayant attaquée.
        /// </summary>
        Dictionary<EntityHero, EntityMemory> m_attackers;

        /// <summary>
        /// Représente la liste des tous les héros.
        /// </summary>
        List<EntityHero> m_allHeroes;
        #region Methods

        #region Core
        /// <summary>
        /// Crée une nouvelle instance de RewardSystem.
        /// </summary>
        public RewardSystem(List<EntityHero> heroes)
        {
            m_assistants = new Dictionary<EntityHero, EntityMemory>();
            m_attackers = new Dictionary<EntityHero, EntityMemory>();
            m_allHeroes = heroes;
        }
        /// <summary>
        /// Mets à jour le système de récompenses
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            foreach (var kvp in m_assistants)
                kvp.Value.UpdateMemory(time);

            foreach (var kvp in m_attackers)
                kvp.Value.UpdateMemory(time);

            // Donne les PA/s à tous les héros.
            foreach(EntityHero hero in m_allHeroes)
            {
                hero.PA += Mobattack.GetScene().Constants.Rewards.PAPerSecond * (float)time.ElapsedGameTime.TotalSeconds;
            }
        }

        /// <summary>
        /// Ajoute un assistant au héros donné.
        /// </summary>
        void AddAssistant(EntityHero hero, EntityHero assistant)
        {
            if (!m_assistants.ContainsKey(hero))
                m_assistants.Add(hero, new EntityMemory());

            if (!m_assistants[hero].ContainsKey(assistant))
                m_assistants[hero].Add(assistant, new EntityMemoryRemainingTime(ASSIST_MEMORY));
            else
                m_assistants[hero][assistant].RemainingTime = ASSIST_MEMORY;
        }

        /// <summary>
        /// Ajoute un attaqueur au héros donné.
        /// </summary>
        void AddAttacker(EntityHero hero, EntityHero attacker)
        {
            if (!m_attackers.ContainsKey(hero))
                m_attackers.Add(hero, new EntityMemory());

            if (!m_attackers[hero].ContainsKey(attacker))
                m_attackers[hero].Add(attacker, new EntityMemoryRemainingTime(ASSIST_MEMORY));
            else
                m_attackers[hero][attacker].RemainingTime = ASSIST_MEMORY;
        }
        #endregion


        #region API
        /// <summary>
        /// Indique au système de récompenses que des dégâts ont été infligés à une entité.
        /// </summary>
        /// <param name="source">Entité infligeant les dégâts.</param>
        /// <param name="destination">Entité subissant les dégâts.</param>
        /// <param name="damageAmount">Nombre de dégâts bruts subis par l'entité destination.</param>
        public void NotifyDamageDealt(EntityBase source, EntityBase destination, float damageAmount)
        {
            EntityHero dst = destination as EntityHero;
            EntityHero src = source as EntityHero;
            if (src != null && dst != null)
            {
                AddAttacker(dst, src);
                RewardConstants constants = Mobattack.GetScene().Constants.Rewards;
                // Distribue des PAs au Tank
                if(dst.Role == EntityHeroRole.Tank)
                {
                    // Vérifie qu'il y a des héros alliés en combat
                    EntityCollection inrange = Mobattack.GetMap().Entities.GetAliveEntitiesInRange(dst.Position, constants.TankPAPerHPLostRange);
                    inrange = inrange.GetEntitiesByType((dst.Type & EntityType.Teams) | EntityType.Player);
                    if (inrange.Count != null)
                        dst.PA += damageAmount * constants.TankPAPerHPLost;
                }

                // Distribues des PAs à l'attaquant si c'est un combattant.
                if(src.Role == EntityHeroRole.Fighter)
                {
                    src.PA += damageAmount * constants.FighterPAPerDamageDealt;
                }
            }
        }

        /// <summary>
        /// Indique au système de récompenses qu'une entité a été soignée.
        /// </summary>
        /// <param name="source">Entité ayant donné le heal.</param>
        /// <param name="destination">Entité ayant reçu le heal.</param>
        /// <param name="healAmount">Quantité de PVs soignés.</param>
        public void NotifyHeal(EntityBase source, EntityBase destination, float healAmount)
        {
            EntityHero dst = destination as EntityHero;
            EntityHero src = source as EntityHero;
            if (src != null && dst != null)
            {
                AddAssistant(dst, src);

                // Donne des HP au mage pour le soin, si l'entité soignée est
                // en combat.
                if(src.Role == EntityHeroRole.Mage)
                {
                    if(dst.GetRecentlyAgressiveEntities().Count != 0)
                    {
                        src.PA += healAmount * Mobattack.GetScene().Constants.Rewards.MageAssistBonus;
                    }
                }
            }
        }

        /// <summary>
        /// Indique au système de récompenses qu'un debuff a été infligé à une unité.
        /// </summary>
        /// <param name="source">Unité ayant envoyé le debuff.</param>
        /// <param name="destination">Unité ayant reçu le debuff.</param>
        public void NotifyDebuffReception(EntityBase source, EntityBase destination, StateAlterationModel alteration)
        {
            EntityHero dst = destination as EntityHero;
            EntityHero src = source as EntityHero;
            if (src != null && dst != null)
            {
                AddAttacker(dst, src);
            }
        }
        /// <summary>
        /// Indique au système de récompenses que des PVs d'un shield ont été consommés.
        /// </summary>
        /// <param name="shieldOwner">Entité dont le shield a été partiellement consommé.</param>
        /// <param name="shieldProvider">Entité ayant fourni le shield.</param>
        /// <param name="shieldAmount">Quantité de PVs enlevés au bouclier.</param>
        public void NotifyShieldConsumption(EntityHero shieldOwner, EntityHero shieldProvider, float shieldAmount)
        {
            shieldProvider.PA += shieldAmount * Mobattack.GetScene().Constants.Rewards.PAPerShieldHPConsumed;
        }

        /// <summary>
        /// Indique au système de récompenses qu'une entité a reçu un buff.
        /// </summary>
        /// <param name="source">Entité ayant donné le buff.</param>
        /// <param name="destination">Entité ayant reçu le buff.</param>
        public void NotifyBuffReception(EntityBase source, EntityBase destination, StateAlterationModel alteration)
        {
            EntityHero dst = destination as EntityHero;
            EntityHero src = source as EntityHero;
            if (src != null && dst != null)
            {
                AddAssistant(dst, src);
            }
        }

        /// <summary>
        /// Indique au système de récompenses qu'une entité a reçu un buff ou débuff.
        /// </summary>
        /// <seealso cref="Clank.View.Engine.RewardSystem.NotifyBuffReception"/>
        public void NotifyBuffOrDebuffReception(EntityBase source, EntityBase destination, StateAlterationModel alteration, float amount)
        {
            if (amount < 0)
                NotifyDebuffReception(source, destination, alteration);
            else if(amount > 0)
                NotifyBuffReception(source, destination, alteration);
        }

        /// <summary>
        /// Indique au système de récompenses qu'un héros a été tué.
        /// </summary>
        /// <param name="unit">Unité tuée.</param>
        /// <param name="killer">Unité ayant porté le dernier coup.</param>
        public void NotityHeroDeath(EntityHero unit, EntityHero killer)
        {
            if (killer == null)
                return;

            EntityType killerTeam = killer.Type & EntityType.Teams;

            // On donne une récompense au tueur.
            RewardConstants constants = Mobattack.GetScene().Constants.Rewards;
            killer.PA += constants.KillReward;

            // On mémorise les héros déjà récompensés pour les assist
            List<EntityHero> rewarded = new List<EntityHero>();
            if(m_assistants.ContainsKey(killer))
            {
                foreach(var kvp in m_assistants[killer])
                {
                    EntityHero assistant = kvp.Key as EntityHero;
                    if (assistant != null)
                    {
                        assistant.PA += constants.AssistReward;

                        // Bonus d'assist du combattant.
                        if (assistant.Role == EntityHeroRole.Fighter)
                            assistant.PA += constants.TankAssistBonus;

                        // Bonus d'assist du mage.
                        if (assistant.Role == EntityHeroRole.Mage)
                            assistant.PA += constants.MageAssistBonus;

                        rewarded.Add(assistant);
                    }
                } 
            }

            if (m_attackers.ContainsKey(unit))
            {
                foreach (var kvp in m_attackers[unit])
                {
                    EntityHero attacker = kvp.Key as EntityHero;
                    // On ne récompense l'attaqueur que s'il n'a pas déjà été récompensé
                    // par la prime d'assist.
                    if (attacker != null && !rewarded.Contains(attacker))
                    {
                        attacker.PA += constants.AssistReward;

                        // Bonus du combattant.
                        if (attacker.Role == EntityHeroRole.Fighter)
                            attacker.PA += constants.TankAssistBonus;

                        // Bonus d'assist du mage.
                        if (attacker.Role == EntityHeroRole.Mage)
                            attacker.PA += constants.MageAssistBonus;
                    }
                }
            }
        }

        /// <summary>
        /// Indique au système de récompenses qu'une unité a été tuée.
        /// </summary>
        /// <param name="unit">Unité tuée.</param>
        /// <param name="killer">Unité ayant porté le dernier coup.</param>
        public void NotifyUnitDeath(EntityBase unit, EntityHero killer)
        {
            RewardConstants constants = Mobattack.GetScene().Constants.Rewards;
            // Si l'unité est un creep, on offre une récompense aux héros proches.
            if(unit.Type.HasFlag(EntityType.Creep) || unit.Type.HasFlag(EntityType.Monster))
            {
                foreach(EntityHero hero in m_allHeroes)
                {
                    // Si le héros est dans l'équipe adverse du creep/monstre tué.
                    if(!hero.Type.HasFlag(unit.Type & EntityType.Teams))
                    {
                        if(Vector2.DistanceSquared(hero.Position, unit.Position) <= 
                            constants.CreepDeathRewardRange * constants.CreepDeathRewardRange)
                        {
                            hero.PA += constants.CreepDeathReward;
                        }
                    }
                }
            }
            else if(unit.Type.HasFlag(EntityType.Tower))
            {
                foreach (EntityHero hero in m_allHeroes)
                {
                    // Si le héros est dans l'équipe adverse de la tour tuée
                    // Applique le bonus du combattant lors de la destruction d'une tour.
                    if (!hero.Type.HasFlag(unit.Type & EntityType.Teams)
                        && hero.Role == EntityHeroRole.Tank)
                    {
                        if (Vector2.DistanceSquared(hero.Position, unit.Position) <=
                            constants.TankTowerDestructionBonusRange * constants.TankTowerDestructionBonusRange)
                        {
                            hero.PA += constants.TankTowerDestructionBonus;
                        }
                    }
                }
            }
        }
        #endregion

        #endregion
    }
}
