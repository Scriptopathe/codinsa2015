using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace Clank.View.Engine
{
    /// <summary>
    /// Classe ayant la responsabilité de s'occuper des ressources du jeu.
    /// </summary>
    public static class Ressources
    {
        public static Texture2D DummyTexture;

        #region Imported
        /// <summary>
        /// Obtient une police de caractère utilisable pour l'affichage de texte.
        /// </summary>
        public static SpriteFont Font
        {
            get;
            private set;
        }

        public static SpriteFont CourrierFont
        {
            get;
            private set;
        }

        public static SpriteFont NumbersFont
        {
            get;
            private set;
        }

        public static Texture2D LifebarEmpty
        {
            get;
            set;
        }

        public static Texture2D LifebarFull
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient une texture représentant une marque de sélection.
        /// </summary>
        public static Texture2D SelectMark
        {
            get;
            private set;
        }

        public static Texture2D HighlightMark
        {
            get;
            private set;
        }

        public static Texture2D CanMoveMark
        {
            get;
            private set;
        }
        public static Texture2D MenuItem
        {
            get;
            private set;
        }

        public static Texture2D MenuItemHover
        {
            get;
            private set;
        }

        public static Texture2D TextBox
        {
            get;
            set;
        }

        public static Texture2D Menu
        {
            get;
            private set;
        }

        public static Texture2D Cursor
        {
            get;
            private set;
        }
        #endregion

        /// <summary>
        /// Charge les ressources à partir du content manager passé en paramètre.
        /// </summary>
        /// <param name="content"></param>
        public static void LoadRessources(ContentManager content)
        {
            DummyTexture = content.Load<Texture2D>("textures/dummy");
            Font = content.Load<SpriteFont>("segoe_ui_16");
            NumbersFont = content.Load<SpriteFont>("numbers_font");
            SelectMark = content.Load<Texture2D>("textures/select_mark");
            MenuItem = content.Load<Texture2D>("textures/gui/menu_item");
            MenuItemHover = content.Load<Texture2D>("textures/gui/menu_item_hover");
            Menu = content.Load<Texture2D>("textures/gui/menu");
            Cursor = content.Load<Texture2D>("textures/gui/cursor");
            HighlightMark = content.Load<Texture2D>("textures/highlight_mark");
            CanMoveMark = content.Load<Texture2D>("textures/canmove_mark");
            TextBox = content.Load<Texture2D>("textures/gui/textbox");
            CourrierFont = content.Load<SpriteFont>("courrier-16pt");
            LifebarEmpty = content.Load<Texture2D>("textures/gui/lifebar_empty");
            LifebarFull = content.Load<Texture2D>("textures/gui/lifebar_full");
        }
    }
}
