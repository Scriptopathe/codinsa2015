using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Entities
{

    /// <summary>
    /// Représente le type d'entité.
    /// /!\ Les types d'entités dans cet enum doivent correspondre aux types
    ///     dans l'enum EntityTypeRelative !!
    /// </summary>
    [Clank.ViewCreator.Enum("Contient les différents types d'entités.")]
    public enum EntityType
    {
        Team1 = 0x02,
        Team2 = 0x04,
        Structure = 0x08,
        Tower = 0x10 | Structure,
        Spawner = 0x40 | Structure,
        Datacenter = 0x80 | Structure,

        // Neutral
        Monster = 0x0100,
        Virus = 0x0200,
        WardPlacement = 0x00010000,
        Ward = 0x00020000,
        Shop = 0x00040000,
        // Virus
        MiningFarm = 0x0400 | Monster,
        Router = 0x0800 | Monster,
        Checkpoint = 0x4000,

        // Player
        Player = 0x8000,

        // Others
        HeroSpawner = 0x00080000,

        // Macros
        AllTeam1 = Team1 | Tower | Spawner | Datacenter | Player | Virus,
        AllTeam2 = Team2 | Tower | Spawner | Datacenter | Player | Virus,
        AllObjectives = Tower | Spawner | Datacenter | MiningFarm | Router,
        AllSaved = AllObjectives | Checkpoint | WardPlacement | HeroSpawner | Shop,
        AllTargettableNeutral = MiningFarm | Router | Monster,
        // Team
        Team1Tower = Team1 | Tower,
        Team2Tower = Team2 | Tower,
        Team1Spawner = Team1 | Spawner,
        Team2Spawner = Team2 | Spawner,
        Team1Datacenter = Team1 | Datacenter,
        Team2Datacenter = Team2 | Datacenter,
        Team1Player = Player | Team1,
        Team2Player = Player | Team2,
        Team1Virus = Team1 | Virus,
        Team2Virus = Team2 | Virus,
        Team1Checkpoint = Team1 | Checkpoint,
        Team2Checkpoint = Team2 | Checkpoint,
        Team1HeroSpawner = Team1 | HeroSpawner,
        Team2HeroSpawner = Team2 | HeroSpawner,
        Team1Shop       = Team1 | Shop,
        Team2Shop       = Team2 | Shop,
        Teams = Team1 | Team2,
        
        All = 0xFFFFFF,
    }

    /// <summary>
    /// Représente le type d'entité, relatif à une équipe.
    /// </summary>
    [Clank.ViewCreator.Enum("Contient les différents types d'entités, relatives à une équipe.")]
    public enum EntityTypeRelative
    {
        Me              = 0x01,
        Ally            = 0x02,
        Ennemy          = 0x04,
        Structure       = 0x08,
        Tower           = 0x10 | Structure,
        Spawner         = 0x40 | Structure,
        Datacenter            = 0x80 | Structure,

        // Neutral
        Monster         = 0x0100,
        Virus           = 0x0200,
        WardPlacement   = 0x00010000,
        Ward            = 0x00020000,
        Shop            = 0x00040000,

        // Virus
        MiningFarm            = 0x0400 | Monster,
        Router        = 0x0800 | Monster,
        Checkpoint      = 0x4000,

        // Player
        Player          = 0x8000,

        // Others
        HeroSpawner = 0x00080000,

        // Macros
        AllEnnemy       = Ennemy | Tower | Spawner | Datacenter | Player | Virus,
        AllAlly         = Ally | Tower | Spawner | Datacenter | Player | Virus,
        AllObjectives   = Tower | Spawner | Datacenter | MiningFarm | Router,
        AllSaved        = AllObjectives | Checkpoint | WardPlacement | HeroSpawner,
        AllTargettableNeutral = MiningFarm | Router | Virus,

        // Team
        AllyTower       = Ally | Tower,
        EnnemyTower     = Ennemy | Tower,
        AllySpawner     = Ally | Spawner,
        EnnemySpawner   = Ennemy | Spawner,
        AllyDatacenter        = Ally | Datacenter,
        EnnemyDatacenter      = Ennemy | Datacenter,
        AllyPlayer      = Player | Ally,
        EnnemyPlayer    = Player | Ennemy,
        AllyVirus       = Ally | Virus,
        EnnemyVirus     = Ennemy | Virus,
        AllyCheckpoint  = Ally | Checkpoint,
        EnnemyCheckpoint = Ennemy | Checkpoint,
        AllyHeroSpawner = Ally | HeroSpawner,
        EnnemyHeroSpawner = Ennemy | HeroSpawner,

        All = 0xFFFFFF,
    }
    public static class EntityTypeConverter
    {
        /// <summary>
        /// Transforme les flags Team1 et Team2 en Ally et Ennemy selon l'équipe passée
        /// en paramètre considérée comme l'équipe alliée.
        /// </summary>
        /// <param name="absolute"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        public static EntityTypeRelative ToRelative(EntityType absolute, EntityType team)
        {
            team = team & (EntityType.Team1 | EntityType.Team2);
            if (team != EntityType.Team1 && team != EntityType.Team2)
                return (EntityTypeRelative)(int)((absolute | EntityType.Teams) ^ EntityType.Teams);

            int absInt = (int)absolute;

            // Team opposée.
            EntityType other;
            if ((team & EntityType.Team1) == EntityType.Team1)
                other = EntityType.Team2;
            else
                other = EntityType.Team1;

            if((absolute & team) == EntityType.Team1)
            {
                // Team alliée = team1
                absInt ^= (int)EntityType.Team1;
                absInt |= (int)EntityTypeRelative.Ally;
            }
            else if((absolute & team) == EntityType.Team2)
            {
                // Team alliée = team2
                absInt ^= (int)EntityType.Team2;
                absInt |= (int)EntityTypeRelative.Ally;
            }
            else if((absolute & other) == EntityType.Team1)
            {
                // Team ennemie = team1
                absInt ^= (int)EntityType.Team1;
                absInt |= (int)EntityTypeRelative.Ennemy;
            }
            else if((absolute & other) == EntityType.Team2)
            {
                // Team ennemie = team2
                absInt ^= (int)EntityType.Team2;
                absInt |= (int)EntityTypeRelative.Ennemy;
            }
            else
            {
                // neutre : ne rien changer.
            }
            return (EntityTypeRelative)absInt;
        }

        public static EntityType ToAbsolute(EntityTypeRelative relative, EntityType team)
        {
            team = team & (EntityType.Team1 | EntityType.Team2);
            if (team != EntityType.Team1 && team != EntityType.Team2)
                throw new InvalidOperationException();

            int absInt = (int)relative;

            // Team opposée.
            EntityType other;
            if ((team & EntityType.Team1) == EntityType.Team1)
                other = EntityType.Team2;
            else
                other = EntityType.Team1;

            if ((relative & EntityTypeRelative.Ally) == EntityTypeRelative.Ally)
            {
                // Team alliée = team1
                absInt ^= (int)EntityTypeRelative.Ally;
                absInt |= (int)team;
            }
            else if ((relative & EntityTypeRelative.Ennemy) == EntityTypeRelative.Ennemy)
            {
                // Team ennemie : on remplace ennemy par other.
                absInt ^= (int)EntityTypeRelative.Ennemy;
                absInt |= (int)other;
            }
            else
            {
                // neutre : ne rien changer.
            }

            return (EntityType)absInt;
        }
    }
    /*public enum EntityTypev
    {
        // Macro types
        Structure               = 0x0001,
        Hero                    = 0x0002,
        Monster                 = 0x0004,
        Vision                  = 0x0008,
        Untargettable           = 0x0010,

        // Structures
        HeroSpawner             = 0x0100 | Untargettable,
        Viruspawner            = 0x0200 | Structure,
        Tower                   = 0x0400 | Structure,
        Inhibitor               = 0x0800 | Structure,
        Datacenter                    = 0x1000 | Structure,

        // Monsters
        Virus                   = 0x00010000 | Monster,
        JungleMonster           = 0x00020000 | Monster,
        RouterMonster         = 0x00040000 | Monster,
        BossMonster             = 0x00080000 | Monster,

        // Vision
        WardPlacement           = 0x00100000 | Vision | Untargettable,
        Ward                    = 0x00200000 | Vision,

        // Autres
        Shop                    = 0x00400000 | Untargettable
        Checkpoint              = 0x00800000 | Untargettable
    }*/
}
