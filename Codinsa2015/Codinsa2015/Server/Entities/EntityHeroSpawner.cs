using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Entities
{
    public class EntityHeroSpawner : EntityBase
    {
        public override bool IsDamageImmune
        {
            get
            {
                return true;
            }
        }
        protected override void ApplyTrueDamage(float damage)
        {
            return;
        }

        public EntityHeroSpawner()
            : base()
        {
            VisionRange = 5;
            Type |= EntityType.HeroSpawner;
        }
        
    }
}
