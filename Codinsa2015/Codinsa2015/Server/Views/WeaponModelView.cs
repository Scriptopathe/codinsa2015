using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class WeaponModelView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// Liste des upgrades possibles de l'arme.
		/// </summary>
		public List<WeaponUpgradeModelView> Upgrades;	
		/// <summary>
		/// Prix d'achat de l'arme
		/// </summary>
		public float Price;	
		public WeaponModelView() {
			Upgrades = new List<WeaponUpgradeModelView>();
		}

		public static WeaponModelView Deserialize(System.IO.StreamReader input) {
			WeaponModelView _obj =  new WeaponModelView();
			// Upgrades
			List<WeaponUpgradeModelView> _obj_Upgrades = new List<WeaponUpgradeModelView>();
			int _obj_Upgrades_count = Int32.Parse(input.ReadLine());
			for(int _obj_Upgrades_i = 0; _obj_Upgrades_i < _obj_Upgrades_count; _obj_Upgrades_i++) {
				WeaponUpgradeModelView _obj_Upgrades_e = WeaponUpgradeModelView.Deserialize(input);
				_obj_Upgrades.Add((WeaponUpgradeModelView)_obj_Upgrades_e);
			}
			_obj.Upgrades = (List<WeaponUpgradeModelView>)_obj_Upgrades;
			// Price
			float _obj_Price = Single.Parse(input.ReadLine());
			_obj.Price = (float)_obj_Price;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Upgrades
			output.WriteLine(this.Upgrades.Count.ToString());
			for(int Upgrades_it = 0; Upgrades_it < this.Upgrades.Count;Upgrades_it++) {
				this.Upgrades[Upgrades_it].Serialize(output);
			}
			// Price
			output.WriteLine(((float)this.Price).ToString());
		}

	}
}
