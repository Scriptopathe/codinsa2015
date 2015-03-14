Value SpellCastTargetInfoView::serialize()
{
	Value root0(Json::objectValue);
	// Type
	Value Type_temp = Value(this->Type);

	root0["Type"] = Type_temp;
	// TargetPosition
	Value TargetPosition_temp = this->TargetPosition.serialize();

	root0["TargetPosition"] = TargetPosition_temp;
	// TargetDirection
	Value TargetDirection_temp = this->TargetDirection.serialize();

	root0["TargetDirection"] = TargetDirection_temp;
	// TargetId
	Value TargetId_temp = Value(this->TargetId);

	root0["TargetId"] = TargetId_temp;
	return root0;

}

static SpellCastTargetInfoView SpellCastTargetInfoView::deserialize(Value& val)
{
	SpellCastTargetInfoView obj0 = SpellCastTargetInfoView();
	// Type
	TargettingType Type = val["Type"].asInt();

	obj0.Type = Type;

	// TargetPosition
	Vector2 TargetPosition = Vector2::deserialize(val["TargetPosition"]);

	obj0.TargetPosition = TargetPosition;

	// TargetDirection
	Vector2 TargetDirection = Vector2::deserialize(val["TargetDirection"]);

	obj0.TargetDirection = TargetDirection;

	// TargetId
	int TargetId = val["TargetId"].asInt();

	obj0.TargetId = TargetId;

	return obj0;

}



