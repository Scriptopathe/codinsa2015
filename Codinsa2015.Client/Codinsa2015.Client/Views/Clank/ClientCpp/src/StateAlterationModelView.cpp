Value StateAlterationModelView::serialize()
{
	Value root0(Json::objectValue);
	// Type
	Value Type_temp = Value(this->Type);

	root0["Type"] = Type_temp;
	// BaseDuration
	Value BaseDuration_temp = Value(this->BaseDuration);

	root0["BaseDuration"] = BaseDuration_temp;
	// DashGoThroughWall
	Value DashGoThroughWall_temp = Value(this->DashGoThroughWall);

	root0["DashGoThroughWall"] = DashGoThroughWall_temp;
	// DashDirectionType
	Value DashDirectionType_temp = Value(this->DashDirectionType);

	root0["DashDirectionType"] = DashDirectionType_temp;
	// FlatValue
	Value FlatValue_temp = Value(this->FlatValue);

	root0["FlatValue"] = FlatValue_temp;
	// SourcePercentADValue
	Value SourcePercentADValue_temp = Value(this->SourcePercentADValue);

	root0["SourcePercentADValue"] = SourcePercentADValue_temp;
	// SourcePercentHPValue
	Value SourcePercentHPValue_temp = Value(this->SourcePercentHPValue);

	root0["SourcePercentHPValue"] = SourcePercentHPValue_temp;
	// SourcePercentMaxHPValue
	Value SourcePercentMaxHPValue_temp = Value(this->SourcePercentMaxHPValue);

	root0["SourcePercentMaxHPValue"] = SourcePercentMaxHPValue_temp;
	// SourcePercentArmorValue
	Value SourcePercentArmorValue_temp = Value(this->SourcePercentArmorValue);

	root0["SourcePercentArmorValue"] = SourcePercentArmorValue_temp;
	// SourcePercentAPValue
	Value SourcePercentAPValue_temp = Value(this->SourcePercentAPValue);

	root0["SourcePercentAPValue"] = SourcePercentAPValue_temp;
	// SourcePercentRMValue
	Value SourcePercentRMValue_temp = Value(this->SourcePercentRMValue);

	root0["SourcePercentRMValue"] = SourcePercentRMValue_temp;
	// DestPercentADValue
	Value DestPercentADValue_temp = Value(this->DestPercentADValue);

	root0["DestPercentADValue"] = DestPercentADValue_temp;
	// DestPercentHPValue
	Value DestPercentHPValue_temp = Value(this->DestPercentHPValue);

	root0["DestPercentHPValue"] = DestPercentHPValue_temp;
	// DestPercentMaxHPValue
	Value DestPercentMaxHPValue_temp = Value(this->DestPercentMaxHPValue);

	root0["DestPercentMaxHPValue"] = DestPercentMaxHPValue_temp;
	// DestPercentArmorValue
	Value DestPercentArmorValue_temp = Value(this->DestPercentArmorValue);

	root0["DestPercentArmorValue"] = DestPercentArmorValue_temp;
	// DestPercentAPValue
	Value DestPercentAPValue_temp = Value(this->DestPercentAPValue);

	root0["DestPercentAPValue"] = DestPercentAPValue_temp;
	// DestPercentRMValue
	Value DestPercentRMValue_temp = Value(this->DestPercentRMValue);

	root0["DestPercentRMValue"] = DestPercentRMValue_temp;
	// StructureBonus
	Value StructureBonus_temp = Value(this->StructureBonus);

	root0["StructureBonus"] = StructureBonus_temp;
	// MonsterBonus
	Value MonsterBonus_temp = Value(this->MonsterBonus);

	root0["MonsterBonus"] = MonsterBonus_temp;
	// CreepBonus
	Value CreepBonus_temp = Value(this->CreepBonus);

	root0["CreepBonus"] = CreepBonus_temp;
	return root0;

}

static StateAlterationModelView StateAlterationModelView::deserialize(Value& val)
{
	StateAlterationModelView obj0 = StateAlterationModelView();
	// Type
	StateAlterationType Type = val["Type"].asInt();

	obj0.Type = Type;

	// BaseDuration
	float BaseDuration = val["BaseDuration"].asDouble();

	obj0.BaseDuration = BaseDuration;

	// DashGoThroughWall
	bool DashGoThroughWall = val["DashGoThroughWall"].asBool();

	obj0.DashGoThroughWall = DashGoThroughWall;

	// DashDirectionType
	DashDirectionType DashDirectionType = val["DashDirectionType"].asInt();

	obj0.DashDirectionType = DashDirectionType;

	// FlatValue
	float FlatValue = val["FlatValue"].asDouble();

	obj0.FlatValue = FlatValue;

	// SourcePercentADValue
	float SourcePercentADValue = val["SourcePercentADValue"].asDouble();

	obj0.SourcePercentADValue = SourcePercentADValue;

	// SourcePercentHPValue
	float SourcePercentHPValue = val["SourcePercentHPValue"].asDouble();

	obj0.SourcePercentHPValue = SourcePercentHPValue;

	// SourcePercentMaxHPValue
	float SourcePercentMaxHPValue = val["SourcePercentMaxHPValue"].asDouble();

	obj0.SourcePercentMaxHPValue = SourcePercentMaxHPValue;

	// SourcePercentArmorValue
	float SourcePercentArmorValue = val["SourcePercentArmorValue"].asDouble();

	obj0.SourcePercentArmorValue = SourcePercentArmorValue;

	// SourcePercentAPValue
	float SourcePercentAPValue = val["SourcePercentAPValue"].asDouble();

	obj0.SourcePercentAPValue = SourcePercentAPValue;

	// SourcePercentRMValue
	float SourcePercentRMValue = val["SourcePercentRMValue"].asDouble();

	obj0.SourcePercentRMValue = SourcePercentRMValue;

	// DestPercentADValue
	float DestPercentADValue = val["DestPercentADValue"].asDouble();

	obj0.DestPercentADValue = DestPercentADValue;

	// DestPercentHPValue
	float DestPercentHPValue = val["DestPercentHPValue"].asDouble();

	obj0.DestPercentHPValue = DestPercentHPValue;

	// DestPercentMaxHPValue
	float DestPercentMaxHPValue = val["DestPercentMaxHPValue"].asDouble();

	obj0.DestPercentMaxHPValue = DestPercentMaxHPValue;

	// DestPercentArmorValue
	float DestPercentArmorValue = val["DestPercentArmorValue"].asDouble();

	obj0.DestPercentArmorValue = DestPercentArmorValue;

	// DestPercentAPValue
	float DestPercentAPValue = val["DestPercentAPValue"].asDouble();

	obj0.DestPercentAPValue = DestPercentAPValue;

	// DestPercentRMValue
	float DestPercentRMValue = val["DestPercentRMValue"].asDouble();

	obj0.DestPercentRMValue = DestPercentRMValue;

	// StructureBonus
	float StructureBonus = val["StructureBonus"].asDouble();

	obj0.StructureBonus = StructureBonus;

	// MonsterBonus
	float MonsterBonus = val["MonsterBonus"].asDouble();

	obj0.MonsterBonus = MonsterBonus;

	// CreepBonus
	float CreepBonus = val["CreepBonus"].asDouble();

	obj0.CreepBonus = CreepBonus;

	return obj0;

}



