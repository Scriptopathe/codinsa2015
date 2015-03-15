#pragma once
#include "Common.h"
#include "StateAlterationModelView.h"


class WeaponEnchantModelView
{

public: 
	std::vector<StateAlterationModelView> OnHitEffects;
	std::vector<StateAlterationModelView> CastingEffects;
	std::vector<StateAlterationModelView> PassiveEffects;
	void serialize(std::ostream& output);

	static WeaponEnchantModelView deserialize(std::istream& input);
private: 

};