#pragma once
#include "Common.h"
#include "WeaponUpgradeModelView.h"


class WeaponModelView
{

public: 
	// ID unique de l'arme.
	int ID;
	// Liste des upgrades possibles de l'arme.
	std::vector<WeaponUpgradeModelView> Upgrades;
	// Prix d'achat de l'arme
	float Price;
	void serialize(std::ostream& output);

	static WeaponModelView deserialize(std::istream& input);
	WeaponModelView();
private: 

};