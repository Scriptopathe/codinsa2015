#include "../inc/GenericShapeView.h"
void GenericShapeView::serialize(std::ostream& output) {
	// Position
	this->Position.serialize(output);
	// Radius
	output << ((float)this->Radius) << '\n';
	// Size
	this->Size.serialize(output);
	// ShapeType
	output << ((int)this->ShapeType) << '\n';
}

GenericShapeView GenericShapeView::deserialize(std::istream& input) {
	GenericShapeView _obj = GenericShapeView();
	// Position
	Vector2 _obj_Position = Vector2::deserialize(input);
	_obj.Position = (::Vector2)_obj_Position;
	// Radius
	float _obj_Radius; input >> _obj_Radius; input.ignore(1000, '\n');
	_obj.Radius = (float)_obj_Radius;
	// Size
	Vector2 _obj_Size = Vector2::deserialize(input);
	_obj.Size = (::Vector2)_obj_Size;
	// ShapeType
	int _obj_ShapeType_asInt; input >> _obj_ShapeType_asInt; input.ignore(1000, '\n');
	GenericShapeType _obj_ShapeType = (GenericShapeType)_obj_ShapeType_asInt;
	_obj.ShapeType = (::GenericShapeType)_obj_ShapeType;
	return _obj;
}

GenericShapeView::GenericShapeView() {
}


