Value GenericShapeView::serialize()
{
	Value root0(Json::objectValue);
	// Position
	Value Position_temp = this->Position.serialize();

	root0["Position"] = Position_temp;
	// Radius
	Value Radius_temp = Value(this->Radius);

	root0["Radius"] = Radius_temp;
	// Size
	Value Size_temp = this->Size.serialize();

	root0["Size"] = Size_temp;
	// ShapeType
	Value ShapeType_temp = Value(this->ShapeType);

	root0["ShapeType"] = ShapeType_temp;
	return root0;

}

static GenericShapeView GenericShapeView::deserialize(Value& val)
{
	GenericShapeView obj0 = GenericShapeView();
	// Position
	Vector2 Position = Vector2::deserialize(val["Position"]);

	obj0.Position = Position;

	// Radius
	float Radius = val["Radius"].asDouble();

	obj0.Radius = Radius;

	// Size
	Vector2 Size = Vector2::deserialize(val["Size"]);

	obj0.Size = Size;

	// ShapeType
	GenericShapeType ShapeType = val["ShapeType"].asInt();

	obj0.ShapeType = ShapeType;

	return obj0;

}



