using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views.Client
{

	public class SpellcastBaseView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// Shape utilisée par ce spell cast.
		/// </summary>
		public GenericShapeView Shape;	
		/// <summary>
		/// nom de ce spellcast (utilisé pour le texturing notamment).
		/// </summary>
		public string Name;	
		public SpellcastBaseView() {
			Shape = new GenericShapeView();
		}

		public static SpellcastBaseView Deserialize(System.IO.StreamReader input) {
			SpellcastBaseView _obj =  new SpellcastBaseView();
			// Shape
			GenericShapeView _obj_Shape = GenericShapeView.Deserialize(input);
			_obj.Shape = (GenericShapeView)_obj_Shape;
			// Name
			string _obj_Name = input.ReadLine();
			_obj.Name = (string)_obj_Name;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Shape
			this.Shape.Serialize(output);
			// Name
			output.WriteLine(this.Name);
		}

	}
}
