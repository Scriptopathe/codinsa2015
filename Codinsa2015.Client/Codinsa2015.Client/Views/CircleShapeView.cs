using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class CircleShapeView
	{

	
		public Vector2 Position;	
		public float Radius;	
		public static CircleShapeView Deserialize(System.IO.StreamReader input) {
			CircleShapeView _obj =  new CircleShapeView();
			// Position
			Vector2 _obj_Position = Vector2.Deserialize(input);
			_obj.Position = (Vector2)_obj_Position;
			// Radius
			float _obj_Radius = Single.Parse(input.ReadLine());
			_obj.Radius = (float)_obj_Radius;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Position
			this.Position.Serialize(output);
			// Radius
			output.WriteLine(((float)this.Radius).ToString());
		}

	}
}
