using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class GenericShapeView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// Position de la forme : cercle => centre ; rectangle => coin supérieur gauche
		/// </summary>
		public Vector2 Position;	
		/// <summary>
		/// Si cercle : rayon du cercle.
		/// </summary>
		public float Radius;	
		/// <summary>
		/// Si rectangle : taille du rectangle.
		/// </summary>
		public Vector2 Size;	
		/// <summary>
		/// Représente le type de la forme.
		/// </summary>
		public GenericShapeType ShapeType;	
		public static GenericShapeView Deserialize(System.IO.StreamReader input) {
			GenericShapeView _obj =  new GenericShapeView();
			// Position
			Vector2 _obj_Position = Vector2.Deserialize(input);
			_obj.Position = (Vector2)_obj_Position;
			// Radius
			float _obj_Radius = Single.Parse(input.ReadLine());
			_obj.Radius = (float)_obj_Radius;
			// Size
			Vector2 _obj_Size = Vector2.Deserialize(input);
			_obj.Size = (Vector2)_obj_Size;
			// ShapeType
			GenericShapeType _obj_ShapeType = (GenericShapeType)Int32.Parse(input.ReadLine());
			_obj.ShapeType = (GenericShapeType)_obj_ShapeType;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Position
			this.Position.Serialize(output);
			// Radius
			output.WriteLine(((float)this.Radius).ToString());
			// Size
			this.Size.Serialize(output);
			// ShapeType
			output.WriteLine(((int)this.ShapeType).ToString());
		}

	}
}
