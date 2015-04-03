#include "../inc/SpellView.h"
void SpellView::serialize(std::ostream& output) {
	// CurrentCooldown
	output << ((float)this->CurrentCooldown) << '\n';
	// SourceCaster
	output << ((int)this->SourceCaster) << '\n';
	// Model
	output << ((int)this->Model) << '\n';
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
	// Model
	int _obj_Model; input >> _obj_Model; input.ignore(1000, '\n');
	_obj.Model = (int)_obj_Model;
	// Level
	int _obj_Level; input >> _obj_Level; input.ignore(1000, '\n');
	_obj.Level = (int)_obj_Level;
	return _obj;
}


