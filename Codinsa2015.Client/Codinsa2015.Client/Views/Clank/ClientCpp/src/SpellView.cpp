Value SpellView::serialize()
{
	Value root0(Json::objectValue);
	// CurrentCooldown
	Value CurrentCooldown_temp = Value(this->CurrentCooldown);

	root0["CurrentCooldown"] = CurrentCooldown_temp;
	// SourceCaster
	Value SourceCaster_temp = Value(this->SourceCaster);

	root0["SourceCaster"] = SourceCaster_temp;
	// Levels
	Value Levels_temp = Value(arrayValue);
	auto Levels_temp_iterator = this->Levels;
	for(auto Levels_temp_it = Levels_temp_iterator.begin();Levels_temp_it != Levels_temp_iterator.end();Levels_temp_it++)
	{
		Value Levels_temp_item = (*Levels_temp_it).serialize();
		Levels_temp.append(Levels_temp_item);
	}

	root0["Levels"] = Levels_temp;
	// Level
	Value Level_temp = Value(this->Level);

	root0["Level"] = Level_temp;
	return root0;

}

static SpellView SpellView::deserialize(Value& val)
{
	SpellView obj0 = SpellView();
	// CurrentCooldown
	float CurrentCooldown = val["CurrentCooldown"].asDouble();

	obj0.CurrentCooldown = CurrentCooldown;

	// SourceCaster
	int SourceCaster = val["SourceCaster"].asInt();

	obj0.SourceCaster = SourceCaster;

	// Levels
	vector<SpellDescriptionView> Levels = vector<SpellDescriptionView>();
	auto Levels_iterator = val["Levels"];
	for(auto Levels_it = Levels_iterator.begin();Levels_it != Levels_iterator.end(); Levels_it++)
	{
		SpellDescriptionView Levels_item = SpellDescriptionView::deserialize((*Levels_it));
		Levels.push_back(Levels_item);
	}

	obj0.Levels = Levels;

	// Level
	int Level = val["Level"].asInt();

	obj0.Level = Level;

	return obj0;

}



