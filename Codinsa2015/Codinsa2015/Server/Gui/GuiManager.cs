using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Codinsa2015.Server.Gui
{
    /// <summary>
    /// Gestionnaire d'interface graphiques.
    /// </summary>
    public sealed class GuiManager
    {
        #region Variables
        /// <summary>
        /// Contient la liste des widgets associés à ce GuiManager.
        /// </summary>
        private List<GuiWidget> m_widgets;

        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de GuiManager.
        /// </summary>
        public GuiManager()
        {
            m_widgets = new List<GuiWidget>();
        }

        /// <summary>
        /// Mets à jour le GuiManager.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            List<GuiWidget> toDelete = new List<GuiWidget>();
            foreach(GuiWidget widget in m_widgets)
            {
                widget.Update(time);
                if (widget.IsDisposed)
                    toDelete.Add(widget);
            }

            // Supprime les widgets à supprimer.
            foreach(GuiWidget widget in toDelete)
            {
                m_widgets.Remove(widget);
            }

        }

        /// <summary>
        /// Dessine tous les composants du GuiManager.
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            foreach(GuiWidget widget in m_widgets)
            {
                widget.Draw(batch);
            }
        }

        /// <summary>
        /// Ajoute un widget au GuiManager.
        /// </summary>
        public void AddWidget(GuiWidget widget)
        {
            m_widgets.Add(widget);
        }

        /// <summary>
        /// Supprime un widget du GuiManager.
        /// </summary>
        /// <param name="widget"></param>
        public void RemoveWidget(GuiWidget widget)
        {
            m_widgets.Remove(widget);
        }

        /// <summary>
        /// Remets le GUIManager dans son état initial.
        /// </summary>
        public void Reset()
        {
            foreach (GuiWidget widget in m_widgets)
            {
                widget.Dispose();
            }
            m_widgets.Clear();
        }
        #endregion
    }
}
