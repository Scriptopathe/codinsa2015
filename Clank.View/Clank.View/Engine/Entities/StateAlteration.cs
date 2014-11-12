using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Clank.View.Engine.Entities
{
    /// <summary>
    /// Représente une altération d'état en cours.
    /// </summary>
    public class StateAlteration
    {
        /// <summary>
        /// Représente la source de l'altération d'état.
        /// </summary>
        public EntityBase Source { get; set; }
        /// <summary>
        /// Représente le modèle d'altération d'état appliquée sur une entité.
        /// </summary>
        public StateAlterationModel Model { get; set; }

        /// <summary>
        /// Temps restant en secondes pour l'altération d'état.
        /// </summary>
        public float RemainingTime { get; set; }

        /// <summary>
        /// Retourne une valeur indiquant si l'intéraction est terminée.
        /// </summary>
        public bool HasEnded
        {
            get { return RemainingTime <= 0; }
        }
        /// <summary>
        /// Crée une nouvelle altération d'état à partir du modèle donné.
        /// La durée restante de l'altération d'état est déterminée à partir
        /// de la durée contenue dans le modèle d'altération d'état donné.
        /// </summary>
        public StateAlteration(StateAlterationModel model)
        {
            Model = model;
            RemainingTime = model.Duration;
        }

        /// <summary>
        /// Mets à jour l'altération d'état (réduit la durée du temps écoulé depuis
        /// le dernier appel à Update()).
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            RemainingTime -= (float)time.ElapsedGameTime.TotalSeconds;
        }
    }
}
