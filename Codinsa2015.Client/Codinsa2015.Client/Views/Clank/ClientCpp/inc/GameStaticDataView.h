#pragma once
#include "Common.h"
#include "WeaponModelView.h"
#include "PassiveEquipmentModelView.h"
#include "WeaponEnchantModelView.h"
#include "SpellModelView.h"
#include "MapView.h"


class GameStaticDataView
{

public: 
	// Obtient une liste de tous les modèles d'armes du jeu
	std::vector<WeaponModelView> Weapons;
	// Obtient une liste de tous les modèles d'armures du jeu
	std::vector<PassiveEquipmentModelView> Armors;
	// Obtient une liste de tous les modèles de bottes du jeu
	std::vector<PassiveEquipmentModelView> Boots;
	// Obtient une liste de tous les modèles d'enchantements d'arme du jeu
	std::vector<WeaponEnchantModelView> Enchants;
	// Obtient une liste de tous les modèles de sorts du jeu.
	std::vector<SpellModelView> Spells;
	// Obtient une vue sur les données statiques de la carte (telles que sa table de passabilité).
	MapView Map;
	void serialize(std::ostream& output);

	static GameStaticDataView deserialize(std::istream& input);
private: 

};