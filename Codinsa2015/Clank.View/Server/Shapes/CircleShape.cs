using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Shapes
{
    /// <summary>
    /// Représente une shape circulaire.
    /// </summary>
    public class CircleShape : Shape
    {
        /// <summary>
        /// Position du centre du cercle.
        /// </summary>
        public override Vector2 Position { get; set; }
        /// <summary>
        /// Rayon du cercle.
        /// </summary>
        public float Radius { get; set; }


        /// <summary>
        /// Crée une nouvelle instance de CircleShape ayant la position et le rayon passés en paramètre.
        /// </summary>
        public CircleShape(Vector2 position, float radius)
        {
            Position = position;
            Radius = radius;
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
                return rectShape.Intersects(this);
            }
            else if(shape is CircleShape)
            {
                CircleShape circ = (CircleShape)shape;
                float distance = Vector2.Distance(circ.Position, Position);
                return distance < circ.Radius + Radius;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Retourne une valeur indiquant si ce cercle contient le point donné.
        /// </summary>
        public bool Contains(Vector2 point)
        {
            return Vector2.Distance(point, Position) <= Radius;
        }
    }
}
