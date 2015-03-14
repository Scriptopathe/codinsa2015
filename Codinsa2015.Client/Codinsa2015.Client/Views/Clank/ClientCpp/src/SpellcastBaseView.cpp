Value SpellcastBaseView::serialize()
{
	Value root0(Json::objectValue);
	// Shape
	Value Shape_temp = this->Shape.serialize();

	root0["Shape"] = Shape_temp;
	return root0;

}

static SpellcastBaseView SpellcastBaseView::deserialize(Value& val)
{
	SpellcastBaseView obj0 = SpellcastBaseView();
	// Shape
	GenericShapeView Shape = GenericShapeView::deserialize(val["Shape"]);

	obj0.Shape = Shape;

	return obj0;

}



