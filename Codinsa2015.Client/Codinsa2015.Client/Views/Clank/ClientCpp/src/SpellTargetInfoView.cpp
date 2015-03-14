Value SpellTargetInfoView::serialize()
{
	Value root0(Json::objectValue);
	// Type
	Value Type_temp = Value(this->Type);

	root0["Type"] = Type_temp;
	// Range
	Value Range_temp = Value(this->Range);

	root0["Range"] = Range_temp;
	// Duration
	Value Duration_temp = Value(this->Duration);

	root0["Duration"] = Duration_temp;
	// AoeRadius
	Value AoeRadius_temp = Value(this->AoeRadius);

	root0["AoeRadius"] = AoeRadius_temp;
	// DieOnCollision
	Value DieOnCollision_temp = Value(this->DieOnCollision);

	root0["DieOnCollision"] = DieOnCollision_temp;
	// AllowedTargetTypes
	Value AllowedTargetTypes_temp = Value(this->AllowedTargetTypes);

	root0["AllowedTargetTypes"] = AllowedTargetTypes_temp;
	return root0;

}

static SpellTargetInfoView SpellTargetInfoView::deserialize(Value& val)
{
	SpellTargetInfoView obj0 = SpellTargetInfoView();
	// Type
	TargettingType Type = val["Type"].asInt();

	obj0.Type = Type;

	// Range
	float Range = val["Range"].asDouble();

	obj0.Range = Range;

	// Duration
	float Duration = val["Duration"].asDouble();

	obj0.Duration = Duration;

	// AoeRadius
	float AoeRadius = val["AoeRadius"].asDouble();

	obj0.AoeRadius = AoeRadius;

	// DieOnCollision
	bool DieOnCollision = val["DieOnCollision"].asBool();

	obj0.DieOnCollision = DieOnCollision;

	// AllowedTargetTypes
	EntityTypeRelative AllowedTargetTypes = val["AllowedTargetTypes"].asInt();

	obj0.AllowedTargetTypes = AllowedTargetTypes;

	return obj0;

}



