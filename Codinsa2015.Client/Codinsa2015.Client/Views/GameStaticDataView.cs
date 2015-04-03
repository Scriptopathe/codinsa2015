using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views.Client
{

	public class GameStaticDataView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// Obtient une liste de tous les modèles d'armes du jeu
		/// </summary>
		public List<WeaponModelView> Weapons;	
		/// <summary>
		/// Obtient une liste de tous les modèles d'armures du jeu
		/// </summary>
		public List<PassiveEquipmentModelView> Armors;	
		/// <summary>
		/// Obtient une liste de tous les modèles de bottes du jeu
		/// </summary>
		public List<PassiveEquipmentModelView> Boots;	
		/// <summary>
		/// Obtient une liste de tous les modèles d'enchantements d'arme du jeu
		/// </summary>
		public List<WeaponEnchantModelView> Enchants;	
		/// <summary>
		/// Obtient une liste de tous les modèles de sorts du jeu.
		/// </summary>
		public List<SpellModelView> Spells;	
		/// <summary>
		/// Obtient une vue sur les données statiques de la carte (telles que sa table de passabilité).
		/// </summary>
		public MapView Map;	
		public static GameStaticDataView Deserialize(System.IO.StreamReader input) {
			GameStaticDataView _obj =  new GameStaticDataView();
			// Weapons
			List<WeaponModelView> _obj_Weapons = new List<WeaponModelView>();
			int _obj_Weapons_count = Int32.Parse(input.ReadLine());
			for(int _obj_Weapons_i = 0; _obj_Weapons_i < _obj_Weapons_count; _obj_Weapons_i++) {
				WeaponModelView _obj_Weapons_e = WeaponModelView.Deserialize(input);
				_obj_Weapons.Add((WeaponModelView)_obj_Weapons_e);
			}
			_obj.Weapons = (List<WeaponModelView>)_obj_Weapons;
			// Armors
			List<PassiveEquipmentModelView> _obj_Armors = new List<PassiveEquipmentModelView>();
			int _obj_Armors_count = Int32.Parse(input.ReadLine());
			for(int _obj_Armors_i = 0; _obj_Armors_i < _obj_Armors_count; _obj_Armors_i++) {
				PassiveEquipmentModelView _obj_Armors_e = PassiveEquipmentModelView.Deserialize(input);
				_obj_Armors.Add((PassiveEquipmentModelView)_obj_Armors_e);
			}
			_obj.Armors = (List<PassiveEquipmentModelView>)_obj_Armors;
			// Boots
			List<PassiveEquipmentModelView> _obj_Boots = new List<PassiveEquipmentModelView>();
			int _obj_Boots_count = Int32.Parse(input.ReadLine());
			for(int _obj_Boots_i = 0; _obj_Boots_i < _obj_Boots_count; _obj_Boots_i++) {
				PassiveEquipmentModelView _obj_Boots_e = PassiveEquipmentModelView.Deserialize(input);
				_obj_Boots.Add((PassiveEquipmentModelView)_obj_Boots_e);
			}
			_obj.Boots = (List<PassiveEquipmentModelView>)_obj_Boots;
			// Enchants
			List<WeaponEnchantModelView> _obj_Enchants = new List<WeaponEnchantModelView>();
			int _obj_Enchants_count = Int32.Parse(input.ReadLine());
			for(int _obj_Enchants_i = 0; _obj_Enchants_i < _obj_Enchants_count; _obj_Enchants_i++) {
				WeaponEnchantModelView _obj_Enchants_e = WeaponEnchantModelView.Deserialize(input);
				_obj_Enchants.Add((WeaponEnchantModelView)_obj_Enchants_e);
			}
			_obj.Enchants = (List<WeaponEnchantModelView>)_obj_Enchants;
			// Spells
			List<SpellModelView> _obj_Spells = new List<SpellModelView>();
			int _obj_Spells_count = Int32.Parse(input.ReadLine());
			for(int _obj_Spells_i = 0; _obj_Spells_i < _obj_Spells_count; _obj_Spells_i++) {
				SpellModelView _obj_Spells_e = SpellModelView.Deserialize(input);
				_obj_Spells.Add((SpellModelView)_obj_Spells_e);
			}
			_obj.Spells = (List<SpellModelView>)_obj_Spells;
			// Map
			MapView _obj_Map = MapView.Deserialize(input);
			_obj.Map = (MapView)_obj_Map;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Weapons
			output.WriteLine(this.Weapons.Count.ToString());
			for(int Weapons_it = 0; Weapons_it < this.Weapons.Count;Weapons_it++) {
				this.Weapons[Weapons_it].Serialize(output);
			}
			// Armors
			output.WriteLine(this.Armors.Count.ToString());
			for(int Armors_it = 0; Armors_it < this.Armors.Count;Armors_it++) {
				this.Armors[Armors_it].Serialize(output);
			}
			// Boots
			output.WriteLine(this.Boots.Count.ToString());
			for(int Boots_it = 0; Boots_it < this.Boots.Count;Boots_it++) {
				this.Boots[Boots_it].Serialize(output);
			}
			// Enchants
			output.WriteLine(this.Enchants.Count.ToString());
			for(int Enchants_it = 0; Enchants_it < this.Enchants.Count;Enchants_it++) {
				this.Enchants[Enchants_it].Serialize(output);
			}
			// Spells
			output.WriteLine(this.Spells.Count.ToString());
			for(int Spells_it = 0; Spells_it < this.Spells.Count;Spells_it++) {
				this.Spells[Spells_it].Serialize(output);
			}
			// Map
			this.Map.Serialize(output);
		}

	}
}
