#pragma once
#include "Common.h"


class SpellTargetInfoView
{

public: 
	// Type de ciblage du sort.
	TargettingType Type;
	// Range du sort en unités métriques.
	float Range;
	// Durée en secondes que met le sort à atteindre la position donnée.
	float Duration;
	// Rayon du sort. (non utilisé pour les sort targetted)
	float AoeRadius;
	// Obtient une valeur indiquant si le sort est détruit lors d'une collision avec une entité
	bool DieOnCollision;
	// Retourne le type de cibles pouvant être touchées par ce sort.
	EntityTypeRelative AllowedTargetTypes;
	void serialize(std::ostream& output);

	static SpellTargetInfoView deserialize(std::istream& input);
	SpellTargetInfoView();
private: 

};