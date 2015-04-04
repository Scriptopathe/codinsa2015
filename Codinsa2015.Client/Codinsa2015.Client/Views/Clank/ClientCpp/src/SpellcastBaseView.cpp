#include "../inc/SpellcastBaseView.h"
void SpellcastBaseView::serialize(std::ostream& output) {
	// Shape
	this->Shape.serialize(output);
	// Name
	output << this->Name << '\n';
}

SpellcastBaseView SpellcastBaseView::deserialize(std::istream& input) {
	SpellcastBaseView _obj = SpellcastBaseView();
	// Shape
	GenericShapeView _obj_Shape = GenericShapeView::deserialize(input);
	_obj.Shape = (::GenericShapeView)_obj_Shape;
	// Name
	std::string _obj_Name; getline(input, _obj_Name);
	_obj.Name = (std::string)_obj_Name;
	return _obj;
}

SpellcastBaseView::SpellcastBaseView() {
}


