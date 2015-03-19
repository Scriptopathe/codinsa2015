#pragma once
#include "Common.h"
#include "Vector2.h"


class SpellCastTargetInfoView
{

public: 
	// Type de ciblage de cet objet TargetInfo.
	TargettingType Type;
	// Retourne la position de la cible, si le type de ciblage (Type) est TargettingType.Position.
	Vector2 TargetPosition;
	// Retourne la direction de la cible, si le type de ciblage (Type) est TargettingType.Direction.
	// Ce vecteur est transformé automatiquement en vecteur unitaire.
	Vector2 TargetDirection;
	// Retourne l'id de la cible, si le type de cibale (Type) est TargettingType.Targetted.
	int TargetId;
	void serialize(std::ostream& output);

	static SpellCastTargetInfoView deserialize(std::istream& input);
private: 

};