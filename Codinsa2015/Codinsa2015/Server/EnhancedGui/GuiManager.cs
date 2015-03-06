using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Codinsa2015.Graphics.Server;
namespace Codinsa2015.Server.EnhancedGui
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
        private List<GuiWidget> m_addList;
        private List<GuiWidget> m_removeList;
        /// <summary>
        /// Contrôle ayant actuellement le focus.
        /// </summary>
        GuiWidget m_focus;
        
        #endregion

        #region Properties
        /// <summary>
        /// Layerdepth de base sur laquelle sont placée les composants.
        /// </summary>
        public float BaseZ
        {
            get;
            set;
        }

        /// <summary>
        /// Z mini que peut atteindre un composant.
        /// </summary>
        public float MinZ
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de GuiManager.
        /// </summary>
        public GuiManager()
        {
            m_widgets = new List<GuiWidget>();
            m_addList = new List<GuiWidget>();
            m_removeList = new List<GuiWidget>();
        }

        /// <summary>
        /// Mets à jour le GuiManager.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            List<GuiWidget> toDelete = new List<GuiWidget>();
            GuiWidget newFocus = m_focus;

            // Ajoute / Supprime les widgets à ajouter / supprimer.
            foreach (GuiWidget widget in m_addList)
                m_widgets.Add(widget);
            foreach (GuiWidget widget in m_removeList)
                m_widgets.Remove(widget);
            m_addList.Clear();
            m_removeList.Clear();

            foreach (GuiWidget widget in m_widgets)
            {
                if (widget.IsHover() && (Input.IsLeftClickTrigger() || Input.IsRightClickTrigger()))
                    newFocus = widget;

                widget.Update(time);
                if (widget.IsDisposed)
                    toDelete.Add(widget);
            }

            var oldFocus = m_focus;

            // On change le focus uniquement à la fin de la boucle.
            m_focus = newFocus;
            if (m_focus != newFocus)
            {
                oldFocus.OnFocusLost();
                newFocus.OnFocus();
            }

            // Supprime les widgets à supprimer.
            foreach (GuiWidget widget in toDelete)
            {
                m_widgets.Remove(widget);
            }

        }

        /// <summary>
        /// Dessine tous les composants du GuiManager.
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(RemoteSpriteBatch batch)
        {
            foreach (GuiWidget widget in m_widgets)
            {
                widget.Draw(batch);
            }
        }

        /// <summary>
        /// Ajoute un widget au GuiManager.
        /// </summary>
        public void AddWidget(GuiWidget widget)
        {
            m_addList.Add(widget);
        }

        /// <summary>
        /// Supprime un widget du GuiManager.
        /// </summary>
        /// <param name="widget"></param>
        public void RemoveWidget(GuiWidget widget)
        {
            m_removeList.Add(widget);
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
            m_addList.Clear();
            m_removeList.Clear();
        }

        /// <summary>
        /// Indique si le contrôle widget est en possession du focus.
        /// </summary>
        public bool HasFocus(GuiWidget widget)
        {
            return widget == m_focus;
        }
        /// <summary>
        /// Obtient une valeur indiquant si la souris survole le widget donné, c'est à dire :
        /// - si elle est comprise dans le rectangle du widget.
        /// - s'il n'y a pas d'autre widget actif au dessus du widget.
        /// </summary>
        /// <returns></returns>
        public bool IsHover(GuiWidget widget)
        {
            var ms = Input.GetMouseState();
            Point mp = new Point(ms.X, ms.Y);
            if (!widget.Area.Contains(mp))
                return false;

            float z = ComputeLayerDepth(widget);

            foreach(GuiWidget w in m_widgets)
            {
                if (!w.IsVisible)
                    continue;

                // Si le widget w est devant et qu'il contient le point => pas bon.
                if (ComputeLayerDepth(w) < z && w.GetRealArea().Contains(mp))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Calcule le z d'un widget.
        /// </summary>
        public float ComputeLayerDepth(GuiWidget widget)
        {
            float current = BaseZ; // 0.5
            float min = MinZ;      // 0.4
            float step = (current - min) / (float)byte.MaxValue;
            foreach(GuiWidget w in GetParentalChain(widget))
            {
                current -= step * (1+w.Layer);
                step /= (float)byte.MaxValue;
            }

            return current;
        }

        /// <summary>
        /// Obtient la chaine de parenté du widget donné.
        /// </summary>
        public List<GuiWidget> GetParentalChain(GuiWidget widget)
        {
            Stack<GuiWidget> widgets = new Stack<GuiWidget>();
            List<GuiWidget> chain = new List<GuiWidget>();
            GuiWidget current = widget;
            while (current != null)
            {
                widgets.Push(current);
                current = widget.Parent;
            }

            while (widgets.Count != 0)
                chain.Add(widgets.Pop());
            return chain;
        }
        #endregion
    }
}
