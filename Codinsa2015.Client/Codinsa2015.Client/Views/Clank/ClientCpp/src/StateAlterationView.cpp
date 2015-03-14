Value StateAlterationView::serialize()
{
	Value root0(Json::objectValue);
	// Source
	Value Source_temp = Value(this->Source);

	root0["Source"] = Source_temp;
	// SourceType
	Value SourceType_temp = Value(this->SourceType);

	root0["SourceType"] = SourceType_temp;
	// Model
	Value Model_temp = this->Model.serialize();

	root0["Model"] = Model_temp;
	// Parameters
	Value Parameters_temp = this->Parameters.serialize();

	root0["Parameters"] = Parameters_temp;
	// RemainingTime
	Value RemainingTime_temp = Value(this->RemainingTime);

	root0["RemainingTime"] = RemainingTime_temp;
	return root0;

}

static StateAlterationView StateAlterationView::deserialize(Value& val)
{
	StateAlterationView obj0 = StateAlterationView();
	// Source
	int Source = val["Source"].asInt();

	obj0.Source = Source;

	// SourceType
	StateAlterationSource SourceType = val["SourceType"].asInt();

	obj0.SourceType = SourceType;

	// Model
	StateAlterationModelView Model = StateAlterationModelView::deserialize(val["Model"]);

	obj0.Model = Model;

	// Parameters
	StateAlterationParametersView Parameters = StateAlterationParametersView::deserialize(val["Parameters"]);

	obj0.Parameters = Parameters;

	// RemainingTime
	float RemainingTime = val["RemainingTime"].asDouble();

	obj0.RemainingTime = RemainingTime;

	return obj0;

}



