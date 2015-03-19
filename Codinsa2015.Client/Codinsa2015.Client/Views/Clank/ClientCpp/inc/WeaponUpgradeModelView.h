#pragma once
#include "Common.h"
#include "SpellDescriptionView.h"
#include "StateAlterationModelView.h"


class WeaponUpgradeModelView
{

public: 
	// Obtient la description de l'upgrade
	SpellDescriptionView Description;
	// 
	std::vector<StateAlterationModelView> PassiveAlterations;
	// Obtient le coût de l'upgrade.
	float Cost;
	void serialize(std::ostream& output);

	static WeaponUpgradeModelView deserialize(std::istream& input);
private: 

};