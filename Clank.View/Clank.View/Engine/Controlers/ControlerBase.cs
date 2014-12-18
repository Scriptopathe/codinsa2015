using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Clank.View.Engine.Graphics.Server;
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
        public abstract void Draw(RemoteSpriteBatch batch, GameTime time);

        /// <summary>
        /// Charge les ressources graphiques et autres dont a besoin ce contrôleur.
        /// </summary>
        public abstract void LoadContent();
        /// <summary>
        /// Lie le client graphique (si existant) du contrôleur au serveur donné.
        /// </summary>
        public abstract void BindGraphicsClient(GraphicsServer server);
        /// <summary>
        /// Obtient une référence vers le gestionnaire de particules du contrôleur.
        /// </summary>
        public Particles.ParticleManager Particles { get; set; }
        /// <summary>
        /// Obtient une référence vers le gestionnaire de GUI du contrôleur.
        /// </summary>
        public Gui.GuiManager GuiManager { get; set; }
        /* ------------------------------------------------------------------
         * IFACE
         * Contient les fonctions auxquelles vont faire appel les IAs.
         * ----------------------------------------------------------------*/
        #region IFACE
        /// <summary>
        /// Achète l'armure dont le numéro est celui donné, au shop d'id donné.
        /// </summary>
        /// <returns>Retourne true si l'armure a pu être achetée, false sinon.</returns>
        public bool ShopItem(int shopId, int itemId, List<Equip.Equipment> collection)
        {
            Entities.EntityShop shopEntity = Mobattack.GetMap().GetEntityById(shopId) as Entities.EntityShop;

            // Contrôle de l'id.
            if (shopEntity == null)
                return false;

            // Shop
            Equip.Shop shop = shopEntity.Shop;
            IEnumerable<Equip.Equipment> requestedItem = collection.Where(new Func<Equip.Equipment, bool>((Equip.Equipment e) =>
            {
                return e.ID == itemId;
            }));

            // Contrôle : id de l'armure correct ?
            if (requestedItem.Count() == 0)
                return false;

            // On a trouvé la bonne armure : vérification du solde du héros.
            Equip.Equipment equip = requestedItem.First();

            // Prix trop élevé.
            if (equip.Price > Hero.PA)
                return false;

            if (equip is Equip.Armor)
                Hero.Armor = (Equip.Armor)equip;
            else if (equip is Equip.Weapon)
                Hero.Weapon = (Equip.Weapon)equip;
            

            Hero.PA -= equip.Price;

            return true;
        }

        /// <summary>
        /// Achète l'arme dont le numéro est celui donné, au shop d'id donné.
        /// </summary>
        /// <returns>Retourne true si l'arme a pu être achetée, false sinon.</returns>
        public bool ShopWeapon(int shopId, int weaponId)
        {
            Entities.EntityShop shopEntity = Mobattack.GetMap().GetEntityById(shopId) as Entities.EntityShop;

            // Contrôle de l'id.
            if (shopEntity == null)
                return false;

            Equip.Shop shop = shopEntity.Shop;

            return ShopItem(shopId, weaponId, shop.GetWeapons(Hero));
        }

        
        public void MovePlayerTo(Vector2 position) { }
        public void UseSpell(int spellId, Spells.SpellCastTargetInfo castInfo) { }
        #endregion
    }
}
