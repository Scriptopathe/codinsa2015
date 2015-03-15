#pragma once
#include "Common.h"
#include "StateAlterationModelView.h"
#include "StateAlterationParametersView.h"


class StateAlterationView
{

public: 
	int Source;
	StateAlterationSource SourceType;
	StateAlterationModelView Model;
	StateAlterationParametersView Parameters;
	float RemainingTime;
	void serialize(std::ostream& output);

	static StateAlterationView deserialize(std::istream& input);
private: 

};