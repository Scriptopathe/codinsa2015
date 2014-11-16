using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Entities
{
    /// <summary>
    /// Représente le type d'entité.
    /// </summary>
    public enum EntityType
    {
        Neutral         = 0x01,
        Team1           = 0x02,
        Team2           = 0x04,
        Struture        = 0x08,
        Tower           = 0x10 | Struture,
        Inhibitor       = 0x20 | Struture,
        Spawner         = 0x40 | Struture,
        Idol            = 0x80 | Struture,

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

        // Neutral
        Boss            = 0x100,
        Miniboss        = 0x200,
        Player          = 0x1000,
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
            if (team != EntityType.Team1 || team != EntityType.Team2)
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
    /// <summary>
    /// Représente le type d'entité, relatif à une équipe.
    /// </summary>
    public enum EntityTypeRelative
    {
        Neutral         = 0x01,
        Ally            = 0x02,
        Ennemy          = 0x04,
        Structure       = 0x08,
        Tower           = 0x10 | Structure,
        Inhibitor       = 0x20 | Structure,
        Spawner         = 0x40 | Structure,
        Idol            = 0x80 | Structure,

        // Team
        AllyTower       = Ally   | Tower,
        EnnemyTower     = Ennemy | Tower,
        AllyInhibitor   = Ally   | Inhibitor,
        EnnemyInhibitor = Ennemy | Inhibitor,
        AllySpawner     = Ally   | Spawner,
        EnnemySpawner   = Ennemy | Spawner,
        AllyIdol        = Ally   | Idol,
        EnnemyIdol      = Ennemy | Idol,
        AllyPlayer      = Player | Ally,
        EnnemyPlayer    = Player | Ennemy,

        // Neutral
        Boss            = 0x100,
        Miniboss        = 0x200,
        Player          = 0x1000,
    }
}
