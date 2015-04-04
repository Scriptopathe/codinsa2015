#include "../inc/SpellTargetInfoView.h"
void SpellTargetInfoView::serialize(std::ostream& output) {
	// Type
	output << ((int)this->Type) << '\n';
	// Range
	output << ((float)this->Range) << '\n';
	// Duration
	output << ((float)this->Duration) << '\n';
	// AoeRadius
	output << ((float)this->AoeRadius) << '\n';
	// DieOnCollision
	output << (this->DieOnCollision ? 1 : 0) << '\n';
	// AllowedTargetTypes
	output << ((int)this->AllowedTargetTypes) << '\n';
}

SpellTargetInfoView SpellTargetInfoView::deserialize(std::istream& input) {
	SpellTargetInfoView _obj = SpellTargetInfoView();
	// Type
	int _obj_Type_asInt; input >> _obj_Type_asInt; input.ignore(1000, '\n');
	TargettingType _obj_Type = (TargettingType)_obj_Type_asInt;
	_obj.Type = (::TargettingType)_obj_Type;
	// Range
	float _obj_Range; input >> _obj_Range; input.ignore(1000, '\n');
	_obj.Range = (float)_obj_Range;
	// Duration
	float _obj_Duration; input >> _obj_Duration; input.ignore(1000, '\n');
	_obj.Duration = (float)_obj_Duration;
	// AoeRadius
	float _obj_AoeRadius; input >> _obj_AoeRadius; input.ignore(1000, '\n');
	_obj.AoeRadius = (float)_obj_AoeRadius;
	// DieOnCollision
	bool _obj_DieOnCollision; input >> _obj_DieOnCollision; input.ignore(1000, '\n');
	_obj.DieOnCollision = (bool)_obj_DieOnCollision;
	// AllowedTargetTypes
	int _obj_AllowedTargetTypes_asInt; input >> _obj_AllowedTargetTypes_asInt; input.ignore(1000, '\n');
	EntityTypeRelative _obj_AllowedTargetTypes = (EntityTypeRelative)_obj_AllowedTargetTypes_asInt;
	_obj.AllowedTargetTypes = (::EntityTypeRelative)_obj_AllowedTargetTypes;
	return _obj;
}

SpellTargetInfoView::SpellTargetInfoView() {
}


