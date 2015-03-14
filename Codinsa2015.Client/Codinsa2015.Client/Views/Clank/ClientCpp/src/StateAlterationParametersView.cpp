Value StateAlterationParametersView::serialize()
{
	Value root0(Json::objectValue);
	// DashTargetDirection
	Value DashTargetDirection_temp = this->DashTargetDirection.serialize();

	root0["DashTargetDirection"] = DashTargetDirection_temp;
	// DashTargetEntity
	Value DashTargetEntity_temp = Value(this->DashTargetEntity);

	root0["DashTargetEntity"] = DashTargetEntity_temp;
	return root0;

}

static StateAlterationParametersView StateAlterationParametersView::deserialize(Value& val)
{
	StateAlterationParametersView obj0 = StateAlterationParametersView();
	// DashTargetDirection
	Vector2 DashTargetDirection = Vector2::deserialize(val["DashTargetDirection"]);

	obj0.DashTargetDirection = DashTargetDirection;

	// DashTargetEntity
	int DashTargetEntity = val["DashTargetEntity"].asInt();

	obj0.DashTargetEntity = DashTargetEntity;

	return obj0;

}



