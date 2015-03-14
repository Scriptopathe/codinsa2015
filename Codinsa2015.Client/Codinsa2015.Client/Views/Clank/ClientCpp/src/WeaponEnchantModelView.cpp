Value WeaponEnchantModelView::serialize()
{
	Value root0(Json::objectValue);
	// OnHitEffects
	Value OnHitEffects_temp = Value(arrayValue);
	auto OnHitEffects_temp_iterator = this->OnHitEffects;
	for(auto OnHitEffects_temp_it = OnHitEffects_temp_iterator.begin();OnHitEffects_temp_it != OnHitEffects_temp_iterator.end();OnHitEffects_temp_it++)
	{
		Value OnHitEffects_temp_item = (*OnHitEffects_temp_it).serialize();
		OnHitEffects_temp.append(OnHitEffects_temp_item);
	}

	root0["OnHitEffects"] = OnHitEffects_temp;
	// CastingEffects
	Value CastingEffects_temp = Value(arrayValue);
	auto CastingEffects_temp_iterator = this->CastingEffects;
	for(auto CastingEffects_temp_it = CastingEffects_temp_iterator.begin();CastingEffects_temp_it != CastingEffects_temp_iterator.end();CastingEffects_temp_it++)
	{
		Value CastingEffects_temp_item = (*CastingEffects_temp_it).serialize();
		CastingEffects_temp.append(CastingEffects_temp_item);
	}

	root0["CastingEffects"] = CastingEffects_temp;
	// PassiveEffects
	Value PassiveEffects_temp = Value(arrayValue);
	auto PassiveEffects_temp_iterator = this->PassiveEffects;
	for(auto PassiveEffects_temp_it = PassiveEffects_temp_iterator.begin();PassiveEffects_temp_it != PassiveEffects_temp_iterator.end();PassiveEffects_temp_it++)
	{
		Value PassiveEffects_temp_item = (*PassiveEffects_temp_it).serialize();
		PassiveEffects_temp.append(PassiveEffects_temp_item);
	}

	root0["PassiveEffects"] = PassiveEffects_temp;
	return root0;

}

static WeaponEnchantModelView WeaponEnchantModelView::deserialize(Value& val)
{
	WeaponEnchantModelView obj0 = WeaponEnchantModelView();
	// OnHitEffects
	vector<StateAlterationModelView> OnHitEffects = vector<StateAlterationModelView>();
	auto OnHitEffects_iterator = val["OnHitEffects"];
	for(auto OnHitEffects_it = OnHitEffects_iterator.begin();OnHitEffects_it != OnHitEffects_iterator.end(); OnHitEffects_it++)
	{
		StateAlterationModelView OnHitEffects_item = StateAlterationModelView::deserialize((*OnHitEffects_it));
		OnHitEffects.push_back(OnHitEffects_item);
	}

	obj0.OnHitEffects = OnHitEffects;

	// CastingEffects
	vector<StateAlterationModelView> CastingEffects = vector<StateAlterationModelView>();
	auto CastingEffects_iterator = val["CastingEffects"];
	for(auto CastingEffects_it = CastingEffects_iterator.begin();CastingEffects_it != CastingEffects_iterator.end(); CastingEffects_it++)
	{
		StateAlterationModelView CastingEffects_item = StateAlterationModelView::deserialize((*CastingEffects_it));
		CastingEffects.push_back(CastingEffects_item);
	}

	obj0.CastingEffects = CastingEffects;

	// PassiveEffects
	vector<StateAlterationModelView> PassiveEffects = vector<StateAlterationModelView>();
	auto PassiveEffects_iterator = val["PassiveEffects"];
	for(auto PassiveEffects_it = PassiveEffects_iterator.begin();PassiveEffects_it != PassiveEffects_iterator.end(); PassiveEffects_it++)
	{
		StateAlterationModelView PassiveEffects_item = StateAlterationModelView::deserialize((*PassiveEffects_it));
		PassiveEffects.push_back(PassiveEffects_item);
	}

	obj0.PassiveEffects = PassiveEffects;

	return obj0;

}



