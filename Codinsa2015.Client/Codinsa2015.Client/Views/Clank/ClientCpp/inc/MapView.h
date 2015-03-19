#pragma once
#include "Common.h"


class MapView
{

public: 
	// Tableau de passabilité de la map. true : passable, false : non passable.
	std::vector<std::vector<bool>> Passability;
	void serialize(std::ostream& output);

	static MapView deserialize(std::istream& input);
private: 

};