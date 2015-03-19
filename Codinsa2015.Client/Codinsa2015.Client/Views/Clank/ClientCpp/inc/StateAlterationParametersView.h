#pragma once
#include "Common.h"
#include "Vector2.h"


class StateAlterationParametersView
{

public: 
	// Position finale que le dash doit atteindre (si le targetting est Direction)
	Vector2 DashTargetDirection;
	// Entité vers laquelle le dash doit se diriger (si le targetting du dash est Entity).
	int DashTargetEntity;
	void serialize(std::ostream& output);

	static StateAlterationParametersView deserialize(std::istream& input);
private: 

};