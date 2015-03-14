Value VisionMapView::serialize()
{
	Value root0(Json::objectValue);
	// Vision
	Value Vision_temp = Value(arrayValue);
	auto Vision_temp_iterator = this->Vision;
	for(auto Vision_temp_it = Vision_temp_iterator.begin();Vision_temp_it != Vision_temp_iterator.end();Vision_temp_it++)
	{
		Value Vision_temp_item = Value(arrayValue);
		auto Vision_temp_item_iterator = (*Vision_temp_it);
		for(auto Vision_temp_item_it = Vision_temp_item_iterator.begin();Vision_temp_item_it != Vision_temp_item_iterator.end();Vision_temp_item_it++)
		{
			Value Vision_temp_item_item = Value((*Vision_temp_item_it));
			Vision_temp_item.append(Vision_temp_item_item);
		}
		Vision_temp.append(Vision_temp_item);
	}

	root0["Vision"] = Vision_temp;
	return root0;

}

static VisionMapView VisionMapView::deserialize(Value& val)
{
	VisionMapView obj0 = VisionMapView();
	// Vision
	vector<vector<VisionFlags>> Vision = vector<vector<VisionFlags>>();
	auto Vision_iterator = val["Vision"];
	for(auto Vision_it = Vision_iterator.begin();Vision_it != Vision_iterator.end(); Vision_it++)
	{
		vector<VisionFlags> Vision_item = vector<VisionFlags>();
		auto Vision_item_iterator = (*Vision_it);
		for(auto Vision_item_it = Vision_item_iterator.begin();Vision_item_it != Vision_item_iterator.end(); Vision_item_it++)
		{
			VisionFlags Vision_item_item = (*Vision_item_it).asInt();
			Vision_item.push_back(Vision_item_item);
		}
		Vision.push_back(Vision_item);
	}

	obj0.Vision = Vision;

	return obj0;

}



