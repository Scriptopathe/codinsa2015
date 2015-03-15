#include "../inc/Vector2.h"
Vector2::Vector2()
{
}
Vector2::Vector2(float x, float y)
{
	this->X = x;
	this->Y = y;
}
void Vector2::serialize(std::ostream& output) {
	// X
	output << ((float)this->X) << '\n';
	// Y
	output << ((float)this->Y) << '\n';
}

Vector2 Vector2::deserialize(std::istream& input) {
	Vector2 _obj = Vector2();
	// X
	float _obj_X; input >> _obj_X; input.ignore(1000, '\n');
	_obj.X = (float)_obj_X;
	// Y
	float _obj_Y; input >> _obj_Y; input.ignore(1000, '\n');
	_obj.Y = (float)_obj_Y;
	return _obj;
}


