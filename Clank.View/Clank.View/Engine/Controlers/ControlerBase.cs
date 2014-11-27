using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Controlers
{
    /// <summary>
    /// Classe abstraite de contrôleur.
    /// 
    /// Un contrôleur permet de contrôler un seul héros.
    /// </summary>
    public abstract class ControlerBase
    {
        /// <summary>
        /// Obtient ou définit le héros contrôlé par ce contrôleur.
        /// </summary>
        public abstract Entities.EntityHero Hero { get; set; }
        /// <summary>
        /// Crée un nouveau contrôleur ayant le contrôle sur le héros donné.
        /// </summary>
        /// <param name="hero"></param>
        public ControlerBase(Entities.EntityHero hero) { }

        /// <summary>
        /// Mets à jour l'état de ce contrôleur, et lui permet d'envoyer des commandes au héros.
        /// </summary>
        /// <param name="time"></param>
        public abstract void Update(GameTime time);

        /// <summary>
        /// Dessine les éléments graphique du contrôleur à l'écran.
        /// </summary>
        public abstract void Draw(SpriteBatch batch, GameTime time);
    }
}
