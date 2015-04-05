#include "../inc/SignalView.h"
void SignalView::serialize(std::ostream& output) {
	// SourceEntity
	output << ((int)this->SourceEntity) << '\n';
	// DestinationEntity
	output << ((int)this->DestinationEntity) << '\n';
	// DestinationPosition
	this->DestinationPosition.serialize(output);
}

SignalView SignalView::deserialize(std::istream& input) {
	SignalView _obj = SignalView();
	// SourceEntity
	int _obj_SourceEntity; input >> _obj_SourceEntity; input.ignore(1000, '\n');
	_obj.SourceEntity = (int)_obj_SourceEntity;
	// DestinationEntity
	int _obj_DestinationEntity; input >> _obj_DestinationEntity; input.ignore(1000, '\n');
	_obj.DestinationEntity = (int)_obj_DestinationEntity;
	// DestinationPosition
	Vector2 _obj_DestinationPosition = Vector2::deserialize(input);
	_obj.DestinationPosition = (::Vector2)_obj_DestinationPosition;
	return _obj;
}

SignalView::SignalView() {
}


