#pragma once
#include "Common.h"
#include "GenericShapeView.h"


class SpellcastBaseView
{

public: 
	GenericShapeView Shape;
	void serialize(std::ostream& output);

	static SpellcastBaseView deserialize(std::istream& input);
private: 

};