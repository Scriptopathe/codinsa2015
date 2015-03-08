using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Shapes
{
    /// <summary>
    /// Enumère les différents types de shape générique.
    /// </summary>
    [Clank.ViewCreator.Enum("Enumère les différents types de shape générique.")]
    public enum GenericShapeType
    {
        Circle,
        Rectangle
    }
    /// <summary>
    /// Représente une forme pouvant être un cercle / rectangle.
    /// Cette forme est utilisée dans la génération de code (qui ne supporte pas le polymorphisme).
    /// </summary>
    public class GenericShape
    {
        /// <summary>
        /// Position de la forme :
        /// cercle => centre
        /// rectangle => coin supérieur gauche.
        /// </summary>
        [Clank.ViewCreator.Export("Vector2", "Position de la forme : cercle => centre ; rectangle => coin supérieur gauche")]
        public Vector2 Position { get; set; }
        /// <summary>
        /// Si cercle : rayon du cercle.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Si cercle : rayon du cercle.")]
        public float Radius { get; set; }
        /// <summary>
        /// Si rectangle : taille du rectangle.
        /// </summary>
        [Clank.ViewCreator.Export("Vector2", "Si rectangle : taille du rectangle.")]
        public Vector2 Size { get; set; }
        /// <summary>
        /// Représente le type de la forme.
        /// </summary>
        [Clank.ViewCreator.Export("GenericShapeType", "Représente le type de la forme.")]
        public GenericShapeType ShapeType { get; set; }
    }
}
