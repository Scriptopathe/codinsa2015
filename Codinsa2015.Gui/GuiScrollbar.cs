using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.EnhancedGui
{
    /// <summary>
    /// Représente une barre de scrolling.
    /// </summary>
    public class GuiScrollbar : GuiWidget
    {
        #region Variables
        bool m_hocked;
        Point m_anchor;
        float m_anchorValue;
        #endregion

        #region Properties
        public float MaxValue { get; set; }
        public float CurrentValue { get; set; }
        public float Step { get; set; }
        public float GrabLen { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de scrollbar.
        /// </summary>
        public GuiScrollbar(GuiManager mgr) : base(mgr)
        {
            mgr.AddWidget(this);
        }

        /// <summary>
        /// Mets à jour ce widget.
        /// </summary>
        /// <param name="time"></param>
        public override void Update(GameTime time)
        {
            if (!IsVisible)
                return;

            bool contains = GetGrabRectangle().Contains(GetMousePos());
            if(IsLeftTrigger() && contains)
            {
                m_hocked = true;
                m_anchor = GetMousePos();
                m_anchorValue = CurrentValue;
            }
            else if(m_hocked && !Input.IsLeftClickPressed())
            {
                m_hocked = false;
            }

            if (m_hocked)
            {
                float newValue = true ? GetMousePos().Y : GetMousePos().X;
                float oldValue = true ? m_anchor.Y : m_anchor.X;
                CurrentValue = m_anchorValue + GetDelta(newValue - oldValue);
            }


            CurrentValue = Math.Max(0, Math.Min(MaxValue, CurrentValue));
        }

        /// <summary>
        /// Dessine la barre de scrolling.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch)
        {
            if (!IsVisible)
                return;
            DrawRectBox(batch, Ressources.Menu, new Rectangle(0, 0, Area.Width, Area.Height), Color.White, 0);
            DrawRectBox(batch, Ressources.MenuItemHover, GetGrabRectangle(), Color.White, 2);
        }


        /// <summary>
        /// Représente le changement de valeur associé à un déplacement
        /// de la barre de défilement de deltaPx.
        /// </summary>
        float GetDelta(float deltaPx)
        {
            float d = (deltaPx / (float)(Area.Height - (GrabLen / MaxValue) * Area.Height) * MaxValue);
            return d - d % Step;
        }


        /// <summary>
        /// Retourne le rectangle contenant la partie à grab.
        /// </summary>
        Rectangle GetGrabRectangle()
        {
            float size = (GrabLen / MaxValue);
            float pos = (CurrentValue / MaxValue);
            
            if(true) // vertical
            {
                int y = (int)(pos * (Area.Height - size * Area.Height));
                int h = (int)(size * Area.Height);
                return new Rectangle(0, y, Area.Width, h);
            }
            else
            {

            }
            
        }
        #endregion


    }
}
