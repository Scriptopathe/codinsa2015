using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views.Client
{

	public class SpellView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// Cooldown actuel du sort, en secondes
		/// </summary>
		public float CurrentCooldown;	
		/// <summary>
		/// Id de l'entité possédant le sort.
		/// </summary>
		public int SourceCaster;	
		/// <summary>
		/// Représente les descriptions du spell pour les différents niveaux.
		/// </summary>
		public List<SpellDescriptionView> Levels;	
		/// <summary>
		/// Niveau actuel du spell.
		/// </summary>
		public int Level;	
		public static SpellView Deserialize(System.IO.StreamReader input) {
			SpellView _obj =  new SpellView();
			// CurrentCooldown
			float _obj_CurrentCooldown = Single.Parse(input.ReadLine());
			_obj.CurrentCooldown = (float)_obj_CurrentCooldown;
			// SourceCaster
			int _obj_SourceCaster = Int32.Parse(input.ReadLine());
			_obj.SourceCaster = (int)_obj_SourceCaster;
			// Levels
			List<SpellDescriptionView> _obj_Levels = new List<SpellDescriptionView>();
			int _obj_Levels_count = Int32.Parse(input.ReadLine());
			for(int _obj_Levels_i = 0; _obj_Levels_i < _obj_Levels_count; _obj_Levels_i++) {
				SpellDescriptionView _obj_Levels_e = SpellDescriptionView.Deserialize(input);
				_obj_Levels.Add((SpellDescriptionView)_obj_Levels_e);
			}
			_obj.Levels = (List<SpellDescriptionView>)_obj_Levels;
			// Level
			int _obj_Level = Int32.Parse(input.ReadLine());
			_obj.Level = (int)_obj_Level;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// CurrentCooldown
			output.WriteLine(((float)this.CurrentCooldown).ToString());
			// SourceCaster
			output.WriteLine(((int)this.SourceCaster).ToString());
			// Levels
			output.WriteLine(this.Levels.Count.ToString());
			for(int Levels_it = 0; Levels_it < this.Levels.Count;Levels_it++) {
				this.Levels[Levels_it].Serialize(output);
			}
			// Level
			output.WriteLine(((int)this.Level).ToString());
		}

	}
}
