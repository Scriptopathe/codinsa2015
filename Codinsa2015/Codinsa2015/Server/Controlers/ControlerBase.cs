using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Graphics.Server;
namespace Codinsa2015.Server.Controlers
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
        #region IFACE-Get
        /// <summary>
        /// Retourne la map de jeu.
        /// </summary>
        /// <returns></returns>
        public Map GetMap()
        {
            return GameServer.GetMap();
        }


        #endregion

        #region IFACE
        /// <summary>
        /// Achète l'armure dont le numéro est celui donné, au shop d'id donné.
        /// </summary>
        /// <returns>Retourne true si l'armure a pu être achetée, false sinon.</returns>
        public bool ShopItem(int shopId, int itemId, List<Equip.Equipment> collection)
        {
            Entities.EntityShop shopEntity = GameServer.GetMap().GetEntityById(shopId) as Entities.EntityShop;

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
            Entities.EntityShop shopEntity = GameServer.GetMap().GetEntityById(shopId) as Entities.EntityShop;

            // Contrôle de l'id.
            if (shopEntity == null)
                return false;

            Equip.Shop shop = shopEntity.Shop;

            return ShopItem(shopId, weaponId, shop.GetWeapons(Hero));
        }

        /// <summary>
        /// Retourne les informations concernant la map actuelle.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Access("Codinsa2015.Server.GameServer.GetScene().Controlers[clientId]", "Retourne les informations concernant la map actuelle")]
        public Views.MapView GetMapView()
        {
            Views.MapView view = new Views.MapView();
            view.Passability = GetMap().Passability;
            return view;
        }

        /// <summary>
        /// Retourne la liste des entités en vue.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Access("Codinsa2015.Server.GameServer.GetScene().Controlers[clientId]", "Retourne la liste des entités en vue")]
        public List<Views.EntityBaseView> GetEntitiesInSight()
        {
            List<Views.EntityBaseView> views = new List<Views.EntityBaseView>();
            foreach(var kvp in GetMap().Entities.GetEntitiesInSight(Hero.Type))
            {
                views.Add(GetEntityById(kvp.Key));
            }
            return views;
        }

        /// <summary>
        /// Obtient une vue sur l'entité dont l'id est passé en paramètre.
        /// </summary>
        public Views.EntityBaseView GetEntityById(int entityId)
        {
            Entities.EntityBase entity =  GetMap().GetEntityById(entityId);
            if(Hero.Sees(entity))
            {
                Views.EntityBaseView view = new Views.EntityBaseView();
                view.BaseAbilityPower = entity.BaseAbilityPower;
                view.BaseArmor = entity.BaseArmor;
                view.BaseAttackDamage = entity.BaseAttackDamage;
                view.BaseAttackSpeed = entity.BaseAttackSpeed;
                view.BaseCooldownReduction = entity.BaseCooldownReduction;
                view.BaseMagicResist = entity.BaseMagicResist;
                view.BaseMaxHP = entity.BaseMaxHP;
                view.BaseMoveSpeed = entity.BaseMoveSpeed;
                view.Direction = entity.Direction;
                view.GetAbilityPower = entity.GetAbilityPower();
                view.GetArmor = entity.GetArmor();
                view.GetAttackDamage = entity.GetAttackDamage();
                view.GetCooldownReduction = entity.GetCooldownReduction();
                view.GetHP = entity.GetHP();
                view.GetMagicResist = entity.GetMagicResist();
                view.GetMaxHP = entity.GetMaxHP();
                view.GetMoveSpeed = entity.GetMoveSpeed();
                view.HasTrueVision = entity.HasTrueVision;
                view.HasWardVision = entity.HasWardVision;
                view.HP = entity.HP;
                view.ID = entity.ID;
                view.IsDead = entity.IsDead;
                view.IsRooted = entity.IsRooted;
                view.IsSilenced = entity.IsSilenced;
                view.IsStealthed = entity.IsStealthed;
                view.IsStuned = entity.IsStuned;
                view.Position = entity.Position;
                view.ShieldPoints = entity.ShieldPoints;
                view.StateAlterations = entity.StateAlterations.ToView();
                view.Type = (Views.EntityType)entity.Type;
                view.VisionRange = entity.VisionRange;
                return view;
            }
            else
            {
                return new Views.EntityBaseView();
            }
        }
        #endregion
    }
}
