#pragma once
#include "Common.h"
#include "StateAlterationModelView.h"


class PassiveEquipmentUpgradeModelView
{

public: 
	std::vector<StateAlterationModelView> PassiveAlterations;
	float Cost;
	void serialize(std::ostream& output);

	static PassiveEquipmentUpgradeModelView deserialize(std::istream& input);
private: 

};