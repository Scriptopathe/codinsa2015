Value MapView::serialize()
{
	Value root0(Json::objectValue);
	// Passability
	Value Passability_temp = Value(arrayValue);
	auto Passability_temp_iterator = this->Passability;
	for(auto Passability_temp_it = Passability_temp_iterator.begin();Passability_temp_it != Passability_temp_iterator.end();Passability_temp_it++)
	{
		Value Passability_temp_item = Value(arrayValue);
		auto Passability_temp_item_iterator = (*Passability_temp_it);
		for(auto Passability_temp_item_it = Passability_temp_item_iterator.begin();Passability_temp_item_it != Passability_temp_item_iterator.end();Passability_temp_item_it++)
		{
			Value Passability_temp_item_item = Value((*Passability_temp_item_it));
			Passability_temp_item.append(Passability_temp_item_item);
		}
		Passability_temp.append(Passability_temp_item);
	}

	root0["Passability"] = Passability_temp;
	return root0;

}

static MapView MapView::deserialize(Value& val)
{
	MapView obj0 = MapView();
	// Passability
	vector<vector<bool>> Passability = vector<vector<bool>>();
	auto Passability_iterator = val["Passability"];
	for(auto Passability_it = Passability_iterator.begin();Passability_it != Passability_iterator.end(); Passability_it++)
	{
		vector<bool> Passability_item = vector<bool>();
		auto Passability_item_iterator = (*Passability_it);
		for(auto Passability_item_it = Passability_item_iterator.begin();Passability_item_it != Passability_item_iterator.end(); Passability_item_it++)
		{
			bool Passability_item_item = (*Passability_item_it).asBool();
			Passability_item.push_back(Passability_item_item);
		}
		Passability.push_back(Passability_item);
	}

	obj0.Passability = Passability;

	return obj0;

}



