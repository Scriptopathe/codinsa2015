using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Server.Controlers
{
    public enum ControlerPermissions
    {
        // Donne le droit d'appeler les fonctions prévues pour les IA
        Player      = 0x0001,
        // Donnes tous les droits.
        Admin       = 0x0002 | Player
    }
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
        public abstract void Draw(SpriteBatch batch, GameTime time);

        /// <summary>
        /// Charge les ressources graphiques et autres dont a besoin ce contrôleur.
        /// </summary>
        public abstract void LoadContent();

        /// <summary>
        /// Obtient les permissions de ce contrôleur.
        /// </summary>
        /// <returns></returns>
        public abstract ControlerPermissions GetPermissions();
        /// <summary>
        /// Obtient une référence vers le gestionnaire de GUI du contrôleur.
        /// </summary>
        public EnhancedGui.GuiManager EnhancedGuiManager { get; set; }
        /* ------------------------------------------------------------------
         * IFACE
         * Contient les fonctions auxquelles vont faire appel les IAs.
         * ----------------------------------------------------------------*/
        #region misc

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

        #region Shop
        /// <summary>
        /// Achète un objet d'id donné au shop.
        /// Les ids peuvent être obtenus via ShopGetWeapons(), ShopGetArmors(), ShopGetBoots() etc...
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Achète et équipe un objet d'id donné au shop. Les ids peuvent être obtenus via ShopGetWeapons()," + 
        "ShopGetArmors(), ShopGetBoots() etc...")]
        public Views.ShopTransactionResult ShopPurchaseItem(int equipId)
        {
            Entities.EntityShop shopEntity = (Entities.EntityShop)GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop & (Hero.Type & Entities.EntityType.Teams)).First().Value;

            return (Views.ShopTransactionResult)shopEntity.Shop.Purchase(this.Hero, equipId);
        }

        /// <summary>
        /// Achète un consommable pour le héros donné, et le place dans le slot donné.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Achète un consommable d'id donné, et le place dans le slot donné.")]
        public Views.ShopTransactionResult ShopPurchaseConsummable(int consummableId, int slot)
        {
            Entities.EntityShop shopEntity = (Entities.EntityShop)GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop & (Hero.Type & Entities.EntityType.Teams)).First().Value;

            return (Views.ShopTransactionResult)shopEntity.Shop.PurchaseConsummable(this.Hero, consummableId, slot);
        }

        /// <summary>
        /// Vend l'équipement passé en paramètre au shop.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Vend l'équipement du type passé en paramètre. (vends l'arme si Weapon, l'armure si Armor etc...)")]
        public Views.ShopTransactionResult ShopSell(Views.EquipmentType equipType)
        {
            Entities.EntityShop shopEntity = (Entities.EntityShop)GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop & (Hero.Type & Entities.EntityType.Teams)).First().Value;

            return (Views.ShopTransactionResult)shopEntity.Shop.Sell(this.Hero, (Equip.EquipmentType)equipType);
        }

        /// <summary>
        /// Vends un consommable situé dans le slot donné.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Vends un consommable situé dans le slot donné.")]
        public Views.ShopTransactionResult ShopSellConsummable(int slot)
        {
            Entities.EntityShop shopEntity = (Entities.EntityShop)GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop & (Hero.Type & Entities.EntityType.Teams)).First().Value;

            return (Views.ShopTransactionResult)shopEntity.Shop.SellConsummable(this.Hero, slot);
        }

        /// <summary>
        /// Effectue une upgrade d'un équipement indiqué en paramètre.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Effectue une upgrade d'un équipement indiqué en paramètre.")]
        public Views.ShopTransactionResult ShopUpgrade(Views.EquipmentType equipType)
        {
            Entities.EntityShop shopEntity = (Entities.EntityShop)GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop & (Hero.Type & Entities.EntityType.Teams)).First().Value;

            return (Views.ShopTransactionResult)shopEntity.Shop.UpgradeEquip(Hero, (Equip.EquipmentType)equipType);
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la liste des modèles d'armes disponibles au shop.")]
        public List<Views.WeaponModelView> ShopGetWeapons()
        {

            Entities.EntityShop shopEntity = (Entities.EntityShop)GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop & (Hero.Type & Entities.EntityType.Teams)).First().Value;

            
            List<Equip.EquipmentModel> items = shopEntity.Shop.GetWeapons(Hero);
            List<Views.WeaponModelView> views = new List<Views.WeaponModelView>();
            foreach(var item in items)
            {
                views.Add(((Equip.WeaponModel)item).ToView());
            }
            return views;
        }
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la liste des modèles d'armures disponibles au shop.")]
        public List<Views.PassiveEquipmentModelView> ShopGetArmors()
        {

            Entities.EntityShop shopEntity = (Entities.EntityShop)GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop & (Hero.Type & Entities.EntityType.Teams)).First().Value;


            List<Equip.EquipmentModel> items = shopEntity.Shop.GetArmors(Hero);
            List<Views.PassiveEquipmentModelView> views = new List<Views.PassiveEquipmentModelView>();
            foreach (var item in items)
            {
                views.Add(((Equip.PassiveEquipmentModel)item).ToView());
            }
            return views;
        }
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la liste des modèles de bottes disponibles au shop.")]
        public List<Views.PassiveEquipmentModelView> ShopGetBoots()
        {

            Entities.EntityShop shopEntity = (Entities.EntityShop)GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop & (Hero.Type & Entities.EntityType.Teams)).First().Value;


            List<Equip.EquipmentModel> items = shopEntity.Shop.GetBoots(Hero);
            List<Views.PassiveEquipmentModelView> views = new List<Views.PassiveEquipmentModelView>();
            foreach (var item in items)
            {
                views.Add(((Equip.PassiveEquipmentModel)item).ToView());
            }
            return views;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la liste des enchantements disponibles au shop.")]
        public List<Views.WeaponEnchantModelView> ShopGetEnchants()
        {

            Entities.EntityShop shopEntity = (Entities.EntityShop)GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop & (Hero.Type & Entities.EntityType.Teams)).First().Value;


            List<Equip.EquipmentModel> items = shopEntity.Shop.GetEnchants(Hero);
            List<Views.WeaponEnchantModelView> views = new List<Views.WeaponEnchantModelView>();
            foreach (var item in items)
            {
                views.Add(((Equip.WeaponEnchantModel)item).ToView());
            }
            return views;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient l'id du modèle d'arme équipé par le héros. (-1 si aucun)")]
        public int GetWeaponId()
        {
            if (Hero.Weapon == null)
                return -1;
            return Hero.Weapon.Model.ID;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient le niveau du modèle d'arme équipé par le héros. (-1 si aucune arme équipée)")]
        public int GetWeaponLevel()
        {
            if (Hero.Weapon == null)
                return -1;
            return Hero.Weapon.Level;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient l'id du modèle d'armure équipé par le héros. (-1 si aucun)")]
        public int GetArmorId()
        {
            if (Hero.Armor == null)
                return -1;
            return Hero.Armor.Model.ID;
        }


        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient le niveau du modèle d'armure équipé par le héros. (-1 si aucune armure équipée)")]
        public int GetArmorLevel()
        {
            if (Hero.Armor == null)
                return -1;
            return Hero.Armor.Level;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient l'id du modèle de bottes équipé par le héros. (-1 si aucun)")]
        public int GetBootsId()
        {
            if (Hero.Boots == null)
                return -1;
            return Hero.Boots.Model.ID;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient le niveau du modèle de bottes équipé par le héros. (-1 si aucune paire équipée)")]
        public int GetBootsLevel()
        {
            if (Hero.Boots == null)
                return -1;
            return Hero.Boots.Level;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient l'id du modèle d'enchantement d'arme équipé par le héros. (-1 si aucun)")]
        public int GetWeaponEnchantId()
        {
            if (Hero.Weapon == null || Hero.Weapon.Enchant == null)
                return -1;
            return Hero.Weapon.Enchant.ID;
        }

        #endregion
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
        public Views.Vector2 GetPosition()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Views.Vector2(-1, -1);

            return V2ToView(Hero.Position);
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
            List<List<bool>> passability = new List<List<bool>>();
            bool[,] mappass = GetMap().Passability;
            for (int x = 0; x < mappass.GetLength(0); x++)
            {
                passability.Add(new List<bool>());
                for(int y = 0; y < mappass.GetLength(1); y++)
                {
                    passability[x].Add(mappass[x, y]);
                }
            }
            view.Passability = passability;
            return view;
        }

        /// <summary>
        /// Déplace le joueur vers la position donnée en utilisant l'A*.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Déplace le joueur vers la position donnée en utilisant l'A*.")]
        public bool StartMoveTo(Views.Vector2 position)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return false;

            return Hero.StartMoveTo(ViewToV2(position));
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
                view.BaseHPRegen = entity.BaseHPRegen;
                view.BaseAbilityPower = entity.BaseAbilityPower;
                view.BaseArmor = entity.BaseArmor;
                view.BaseAttackDamage = entity.BaseAttackDamage;
                view.BaseAttackSpeed = entity.BaseAttackSpeed;
                view.BaseCooldownReduction = entity.BaseCooldownReduction;
                view.BaseMagicResist = entity.BaseMagicResist;
                view.BaseMaxHP = entity.BaseMaxHP;
                view.BaseMoveSpeed = entity.BaseMoveSpeed;
                view.Direction = V2ToView(entity.Direction);
                view.GetAbilityPower = entity.GetAbilityPower();
                view.GetArmor = entity.GetArmor();
                view.GetAttackDamage = entity.GetAttackDamage();
                view.GetCooldownReduction = entity.GetCooldownReduction();
                view.GetHP = entity.GetHP();
                view.GetMagicResist = entity.GetMagicResist();
                view.GetMaxHP = entity.GetMaxHP();
                view.GetMoveSpeed = entity.GetMoveSpeed();
                view.GetHPRegen = entity.GetHPRegen();
                view.UniquePassive = (Views.EntityUniquePassives)entity.UniquePassive;
                view.UniquePassiveLevel = entity.UniquePassiveLevel;
                view.HasTrueVision = entity.HasTrueVision;
                view.HasWardVision = entity.HasWardVision;
                view.HP = entity.HP;
                view.ID = entity.ID;
                view.IsDead = entity.IsDead;
                view.IsRooted = entity.IsRooted;
                view.IsSilenced = entity.IsSilenced;
                view.IsStealthed = entity.IsStealthed;
                view.IsStuned = entity.IsStuned;
                view.Position = V2ToView(entity.Position);
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
                TargetDirection = ViewToV2(target.TargetDirection),
                TargetPosition = ViewToV2(target.TargetPosition),
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


        #region XNA macros
        float cf(float f)
        {
            if (float.IsInfinity(f) || float.IsNaN(f))
                return 1;
            return f;
        }
        Views.Vector2 V2ToView(Vector2 xnaVector)
        {
            return new Views.Vector2() { X = cf(xnaVector.X), Y = cf(xnaVector.Y) };
        }
        Vector2 ViewToV2(Views.Vector2 viewVector)
        {
            return new Vector2(viewVector.X, viewVector.Y);
        }
        #endregion
    }

    
}
