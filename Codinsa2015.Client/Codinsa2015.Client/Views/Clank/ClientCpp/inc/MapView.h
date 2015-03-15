#pragma once
#include "Common.h"


class MapView
{

public: 
	std::vector<std::vector<bool>> Passability;
	void serialize(std::ostream& output);

	static MapView deserialize(std::istream& input);
private: 

};