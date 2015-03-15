#pragma once
#include "Common.h"
#include "SpellDescriptionView.h"


class SpellView
{

public: 
	float CurrentCooldown;
	int SourceCaster;
	std::vector<SpellDescriptionView> Levels;
	int Level;
	void serialize(std::ostream& output);

	static SpellView deserialize(std::istream& input);
private: 

};