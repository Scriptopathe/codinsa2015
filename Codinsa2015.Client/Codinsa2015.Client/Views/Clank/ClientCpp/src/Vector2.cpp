Vector2::Vector2()
{
}
Vector2::Vector2(float x, float y)
{
	this.X = x;
	this.Y = y;
}
Value Vector2::serialize()
{
	Value root0(Json::objectValue);
	// X
	Value X_temp = Value(this->X);

	root0["X"] = X_temp;
	// Y
	Value Y_temp = Value(this->Y);

	root0["Y"] = Y_temp;
	return root0;

}

static Vector2 Vector2::deserialize(Value& val)
{
	Vector2 obj0 = Vector2();
	// X
	float X = val["X"].asDouble();

	obj0.X = X;

	// Y
	float Y = val["Y"].asDouble();

	obj0.Y = Y;

	return obj0;

}



