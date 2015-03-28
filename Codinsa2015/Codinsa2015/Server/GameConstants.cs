﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Codinsa2015.Server
{
    /// <summary>
    /// Représente les constantes de vision du jeu.
    /// </summary>
    public class VisionConstants
    {
        public float WardRange;
        public float WardPutRange;
        
        public float WardDuration;
        public float WardRevealDuration;
        public int MaxWardsPerHero;
        public VisionConstants()
        {
            WardRange = 5.0f;
            WardDuration = 30.0f;
            MaxWardsPerHero = 5;
            WardRevealDuration = 10;
            WardPutRange = 5.0f;
        }
    }

    /// <summary>
    /// Constantes des passifs uniques.
    /// </summary>
    public class UniquePassiveConstants
    {
        public float HunterMonsterArmorDebuff = 10;
        public float HunterMonsterMRDebuff = 10;
        /// <summary>
        /// Bonus de regen du hunter en présence de monstres neutres (lvl 1).
        /// </summary>
        public float HunterBonusRegen = 2;
        /// <summary>
        /// Bonus du hunter lorsqu'il tue un monstre neutre. (lvl 2)
        /// </summary>
        public float HunterBonusGold = 10;
        /// <summary>
        /// Bonus de Armure du Hunter en présence de monstre (lvl 3)
        /// </summary>
        public float HunterBonusArmor = 50;
        /// <summary>
        /// Bonus de MR du Hunter en présence de monstre (lvl 3)
        /// </summary>
        public float HunterBonusMR = 50;
        /// <summary>
        /// Range à partir de laquelle les bonus du hunter sont actifs.
        /// </summary>
        public float HunterActivationRange = 3;
    }
    /// <summary>
    /// Constantes de récompenses.
    /// </summary>
    public class RewardConstants
    {
        public float KillReward = 100;
        public float AssistReward = 100;
        public float PAPerSecond = 1;
        public float PAPerShieldHPConsumed = 0.1f;

        public float TankPAPerHPLost = 0.1f;
        public float TankPAPerHPLostRange = 8;
        public float TankAssistBonus = 50;
        public float TankTowerDestructionBonus = 50;
        public float TankTowerDestructionBonusRange = 5f;

        public float MageAssistBonus = 50;
        public float MagePAPerHPHealed = 0.2f;
        public float MagePAPerDamageDealt = 0.2f;

        public float FighterPAPerDamageDealt = 0.2f;

        public float CreepDeathRewardRange = 5f;
        public float CreepDeathReward = 4f;
    }

    /// <summary>
    /// Constantes pour les camps de monstres.
    /// </summary>
    public class MonsterCampEventConstants
    {
        /// <summary>
        /// Temps de respawn en secondes du camp.
        /// </summary>
        public float RespawnTimer;
        /// <summary>
        /// Intervalle en secondes entre 2 spawn de creeps par le camp
        /// lorsqu'il est possédé.
        /// </summary>
        public float CreepSpawnInterval;
        /// <summary>
        /// Montant en PA de la récompense attribuée au tueur du camp.
        /// </summary>
        public float Reward;
        public MonsterCampEventConstants()
        {
            RespawnTimer = 10;
            CreepSpawnInterval = 5;
            Reward = 30;
        }
    }

    /// <summary>
    /// Constantes concernant les mini-boss.
    /// </summary>
    public class MinibossesEventConstants
    {
        /// <summary>
        /// Range de la vision accordée à l'équipe tuant le mini-boss.
        /// </summary>
        public float VisionRange;
        /// <summary>
        /// Temps de respawn en secondes du mini-boss après sa mort.
        /// </summary>
        public float RespawnTimer;
        /// <summary>
        /// Montant en PA de la récompense attribuée au tueur du camp.
        /// </summary>
        public float Reward;
        public MinibossesEventConstants()
        {
            VisionRange = 8;
            RespawnTimer = 5;
            Reward = 30;
        }
    }
    /// <summary>
    /// Constantes concernant le big boss.
    /// </summary>
    public class BigBossEventConstants
    {
        /// <summary>
        /// Temps de respawn en secondes du boss après sa mort.
        /// </summary>
        public float RespawnTimer;
        /// <summary>
        /// Montant en PA de la récompense attribuée au tueur du camp.
        /// </summary>
        public float Reward;
        /// <summary>
        /// Durée pendant laquelle le buff sur les creeps est actif.
        /// </summary>
        public float BuffDuration;
        public BigBossEventConstants()
        {
            RespawnTimer = 30;
            Reward = 50;
            BuffDuration = 25;
        }
    }
    /// <summary>
    /// Constantes concernant les évènements.
    /// </summary>
    public class EventConstants
    {
        public MonsterCampEventConstants MonsterCamp;
        public MinibossesEventConstants MinibossCamp;
        public BigBossEventConstants BigBossCamp;
        public EventConstants()
        {
            MonsterCamp = new MonsterCampEventConstants();
            MinibossCamp = new MinibossesEventConstants();
            BigBossCamp = new BigBossEventConstants();
        }
    }

    /// <summary>
    /// Constantes pour les tours.
    /// </summary>
    public class TowerConstants : EntityConstants
    {
        public float AttackRange;
        public TowerConstants()
        {
            HP = 100;
            Armor = 100;
            MagicResist = 200;
            AttackSpeed = 1.5f;
            AttackDamage = 90;
            AttackRange = 6f;
        }
    }

    /// <summary>
    /// Constantes pour les inhibs.
    /// </summary>
    public class InhibitorConstants : EntityConstants
    {
        public InhibitorConstants()
        {
            HP = 5000;
        }

    }

    /// <summary>
    /// Constante pour les spawners.
    /// </summary>
    public class SpawnerConstants : EntityConstants
    {
        public float CreepsPerWave;
        public float WavesInterval;
        public float Rows;

        public SpawnerConstants() : base()
        {
            CreepsPerWave = 6.0f;
            WavesInterval = 30.0f;
            Rows = 3;
        }
    }

    /// <summary>
    /// Contient les constantes concernant les échoppes.
    /// </summary>
    public class ShopConstants
    {
        public float DefaultBuyRange;
        public float SellingPriceFactor;
        public ShopConstants()
        {
            DefaultBuyRange = 4.0f;
            SellingPriceFactor = 0.9f;
        }
    }

    /// <summary>
    /// Contient les constantes concernant les structures.
    /// </summary>
    public class StructureConstants
    {
        public TowerConstants Towers;
        public InhibitorConstants Inhibs;
        public SpawnerConstants Spawners;
        public ShopConstants Shops;
        public StructureConstants()
        {
            Towers = new TowerConstants();
            Inhibs = new InhibitorConstants();
            Spawners = new SpawnerConstants();
            Shops = new ShopConstants();
        }
    }

    /// <summary>
    /// Constantes concernant toutes les entités
    /// </summary>
    public class EntityConstants
    {
        public float HP;
        public float HPRegen;
        public float AttackDamage;
        public float AbilityPower;
        public float Armor;
        public float MagicResist;
        public float AttackSpeed;
        public float MoveSpeed;
        public float VisionRange;
        public float CooldownReduction;
        public EntityConstants()
        {
            HPRegen = 1;
            VisionRange = 8;
            MoveSpeed = 4;
            HP = 100;
            Armor = 10;
            MagicResist = 10;
            CooldownReduction = 0;
            AttackDamage = 10;
            AttackSpeed = 1.0f;
        }
    }

    /// <summary>
    /// Constantes des creeps.
    /// </summary>
    public class CreepConstants : EntityConstants
    {
        public float AttackRange;

        public CreepConstants() : base()
        {
            HP = 100;
            VisionRange = 4.0f;
            MoveSpeed = 4.0f;
            AttackSpeed = 1.0f;
            AttackDamage = 40;
            AttackRange = 5f;
            Armor = 10f;
        }
    }

    public class CampMonsterConstants : EntityConstants
    {
        /// <summary>
        /// Distance maximale à laquelle cette unité peut s'éloigner de GuardPosition.
        /// </summary>
        public float MaxMoveDistance;
        public float AttackRange;
        public CampMonsterConstants() 
        {
            MoveSpeed = 4;
            MaxMoveDistance = 5;
            AttackSpeed = 0.75f;
            AttackDamage = 30;
            VisionRange = 9;
            AttackRange = 4;
            HP = 300;
        }

    }

    public class MinibossConstants : EntityConstants
    {        
        /// <summary>
        /// Distance maximale à laquelle cette unité peut s'éloigner de GuardPosition.
        /// </summary>
        public float MaxMoveDistance;
        public float AttackRange;
        public MinibossConstants()
        {
            MaxMoveDistance = 5;
            AttackSpeed = 0.75f;
            AttackDamage = 30;
            VisionRange = 8;
            AttackRange = 4;
            HP = 100;
        }
    }

    public class RoleConstants
    {
        // Fighter
        public float FighterAttackSpeedMultiplier = 2;
        public float FighterAttackDamageMultiplier = 2;
        public float FighterTrueDamageMultiplier = 2;
        public float FighterMagicDamageMultiplier = 2;
        public float FighterStealthDurationMultiplier = 2;
        
        // Mage
        public float MageHealValueMultiplier = 2;
        public float MageShieldValueMultiplier = 2;
        public float MageSilenceDurationMultiplier = 2;
        public float MageSightDurationMultiplier = 2;

        // Tank
        public float TankMoveSpeedBonusMultiplier = 2;
        public float TankArmorBonusMultiplier = 2;
        public float TankRMBonusMultiplier = 2;
        public float TankStunDurationMultiplier = 2;

        public RoleConstants()
        {

        }
    }
    /// <summary>
    /// Représente toutes les constantes du jeu.
    /// Elles sont hierarchisées dans d'autres objets, afin que le fichier
    /// xml sérialisé soit plus clair à lire / écrire à la main.
    /// </summary>
    public class GameConstants
    {
        public VisionConstants Vision;
        public StructureConstants Structures;
        public CreepConstants Creeps;
        public CreepConstants BuffedCreeps;
        public RoleConstants Roles;
        public RewardConstants Rewards;
        public CampMonsterConstants CampMonsters;
        public MinibossConstants Minibosses;
        public EventConstants Events;
        public UniquePassiveConstants UniquePassives;
        /// <summary>
        /// Crée une nouvelle instance de GameConstants avec des constantes par défaut.
        /// </summary>
        public GameConstants()
        {
            Vision = new VisionConstants();
            Structures = new StructureConstants();
            Creeps = new CreepConstants();
            Roles = new RoleConstants();
            Rewards = new RewardConstants();
            Events = new EventConstants();
            CampMonsters = new CampMonsterConstants();
            Minibosses = new MinibossConstants();
            BuffedCreeps = new CreepConstants();
            UniquePassives = new UniquePassiveConstants();
        }

        /// <summary>
        /// Sauvegarde les constantes dans le fichier donné.
        /// </summary>
        public void Save(string path)
        {
            System.IO.File.WriteAllText(path, Tools.Serializer.Serialize<GameConstants>(this));
        }

        /// <summary>
        /// Charge les constantes depuis le fichier donné.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static GameConstants LoadFromFile(string path)
        {
            return Tools.Serializer.Deserialize<GameConstants>(System.IO.File.ReadAllText(path));
        }
    }
}
