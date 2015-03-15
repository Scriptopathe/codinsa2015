#pragma once
#include "Common.h"


class VisionMapView
{

public: 
	std::vector<std::vector<VisionFlags>> Vision;
	void serialize(std::ostream& output);

	static VisionMapView deserialize(std::istream& input);
private: 

};