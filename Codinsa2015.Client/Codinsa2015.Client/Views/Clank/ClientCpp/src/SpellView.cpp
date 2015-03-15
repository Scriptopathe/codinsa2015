#include "../inc/SpellView.h"
void SpellView::serialize(std::ostream& output) {
	// CurrentCooldown
	output << ((float)this->CurrentCooldown) << '\n';
	// SourceCaster
	output << ((int)this->SourceCaster) << '\n';
	// Levels
	output << this->Levels.size() << '\n';
	for(int Levels_it = 0; Levels_it < this->Levels.size(); Levels_it++) {
		this->Levels[Levels_it].serialize(output);
	}

	// Level
	output << ((int)this->Level) << '\n';
}

SpellView SpellView::deserialize(std::istream& input) {
	SpellView _obj = SpellView();
	// CurrentCooldown
	float _obj_CurrentCooldown; input >> _obj_CurrentCooldown; input.ignore(1000, '\n');
	_obj.CurrentCooldown = (float)_obj_CurrentCooldown;
	// SourceCaster
	int _obj_SourceCaster; input >> _obj_SourceCaster; input.ignore(1000, '\n');
	_obj.SourceCaster = (int)_obj_SourceCaster;
	// Levels
	std::vector<SpellDescriptionView> _obj_Levels = std::vector<SpellDescriptionView>();
	int _obj_Levels_count; input >> _obj_Levels_count; input.ignore(1000, '\n');
	for(int _obj_Levels_i = 0; _obj_Levels_i < _obj_Levels_count; _obj_Levels_i++) {
		SpellDescriptionView _obj_Levels_e = SpellDescriptionView::deserialize(input);
		_obj_Levels.push_back((SpellDescriptionView)_obj_Levels_e);
	}

	_obj.Levels = (::std::vector<SpellDescriptionView>)_obj_Levels;
	// Level
	int _obj_Level; input >> _obj_Level; input.ignore(1000, '\n');
	_obj.Level = (int)_obj_Level;
	return _obj;
}


