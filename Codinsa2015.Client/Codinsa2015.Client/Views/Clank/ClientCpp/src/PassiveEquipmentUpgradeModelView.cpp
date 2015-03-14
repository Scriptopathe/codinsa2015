Value PassiveEquipmentUpgradeModelView::serialize()
{
	Value root0(Json::objectValue);
	// PassiveAlterations
	Value PassiveAlterations_temp = Value(arrayValue);
	auto PassiveAlterations_temp_iterator = this->PassiveAlterations;
	for(auto PassiveAlterations_temp_it = PassiveAlterations_temp_iterator.begin();PassiveAlterations_temp_it != PassiveAlterations_temp_iterator.end();PassiveAlterations_temp_it++)
	{
		Value PassiveAlterations_temp_item = (*PassiveAlterations_temp_it).serialize();
		PassiveAlterations_temp.append(PassiveAlterations_temp_item);
	}

	root0["PassiveAlterations"] = PassiveAlterations_temp;
	// Cost
	Value Cost_temp = Value(this->Cost);

	root0["Cost"] = Cost_temp;
	return root0;

}

static PassiveEquipmentUpgradeModelView PassiveEquipmentUpgradeModelView::deserialize(Value& val)
{
	PassiveEquipmentUpgradeModelView obj0 = PassiveEquipmentUpgradeModelView();
	// PassiveAlterations
	vector<StateAlterationModelView> PassiveAlterations = vector<StateAlterationModelView>();
	auto PassiveAlterations_iterator = val["PassiveAlterations"];
	for(auto PassiveAlterations_it = PassiveAlterations_iterator.begin();PassiveAlterations_it != PassiveAlterations_iterator.end(); PassiveAlterations_it++)
	{
		StateAlterationModelView PassiveAlterations_item = StateAlterationModelView::deserialize((*PassiveAlterations_it));
		PassiveAlterations.push_back(PassiveAlterations_item);
	}

	obj0.PassiveAlterations = PassiveAlterations;

	// Cost
	float Cost = val["Cost"].asDouble();

	obj0.Cost = Cost;

	return obj0;

}



