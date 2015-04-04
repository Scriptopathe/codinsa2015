#include "../inc/GameStaticDataView.h"
void GameStaticDataView::serialize(std::ostream& output) {
	// Weapons
	output << this->Weapons.size() << '\n';
	for(int Weapons_it = 0; Weapons_it < this->Weapons.size(); Weapons_it++) {
		this->Weapons[Weapons_it].serialize(output);
	}

	// Armors
	output << this->Armors.size() << '\n';
	for(int Armors_it = 0; Armors_it < this->Armors.size(); Armors_it++) {
		this->Armors[Armors_it].serialize(output);
	}

	// Boots
	output << this->Boots.size() << '\n';
	for(int Boots_it = 0; Boots_it < this->Boots.size(); Boots_it++) {
		this->Boots[Boots_it].serialize(output);
	}

	// Enchants
	output << this->Enchants.size() << '\n';
	for(int Enchants_it = 0; Enchants_it < this->Enchants.size(); Enchants_it++) {
		this->Enchants[Enchants_it].serialize(output);
	}

	// Spells
	output << this->Spells.size() << '\n';
	for(int Spells_it = 0; Spells_it < this->Spells.size(); Spells_it++) {
		this->Spells[Spells_it].serialize(output);
	}

	// Map
	this->Map.serialize(output);
}

GameStaticDataView GameStaticDataView::deserialize(std::istream& input) {
	GameStaticDataView _obj = GameStaticDataView();
	// Weapons
	std::vector<WeaponModelView> _obj_Weapons = std::vector<WeaponModelView>();
	int _obj_Weapons_count; input >> _obj_Weapons_count; input.ignore(1000, '\n');
	for(int _obj_Weapons_i = 0; _obj_Weapons_i < _obj_Weapons_count; _obj_Weapons_i++) {
		WeaponModelView _obj_Weapons_e = WeaponModelView::deserialize(input);
		_obj_Weapons.push_back((WeaponModelView)_obj_Weapons_e);
	}

	_obj.Weapons = (::std::vector<WeaponModelView>)_obj_Weapons;
	// Armors
	std::vector<PassiveEquipmentModelView> _obj_Armors = std::vector<PassiveEquipmentModelView>();
	int _obj_Armors_count; input >> _obj_Armors_count; input.ignore(1000, '\n');
	for(int _obj_Armors_i = 0; _obj_Armors_i < _obj_Armors_count; _obj_Armors_i++) {
		PassiveEquipmentModelView _obj_Armors_e = PassiveEquipmentModelView::deserialize(input);
		_obj_Armors.push_back((PassiveEquipmentModelView)_obj_Armors_e);
	}

	_obj.Armors = (::std::vector<PassiveEquipmentModelView>)_obj_Armors;
	// Boots
	std::vector<PassiveEquipmentModelView> _obj_Boots = std::vector<PassiveEquipmentModelView>();
	int _obj_Boots_count; input >> _obj_Boots_count; input.ignore(1000, '\n');
	for(int _obj_Boots_i = 0; _obj_Boots_i < _obj_Boots_count; _obj_Boots_i++) {
		PassiveEquipmentModelView _obj_Boots_e = PassiveEquipmentModelView::deserialize(input);
		_obj_Boots.push_back((PassiveEquipmentModelView)_obj_Boots_e);
	}

	_obj.Boots = (::std::vector<PassiveEquipmentModelView>)_obj_Boots;
	// Enchants
	std::vector<WeaponEnchantModelView> _obj_Enchants = std::vector<WeaponEnchantModelView>();
	int _obj_Enchants_count; input >> _obj_Enchants_count; input.ignore(1000, '\n');
	for(int _obj_Enchants_i = 0; _obj_Enchants_i < _obj_Enchants_count; _obj_Enchants_i++) {
		WeaponEnchantModelView _obj_Enchants_e = WeaponEnchantModelView::deserialize(input);
		_obj_Enchants.push_back((WeaponEnchantModelView)_obj_Enchants_e);
	}

	_obj.Enchants = (::std::vector<WeaponEnchantModelView>)_obj_Enchants;
	// Spells
	std::vector<SpellModelView> _obj_Spells = std::vector<SpellModelView>();
	int _obj_Spells_count; input >> _obj_Spells_count; input.ignore(1000, '\n');
	for(int _obj_Spells_i = 0; _obj_Spells_i < _obj_Spells_count; _obj_Spells_i++) {
		SpellModelView _obj_Spells_e = SpellModelView::deserialize(input);
		_obj_Spells.push_back((SpellModelView)_obj_Spells_e);
	}

	_obj.Spells = (::std::vector<SpellModelView>)_obj_Spells;
	// Map
	MapView _obj_Map = MapView::deserialize(input);
	_obj.Map = (::MapView)_obj_Map;
	return _obj;
}


