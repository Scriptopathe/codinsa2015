using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Shapes
{
    /// <summary>
    /// Représente une forme dans le jeu.
    /// </summary>
    public abstract class Shape
    {
        /// <summary>
        /// Retourne une valeur indiquant si cette forme intersecte la forme passée en paramètre.
        /// </summary>
        /// <param name="shape">forme avec laquelle tester la collision.</param>
        /// <returns>True si il y a intersection, false sinon.</returns>
        public abstract bool Intersects(Shape shape);
    }
}
