using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Events
{
    /// <summary>
    /// Représente un type d'évènement.
    /// </summary>
    public enum EventId
    {
        Camps,
        MinibossWest,
        MinibossEast,
        Resurrector,
    }
    /// <summary>
    /// Classe de base de tous les évènements du jeu.
    /// Les évènements sont construits en deux temps : l'exécution du code du constructeur
    /// n'est pas garantie d'être exécutée une fois le jeu initialisé.
    /// </summary>
    public abstract class GameEvent
    {

        /// <summary>
        /// Initialise l'évènement.
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// Mets à jour l'évènement.
        /// </summary>
        public abstract void Update(GameTime time);
    }
}
