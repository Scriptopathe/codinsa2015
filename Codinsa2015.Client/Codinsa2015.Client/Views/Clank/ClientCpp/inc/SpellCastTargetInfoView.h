#pragma once
#include "Common.h"
#include "Vector2.h"


class SpellCastTargetInfoView
{

public: 
	TargettingType Type;
	Vector2 TargetPosition;
	Vector2 TargetDirection;
	int TargetId;
	void serialize(std::ostream& output);

	static SpellCastTargetInfoView deserialize(std::istream& input);
private: 

};