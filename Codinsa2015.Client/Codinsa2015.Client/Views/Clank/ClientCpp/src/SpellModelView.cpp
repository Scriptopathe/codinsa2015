#include "../inc/SpellModelView.h"
void SpellModelView::serialize(std::ostream& output) {
	// ID
	output << ((int)this->ID) << '\n';
	// Levels
	output << this->Levels.size() << '\n';
	for(int Levels_it = 0; Levels_it < this->Levels.size(); Levels_it++) {
		this->Levels[Levels_it].serialize(output);
	}

}

SpellModelView SpellModelView::deserialize(std::istream& input) {
	SpellModelView _obj = SpellModelView();
	// ID
	int _obj_ID; input >> _obj_ID; input.ignore(1000, '\n');
	_obj.ID = (int)_obj_ID;
	// Levels
	std::vector<SpellLevelDescriptionView> _obj_Levels = std::vector<SpellLevelDescriptionView>();
	int _obj_Levels_count; input >> _obj_Levels_count; input.ignore(1000, '\n');
	for(int _obj_Levels_i = 0; _obj_Levels_i < _obj_Levels_count; _obj_Levels_i++) {
		SpellLevelDescriptionView _obj_Levels_e = SpellLevelDescriptionView::deserialize(input);
		_obj_Levels.push_back((SpellLevelDescriptionView)_obj_Levels_e);
	}

	_obj.Levels = (::std::vector<SpellLevelDescriptionView>)_obj_Levels;
	return _obj;
}

SpellModelView::SpellModelView() {
}


