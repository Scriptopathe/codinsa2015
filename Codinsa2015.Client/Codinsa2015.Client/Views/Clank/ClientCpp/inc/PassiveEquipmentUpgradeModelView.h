#pragma once
#include "Common.h"
#include "StateAlterationModelView.h"


class PassiveEquipmentUpgradeModelView
{

public: 
	// Obtient les altérations d'état appliquées passivement par cet équipement.
	std::vector<StateAlterationModelView> PassiveAlterations;
	// Obtient le coût de l'upgrade.
	float Cost;
	void serialize(std::ostream& output);

	static PassiveEquipmentUpgradeModelView deserialize(std::istream& input);
	PassiveEquipmentUpgradeModelView();
private: 

};