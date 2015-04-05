using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class PassiveEquipmentModelView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// ID unique de l'équipement
		/// </summary>
		public int ID;	
		/// <summary>
		/// prix d'achat de l'équipement
		/// </summary>
		public float Price;	
		/// <summary>
		/// liste des upgrades de cet équipement.
		/// </summary>
		public List<PassiveEquipmentUpgradeModelView> Upgrades;	
		public PassiveEquipmentModelView() {
			Upgrades = new List<PassiveEquipmentUpgradeModelView>();
		}

		public static PassiveEquipmentModelView Deserialize(System.IO.StreamReader input) {
			PassiveEquipmentModelView _obj =  new PassiveEquipmentModelView();
			// ID
			int _obj_ID = Int32.Parse(input.ReadLine());
			_obj.ID = (int)_obj_ID;
			// Price
			float _obj_Price = Single.Parse(input.ReadLine());
			_obj.Price = (float)_obj_Price;
			// Upgrades
			List<PassiveEquipmentUpgradeModelView> _obj_Upgrades = new List<PassiveEquipmentUpgradeModelView>();
			int _obj_Upgrades_count = Int32.Parse(input.ReadLine());
			for(int _obj_Upgrades_i = 0; _obj_Upgrades_i < _obj_Upgrades_count; _obj_Upgrades_i++) {
				PassiveEquipmentUpgradeModelView _obj_Upgrades_e = PassiveEquipmentUpgradeModelView.Deserialize(input);
				_obj_Upgrades.Add((PassiveEquipmentUpgradeModelView)_obj_Upgrades_e);
			}
			_obj.Upgrades = (List<PassiveEquipmentUpgradeModelView>)_obj_Upgrades;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// ID
			output.WriteLine(((int)this.ID).ToString());
			// Price
			output.WriteLine(((float)this.Price).ToString());
			// Upgrades
			output.WriteLine(this.Upgrades.Count.ToString());
			for(int Upgrades_it = 0; Upgrades_it < this.Upgrades.Count;Upgrades_it++) {
				this.Upgrades[Upgrades_it].Serialize(output);
			}
		}

	}
}
