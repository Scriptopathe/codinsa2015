using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Clank.View.Engine.Entities
{
    public class EntityWardPlacement : EntityBase
    {
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
        public EntityWardPlacement() : base()
        {
            VisionRange = 0;
            Type |= EntityType.WardPlacement;
            Shape = new Shapes.CircleShape(Vector2.Zero, 2);
        }
        
    }
}
