using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Graphics.Server;
using Codinsa2015.Server.Equip;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Controlers
{
    /// <summary>
    /// Représente l'interface graphique d'une échoppe.
    /// </summary>
    public class ShopInterface
    {
        
        #region Variables
        EntityShop m_shop;
        /// <summary>
        /// Type d'équipement affiché.
        /// </summary>
        EquipmentType m_shown;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit une référence vers le shop représenté par cette interface.
        /// </summary>
        public EntityShop Shop
        {
            get { return m_shop; }
            set { m_shop = value; }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si cette interface est visible.
        /// </summary>
        public bool Visible
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Mets à jour l'interface du shop, et traite les entrées pour effectuer une action.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            
        }

        /// <summary>
        /// Dessine l'interface du shop.
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(RemoteSpriteBatch batch)
        {

        }
        #endregion

    }
}
