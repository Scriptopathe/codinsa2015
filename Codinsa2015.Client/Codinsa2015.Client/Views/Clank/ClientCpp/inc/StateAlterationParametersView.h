#pragma once
#include "Common.h"
#include "Vector2.h"


class StateAlterationParametersView
{

public: 
	// Direction dans laquelle de dash doit aller. (si le targetting est Direction)
	Vector2 DashTargetDirection;
	// Entité vers laquelle le dash doit se diriger (si le targetting du dash est TowardsEntity).
	int DashTargetEntity;
	// Position finale du du dash (si targetting TowardsPosition)
	Vector2 DashTargetPosition;
	void serialize(std::ostream& output);

	static StateAlterationParametersView deserialize(std::istream& input);
private: 

};