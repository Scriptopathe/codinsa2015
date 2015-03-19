#pragma once
#include "Common.h"
#include "SpellDescriptionView.h"


class SpellView
{

public: 
	// Cooldown actuel du sort, en secondes
	float CurrentCooldown;
	// Id de l'entité possédant le sort.
	int SourceCaster;
	// Représente les descriptions du spell pour les différents niveaux.
	std::vector<SpellDescriptionView> Levels;
	// Niveau actuel du spell.
	int Level;
	void serialize(std::ostream& output);

	static SpellView deserialize(std::istream& input);
private: 

};