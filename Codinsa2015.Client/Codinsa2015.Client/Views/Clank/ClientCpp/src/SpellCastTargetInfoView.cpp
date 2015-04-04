#include "../inc/SpellCastTargetInfoView.h"
void SpellCastTargetInfoView::serialize(std::ostream& output) {
	// Type
	output << ((int)this->Type) << '\n';
	// TargetPosition
	this->TargetPosition.serialize(output);
	// TargetDirection
	this->TargetDirection.serialize(output);
	// TargetId
	output << ((int)this->TargetId) << '\n';
}

SpellCastTargetInfoView SpellCastTargetInfoView::deserialize(std::istream& input) {
	SpellCastTargetInfoView _obj = SpellCastTargetInfoView();
	// Type
	int _obj_Type_asInt; input >> _obj_Type_asInt; input.ignore(1000, '\n');
	TargettingType _obj_Type = (TargettingType)_obj_Type_asInt;
	_obj.Type = (::TargettingType)_obj_Type;
	// TargetPosition
	Vector2 _obj_TargetPosition = Vector2::deserialize(input);
	_obj.TargetPosition = (::Vector2)_obj_TargetPosition;
	// TargetDirection
	Vector2 _obj_TargetDirection = Vector2::deserialize(input);
	_obj.TargetDirection = (::Vector2)_obj_TargetDirection;
	// TargetId
	int _obj_TargetId; input >> _obj_TargetId; input.ignore(1000, '\n');
	_obj.TargetId = (int)_obj_TargetId;
	return _obj;
}

SpellCastTargetInfoView::SpellCastTargetInfoView() {
}


