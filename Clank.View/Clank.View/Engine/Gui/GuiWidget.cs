using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Clank.View.Engine.Graphics.Server;
namespace Clank.View.Engine.Gui
{
    /// <summary>
    /// Classe de base de tous les widgets de la GUI.
    /// </summary>
    public abstract class GuiWidget
    {
        #region Variables
        /// <summary>
        /// Représente la position du widget.
        /// </summary>
        protected Vector2 m_position;

        /// <summary>
        /// Valeur indiquant si le widget est supprimé.
        /// </summary>
        protected bool m_isDisposed;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit la position du widget.
        /// </summary>
        public virtual Vector2 Position
        {
            get { return m_position; }
            set { m_position = value; }
        }
        /// <summary>
        /// Obtient une valeur indiquant si le widget est supprimé.
        /// </summary>
        public virtual bool IsDisposed
        {
            get { return m_isDisposed; }
            protected set { m_isDisposed = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Mets à jour la logique du Widget.
        /// </summary>
        /// <param name="time"></param>
        public abstract void Update(GameTime time);
        /// <summary>
        /// Dessine le widget.
        /// </summary>
        /// <param name="batch"></param>
        public abstract void Draw(RemoteSpriteBatch batch);
        /// <summary>
        /// Supprime le widget.
        /// </summary>
        public virtual void Dispose()
        {
            IsDisposed = true;
        }
        #endregion
    }
}
