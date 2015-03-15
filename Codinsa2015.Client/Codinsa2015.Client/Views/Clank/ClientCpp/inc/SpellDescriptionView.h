#pragma once
#include "Common.h"
#include "StateAlterationModelView.h"
#include "SpellTargetInfoView.h"


class SpellDescriptionView
{

public: 
	float BaseCooldown;
	float CastingTime;
	std::vector<StateAlterationModelView> CastingTimeAlterations;
	SpellTargetInfoView TargetType;
	std::vector<StateAlterationModelView> OnHitEffects;
	void serialize(std::ostream& output);

	static SpellDescriptionView deserialize(std::istream& input);
private: 

};