#pragma once
#include "Common.h"
#include "Vector2.h"


class SignalView
{

public: 
	// id de l'entité émétrice du signal
	int SourceEntity;
	// ID de l'entité que cible le signal (pour les signaux AttackEntity, DefendEntity)
	int DestinationEntity;
	// Position que cible le signal (pour les signaux ComingToPosition)
	Vector2 DestinationPosition;
	void serialize(std::ostream& output);

	static SignalView deserialize(std::istream& input);
	SignalView();
private: 

};