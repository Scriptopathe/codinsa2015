using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Entities
{
    /// <summary>
    /// Représente le type d'entité.
    /// /!\ Les types d'entités dans cet enum doivent correspondre aux types
    ///     dans l'enum EntityTypeRelative !!
    /// </summary>
    public enum EntityType
    {
        Monster         = 0x01,
        Team1           = 0x02,
        Team2           = 0x04,
        Struture        = 0x08,
        Tower           = 0x10 | Struture,
        Inhibitor       = 0x20 | Struture,
        Spawner         = 0x40 | Struture,
        Idol            = 0x80 | Struture,

        // Neutral
        Boss            = 0x0100 | Monster,
        Miniboss        = 0x0200 | Monster,
        WardPlacement   = 0x00010000,
        Ward            = 0x00020000,

        // Creeps
        Creep           = 0x0400,
        Checkpoint      = 0x4000,

        // Player
        Player          = 0x8000,

        // Macros
        AllTeam1    = Team1 | Tower | Inhibitor | Spawner | Idol | Player | Creep,
        AllTeam2    = Team2 | Tower | Inhibitor | Spawner | Idol | Player | Creep,
        AllObjectives   = Tower | Inhibitor | Spawner | Idol | Boss | Miniboss,
        AllSaved        = AllObjectives | Checkpoint,
        
        // Team
        Team1Tower      = Team1 | Tower,
        Team2Tower      = Team2 | Tower,
        Team1Inhibitor  = Team1 | Inhibitor,
        Team2Inhibitor  = Team2 | Inhibitor,
        Team1Spawner    = Team1 | Spawner,
        Team2Spawner    = Team2 | Spawner,
        Team1Idol       = Team1 | Idol,
        Team2Idol       = Team2 | Idol,
        Team1Player     = Player | Team1,
        Team2Player     = Player | Team2,
        Team1Creep      = Team1 | Creep,
        Team2Creep      = Team2 | Creep,
        Team1Checkpoint = Team1 | Checkpoint,
        Team2CheckPoint = Team2 | Checkpoint
    }

    /// <summary>
    /// Représente le type d'entité, relatif à une équipe.
    /// </summary>
    public enum EntityTypeRelative
    {
        Monster         = 0x01,
        Ally            = 0x02,
        Ennemy          = 0x04,
        Structure       = 0x08,
        Tower           = 0x10 | Structure,
        Inhibitor       = 0x20 | Structure,
        Spawner         = 0x40 | Structure,
        Idol            = 0x80 | Structure,

        // Neutral
        Boss            = 0x0100 | Monster,
        Miniboss        = 0x0200 | Monster,
        
        // Creeps
        Creep           = 0x0400,
        Checkpoint      = 0x4000,
        Player          = 0x8000,

        // Wards
        WardPlacement   = 0x00010000,
        Ward            = 0x00020000,

        // Macros
        AllEnnemy       = Ennemy | Tower | Inhibitor | Spawner | Idol | Player | Creep,
        AllAlly         = Ally | Tower | Inhibitor | Spawner | Idol | Player | Creep,

        // Team
        AllyTower       = Ally | Tower,
        EnnemyTower     = Ennemy | Tower,
        AllyInhibitor   = Ally | Inhibitor,
        EnnemyInhibitor = Ennemy | Inhibitor,
        AllySpawner     = Ally | Spawner,
        EnnemySpawner   = Ennemy | Spawner,
        AllyIdol        = Ally | Idol,
        EnnemyIdol      = Ennemy | Idol,
        AllyPlayer      = Player | Ally,
        EnnemyPlayer    = Player | Ennemy,
        AllyCreep       = Ally | Creep,
        EnnemyCreep     = Ennemy | Creep,
        AllyCheckpoint  = Ally | Checkpoint,
        EnnemyCheckpoint = Ennemy | Checkpoint,

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
                throw new InvalidOperationException();

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

}
