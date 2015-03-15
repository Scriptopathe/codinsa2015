#pragma once
#include "Common.h"
#include "Vector2.h"


class StateAlterationParametersView
{

public: 
	Vector2 DashTargetDirection;
	int DashTargetEntity;
	void serialize(std::ostream& output);

	static StateAlterationParametersView deserialize(std::istream& input);
private: 

};