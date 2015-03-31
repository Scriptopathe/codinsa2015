#pragma once
#include "Common.h"
#include "GenericShapeView.h"


class SpellcastBaseView
{

public: 
	// Shape utilisée par ce spell cast.
	GenericShapeView Shape;
	// nom de ce spellcast (utilisé pour le texturing notamment).
	string Name;
	void serialize(std::ostream& output);

	static SpellcastBaseView deserialize(std::istream& input);
private: 

};