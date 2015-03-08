using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Graphics.Server;
namespace Codinsa2015.Server.EnhancedGui
{
    /// <summary>
    /// Représente une fenêtre dans la GUI.
    /// </summary>
    public class GuiWindow : GuiWidget
    {
        #region Delegate / Events / Classes
        #endregion

        #region Variables
        /// <summary>
        /// Taille de la marge globale du bouton.
        /// </summary>
        int m_mainMarginSize = 8;
        /// <summary>
        /// Taille des icones en pixels.
        /// Les icones sont des textures carrées.
        /// </summary>
        int m_iconSize = 16;
        Point m_oldMousePos;
        
        bool m_isAnchored;
        /// <summary>
        /// Obtient ou définit la texture utilisée pour dessiner la barre de titre de la
        /// fenêtre.
        /// </summary>
        public RemoteTexture2D TitleBarTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Color du background de la fenêtre.
        /// </summary>
        public Color BackColor
        {
            get;
            set;
        }

        /// <summary>
        /// Couleur du texte de la barre de titre.
        /// </summary>
        public Color TextColor
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
        public RemoteTexture2D Icon
        {
            get;
            set;
        }

        public int TitleBarHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Indique si cette fenêtre est cachée (mais toujours prise en compte par la GUI).
        /// </summary>
        public bool IsHiden
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si cette fenêtre est déplaçable.
        /// </summary>
        public bool IsMoveable
        {
            get;
            set;
        }
        #endregion 

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de GuiMenu.
        /// </summary>
        public GuiWindow(GuiManager manager) : base(manager)
        {
            TextColor = Color.Gray;
            TitleBarTexture = Ressources.MenuItem;
            TitleBarHeight = 16;
            IsMoveable = true;
            Area = new Rectangle(Area.X, Area.Y, 100, 20);
            BackColor = Color.Black;
            MainMarginSize = 2;
            Title = "";
            IsHiden = false;
            manager.AddWidget(this);
        }

        /// <summary>
        /// Mets à jour le menu.
        /// </summary>
        /// <param name="time"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            if (!IsVisible || IsHiden || !IsMoveable)
                return;

            var ms = Input.GetMouseState();
            // Gestion du click.
            if (IsLeftTrigger())
            {
                m_oldMousePos = new Point(ms.X, ms.Y);
                m_isAnchored = true;
            }

            if(Input.IsLeftClickReleased())
            {
                m_isAnchored = false;
            }

            if(m_isAnchored)
            {
                Location = new Point(Location.X + (ms.X - m_oldMousePos.X), Location.Y + (ms.Y - m_oldMousePos.Y));
                m_oldMousePos = new Point(ms.X, ms.Y);
            }
        }

        /// <summary>
        /// Dessine les items de ce menu.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(RemoteSpriteBatch batch)
        {
            if (!IsVisible || IsHiden)
                return;

            // Variables d'état.
            MouseState state = Mouse.GetState();
            bool hover = IsHover();
            // Gestion du hover
            Point mousePos = new Point(state.X, state.Y);

            // Taille du texte
            Vector2 tSize = Ressources.Font.MeasureString(Title);


            // Dessin de la box
            DrawRectBox(batch, Ressources.DummyTexture, new Rectangle(0, 0, Area.Width, Area.Height), BackColor, 4);

            // Dessin de la barre de titre.
            RemoteTexture2D t = TitleBarTexture;
            DrawRectBox(batch, t, new Rectangle(0, 0, Area.Width, TitleBarHeight), Color.White, 5);

            // Dessin de l'icone
            if (Icon != null)
            {
                Rectangle dstRect = new Rectangle(MainMarginSize, (TitleBarHeight - m_iconSize) / 2, m_iconSize, m_iconSize);
                Color color = true ? Color.White : new Color(125, 125, 125, 125);
                Draw(batch, Icon, dstRect, null, color, 0.0f, Vector2.Zero, 6);
            }

            // Dessin du texte
            var pos = new Vector2(2 * MainMarginSize + m_iconSize, TitleBarHeight / 2 - tSize.Y / 2);
            Color textColor = TextColor;
            DrawString(batch, Ressources.Font, Title, pos, textColor, 0.0f, Vector2.Zero, 1.0f, 8);

        }
        #endregion
    }
}
