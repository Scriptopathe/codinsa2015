using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace Codinsa2015
{

    /// <summary>
    /// Classe ayant la responsabilité de s'occuper des ressources du jeu.
    /// </summary>
    public static class Ressources
    {
        public static ContentManager Content;
        public static GraphicsDevice Device;
        public static Texture2D DummyTexture;
        public static Vector2 ScreenSize { get; set; }
        public static string MapFilename = "Content/map.txt";

        #region ByName
        static Dictionary<string, Texture2D> s_textureCache = new Dictionary<string, Texture2D>();
        public static Texture2D GetSpellTexture(string spellname)
        {
            Texture2D tex;
            try
            {
                if (s_textureCache.ContainsKey(spellname))
                    return s_textureCache[spellname];
                else
                {
                    tex = Content.Load<Texture2D>("textures/spells/" + spellname);
                    s_textureCache.Add(spellname, tex);
                }
            }
            catch
            {
                tex = DummyTexture;
                s_textureCache.Add(spellname, tex);
            }

            return tex;
        }
        #endregion

        #region Effects
        public static Effect MapEffect { get; set; }
        public static Texture2D WallTexture { get; set; }
        public static Texture2D WallBorderTexture { get; set; }
        public static Texture2D GrassTexture { get; set; }
        public static Texture2D LavaTexture { get; set; }

        /// <summary>
        /// Matrice de transformation pour les dessins 2D.
        /// </summary>
        public static Matrix PlaneTransform2D { get; set; }
        #endregion
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

        public static Texture2D IconMage { get; set; }
        public static Texture2D IconFighter { get; set; }
        public static Texture2D IconTank { get; set; }
        /// <summary>
        /// Charge les ressources à partir du content manager passé en paramètre.
        /// </summary>
        /// <param name="content"></param>
        public static void LoadRessources(GraphicsDevice device, ContentManager content)
        {
            Content = content;
            Device = device;

            IconMage = content.Load<Texture2D>( "textures/icons/mage");
            IconFighter = content.Load<Texture2D>( "textures/icons/fighter");
            IconTank = content.Load<Texture2D>( "textures/icons/tank");
            DummyTexture = content.Load<Texture2D>( "textures/dummy");
            Font = content.Load<SpriteFont>( "textfont");
            NumbersFont = content.Load<SpriteFont>( "numbers_font");
            CourrierFont = content.Load<SpriteFont>( "courrier-16pt");
            SelectMark = content.Load<Texture2D>( "textures/select_mark");
            MenuItem = content.Load<Texture2D>( "textures/gui/menu_item");
            MenuItemHover = content.Load<Texture2D>( "textures/gui/menu_item_hover");
            Menu = content.Load<Texture2D>( "textures/gui/menu");
            Cursor = content.Load<Texture2D>( "textures/gui/cursor");
            HighlightMark = content.Load<Texture2D>( "textures/highlight_mark");
            CanMoveMark = content.Load<Texture2D>( "textures/canmove_mark");
            TextBox = content.Load<Texture2D>( "textures/gui/textbox");
            LifebarEmpty = content.Load<Texture2D>( "textures/gui/lifebar_empty");
            LifebarFull = content.Load<Texture2D>( "textures/gui/lifebar_full");
            LavaTexture = content.Load<Texture2D>( "textures/lava");
            // Effet de la map
            WallTexture = content.Load<Texture2D>( "textures/wall");
            WallBorderTexture = content.Load<Texture2D>( "textures/border");
            GrassTexture = content.Load<Texture2D>( "textures/grass");
            MapEffect = content.Load<Effect>("shaders/mapshader");
            MapEffect.Parameters["xBorderTexture"].SetValue(WallBorderTexture);
            MapEffect.Parameters["xWallTexture"].SetValue(WallTexture);
            MapEffect.Parameters["xGrassTexture"].SetValue(GrassTexture);
            MapEffect.Parameters["xLavaTexture"].SetValue(LavaTexture);

            /* Matrice
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            PlaneTransform2D = halfPixelOffset * projection;*/
        }

    }
    

}
