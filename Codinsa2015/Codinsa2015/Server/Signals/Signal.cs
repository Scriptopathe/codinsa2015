using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Signals
{
    public class Signal
    {
        /// <summary>
        /// ID de l'entité émetrice du signal.
        /// </summary>
        [Clank.ViewCreator.Export("int", "id de l'entité émétrice du signal")]
        public int SourceEntity;

        /// <summary>
        /// ID de l'entité que cible le signal.
        /// </summary>
        [Clank.ViewCreator.Export("int", "ID de l'entité que cible le signal (pour les signaux AttackEntity, DefendEntity)")]
        public int DestinationEntity;

        [Clank.ViewCreator.Export("Vector2", "Position que cible le signal (pour les signaux ComingToPosition)")]
        public Vector2 DestinationPosition;
        

        public Entities.EntityType Team
        {
            get 
            { 
                var entity = GameServer.GetMap().GetEntityById(SourceEntity);
                if (entity == null)
                    return 0;

                return entity.Type & Entities.EntityType.Teams;
            }
        }

        public Signal()
        {

        }

        public Views.SignalView ToView()
        {
            Views.SignalView view = new Views.SignalView();
            view.SourceEntity = SourceEntity;
            view.DestinationEntity = DestinationEntity;
            view.DestinationPosition = new Views.Vector2(DestinationPosition.X, DestinationPosition.Y);
            return view;
        }
    }
}
