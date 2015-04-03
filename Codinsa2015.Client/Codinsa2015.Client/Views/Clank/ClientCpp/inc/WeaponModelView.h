#pragma once
#include "Common.h"
#include "WeaponUpgradeModelView.h"


class WeaponModelView
{

public: 
	// Liste des upgrades possibles de l'arme.
	std::vector<WeaponUpgradeModelView> Upgrades;
	// Prix d'achat de l'arme
	float Price;
	void serialize(std::ostream& output);

	static WeaponModelView deserialize(std::istream& input);
private: 

};