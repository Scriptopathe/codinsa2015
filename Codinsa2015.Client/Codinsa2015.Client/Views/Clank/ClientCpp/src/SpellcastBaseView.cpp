#include "../inc/SpellcastBaseView.h"
void SpellcastBaseView::serialize(std::ostream& output) {
	// Shape
	this->Shape.serialize(output);
}

SpellcastBaseView SpellcastBaseView::deserialize(std::istream& input) {
	SpellcastBaseView _obj = SpellcastBaseView();
	// Shape
	GenericShapeView _obj_Shape = GenericShapeView::deserialize(input);
	_obj.Shape = (::GenericShapeView)_obj_Shape;
	return _obj;
}


