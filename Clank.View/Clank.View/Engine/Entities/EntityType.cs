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
        Team2           = 0x03,
        Tower           = 0x10,
        Inhibitor       = 0x20,
        Spawner         = 0x40,
        Idol            = 0x80,

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
}
