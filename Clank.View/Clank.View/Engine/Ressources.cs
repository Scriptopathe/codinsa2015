using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Clank.View.Engine.Graphics.Server;
namespace Clank.View.Engine
{
    /// <summary>
    /// Classe ayant la responsabilité de s'occuper des ressources du jeu.
    /// </summary>
    public static class Ressources
    {
        public static RemoteTexture2D DummyTexture;
        public static string MapFilename = "Content/map.txt";

        #region ByName
        static Dictionary<string, RemoteTexture2D> s_textureCache = new Dictionary<string, RemoteTexture2D>();
        public static RemoteTexture2D GetSpellTexture(string spellname)
        {
            RemoteTexture2D tex;
            try
            {
                if (s_textureCache.ContainsKey(spellname))
                    return s_textureCache[spellname];
                else
                {
                    tex = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/spells/" + spellname);
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
        public static RemoteEffect MapEffect { get; set; }
        public static RemoteTexture2D WallTexture { get; set; }
        public static RemoteTexture2D WallBorderTexture { get; set; }
        public static RemoteTexture2D GrassTexture { get; set; }
        public static RemoteTexture2D LavaTexture { get; set; }
        
        /// <summary>
        /// Matrice de transformation pour les dessins 2D.
        /// </summary>
        public static Matrix PlaneTransform2D { get;set; }
        #endregion
        #region Imported
        /// <summary>
        /// Obtient une police de caractère utilisable pour l'affichage de texte.
        /// </summary>
        public static RemoteSpriteFont Font
        {
            get;
            private set;
        }

        public static RemoteSpriteFont CourrierFont
        {
            get;
            private set;
        }

        public static RemoteSpriteFont NumbersFont
        {
            get;
            private set;
        }

        public static RemoteTexture2D LifebarEmpty
        {
            get;
            set;
        }

        public static RemoteTexture2D LifebarFull
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient une texture représentant une marque de sélection.
        /// </summary>
        public static RemoteTexture2D SelectMark
        {
            get;
            private set;
        }

        public static RemoteTexture2D HighlightMark
        {
            get;
            private set;
        }

        public static RemoteTexture2D CanMoveMark
        {
            get;
            private set;
        }
        public static RemoteTexture2D MenuItem
        {
            get;
            private set;
        }

        public static RemoteTexture2D MenuItemHover
        {
            get;
            private set;
        }

        public static RemoteTexture2D TextBox
        {
            get;
            set;
        }

        public static RemoteTexture2D Menu
        {
            get;
            private set;
        }

        public static RemoteTexture2D Cursor
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
            
            DummyTexture = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/dummy");
            Font = new RemoteSpriteFont(Mobattack.GetScene().GraphicsServer, "textfont");
            NumbersFont = new RemoteSpriteFont(Mobattack.GetScene().GraphicsServer, "numbers_font");
            CourrierFont = new RemoteSpriteFont(Mobattack.GetScene().GraphicsServer, "courrier-16pt");
            SelectMark = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/select_mark");
            MenuItem = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/gui/menu_item");
            MenuItemHover = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/gui/menu_item_hover");
            Menu = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/gui/menu");
            Cursor = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/gui/cursor");
            HighlightMark = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/highlight_mark");
            CanMoveMark = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/canmove_mark");
            TextBox = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/gui/textbox");
            LifebarEmpty = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/gui/lifebar_empty");
            LifebarFull = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/gui/lifebar_full");
            LavaTexture = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/lava");
            // Effet de la map
            WallTexture = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/wall");
            WallBorderTexture = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/border");
            GrassTexture = new RemoteTexture2D(Mobattack.GetScene().GraphicsServer, "textures/grass");
            MapEffect = new RemoteEffect(Mobattack.GetScene().GraphicsServer, "shaders/mapshader");
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
