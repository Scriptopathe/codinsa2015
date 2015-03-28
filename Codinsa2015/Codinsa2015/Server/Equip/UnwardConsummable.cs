using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Equip
{
    /// <summary>
    /// Représente un consommable de type ward
    /// </summary>
    public class UnwardConsummable : Consummable
    {
        bool _started = true;

        /// <summary>
        /// Donne un buff au possesseur du consommable qui révèle les wards environnantes lors de la première utilisation.
        /// La 2e utilisation supprime la ward la plus proche (si l'effet est encore actif).
        /// </summary>
        public override ConsummableUseResult Use(EntityHero owner)
        {
            if(!UsingStarted)
            {
                // Permet au héros de révéler les wards.
                owner.AddAlteration(new StateAlteration("unward-consummable",
                    owner, new StateAlterationModel()
                        {
                            Type = StateAlterationType.WardSight,
                            BaseDuration = GameServer.GetScene().Constants.Vision.WardRevealDuration,
                        }, new StateAlterationParameters(), StateAlterationSource.Consumable));

                RemainingTime = GameServer.GetScene().Constants.Vision.WardRevealDuration;
                UsingStarted = true;
                return ConsummableUseResult.Success;
            }
            else
            {
                EntityWardPlacement nearest = GameServer.GetMap().Entities.
                        GetEntitiesByType(EntityType.WardPlacement).
                        GetAliveEntitiesInRange(owner.Position, owner.VisionRange, 0).
                        NearestFrom(owner.Position) as EntityWardPlacement;

                // Détruire la ward détruit aussi le consommable.
                if(nearest != null)
                {
                    nearest.DestroyWard(owner);
                    return ConsummableUseResult.SuccessAndDestroyed;
                }
            }

            return ConsummableUseResult.Fail;
        }

        /// <summary>
        /// Mets à jour le consommable.
        /// </summary>
        /// <returns>True si le consommable doit être supprimé.</returns>
        public override bool Update(GameTime time, EntityHero owner)
        {
            base.Update(time, owner);

            if (RemainingTime <= 0 && UsingStarted)
                return true;

            return false;
        }
    }
}
