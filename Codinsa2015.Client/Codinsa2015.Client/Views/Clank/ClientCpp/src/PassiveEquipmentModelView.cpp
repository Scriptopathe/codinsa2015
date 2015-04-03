#include "../inc/PassiveEquipmentModelView.h"
void PassiveEquipmentModelView::serialize(std::ostream& output) {
	// Price
	output << ((float)this->Price) << '\n';
	// Upgrades
	output << this->Upgrades.size() << '\n';
	for(int Upgrades_it = 0; Upgrades_it < this->Upgrades.size(); Upgrades_it++) {
		this->Upgrades[Upgrades_it].serialize(output);
	}

}

PassiveEquipmentModelView PassiveEquipmentModelView::deserialize(std::istream& input) {
	PassiveEquipmentModelView _obj = PassiveEquipmentModelView();
	// Price
	float _obj_Price; input >> _obj_Price; input.ignore(1000, '\n');
	_obj.Price = (float)_obj_Price;
	// Upgrades
	std::vector<PassiveEquipmentUpgradeModelView> _obj_Upgrades = std::vector<PassiveEquipmentUpgradeModelView>();
	int _obj_Upgrades_count; input >> _obj_Upgrades_count; input.ignore(1000, '\n');
	for(int _obj_Upgrades_i = 0; _obj_Upgrades_i < _obj_Upgrades_count; _obj_Upgrades_i++) {
		PassiveEquipmentUpgradeModelView _obj_Upgrades_e = PassiveEquipmentUpgradeModelView::deserialize(input);
		_obj_Upgrades.push_back((PassiveEquipmentUpgradeModelView)_obj_Upgrades_e);
	}

	_obj.Upgrades = (::std::vector<PassiveEquipmentUpgradeModelView>)_obj_Upgrades;
	return _obj;
}


