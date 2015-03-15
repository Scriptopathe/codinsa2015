#pragma once
#include "Common.h"
#include "Vector2.h"


class Vector2
{

public: 
	Vector2();

	Vector2(float x, float y);

	float X;
	float Y;
	void serialize(std::ostream& output);

	static Vector2 deserialize(std::istream& input);
private: 

};