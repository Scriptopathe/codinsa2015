#pragma once
#include "Common.h"
#include "Vector2.h"


class CircleShapeView
{

public: 
	// Position du centre du cercle.
	Vector2 Position;
	// Rayon du cercle.
	float Radius;
	void serialize(std::ostream& output);

	static CircleShapeView deserialize(std::istream& input);
	CircleShapeView();
private: 

};