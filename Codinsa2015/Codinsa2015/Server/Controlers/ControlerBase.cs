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
        /// Obtient le nom affiché du héros (nom de l'école / pseudo).
        /// </summary>
        public string HeroName { get; set; }
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
        public EnhancedGui.GuiManager EnhancedGuiManager { get; set; }
        /* ------------------------------------------------------------------
         * IFACE
         * Contient les fonctions auxquelles vont faire appel les IAs.
         * ----------------------------------------------------------------*/
        #region misc
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
        #endregion
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

        #region  Picks IFACE

        #endregion

        #region  Game IFACE
        const string controlerAccessStr = "Codinsa2015.Server.GameServer.GetScene().GetControler(clientId)";
        /// <summary>
        /// Achète l'armure dont le numéro est celui donné, au shop d'id donné.
        /// </summary>
        /// <returns>Retourne true si l'armure a pu être achetée, false sinon.</returns>
        public bool ShopItem(int shopId, int itemId, List<Equip.EquipmentModel> collection)
        {
            Entities.EntityShop shopEntity = GameServer.GetMap().GetEntityById(shopId) as Entities.EntityShop;

            // Contrôle de l'id.
            if (shopEntity == null)
                return false;

            // Shop
            Equip.Shop shop = shopEntity.Shop;
            IEnumerable<Equip.EquipmentModel> requestedItem = collection.Where(new Func<Equip.EquipmentModel, bool>((Equip.EquipmentModel e) =>
            {
                return e.ID == itemId;
            }));

            // Contrôle : id de l'armure correct ?
            if (requestedItem.Count() == 0)
                return false;

            // On a trouvé la bonne armure : vérification du solde du héros.
            Equip.EquipmentModel equip = requestedItem.First();

            // Prix trop élevé.
            if (equip.Price > Hero.PA)
                return false;

            if (equip.Type == Equip.EquipmentType.Armor)
                Hero.Armor = new Equip.Armor(Hero, (Equip.PassiveEquipmentModel)equip);
            else if (equip is Equip.WeaponModel)
                Hero.Weapon = new Equip.Weapon(Hero, (Equip.WeaponModel)equip) { Enchant = Hero.Weapon.Enchant };
            

            Hero.PA -= equip.Price;

            return true;
        }




        
        /// <summary>
        /// Retourne une vue vers le héros contrôlé par ce contrôleur.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Retourne une vue vers le héros contrôlé par ce contrôleur.")]
        public Views.EntityBaseView GetHero()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Views.EntityBaseView();

            return GetEntityById(Hero.ID);
        }
        
        /// <summary>
        /// Retourne la position du héros.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Retourne la position du héros.")]
        public Vector2 GetPosition()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Vector2(-1, -1);

            return Hero.Position;
        }

        /// <summary>
        /// Retourne les informations concernant la map actuelle.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Access(controlerAccessStr, "Retourne les informations concernant la map actuelle")]
        public Views.MapView GetMapView()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Views.MapView();

            Views.MapView view = new Views.MapView();
            view.Passability = GetMap().Passability;
            return view;
        }

        /// <summary>
        /// Déplace le joueur vers la position donnée en utilisant l'A*.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Déplace le joueur vers la position donnée en utilisant l'A*.")]
        public bool StartMoveTo(Vector2 position)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return false;

            return Hero.StartMoveTo(position);
        }

        /// <summary>
        /// Indique si le joueur est entrain de se déplacer en utilisant son A*.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Access(controlerAccessStr, "Indique si le joueur est entrain de se déplacer en utilisant son A*.")]
        public bool IsAutoMoving()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return false;

            return Hero.IsAutoMoving();
        }

        /// <summary>
        /// Arrête le déplacement automatique (A*) du joueur.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Access(controlerAccessStr, "Arrête le déplacement automatique (A*) du joueur.")]
        public bool EndMoveTo()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return false;

            Hero.EndMoveTo();
            return true;
        }

        /// <summary>
        /// Retourne la liste des entités en vue.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Access(controlerAccessStr, "Retourne la liste des entités en vue")]
        public List<Views.EntityBaseView> GetEntitiesInSight()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<Views.EntityBaseView>();

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
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient une vue sur l'entité dont l'id est passé en paramètre. (si l'id retourné est -1 : accès refusé)")]
        public Views.EntityBaseView GetEntityById(int entityId)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Views.EntityBaseView() { ID = -1 };

            Entities.EntityBase entity =  GetMap().GetEntityById(entityId);
            if (entity == null)
                return new Views.EntityBaseView() { ID = -1 };

            if(Hero.HasSightOn(entity))
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
                return new Views.EntityBaseView() { ID = -1 };
            }
        }

        /// <summary>
        /// Utilise le sort d'id donné.
        /// </summary>
        /// <returns>Retourne true si l'action a été effectuée.</returns>
        [Clank.ViewCreator.Access(controlerAccessStr, "Utilise le sort d'id donné. Retourne true si l'action a été effectuée.")]
        public bool UseSpell(int spellId, Views.SpellCastTargetInfoView target)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return false;

            if(Hero.Spells.Count <= spellId)
            {
                return false;
            }

            return Hero.Spells[spellId].Use(new Spells.SpellCastTargetInfo()
            {
                TargetDirection = target.TargetDirection,
                TargetPosition = target.TargetPosition,
                TargetId = target.TargetId,
                Type = (Codinsa2015.Server.Spells.TargettingType)target.Type,
                AlterationParameters = new Entities.StateAlterationParameters()
            }) == Spells.SpellUseResult.Success; // TODO
        }

        /// <summary>
        /// Obtient le mode de scène actuel.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient le mode actuel de la scène.")]
        public Views.SceneMode GetMode()
        {
            return (Views.SceneMode)GameServer.GetScene().Mode;
        }
        /// <summary>
        /// Obtient la description du spell dont l'id est donné en paramètre.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la description du spell dont l'id est donné en paramètre.")]
        public Views.SpellDescriptionView GetSpellCurrentLevelDescription(int spellId)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Views.SpellDescriptionView();

            // Check de l'id du spell.
            if (Hero.Spells.Count <= spellId)
            {
                return new Views.SpellDescriptionView();
            }

            return Hero.Spells[spellId].Description.ToView();
        }

        /// <summary>
        /// Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre.")]
        public Views.SpellView GetSpell(int spellId)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Views.SpellView();

            // Check de l'id du spell.
            if (Hero.Spells.Count <= spellId)
            {
                return new Views.SpellView();
            }

            return Hero.Spells[spellId].ToView();
        }

        /// <summary>
        /// Obtient la liste des spells du héros contrôlé.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la liste des spells du héros contrôlé.")]
        public List<Views.SpellView> GetSpells()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<Views.SpellView>();

            List<Views.SpellView> spells = new List<Views.SpellView>();
            foreach (var spell in Hero.Spells)
                spells.Add(spell.ToView());

            return spells;
        }

        /// <summary>
        /// Obtient les spells possédés par le héros dont l'id est passé en paramètre.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient les spells possédés par le héros dont l'id est passé en paramètre.")]
        public List<Views.SpellView> GetHeroSpells(int entityId)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<Views.SpellView>();

            // Vérifie que l'entité existe.
            if(!GetMap().Entities.ContainsKey(entityId))
                return new List<Views.SpellView>();

            Entities.EntityHero hero = GetMap().Entities[entityId] as Entities.EntityHero;

            // Vérifie que l'entité est bien un héros.
            if (hero == null)
                return new List<Views.SpellView>();

            List<Views.SpellView> spells = new List<Views.SpellView>();
            foreach (var spell in hero.Spells)
                spells.Add(spell.ToView());

            return spells;
        }
        #endregion
    }
}
