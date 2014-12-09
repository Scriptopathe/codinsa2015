using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Clank.View.Engine.Entities;
namespace Clank.View.Engine.Equip
{
    public enum ConsummableType
    {
        Empty,
        Ward,
        Unward,
    }
    /// <summary>
    /// Représente un consommable.
    /// </summary>
    public abstract class Consummable
    {
        /// <summary>
        /// Obtient le type du consommable.
        /// </summary>
        public abstract ConsummableType Type { get; }

        /// <summary>
        /// Retourne une variable indiquant le temps restant avant disparition du consommable.
        /// Cette variable est maintenue à titre indicatif.
        /// </summary>
        public float RemainingTime { get; protected set; }
        /// <summary>
        /// Retourne une valeur indiquant si le consommable a été utilisé.
        /// (i.e. : son utilisation a commencé).
        /// </summary>
        public bool UsingStarted { get; set; }
        /// <summary>
        /// Utilise le consommable.
        /// Retourne true si le consommable doit être détruit.
        /// </summary>
        public abstract bool Use(EntityHero owner);

        /// <summary>
        /// Mets à jour le consommable.
        /// Retourne true si le consommable doit être détruit.
        /// </summary>
        public virtual bool Update(GameTime time, EntityHero owner)
        {
            RemainingTime -= (float)time.ElapsedGameTime.TotalSeconds;
            if (RemainingTime <= 0) RemainingTime = 0;
            return false;
        }

    }

    public class EmptyConsummable : Consummable
    {
        public override ConsummableType Type
        {
            get { return ConsummableType.Empty; }
        }

        public override bool Use(EntityHero owner)
        {
            return false;
        }
    }
}
