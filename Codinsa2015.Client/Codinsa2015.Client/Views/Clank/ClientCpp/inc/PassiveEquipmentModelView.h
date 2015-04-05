#pragma once
#include "Common.h"
#include "PassiveEquipmentUpgradeModelView.h"


class PassiveEquipmentModelView
{

public: 
	// ID unique de l'équipement
	int ID;
	// prix d'achat de l'équipement
	float Price;
	// liste des upgrades de cet équipement.
	std::vector<PassiveEquipmentUpgradeModelView> Upgrades;
	void serialize(std::ostream& output);

	static PassiveEquipmentModelView deserialize(std::istream& input);
	PassiveEquipmentModelView();
private: 

};