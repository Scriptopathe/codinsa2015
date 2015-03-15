#pragma once
#include "Common.h"
#include "Vector2.h"


class CircleShapeView
{

public: 
	Vector2 Position;
	float Radius;
	void serialize(std::ostream& output);

	static CircleShapeView deserialize(std::istream& input);
private: 

};