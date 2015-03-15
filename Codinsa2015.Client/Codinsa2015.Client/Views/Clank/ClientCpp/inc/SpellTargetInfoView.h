#pragma once
#include "Common.h"


class SpellTargetInfoView
{

public: 
	TargettingType Type;
	float Range;
	float Duration;
	float AoeRadius;
	bool DieOnCollision;
	EntityTypeRelative AllowedTargetTypes;
	void serialize(std::ostream& output);

	static SpellTargetInfoView deserialize(std::istream& input);
private: 

};