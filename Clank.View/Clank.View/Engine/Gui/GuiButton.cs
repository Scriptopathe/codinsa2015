using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Gui
{
    /// <summary>
    /// Représente un bouton dans la GUI.
    /// </summary>
    public class GuiButton : GuiWidget
    {
        #region Delegate / Events / Classes
        public delegate void ButtonClickedDelegate();
        public event ButtonClickedDelegate Clicked;
        #endregion

        #region Variables
        bool firstFrame = true;
        /// <summary>
        /// Taille de la marge globale du bouton.
        /// </summary>
        int m_mainMarginSize = 8;
        /// <summary>
        /// Largeur du bouton en pixels.
        /// </summary>
        int m_width;
        /// <summary>
        /// Hauteur du bouton en pixels.
        /// </summary>
        int m_height;
        /// <summary>
        /// Taille des icones en pixels.
        /// Les icones sont des textures carrées.
        /// </summary>
        int m_iconSize = 16;

        /// <summary>
        /// Obtient ou définit la texture utilisée pour dessiner le bouton.
        /// </summary>
        public Texture2D ButtonBoxTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit la texture utilisée pour dessiner le bouton, lorsque la souris est dessus.
        /// </summary>
        public Texture2D ButtonHoverBoxTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Couleur du texte lorsque le bouton est désactivé.
        /// </summary>
        public Color DisabledTextColor
        {
            get;
            set;
        }
        /// <summary>
        /// Couleur du texte.
        /// </summary>
        public Color EnabledTextColor
        {
            get;
            set;
        }
        /// <summary>
        /// Couleur du texte lorsque le bouton est en surbrillance.
        /// </summary>
        public Color HoverTextColor
        {
            get;
            set;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit la taille de la marge globale du bouton.
        /// </summary>
        public int MainMarginSize
        {
            get { return m_mainMarginSize; }
            set { m_mainMarginSize = value; }
        }
        /// <summary>
        /// Obtient ou définit la hauteur du bouton.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        /// <summary>
        /// Obtient ou définit la largeur du bouton
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        /// Obtient ou définit le titre du menu.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit l'icone du bouton.
        /// </summary>
        public Texture2D Icon
        {
            get;
            set;
        }
        /// <summary>
        /// Obtient ou définit une valeur indiquant si ce bouton est clicable.
        /// </summary>
        public bool IsEnabled
        {
            get;
            set;
        }
        /// <summary>
        /// Obtient une valeur indiquant si ce bouton est visible.
        /// </summary>
        public bool Visible
        {
            get;
            set;
        }
        #endregion 

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de GuiMenu.
        /// </summary>
        public GuiButton() : base()
        {
            EnabledTextColor = Color.White;
            DisabledTextColor = Color.Gray;
            HoverTextColor = Color.White;
            ButtonBoxTexture = Ressources.MenuItem;
            ButtonHoverBoxTexture = Ressources.MenuItemHover;
            Visible = true;
            IsEnabled = true;
            Width = 100;
            Height = 25;
            MainMarginSize = 2;
            Title = "";
        }

        /// <summary>
        /// Mets à jour le menu.
        /// </summary>
        /// <param name="time"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            if (!Visible)
                return;

            // Retourne si première frame : évite certains artifacts de clic.
            if(firstFrame)
            {
                firstFrame = false;
                return;
            }

            // Gestion du click.
            bool hover = false;
            MouseState state = Mouse.GetState();
            Vector2 pos = Position;
            Rectangle pxRect = new Rectangle((int)pos.X, (int)pos.Y, m_width, m_height);
            Point mousePos = new Point(state.X, state.Y);
            hover = (pxRect.Contains(mousePos));
  
            if (Input.IsLeftClickTrigger() && hover)
            {
                // Envoie un signal indiquant que la sélection a changé si le menu est cliqué.
                Input.CancelClick();
                if(Clicked != null)
                    Clicked();
                

               
            }
        }

        /// <summary>
        /// Dessine les items de ce menu.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            if (!Visible)
                return;

            // Variables d'état.
            MouseState state = Mouse.GetState();
            Vector2 pos = Position;

            // Gestion du hover
            Rectangle pxRect = new Rectangle((int)pos.X, (int)pos.Y, m_width, m_height);
            Point mousePos = new Point(state.X, state.Y);
            bool hover = (pxRect.Contains(mousePos));

            // Taille du texte
            Vector2 tSize = Ressources.Font.MeasureString(Title);


            // Dessin de la box
            pxRect = new Rectangle((int)Position.X, (int)Position.Y, m_width, m_height);
            Texture2D t = hover && IsEnabled ? ButtonHoverBoxTexture : ButtonBoxTexture;
            Drawing.DrawRectBox(batch, t, pxRect, Color.White, Graphics.Z.GUI);

            // Dessin de l'icone
            if (Icon != null)
            {
                Rectangle dstRect = new Rectangle(pxRect.X+MainMarginSize, pxRect.Y + (pxRect.Height - m_iconSize) / 2, m_iconSize, m_iconSize);
                Color color = IsEnabled ? Color.White : new Color(125, 125, 125, 125);
                batch.Draw(Icon, dstRect, null, color, 0.0f, Vector2.Zero, SpriteEffects.None, Graphics.Z.GUI + Graphics.Z.FrontStep);
            }

            // Dessin du texte
            pos = new Vector2(pxRect.X + 2 * MainMarginSize + m_iconSize, pxRect.Y + pxRect.Height / 2 - tSize.Y / 2);
            Color textColor = IsEnabled ? (hover ? HoverTextColor : EnabledTextColor) : DisabledTextColor;
            batch.DrawString(Ressources.Font, Title, pos, textColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, Graphics.Z.GUI + Graphics.Z.FrontStep);

        }
        #endregion
    }
}
