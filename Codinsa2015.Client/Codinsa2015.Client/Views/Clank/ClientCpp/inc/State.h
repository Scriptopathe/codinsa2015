/** 
 * Contient toutes les informations concernant l'état du serveur.
 */
#pragma once
#include "Common.h"
#include "EntityBaseView.h"
#include "Vector2.h"
#include "MapView.h"
#include "SpellCastTargetInfoView.h"
#include "SpellDescriptionView.h"
#include "SpellView.h"


class State
{

public: 
	// 		 * @brief Retourne une vue vers le héros.
			 * @param:lol testtest
			 * @param:mdr test
	// test
			 * @returns hahaha
	EntityBaseView GetHero();

	//  Retourne la position du héros.
	Vector2 GetPosition();

	//  Retourne les informations concernant la map actuelle
	MapView GetMapView();

	//  Déplace le joueur vers la position donnée en utilisant l'A*.
	bool StartMoveTo(Vector2 position);

	//  Indique si le joueur est entrain de se déplacer en utilisant son A*.
	bool IsAutoMoving();

	//  Arrête le déplacement automatique (A*) du joueur.
	bool EndMoveTo();

	//  Retourne la liste des entités en vue
	std::vector<EntityBaseView> GetEntitiesInSight();

	//  Obtient une vue sur l'entité dont l'id est passé en paramètre. (si l'id retourné est -1 : accès
	// refusé)
	EntityBaseView GetEntityById(int entityId);

	//  Utilise le sort d'id donné. Retourne true si l'action a été effectuée.
	bool UseSpell(int spellId,SpellCastTargetInfoView target);

	//  Obtient le mode actuel de la scène.
	SceneMode GetMode();

	//  Obtient la description du spell dont l'id est donné en paramètre.
	SpellDescriptionView GetSpellCurrentLevelDescription(int spellId);

	//  Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre.
	SpellView GetSpell(int spellId);

	//  Obtient la liste des spells du héros contrôlé.
	std::vector<SpellView> GetSpells();

	//  Obtient les spells possédés par le héros dont l'id est passé en paramètre.
	std::vector<SpellView> GetHeroSpells(int entityId);

	void serialize(std::ostream& output);

	static State deserialize(std::istream& input);
};