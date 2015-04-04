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
		/// Obtient la position de tous les camps
		/// </summary>
		public List<Vector2> CampsPositions;	
		/// <summary>
		/// Obtient la position de tous les routeurs.
		/// </summary>
		public List<Vector2> RouterPositions;	
		/// <summary>
		/// 
		/// </summary>
		public List<EntityBaseView> VirusCheckpoints;	
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
		/// Obtient la liste des structures présentes sur la carte. Attention : cette liste n'est pas tenue
		/// à jour (statistiques / PV).
		/// </summary>
		public List<EntityBaseView> Structures;	
		/// <summary>
		/// Obtient une vue sur les données statiques de la carte (telles que sa table de passabilité).
		/// </summary>
		public MapView Map;	
		public GameStaticDataView() {
			CampsPositions = new List<Vector2>();
			RouterPositions = new List<Vector2>();
			VirusCheckpoints = new List<EntityBaseView>();
			Weapons = new List<WeaponModelView>();
			Armors = new List<PassiveEquipmentModelView>();
			Boots = new List<PassiveEquipmentModelView>();
			Enchants = new List<WeaponEnchantModelView>();
			Spells = new List<SpellModelView>();
			Structures = new List<EntityBaseView>();
			Map = new MapView();
		}

		public static GameStaticDataView Deserialize(System.IO.StreamReader input) {
			GameStaticDataView _obj =  new GameStaticDataView();
			// CampsPositions
			List<Vector2> _obj_CampsPositions = new List<Vector2>();
			int _obj_CampsPositions_count = Int32.Parse(input.ReadLine());
			for(int _obj_CampsPositions_i = 0; _obj_CampsPositions_i < _obj_CampsPositions_count; _obj_CampsPositions_i++) {
				Vector2 _obj_CampsPositions_e = Vector2.Deserialize(input);
				_obj_CampsPositions.Add((Vector2)_obj_CampsPositions_e);
			}
			_obj.CampsPositions = (List<Vector2>)_obj_CampsPositions;
			// RouterPositions
			List<Vector2> _obj_RouterPositions = new List<Vector2>();
			int _obj_RouterPositions_count = Int32.Parse(input.ReadLine());
			for(int _obj_RouterPositions_i = 0; _obj_RouterPositions_i < _obj_RouterPositions_count; _obj_RouterPositions_i++) {
				Vector2 _obj_RouterPositions_e = Vector2.Deserialize(input);
				_obj_RouterPositions.Add((Vector2)_obj_RouterPositions_e);
			}
			_obj.RouterPositions = (List<Vector2>)_obj_RouterPositions;
			// VirusCheckpoints
			List<EntityBaseView> _obj_VirusCheckpoints = new List<EntityBaseView>();
			int _obj_VirusCheckpoints_count = Int32.Parse(input.ReadLine());
			for(int _obj_VirusCheckpoints_i = 0; _obj_VirusCheckpoints_i < _obj_VirusCheckpoints_count; _obj_VirusCheckpoints_i++) {
				EntityBaseView _obj_VirusCheckpoints_e = EntityBaseView.Deserialize(input);
				_obj_VirusCheckpoints.Add((EntityBaseView)_obj_VirusCheckpoints_e);
			}
			_obj.VirusCheckpoints = (List<EntityBaseView>)_obj_VirusCheckpoints;
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
			// Structures
			List<EntityBaseView> _obj_Structures = new List<EntityBaseView>();
			int _obj_Structures_count = Int32.Parse(input.ReadLine());
			for(int _obj_Structures_i = 0; _obj_Structures_i < _obj_Structures_count; _obj_Structures_i++) {
				EntityBaseView _obj_Structures_e = EntityBaseView.Deserialize(input);
				_obj_Structures.Add((EntityBaseView)_obj_Structures_e);
			}
			_obj.Structures = (List<EntityBaseView>)_obj_Structures;
			// Map
			MapView _obj_Map = MapView.Deserialize(input);
			_obj.Map = (MapView)_obj_Map;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// CampsPositions
			output.WriteLine(this.CampsPositions.Count.ToString());
			for(int CampsPositions_it = 0; CampsPositions_it < this.CampsPositions.Count;CampsPositions_it++) {
				this.CampsPositions[CampsPositions_it].Serialize(output);
			}
			// RouterPositions
			output.WriteLine(this.RouterPositions.Count.ToString());
			for(int RouterPositions_it = 0; RouterPositions_it < this.RouterPositions.Count;RouterPositions_it++) {
				this.RouterPositions[RouterPositions_it].Serialize(output);
			}
			// VirusCheckpoints
			output.WriteLine(this.VirusCheckpoints.Count.ToString());
			for(int VirusCheckpoints_it = 0; VirusCheckpoints_it < this.VirusCheckpoints.Count;VirusCheckpoints_it++) {
				this.VirusCheckpoints[VirusCheckpoints_it].Serialize(output);
			}
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
			// Structures
			output.WriteLine(this.Structures.Count.ToString());
			for(int Structures_it = 0; Structures_it < this.Structures.Count;Structures_it++) {
				this.Structures[Structures_it].Serialize(output);
			}
			// Map
			this.Map.Serialize(output);
		}

	}
}
