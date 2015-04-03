#pragma once
#include "Common.h"


class SpellView
{

public: 
	// Cooldown actuel du sort, en secondes
	float CurrentCooldown;
	// Id de l'entité possédant le sort.
	int SourceCaster;
	// Représente l'id du modèle du spell. Ce modèle décrit les différents effets du spell pour chacun
	// de ses niveaux
	int Model;
	// Niveau actuel du spell.
	int Level;
	void serialize(std::ostream& output);

	static SpellView deserialize(std::istream& input);
private: 

};