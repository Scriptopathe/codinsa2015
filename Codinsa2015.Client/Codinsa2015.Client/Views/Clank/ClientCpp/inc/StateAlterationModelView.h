#pragma once
#include "Common.h"


class StateAlterationModelView
{

public: 
	StateAlterationType Type;
	float BaseDuration;
	bool DashGoThroughWall;
	DashDirectionType DashDirType;
	float FlatValue;
	float SourcePercentADValue;
	float SourcePercentHPValue;
	float SourcePercentMaxHPValue;
	float SourcePercentArmorValue;
	float SourcePercentAPValue;
	float SourcePercentRMValue;
	float DestPercentADValue;
	float DestPercentHPValue;
	float DestPercentMaxHPValue;
	float DestPercentArmorValue;
	float DestPercentAPValue;
	float DestPercentRMValue;
	float StructureBonus;
	float MonsterBonus;
	float CreepBonus;
	void serialize(std::ostream& output);

	static StateAlterationModelView deserialize(std::istream& input);
private: 

};