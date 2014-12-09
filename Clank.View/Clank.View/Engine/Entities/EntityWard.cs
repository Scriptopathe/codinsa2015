using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Clank.View.Engine.Entities
{
    public class EntityWard : EntityBase
    {
        #region Variables
        #endregion

        

        /// <summary>
        /// Ne peut pas prendre de dégâts.
        /// </summary>
        protected override void ApplyTrueDamage(float damage)
        {
            return;
        }

        /// <summary>
        /// Crée une nouvelle instance de EntityWardPlacement.
        /// </summary>
        public EntityWard()
            : base()
        {
            VisionRange = Mobattack.GetScene().Constants.Vision.WardRange;
            Type |= EntityType.Ward;
            Shape = new Shapes.CircleShape(Vector2.Zero, 1);
        }

        
        #region API

        #endregion

    }
}
