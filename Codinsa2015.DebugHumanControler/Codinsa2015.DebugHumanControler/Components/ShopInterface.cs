using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
using Codinsa2015.EnhancedGui;
using Codinsa2015.Server.Equip;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.DebugHumanControler.Components
{
    /// <summary>
    /// Représente une interface graphique permettant de visualiser le shop.
    /// </summary>
    public class ShopInterface : GuiWindow
    {
        /// <summary>
        /// Obtient ou définit le shop dont on doit afficher l'interface.
        /// </summary>
        public Server.Equip.Shop Shop { get; private set; }

        /// <summary>
        /// Héro lié au shop.
        /// </summary>
        public EntityHero Hero { get; private set; }

        /// <summary>
        /// Hash map ayant en clef les boutons de l'interface et en valeurs
        /// les équipements correspondants.
        /// </summary>
        Dictionary<GuiButton, EquipmentModel> m_models;
        GuiButton m_closeButton;
        DeveloperConsole m_console;
        /// <summary>
        /// Crée une nouvelle instance de ShopInterface à partir du shop donné.
        /// </summary>
        /// <param name="shop"></param>
        public ShopInterface(DeveloperConsole console, GuiManager mgr, EntityHero hero, Shop shop) : base(mgr)
        {
            Shop = shop;
            Hero = hero;
            m_console = console;
            m_models = new Dictionary<GuiButton, EquipmentModel>();
            SetupControls();
        }


        /// <summary>
        /// Mets à jour les contrôles etc...
        /// </summary>
        /// <param name="time"></param>
        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void Draw(SpriteBatch batch)
        {
            // Dessine les équips actuels du héros.
            int colSize = this.Size.X / 5 - 10;
            int sY = 5;
            int sX = 5;
            int layer = 10;
            
            DrawString(batch, Ressources.Font, "Arme", new Vector2(sX, sY), Color.White, 0, Vector2.Zero, 1.0f, layer);
            sX += colSize;
            DrawString(batch, Ressources.Font, "Armure", new Vector2(sX, sY), Color.White, 0, Vector2.Zero, 1.0f, layer);
            sX += colSize;
            DrawString(batch, Ressources.Font, "Bottes", new Vector2(sX, sY), Color.White, 0, Vector2.Zero, 1.0f, layer);
            sX += colSize;
            DrawString(batch, Ressources.Font, "Consommable 1", new Vector2(sX, sY), Color.White, 0, Vector2.Zero, 1.0f, layer);
            sX += colSize;
            DrawString(batch, Ressources.Font, "Enchant arme", new Vector2(sX, sY), Color.White, 0, Vector2.Zero, 1.0f, layer);

            sY += 20;
            sX = 5;

            string lvl = Hero.Weapon == null ? "" : Hero.Weapon.Level.ToString();
            DrawString(batch, Ressources.Font, Hero.Weapon == null ? "Aucune" : Hero.Weapon.Model.Name + "(" + lvl + ")", new Vector2(sX, sY), Color.White, 0, Vector2.Zero, 1.0f, layer);
            sX += colSize;
            lvl = Hero.Armor == null ? "" : Hero.Armor.Level.ToString();
            DrawString(batch, Ressources.Font, Hero.Armor == null ? "Aucune" : Hero.Armor.Model.Name + "(" + lvl + ")", new Vector2(sX, sY), Color.White, 0, Vector2.Zero, 1.0f, layer);
            sX += colSize;
            lvl = Hero.Boots == null ? "" : Hero.Boots.Level.ToString();
            DrawString(batch, Ressources.Font, Hero.Boots == null ? "Aucune" : Hero.Boots.Model.Name + "(" + lvl + ")", new Vector2(sX, sY), Color.White, 0, Vector2.Zero, 1.0f, layer);
            sX += colSize;
            DrawString(batch, Ressources.Font, Hero.Consummable1 == null ? "Aucun" : Hero.Consummable1.Model.Name, new Vector2(sX, sY), Color.White, 0, Vector2.Zero, 1.0f, layer);
            sX += colSize;
            DrawString(batch, Ressources.Font, (Hero.Weapon == null || Hero.Weapon.Enchant == null) ? "Aucun" : Hero.Weapon.Enchant.Name, new Vector2(sX, sY), Color.White, 0, Vector2.Zero, 1.0f, layer);
            base.Draw(batch);
        }

        /// <summary>
        /// Crée tous les boutons de l'interface.
        /// </summary>
        void SetupControls()
        {
            this.Size = new Microsoft.Xna.Framework.Point(1200, 500);
            int colSize = this.Size.X / 5 - 10;

            // Bouton close
            m_closeButton = new GuiButton(Manager) { Parent = this, Title = "x", Location = new Point(this.Size.X - 16, 1), Size = new Point(16, 16) };
            m_closeButton.Clicked += close_Clicked;

            // Début des équips à acheter
            int bY = 5;
            int startY = 50;
            int sY = startY;
            int sX = 5;
            int btnW = 200;
            int btnMargin = btnW + 1;
            int btnH = 20;

            new GuiButton(Manager) { Parent = this, Size = new Point(20, 20), Location = new Point(sX + 200, bY), Title = "-", }.Clicked += delegate() { Shop.Sell(Hero, EquipmentType.Weapon); };
            
            foreach(var item in Shop.GetWeapons(Hero))
            {
                var itemRef = item;
                GuiButton buyButton = new GuiButton(Manager) { Parent = this, Title = item.Name, Location = new Point(sX, sY), Size = new Point(btnW, btnH) };
                buyButton.Clicked += delegate()
                {
                    m_console.Output.AppendLine("Achat de " +itemRef.Name + " : " + Shop.Purchase(Hero, itemRef.ID));
                };

                GuiButton upgradeButton = new GuiButton(Manager) { Parent = this, Title = "+", Location = new Point(sX + btnMargin, sY), Size = new Point(btnH, btnH) };
                upgradeButton.Clicked += delegate()
                {
                    m_console.Output.AppendLine("Upgrade de " + itemRef.Name + " : " + Shop.UpgradeWeapon(Hero));
                };
                sY += 25;
            }
            sX += colSize;
            sY = startY;
            new GuiButton(Manager) { Parent = this, Size = new Point(20, 20), Location = new Point(sX + 200, bY), Title = "-", }.Clicked += delegate() { Shop.Sell(Hero, EquipmentType.Armor); };
            foreach (var item in Shop.GetArmors(Hero))
            {
                var itemRef = item;
                GuiButton buyButton = new GuiButton(Manager) { Parent = this, Title = item.Name, Location = new Point(sX, sY), Size = new Point(btnW, btnH) };
                buyButton.Clicked += delegate()
                {
                    m_console.Output.AppendLine("Achat de " + itemRef.Name + " : " + Shop.Purchase(Hero, itemRef.ID));
                };

                GuiButton upgradeButton = new GuiButton(Manager) { Parent = this, Title = "+", Location = new Point(sX + btnMargin, sY), Size = new Point(btnH, btnH) };
                upgradeButton.Clicked += delegate()
                {
                    m_console.Output.AppendLine("Upgrade de " + itemRef.Name + " : " + Shop.UpgradeArmor(Hero));
                };
                sY += 25;
            }
            sX += colSize;
            sY = startY;
            new GuiButton(Manager) { Parent = this, Size = new Point(20, 20), Location = new Point(sX + 200, bY), Title = "-", }.Clicked += delegate() { Shop.Sell(Hero, EquipmentType.Boots); };
            foreach (var item in Shop.GetBoots(Hero))
            {
                var itemRef = item;
                GuiButton buyButton = new GuiButton(Manager) { Parent = this, Title = item.Name, Location = new Point(sX, sY), Size = new Point(btnW, btnH) };
                buyButton.Clicked += delegate()
                {
                    m_console.Output.AppendLine("Achat de " + itemRef.Name + " : " + Shop.Purchase(Hero, itemRef.ID));
                };

                GuiButton upgradeButton = new GuiButton(Manager) { Parent = this, Title = "+", Location = new Point(sX + btnMargin, sY), Size = new Point(btnH, btnH) };
                upgradeButton.Clicked += delegate()
                {
                    m_console.Output.AppendLine("Upgrade de " + itemRef.Name + " : " + Shop.UpgradeBoots(Hero));
                };
                sY += 25;
            }
            sX += colSize;
            sY = startY;
            new GuiButton(Manager) { Parent = this, Size = new Point(20, 20), Location = new Point(sX + 200, bY), Title = "-", }.Clicked += delegate() { Shop.SellConsummable(Hero, 0); };
            foreach (var item in Shop.GetConsummables(Hero))
            {
                GuiButton buyButton = new GuiButton(Manager) { Parent = this, Title = item.Name, Location = new Point(sX, sY), Size = new Point(btnW, btnH) };
                var itemRef = item;
                buyButton.Clicked += delegate()
                {
                    m_console.Output.AppendLine("Achat de " + itemRef.Name + " (slot 0) : " + Shop.PurchaseConsummable(Hero, item.ID, 0));
                };
                sY += 25;
            }
            sX += colSize;
            sY = startY;
            new GuiButton(Manager) { Parent = this, Size = new Point(20, 20), Location = new Point(sX + 200, bY), Title = "-", }.Clicked += delegate() { Shop.Sell(Hero, EquipmentType.WeaponEnchant); };
            foreach (var item in Shop.GetEnchants(Hero))
            {
                GuiButton buyButton = new GuiButton(Manager) { Parent = this, Title = item.Name, Location = new Point(sX, sY), Size = new Point(btnW, btnH) };
                var itemRef = item;
                buyButton.Clicked += delegate()
                {
                    m_console.Output.AppendLine("Achat de " + itemRef.Name + " : " + Shop.Purchase(Hero, itemRef.ID));
                };
                sY += 25;
            }
        }


        #region Events
        void close_Clicked()
        {
            this.Dispose();
            foreach(var kvp in m_models)
            {
                if(!kvp.Key.IsDisposed)
                    kvp.Key.Dispose();
            }
            m_closeButton.Dispose();
        }
        #endregion
    }
}
