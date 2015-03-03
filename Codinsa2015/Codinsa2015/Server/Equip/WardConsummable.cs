using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Equip
{
    /// <summary>
    /// Représente un consommable de type ward
    /// </summary>
    public class WardConsummable : Consummable
    {

        /// <summary>
        /// Place la ward sur l'emplacement à ward le plus proche du héros.
        /// </summary>
        public override ConsummableUseResult Use(EntityHero owner)
        {
            EntityWardPlacement nearest = GameServer.GetMap().Entities.
                                GetEntitiesByType(EntityType.WardPlacement).
                                GetAliveEntitiesInRange(owner.Position, GameServer.GetScene().Constants.Vision.WardPutRange, 0).
                                NearestFrom(owner.Position) as EntityWardPlacement;

            // Pose la ward.
            if(nearest != null)
            {
                nearest.PutWard(owner);
                UsingStarted = true;
                return  ConsummableUseResult.SuccessAndDestroyed;
            }

            return ConsummableUseResult.Fail;
        }
    }
}
