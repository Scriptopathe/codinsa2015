using System;
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
    /// Constantes concernant les évènements.
    /// </summary>
    public class EventConstants
    {
        public MonsterCampEventConstants MonsterCamp;
        public MinibossesEventConstants MinibossCamp;
        public EventConstants()
        {
            MonsterCamp = new MonsterCampEventConstants();
            MinibossCamp = new MinibossesEventConstants();
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
            VisionRange = 8;
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
            VisionRange = 8.0f;
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

        public CampMonsterConstants() 
        {
            MaxMoveDistance = 5;
        }
    }

    public class MinibossConstants : EntityConstants
    {        
        /// <summary>
        /// Distance maximale à laquelle cette unité peut s'éloigner de GuardPosition.
        /// </summary>
        public float MaxMoveDistance;
        public MinibossConstants()
        {
            MaxMoveDistance = 5;
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
        public RoleConstants Roles;
        public RewardConstants Rewards;
        public CampMonsterConstants CampMonsters;
        public MinibossConstants Minibosses;
        public EventConstants Events;
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
