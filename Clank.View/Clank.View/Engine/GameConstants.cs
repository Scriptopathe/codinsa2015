using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Clank.View.Engine
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
    /// Contient les constantes concernant les structures.
    /// </summary>
    public class StructureConstants
    {
        public TowerConstants Towers;
        public InhibitorConstants Inhibs;
        public SpawnerConstants Spawners;
        public StructureConstants()
        {
            Towers = new TowerConstants();
            Inhibs = new InhibitorConstants();
            Spawners = new SpawnerConstants();
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
            VisionRange = 5.0f;
            MoveSpeed = 4.0f;
            AttackSpeed = 1.0f;
            AttackDamage = 40;
            AttackRange = 5f;
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
        }

        /// <summary>
        /// Sauvegarde les constantes dans le fichier donné.
        /// </summary>
        public void Save(string path)
        {
            Tools.Serializer.Serialize<GameConstants>(this, path);
        }

        /// <summary>
        /// Charge les constantes depuis le fichier donné.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static GameConstants LoadFromFile(string path)
        {
            return Tools.Serializer.Deserialize<GameConstants>(path);
        }
    }
}
