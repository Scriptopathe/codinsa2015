#include "../inc/WeaponModelView.h"
void WeaponModelView::serialize(std::ostream& output) {
	// ID
	output << ((int)this->ID) << '\n';
	// Upgrades
	output << this->Upgrades.size() << '\n';
	for(int Upgrades_it = 0; Upgrades_it < this->Upgrades.size(); Upgrades_it++) {
		this->Upgrades[Upgrades_it].serialize(output);
	}

	// Price
	output << ((float)this->Price) << '\n';
}

WeaponModelView WeaponModelView::deserialize(std::istream& input) {
	WeaponModelView _obj = WeaponModelView();
	// ID
	int _obj_ID; input >> _obj_ID; input.ignore(1000, '\n');
	_obj.ID = (int)_obj_ID;
	// Upgrades
	std::vector<WeaponUpgradeModelView> _obj_Upgrades = std::vector<WeaponUpgradeModelView>();
	int _obj_Upgrades_count; input >> _obj_Upgrades_count; input.ignore(1000, '\n');
	for(int _obj_Upgrades_i = 0; _obj_Upgrades_i < _obj_Upgrades_count; _obj_Upgrades_i++) {
		WeaponUpgradeModelView _obj_Upgrades_e = WeaponUpgradeModelView::deserialize(input);
		_obj_Upgrades.push_back((WeaponUpgradeModelView)_obj_Upgrades_e);
	}

	_obj.Upgrades = (::std::vector<WeaponUpgradeModelView>)_obj_Upgrades;
	// Price
	float _obj_Price; input >> _obj_Price; input.ignore(1000, '\n');
	_obj.Price = (float)_obj_Price;
	return _obj;
}

WeaponModelView::WeaponModelView() {
}


