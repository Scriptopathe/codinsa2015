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
        MiningFarm,
        Camp1,
        Camp2,
        Camp3,
        Camp4,
        Camp5,
        Camp6,
        Camp7,
        Camp8,
        Router1,
        Router2,
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

        /// <summary>
        /// Obtient ou définit la position de l'évènement (si applicable).
        /// </summary>
        public abstract Vector2 Position { get; set; }
    }
}
