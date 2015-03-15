#pragma once
#include "Common.h"
#include "SpellDescriptionView.h"
#include "StateAlterationModelView.h"


class WeaponUpgradeModelView
{

public: 
	SpellDescriptionView Description;
	std::vector<StateAlterationModelView> PassiveAlterations;
	float Cost;
	void serialize(std::ostream& output);

	static WeaponUpgradeModelView deserialize(std::istream& input);
private: 

};