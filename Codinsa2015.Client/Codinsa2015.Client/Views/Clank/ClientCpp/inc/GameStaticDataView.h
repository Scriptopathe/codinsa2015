#pragma once
#include "Common.h"
#include "Vector2.h"
#include "EntityBaseView.h"
#include "WeaponModelView.h"
#include "PassiveEquipmentModelView.h"
#include "WeaponEnchantModelView.h"
#include "SpellModelView.h"
#include "MapView.h"


class GameStaticDataView
{

public: 
	// Obtient la position de tous les camps
	std::vector<Vector2> CampsPositions;
	// Obtient la position de tous les routeurs.
	std::vector<Vector2> RouterPositions;
	// 
	std::vector<EntityBaseView> VirusCheckpoints;
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
	// Obtient la liste des structures présentes sur la carte. Attention : cette liste n'est pas tenue
	// à jour (statistiques / PV).
	std::vector<EntityBaseView> Structures;
	// Obtient une vue sur les données statiques de la carte (telles que sa table de passabilité).
	MapView Map;
	void serialize(std::ostream& output);

	static GameStaticDataView deserialize(std::istream& input);
	GameStaticDataView();
private: 

};