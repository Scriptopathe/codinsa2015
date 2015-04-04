#pragma once
#include "Common.h"
#include "StateAlterationModelView.h"
#include "SpellTargetInfoView.h"


class SpellLevelDescriptionView
{

public: 
	// Cooldown de base du sort.
	float BaseCooldown;
	// Casting time du sort.
	float CastingTime;
	// Altération d'état appliquée pendant le casting time.
	std::vector<StateAlterationModelView> CastingTimeAlterations;
	// Indique la manière dont le ciblage du sort est effectué.
	SpellTargetInfoView TargetType;
	// Effets à l'impact du sort. Ils sont appliqués une fois le casting time terminé.
	std::vector<StateAlterationModelView> OnHitEffects;
	void serialize(std::ostream& output);

	static SpellLevelDescriptionView deserialize(std::istream& input);
	SpellLevelDescriptionView();
private: 

};