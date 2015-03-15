#include "../inc/CircleShapeView.h"
void CircleShapeView::serialize(std::ostream& output) {
	// Position
	this->Position.serialize(output);
	// Radius
	output << ((float)this->Radius) << '\n';
}

CircleShapeView CircleShapeView::deserialize(std::istream& input) {
	CircleShapeView _obj = CircleShapeView();
	// Position
	Vector2 _obj_Position = Vector2::deserialize(input);
	_obj.Position = (::Vector2)_obj_Position;
	// Radius
	float _obj_Radius; input >> _obj_Radius; input.ignore(1000, '\n');
	_obj.Radius = (float)_obj_Radius;
	return _obj;
}


