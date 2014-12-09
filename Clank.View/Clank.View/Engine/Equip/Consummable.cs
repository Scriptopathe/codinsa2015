using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clank.View.Engine.Entities;
namespace Clank.View.Engine.Equip
{
    public enum ConsummableType
    {
        Empty,
        Ward
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
        /// Utilise le consommable.
        /// Retourne true si le consommable doit être détruit.
        /// </summary>
        public abstract bool Use(EntityHero owner);
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
