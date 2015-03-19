#pragma once
#include "Common.h"
#include "StateAlterationModelView.h"


class WeaponEnchantModelView
{

public: 
	// Obtient les altértions d'état appliquées à l'impact de l'attaque sur la cible.
	std::vector<StateAlterationModelView> OnHitEffects;
	// Obtient les altérations d'état appliquées lors de l'attaque sur le caster.
	std::vector<StateAlterationModelView> CastingEffects;
	// Obtient les effets passifs appliqués par l'enchantement.
	std::vector<StateAlterationModelView> PassiveEffects;
	void serialize(std::ostream& output);

	static WeaponEnchantModelView deserialize(std::istream& input);
private: 

};