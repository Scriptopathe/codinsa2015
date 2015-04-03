package net.codinsa2015;
import java.lang.*;
import java.util.ArrayList;
import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;
import net.codinsa2015.WeaponModelView.*;
import java.util.ArrayList;
import net.codinsa2015.PassiveEquipmentModelView.*;
import net.codinsa2015.WeaponEnchantModelView.*;
import net.codinsa2015.SpellModelView.*;
import net.codinsa2015.MapView.*;


@SuppressWarnings("unused")
public class GameStaticDataView
{


	// Obtient une liste de tous les modèles d'armes du jeu
	public ArrayList<WeaponModelView> Weapons;
	// Obtient une liste de tous les modèles d'armures du jeu
	public ArrayList<PassiveEquipmentModelView> Armors;
	// Obtient une liste de tous les modèles de bottes du jeu
	public ArrayList<PassiveEquipmentModelView> Boots;
	// Obtient une liste de tous les modèles d'enchantements d'arme du jeu
	public ArrayList<WeaponEnchantModelView> Enchants;
	// Obtient une liste de tous les modèles de sorts du jeu.
	public ArrayList<SpellModelView> Spells;
	// Obtient une vue sur les données statiques de la carte (telles que sa table de passabilité).
	public MapView Map;
	public static GameStaticDataView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		GameStaticDataView _obj =  new GameStaticDataView();
		// Weapons
		ArrayList<WeaponModelView> _obj_Weapons = new ArrayList<WeaponModelView>();
		int _obj_Weapons_count = Integer.valueOf(input.readLine());
		for(int _obj_Weapons_i = 0; _obj_Weapons_i < _obj_Weapons_count; _obj_Weapons_i++) {
			WeaponModelView _obj_Weapons_e = WeaponModelView.deserialize(input);
			_obj_Weapons.add((WeaponModelView)_obj_Weapons_e);
		}
		_obj.Weapons = _obj_Weapons;
		// Armors
		ArrayList<PassiveEquipmentModelView> _obj_Armors = new ArrayList<PassiveEquipmentModelView>();
		int _obj_Armors_count = Integer.valueOf(input.readLine());
		for(int _obj_Armors_i = 0; _obj_Armors_i < _obj_Armors_count; _obj_Armors_i++) {
			PassiveEquipmentModelView _obj_Armors_e = PassiveEquipmentModelView.deserialize(input);
			_obj_Armors.add((PassiveEquipmentModelView)_obj_Armors_e);
		}
		_obj.Armors = _obj_Armors;
		// Boots
		ArrayList<PassiveEquipmentModelView> _obj_Boots = new ArrayList<PassiveEquipmentModelView>();
		int _obj_Boots_count = Integer.valueOf(input.readLine());
		for(int _obj_Boots_i = 0; _obj_Boots_i < _obj_Boots_count; _obj_Boots_i++) {
			PassiveEquipmentModelView _obj_Boots_e = PassiveEquipmentModelView.deserialize(input);
			_obj_Boots.add((PassiveEquipmentModelView)_obj_Boots_e);
		}
		_obj.Boots = _obj_Boots;
		// Enchants
		ArrayList<WeaponEnchantModelView> _obj_Enchants = new ArrayList<WeaponEnchantModelView>();
		int _obj_Enchants_count = Integer.valueOf(input.readLine());
		for(int _obj_Enchants_i = 0; _obj_Enchants_i < _obj_Enchants_count; _obj_Enchants_i++) {
			WeaponEnchantModelView _obj_Enchants_e = WeaponEnchantModelView.deserialize(input);
			_obj_Enchants.add((WeaponEnchantModelView)_obj_Enchants_e);
		}
		_obj.Enchants = _obj_Enchants;
		// Spells
		ArrayList<SpellModelView> _obj_Spells = new ArrayList<SpellModelView>();
		int _obj_Spells_count = Integer.valueOf(input.readLine());
		for(int _obj_Spells_i = 0; _obj_Spells_i < _obj_Spells_count; _obj_Spells_i++) {
			SpellModelView _obj_Spells_e = SpellModelView.deserialize(input);
			_obj_Spells.add((SpellModelView)_obj_Spells_e);
		}
		_obj.Spells = _obj_Spells;
		// Map
		MapView _obj_Map = MapView.deserialize(input);
		_obj.Map = _obj_Map;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Weapons
		output.append(String.valueOf(this.Weapons.size()) + "\n");
		for(int Weapons_it = 0; Weapons_it < this.Weapons.size();Weapons_it++) {
			this.Weapons.get(Weapons_it).serialize(output);
		}
		// Armors
		output.append(String.valueOf(this.Armors.size()) + "\n");
		for(int Armors_it = 0; Armors_it < this.Armors.size();Armors_it++) {
			this.Armors.get(Armors_it).serialize(output);
		}
		// Boots
		output.append(String.valueOf(this.Boots.size()) + "\n");
		for(int Boots_it = 0; Boots_it < this.Boots.size();Boots_it++) {
			this.Boots.get(Boots_it).serialize(output);
		}
		// Enchants
		output.append(String.valueOf(this.Enchants.size()) + "\n");
		for(int Enchants_it = 0; Enchants_it < this.Enchants.size();Enchants_it++) {
			this.Enchants.get(Enchants_it).serialize(output);
		}
		// Spells
		output.append(String.valueOf(this.Spells.size()) + "\n");
		for(int Spells_it = 0; Spells_it < this.Spells.size();Spells_it++) {
			this.Spells.get(Spells_it).serialize(output);
		}
		// Map
		this.Map.serialize(output);
	}

}
