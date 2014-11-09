using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Spells
{
    /// <summary>
    /// Classe de base pour représenter les sorts.
    /// </summary>
    public abstract class SpellBase
    {
        #region Properties
        /// <summary>
        /// Obtient le type de ciblage à utiliser pour ce sort.
        /// </summary>
        public abstract TargettingType TargetType { get; }

        /// <summary>
        /// Obtient le temps de récupération de base de ce sort, en secondes.
        /// </summary>
        public abstract int BaseCooldown { get; }

        /// <summary>
        /// Cooldown actuel de ce sort, en secondes.
        /// </summary>
        public float CurrentCooldown
        {
            get;
            protected set;
        }

        /// <summary>
        /// Entité rattachée à ce sort.
        /// </summary>
        public Entities.EntityHero Hero
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtient le temps de récupération de ce spell après l'utilisation d'un
        /// sort. Ce temps est calculé à partir du cooldown de base, et de la réduction
        /// de cooldown du héros.
        /// </summary>
        /// <returns></returns>
        int GetUseCooldown()
        {
            return BaseCooldown;
        }

        /// <summary>
        /// Utilise ce spell, si il n'est pas en cooldown.
        /// </summary>
        /// <returns>Retourne true si le sort a pu être casté, false sinon. Le sort n'est pas casté si : la
        /// cible </returns>
        public virtual bool Use(TargetInfo target)
        {
            if (target.Type != TargetType || CurrentCooldown > 0)
                return false;

            CurrentCooldown = GetUseCooldown();


            return true;
        }


        /// <summary>
        /// Mets à jour le cooldown de ce sort.
        /// </summary>
        public void UpdateCooldown(float elapsedSeconds)
        {
            CurrentCooldown = (int)Math.Max(0.0f, CurrentCooldown - elapsedSeconds);
        }
        #endregion
    }
}
