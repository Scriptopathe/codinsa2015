using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Entities
{
    public class EntityCheckpoint : EntityBase
    {
        /// <summary>
        /// Pour une ligne, numéro du checkpoint permettant de déterminer
        /// l'ordre dans lequel les checkpoints doivent être parcourus.
        /// </summary>
        public int CheckpointID { get; set; }
        /// <summary>
        /// Représente la ligne de checkpoints dont ce checkpoint fait partie.
        /// </summary>
        public int CheckpointRow { get; set; }
        protected override void ApplyTrueDamage(float damage)
        {
            return;
        }

        public EntityCheckpoint() : base()
        {
            VisionRange = 0;
        }
        
    }
}
