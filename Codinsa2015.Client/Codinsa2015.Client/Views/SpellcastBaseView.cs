using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class SpellcastBaseView
	{

	
		public GenericShapeView Shape;	
		public static SpellcastBaseView Deserialize(System.IO.StreamReader input) {
			SpellcastBaseView _obj =  new SpellcastBaseView();
			// Shape
			GenericShapeView _obj_Shape = GenericShapeView.Deserialize(input);
			_obj.Shape = (GenericShapeView)_obj_Shape;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Shape
			this.Shape.Serialize(output);
		}

	}
}
