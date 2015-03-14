Value CircleShapeView::serialize()
{
	Value root0(Json::objectValue);
	// Position
	Value Position_temp = this->Position.serialize();

	root0["Position"] = Position_temp;
	// Radius
	Value Radius_temp = Value(this->Radius);

	root0["Radius"] = Radius_temp;
	return root0;

}

static CircleShapeView CircleShapeView::deserialize(Value& val)
{
	CircleShapeView obj0 = CircleShapeView();
	// Position
	Vector2 Position = Vector2::deserialize(val["Position"]);

	obj0.Position = Position;

	// Radius
	float Radius = val["Radius"].asDouble();

	obj0.Radius = Radius;

	return obj0;

}



