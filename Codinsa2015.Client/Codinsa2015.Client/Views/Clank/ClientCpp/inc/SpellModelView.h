#pragma once
#include "Common.h"
#include "SpellLevelDescriptionView.h"


class SpellModelView
{

public: 
	// ID du spell permettant de le retrouver dans le tableau de sorts du jeu.
	int ID;
	// Obtient la liste des descriptions des niveaux de ce sort.
	std::vector<SpellLevelDescriptionView> Levels;
	void serialize(std::ostream& output);

	static SpellModelView deserialize(std::istream& input);
private: 

};