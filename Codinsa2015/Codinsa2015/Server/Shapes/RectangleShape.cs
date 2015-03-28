using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Shapes
{
    /// <summary>
    /// Représente une forme de collision rectangulaire.
    /// </summary>
    public class RectangleShape : Shape
    {
        const float RectPrecisionScale = 96;
        /// <summary>
        /// Rectangle représenté par cette structure.
        /// Comme le rectangle stoque des entiers, il est scalé par rapport à la taille des unités
        /// de la map. 96 (rectPrecisionScale) units => 1 map unit.
        /// </summary>
        Rectangle m_rect;

        /// <summary>
        /// Obtient la position X du coin supérieur gauche du rectangle.
        /// </summary>
        public float X
        {
            get 
            { 
                return m_rect.X / RectPrecisionScale;
            }
            set
            {
                m_rect.X = (int)(value * RectPrecisionScale);
            }
        }
        /// <summary>
        /// Obtient la position Y du coin supérieur gauche du rectangle.
        /// </summary>
        public float Y
        {
            get
            {
                return m_rect.Y / RectPrecisionScale;
            }
            set
            {
                m_rect.Y = (int)(value * RectPrecisionScale);
            }
        }
        /// <summary>
        /// Obtient la largeur rectangle.
        /// </summary>
        public float Width
        {
            get
            {
                return m_rect.Width / RectPrecisionScale;
            }
            set
            {
                m_rect.Width = (int)(value * RectPrecisionScale);
            }
        }
        /// <summary>
        /// Obtient la hauteur du rectangle.
        /// </summary>
        public float Height
        {
            get
            {
                return m_rect.Height / RectPrecisionScale;
            }
            set
            {
                m_rect.Height = (int)(value * RectPrecisionScale);
            }
        }

        /// <summary>
        /// Obtient ou définit la position du centre de rectangle.
        /// </summary>
        public override Vector2 Position
        {
            get
            {
                return new Vector2((m_rect.X + m_rect.Width/2) / RectPrecisionScale, (m_rect.Y + m_rect.Height/2) / RectPrecisionScale);
            }
            set
            {
                m_rect.X = (int)(value.X * RectPrecisionScale) - m_rect.Width / 2;
                m_rect.Y = (int)(value.Y * RectPrecisionScale) - m_rect.Height / 2;
            }
        }
        /// <summary>
        /// Obtient les points formant les coins du rectangle.
        /// </summary>
        /// <returns></returns>
        public Vector2[] GetCorners()
        {
            Vector2[] corners = new Vector2[4];
            corners[0] = new Vector2(X, Y);
            corners[1] = new Vector2(X + Width, Y);
            corners[2] = new Vector2(X + Width, Y + Height);
            corners[3] = new Vector2(X, Y + Height);
            return corners;
        }

        /// <summary>
        /// Crée une nouvelle instance de RectangleShape.
        /// </summary>
        /// <param name="rect"></param>
        public RectangleShape(Vector2 position, Vector2 size)
        {
            float scale = RectPrecisionScale;
            m_rect = new Rectangle((int)(position.X * scale), (int)(position.Y * scale), (int)(size.X * scale), (int)(size.Y * scale));
        }


        /// <summary>
        /// Retourne une valeur indiquant si cette forme intersecte la forme passée en paramètre.
        /// </summary>
        /// <param name="shape">forme avec laquelle tester la collision.</param>
        /// <returns>True si il y a intersection, false sinon.</returns>
        public override bool Intersects(Shape shape)
        {
            if (shape is RectangleShape)
            {
                RectangleShape rectShape = (RectangleShape)shape;
                return rectShape.m_rect.Intersects(rectShape.m_rect);
            }
            else if(shape is CircleShape)
            {
                CircleShape circleShape = (CircleShape)shape;

                Vector2 circleDistance;
                circleDistance.X = Math.Abs(circleShape.Position.X - X);
                circleDistance.Y = Math.Abs(circleShape.Position.Y - Y);
                float wOver2 = Width / 2;
                float hOver2 = Height / 2;

                if (circleDistance.X > (wOver2 + circleShape.Radius)) { return false; }
                if (circleDistance.Y > (hOver2 + circleShape.Radius)) { return false; }

                if (circleDistance.X <= (wOver2)) { return true; }
                if (circleDistance.Y <= (hOver2)) { return true; }

                float dx = circleDistance.X - wOver2;
                float dy = circleDistance.Y - hOver2;
                double cornerDistance_sq = dx * dx + dy * dy;

                return (cornerDistance_sq <= circleShape.Radius * circleShape.Radius);
            }

            throw new NotImplementedException();
        }

    }
}
