#pragma once
#include "Common.h"
#include "SpellLevelDescriptionView.h"
#include "StateAlterationModelView.h"


class WeaponUpgradeModelView
{

public: 
	// Obtient du sort que lance l'arme à ce niveau d'upgrade.
	SpellLevelDescriptionView Description;
	// Obtient les altérations d'état appliquées passivement par l'arme à ce niveau d'upgrade.
	std::vector<StateAlterationModelView> PassiveAlterations;
	// Obtient le coût de cette upgrade.
	float Cost;
	void serialize(std::ostream& output);

	static WeaponUpgradeModelView deserialize(std::istream& input);
private: 

};