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
	EntityBaseView GetHero();

	Vector2 GetPosition();

	MapView GetMapView();

	bool StartMoveTo(Vector2 position);

	bool IsAutoMoving();

	bool EndMoveTo();

	std::vector<EntityBaseView> GetEntitiesInSight();

	EntityBaseView GetEntityById(int entityId);

	bool UseSpell(int spellId,SpellCastTargetInfoView target);

	SceneMode GetMode();

	SpellDescriptionView GetSpellCurrentLevelDescription(int spellId);

	SpellView GetSpell(int spellId);

	std::vector<SpellView> GetSpells();

	std::vector<SpellView> GetHeroSpells(int entityId);

	void serialize(std::ostream& output);

	static State deserialize(std::istream& input);
};