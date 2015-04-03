using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
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
		/// Représente l'id du modèle du spell. Ce modèle décrit les différents effets du spell pour chacun
		/// de ses niveaux
		/// </summary>
		public int Model;	
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
			// Model
			int _obj_Model = Int32.Parse(input.ReadLine());
			_obj.Model = (int)_obj_Model;
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
			// Model
			output.WriteLine(((int)this.Model).ToString());
			// Level
			output.WriteLine(((int)this.Level).ToString());
		}

	}
}
