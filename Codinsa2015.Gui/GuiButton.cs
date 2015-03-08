using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Codinsa2015.EnhancedGui
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
        #endregion 

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de GuiMenu.
        /// </summary>
        public GuiButton(GuiManager manager) : base(manager)
        {
            EnabledTextColor = Color.White;
            DisabledTextColor = Color.Gray;
            HoverTextColor = Color.White;
            ButtonBoxTexture = Ressources.MenuItem;
            ButtonHoverBoxTexture = Ressources.MenuItemHover;
            IsVisible = true;
            IsEnabled = true;
            Area = new Rectangle(Area.X, Area.Y, 100, 20);
            MainMarginSize = 2;
            Title = "";
            manager.AddWidget(this);
        }

        /// <summary>
        /// Mets à jour le menu.
        /// </summary>
        /// <param name="time"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            if (!IsVisible)
                return;

            // Retourne si première frame : évite certains artifacts de clic.
            if(firstFrame)
            {
                firstFrame = false;
                return;
            }

            // Gestion du click.
            if (IsLeftTrigger())
            {
                if(Clicked != null)
                    Clicked();
            }
        }

        /// <summary>
        /// Dessine les items de ce menu.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch)
        {
            if (!IsVisible)
                return;

            // Variables d'état.
            MouseState state = Mouse.GetState();
            bool hover = IsHover();
            // Gestion du hover
            Point mousePos = new Point(state.X, state.Y);

            // Taille du texte
            Vector2 tSize = Ressources.Font.MeasureString(Title);


            // Dessin de la box
            Texture2D t = ((hover && !Input.IsLeftClickPressed()) && IsEnabled)? ButtonHoverBoxTexture : ButtonBoxTexture;
            DrawRectBox(batch, t, new Rectangle(0, 0, Area.Width, Area.Height), Color.White, 4);

            // Dessin de l'icone
            if (Icon != null)
            {
                Rectangle dstRect = new Rectangle(MainMarginSize, (Area.Height - m_iconSize) / 2, m_iconSize, m_iconSize);
                Color color = IsEnabled ? Color.White : new Color(125, 125, 125, 125);
                Draw(batch, Icon, dstRect, null, color, 0.0f, Vector2.Zero, 1);
            }

            // Dessin du texte
            var pos = new Vector2(2 * MainMarginSize + m_iconSize, Area.Height / 2 - tSize.Y / 2);
            Color textColor = IsEnabled ? (hover ? HoverTextColor : EnabledTextColor) : DisabledTextColor;
            DrawString(batch, Ressources.Font, Title, pos, textColor, 0.0f, Vector2.Zero, 1.0f, 8);

        }
        #endregion
    }
}
