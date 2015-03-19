#pragma once
#include "Common.h"


class VisionMapView
{

public: 
	// Représente la vision qu'ont les 2 équipes sur l'ensemble de la map.
	std::vector<std::vector<VisionFlags>> Vision;
	void serialize(std::ostream& output);

	static VisionMapView deserialize(std::istream& input);
private: 

};