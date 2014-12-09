using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clank.View.Engine.Entities;
namespace Clank.View.Engine.Equip
{
    /// <summary>
    /// Représente un consommable de type ward
    /// </summary>
    public class WardConsummable : Consummable
    {
        /// <summary>
        /// Obtient le type de consommable de cette ward.
        /// </summary>
        public override ConsummableType Type
        {
            get { return ConsummableType.Ward; }
        }

        /// <summary>
        /// Place la ward sur l'emplacement à ward le plus proche du héros.
        /// </summary>
        public override bool Use(EntityHero owner)
        {
            EntityWardPlacement nearest = Mobattack.GetMap().Entities.
                                GetEntitiesByType(EntityType.WardPlacement).
                                GetAliveEntitiesInRange(owner.Position, Mobattack.GetScene().Constants.Vision.WardPutRange, owner.Type & EntityType.Teams).
                                NearestFrom(owner.Position) as EntityWardPlacement;

            // Pose la ward.
            if(nearest != null )
            {
                nearest.PutWard(owner);
                return true;
            }
            return false;
        }
    }
}
