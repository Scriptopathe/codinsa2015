using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
using Codinsa2015.Server;
using Codinsa2015.Server.Spellcasts;
using Codinsa2015.Server.Spells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codinsa2015.Server.Controlers
{
    /// <summary>
    /// Classe abstraite de contrôleur.
    /// 
    /// Un contrôleur permet de contrôler un seul héros.
    /// </summary>
    public class IAControler : ControlerBase
    {
        #region Variables
        /// <summary>
        /// Héros contrôlé par cette instance de contrôleur.
        /// </summary>
        EntityHero m_hero;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit le héros contrôlé.
        /// </summary>
        public override EntityHero Hero
        {
            get { return m_hero; }
            set { m_hero = value; }
        }



        /// <summary>
        /// Obtient une référence vers la map contrôlée.
        /// </summary>
        public Map Map
        {
            get { return GameServer.GetScene().Map; }
        }



        #endregion

        #region Methods

        #region Init
        /// <summary>
        /// Crée un nouveau contrôleur ayant le contrôle sur le héros donné.
        /// </summary>
        /// <param name="hero"></param>
        public IAControler(EntityHero hero) : base(hero)
        {
            m_hero = hero;
            EnhancedGuiManager = new EnhancedGui.GuiManager();
        }

        /// <summary>
        /// Charges les ressources (graphiques et autres) dont a besoin de contrôleur.
        /// </summary>
        public override void LoadContent()
        {

        }
        #endregion

        #region Update
        int __oldScroll;
        Point __oldPos;
        /// <summary>
        /// Mets à jour l'état de ce contrôleur, et lui permet d'envoyer des commandes au héros.
        /// </summary>
        /// <param name="time"></param>
        public override void Update(GameTime time)
        {
          
            
        }

        
        #endregion

        #region Draw

        /// <summary>
        /// Dessine les éléments graphiques du contrôleur à l'écran.
        /// </summary>
        public override void Draw(SpriteBatch batch, GameTime time)
        {
            
        }


        #endregion

        
        #endregion
    }
}
