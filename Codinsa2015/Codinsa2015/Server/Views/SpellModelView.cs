using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class SpellModelView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// ID du spell permettant de le retrouver dans le tableau de sorts du jeu.
		/// </summary>
		public int ID;	
		/// <summary>
		/// Obtient la liste des descriptions des niveaux de ce sort.
		/// </summary>
		public List<SpellLevelDescriptionView> Levels;	
		public static SpellModelView Deserialize(System.IO.StreamReader input) {
			SpellModelView _obj =  new SpellModelView();
			// ID
			int _obj_ID = Int32.Parse(input.ReadLine());
			_obj.ID = (int)_obj_ID;
			// Levels
			List<SpellLevelDescriptionView> _obj_Levels = new List<SpellLevelDescriptionView>();
			int _obj_Levels_count = Int32.Parse(input.ReadLine());
			for(int _obj_Levels_i = 0; _obj_Levels_i < _obj_Levels_count; _obj_Levels_i++) {
				SpellLevelDescriptionView _obj_Levels_e = SpellLevelDescriptionView.Deserialize(input);
				_obj_Levels.Add((SpellLevelDescriptionView)_obj_Levels_e);
			}
			_obj.Levels = (List<SpellLevelDescriptionView>)_obj_Levels;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// ID
			output.WriteLine(((int)this.ID).ToString());
			// Levels
			output.WriteLine(this.Levels.Count.ToString());
			for(int Levels_it = 0; Levels_it < this.Levels.Count;Levels_it++) {
				this.Levels[Levels_it].Serialize(output);
			}
		}

	}
}
