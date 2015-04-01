using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Server.Entities
{
    /// <summary>
    /// Représente une entité avec laquelle les héros peuvent intéragir pour acheter de l'équipement.
    /// </summary>
    public class EntityShop : EntityBase
    {
        #region Variables

        #endregion

        #region Properties
        /// <summary>
        /// Obtient une référence vers l'échoppe tenue par cette entité.
        /// </summary>
        public Equip.Shop Shop
        {
            get;
            set;
        }

        public override bool IsDamageImmune
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de EntityTower.
        /// </summary>
        public EntityShop()
            : base()
        {
            Type = EntityType.Shop;
            Shop = new Equip.Shop(GameServer.GetScene().ShopDB, this, GameServer.GetScene().Constants.Structures.Shops.DefaultBuyRange);
        }

        /// <summary>
        /// Mets à jour l'entité.
        /// </summary>
        protected override void DoUpdate(GameTime time)
        {
            base.DoUpdate(time);
        }




        #endregion

        
    }
}
