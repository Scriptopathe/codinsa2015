#include "../inc/MapView.h"
void MapView::serialize(std::ostream& output) {
	// Passability
	output << this->Passability.size() << '\n';
	for(int Passability_it = 0; Passability_it < this->Passability.size(); Passability_it++) {
		output << this->Passability[Passability_it].size() << '\n';
		for(int PassabilityPassability_it_it = 0; PassabilityPassability_it_it < this->Passability[Passability_it].size(); PassabilityPassability_it_it++) {
			output << (this->Passability[Passability_it][PassabilityPassability_it_it] ? 1 : 0) << '\n';
		}
	}

}

MapView MapView::deserialize(std::istream& input) {
	MapView _obj = MapView();
	// Passability
	std::vector<std::vector<bool>> _obj_Passability = std::vector<std::vector<bool>>();
	int _obj_Passability_count; input >> _obj_Passability_count; input.ignore(1000, '\n');
	for(int _obj_Passability_i = 0; _obj_Passability_i < _obj_Passability_count; _obj_Passability_i++) {
		std::vector<bool> _obj_Passability_e = std::vector<bool>();
		int _obj_Passability_e_count; input >> _obj_Passability_e_count; input.ignore(1000, '\n');
		for(int _obj_Passability_e_i = 0; _obj_Passability_e_i < _obj_Passability_e_count; _obj_Passability_e_i++) {
			bool _obj_Passability_e_e; input >> _obj_Passability_e_e; input.ignore(1000, '\n');
			_obj_Passability_e.push_back((bool)_obj_Passability_e_e);
		}
		_obj_Passability.push_back((std::vector<bool>)_obj_Passability_e);
	}

	_obj.Passability = (::std::vector<std::vector<bool>>)_obj_Passability;
	return _obj;
}

MapView::MapView() {
}


