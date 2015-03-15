#pragma once
#include "Common.h"
#include "Vector2.h"


class GenericShapeView
{

public: 
	Vector2 Position;
	float Radius;
	Vector2 Size;
	GenericShapeType ShapeType;
	void serialize(std::ostream& output);

	static GenericShapeView deserialize(std::istream& input);
private: 

};