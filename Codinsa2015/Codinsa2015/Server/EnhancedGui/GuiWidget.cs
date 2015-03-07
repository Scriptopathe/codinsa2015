using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Codinsa2015.Graphics.Server;
namespace Codinsa2015.Server.EnhancedGui
{
    public abstract class GuiWidget
    {
        private Rectangle m_area;
        /// <summary>
        /// Obtient ou définit une référence vers le manager de ce widget.
        /// </summary>
        public GuiManager Manager { get; set; }
        /// <summary>
        /// Obtient le parent de ce widget.
        /// </summary>
        public GuiWidget Parent { get; set; }
        /// <summary>
        /// Obtient ou définit la zone sur laquelle est établi le widget par rapport à son parent.
        /// </summary>
        public Rectangle Area { get { return m_area; } set { m_area = value; } }
        public Point Location { get { return m_area.Location; } set { m_area.Location = value; } }
        public Point Size { get { return new Point(m_area.Width, m_area.Height); } set { m_area.Width = value.X; m_area.Height = value.Y; } }

        /// <summary>
        /// Obtient une valeur indiquant si ce contrôle doit être supprimé.
        /// </summary>
        public bool IsDisposed { get; set; }
        /// <summary>
        /// Obtient le layer sur lequel est situé ce composant.
        /// </summary>
        public byte Layer { get; set; }
        /// <summary>
        /// Obtient une valeur indiquant si ce widget est visible ou non.
        /// </summary>
        public bool IsVisible { get; set; }
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

        /// <summary>
        /// Crée une nouvelle instance du widget avec son manager associé.
        /// </summary>
        /// <param name="manager"></param>
        public GuiWidget(GuiManager manager)
        {
            Manager = manager;
            IsVisible = true;
            IsDisposed = false;

        }

        #region Drawing
        const float layerDepthStep = 0.000001f;
        protected void Draw(RemoteSpriteBatch batch, RemoteTexture texture, Rectangle dstRect, Rectangle? srcRect, Color color, float rotation, Vector2 origin, int layer)
        {
            dstRect.Location = GetRealPosition(dstRect.Location);
            batch.Draw(texture, 
                dstRect, 
                srcRect, 
                color, 
                rotation,
                origin,
                Microsoft.Xna.Framework.Graphics.SpriteEffects.None,
                (float)(Manager.ComputeLayerDepth(this) - layer * layerDepthStep));
        }

        protected void DrawString(RemoteSpriteBatch batch, RemoteSpriteFont font, string str, Vector2 position, Color color, float rotation, Vector2 origin, float scale, int layer)
        {
            position = v2(GetRealPosition(new Point((int)position.X, (int)position.Y)));
            batch.DrawString(font, 
                str,
                position, 
                color,
                rotation,
                origin, 
                scale,
                (float)(Manager.ComputeLayerDepth(this) - layer * layerDepthStep));
        }

        protected void DrawRectBox(RemoteSpriteBatch batch, RemoteTexture2D texture, Rectangle rect, Color color, int layer)
        {
            rect.Location = GetRealPosition(rect.Location);
            Gui.Drawing.DrawRectBox(batch, texture, rect, color, (float)( Manager.ComputeLayerDepth(this) - layer * layerDepthStep));
        }
        /// <summary>
        /// Obtient la position réelle de ce contrôle (non relative à son parent).
        /// </summary>
        /// <returns></returns>
        protected Point GetRealPosition()
        {
            Point pos = Area.Location;
            GuiWidget parent = Parent;
            while(parent != null)
            {
                pos.X += parent.Area.X;
                pos.Y += parent.Area.Y;
                parent = parent.Parent;
            }
            return pos;
        }

        /// <summary>
        /// Obtient la position réelle du point donné relativement à ce contrôle.
        /// </summary>
        protected Point GetRealPosition(Point pt)
        {
            Point p = GetRealPosition();
            pt.X += p.X; pt.Y += p.Y;
            return pt;
        }        
        
        /// <summary>
        /// Obtient la zone réelle de ce contrôle (non relative à son parent).
        /// </summary>
        public Rectangle GetRealArea()
        {
            Rectangle area = Area;
            area.Location = GetRealPosition();
            return area;
        }

        /// <summary>
        /// Transforme le point donné en Vector2.
        /// </summary>
        protected Vector2 v2(Point pt)
        {
            return new Vector2(pt.X, pt.Y);
        }
        #endregion

        #region Input
        /// <summary>
        /// Obtient la position de la souris par rapport à la position de ce widget.
        /// </summary>
        /// <returns></returns>
        public Point GetMousePos()
        {
            var pos = GetRealPosition();
            var ms = Input.GetMouseState();
            Point pt = new Point(ms.X - pos.X, ms.Y - pos.Y);
            return pt;
        }

        /// <summary>
        /// Retourne une valeur indiquant si la souris survole ce widget.
        /// </summary>
        /// <returns></returns>
        public bool IsHover()
        {
            return Manager.IsHover(this);
        }

        /// <summary>
        /// Donne le focus à ce contrôle.
        /// </summary>
        public void Focus()
        {
            Manager.SetFocus(this);
        }
        /// <summary>
        /// Obtient une valeur indiquant si ce widget a le focus.
        /// </summary>
        /// <returns></returns>
        public bool HasFocus()
        {
            return Manager.HasFocus(this);
        }
        /// <summary>
        /// Obtient une valeur indiquant si le bouton gauche de la souris est clické sur ce widget.
        /// </summary>
        /// <returns></returns>
        public bool IsLeftTrigger()
        {
            return IsHover() && Input.IsLeftClickTrigger();
        }        
        /// <summary>
        /// Obtient une valeur indiquant si le bouton gauche de la souris est pressé sur ce widget.
        /// </summary>
        /// <returns></returns>
        public bool IsLeftPressed()
        {
            return IsHover() && Input.IsRightClickPressed();
        }
        /// <summary>
        /// Obtient une valeur indiquant si le bouton droit de la souris est clické sur ce widget.
        /// </summary>
        /// <returns></returns>
        public bool IsRightTrigger()
        {
            return IsHover() && Input.IsRightClickTrigger();
        }
        /// <summary>
        /// Obtient une valeur indiquant si le bouton droit de la souris est pressé sur ce widget.
        /// </summary>
        /// <returns></returns>
        public bool IsRightPressed()
        {
            return IsHover() && Input.IsRightClickPressed();
        }

        /// <summary>
        /// Obtient une valeur indiquant si la touche key est pressée sur ce contrôle.
        /// </summary>
        public bool IsKeyPressed(Keys key)
        {
            return IsHover() && Input.IsPressed(key) && HasFocus();
        }

        /// <summary>
        /// Callback appelé lorsque le widget prend le focus.
        /// </summary>
        public virtual void OnFocus()
        {

        }
        /// <summary>
        /// Callback appelé lorsque le widget perd le focus.
        /// </summary>
        public virtual void OnFocusLost()
        {

        }
        #endregion
    }
}
