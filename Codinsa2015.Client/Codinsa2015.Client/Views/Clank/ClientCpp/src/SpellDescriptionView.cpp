Value SpellDescriptionView::serialize()
{
	Value root0(Json::objectValue);
	// BaseCooldown
	Value BaseCooldown_temp = Value(this->BaseCooldown);

	root0["BaseCooldown"] = BaseCooldown_temp;
	// CastingTime
	Value CastingTime_temp = Value(this->CastingTime);

	root0["CastingTime"] = CastingTime_temp;
	// CastingTimeAlterations
	Value CastingTimeAlterations_temp = Value(arrayValue);
	auto CastingTimeAlterations_temp_iterator = this->CastingTimeAlterations;
	for(auto CastingTimeAlterations_temp_it = CastingTimeAlterations_temp_iterator.begin();CastingTimeAlterations_temp_it != CastingTimeAlterations_temp_iterator.end();CastingTimeAlterations_temp_it++)
	{
		Value CastingTimeAlterations_temp_item = (*CastingTimeAlterations_temp_it).serialize();
		CastingTimeAlterations_temp.append(CastingTimeAlterations_temp_item);
	}

	root0["CastingTimeAlterations"] = CastingTimeAlterations_temp;
	// TargetType
	Value TargetType_temp = this->TargetType.serialize();

	root0["TargetType"] = TargetType_temp;
	// OnHitEffects
	Value OnHitEffects_temp = Value(arrayValue);
	auto OnHitEffects_temp_iterator = this->OnHitEffects;
	for(auto OnHitEffects_temp_it = OnHitEffects_temp_iterator.begin();OnHitEffects_temp_it != OnHitEffects_temp_iterator.end();OnHitEffects_temp_it++)
	{
		Value OnHitEffects_temp_item = (*OnHitEffects_temp_it).serialize();
		OnHitEffects_temp.append(OnHitEffects_temp_item);
	}

	root0["OnHitEffects"] = OnHitEffects_temp;
	return root0;

}

static SpellDescriptionView SpellDescriptionView::deserialize(Value& val)
{
	SpellDescriptionView obj0 = SpellDescriptionView();
	// BaseCooldown
	float BaseCooldown = val["BaseCooldown"].asDouble();

	obj0.BaseCooldown = BaseCooldown;

	// CastingTime
	float CastingTime = val["CastingTime"].asDouble();

	obj0.CastingTime = CastingTime;

	// CastingTimeAlterations
	vector<StateAlterationModelView> CastingTimeAlterations = vector<StateAlterationModelView>();
	auto CastingTimeAlterations_iterator = val["CastingTimeAlterations"];
	for(auto CastingTimeAlterations_it = CastingTimeAlterations_iterator.begin();CastingTimeAlterations_it != CastingTimeAlterations_iterator.end(); CastingTimeAlterations_it++)
	{
		StateAlterationModelView CastingTimeAlterations_item = StateAlterationModelView::deserialize((*CastingTimeAlterations_it));
		CastingTimeAlterations.push_back(CastingTimeAlterations_item);
	}

	obj0.CastingTimeAlterations = CastingTimeAlterations;

	// TargetType
	SpellTargetInfoView TargetType = SpellTargetInfoView::deserialize(val["TargetType"]);

	obj0.TargetType = TargetType;

	// OnHitEffects
	vector<StateAlterationModelView> OnHitEffects = vector<StateAlterationModelView>();
	auto OnHitEffects_iterator = val["OnHitEffects"];
	for(auto OnHitEffects_it = OnHitEffects_iterator.begin();OnHitEffects_it != OnHitEffects_iterator.end(); OnHitEffects_it++)
	{
		StateAlterationModelView OnHitEffects_item = StateAlterationModelView::deserialize((*OnHitEffects_it));
		OnHitEffects.push_back(OnHitEffects_item);
	}

	obj0.OnHitEffects = OnHitEffects;

	return obj0;

}



