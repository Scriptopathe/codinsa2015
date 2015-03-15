#include "../inc/StateAlterationParametersView.h"
void StateAlterationParametersView::serialize(std::ostream& output) {
	// DashTargetDirection
	this->DashTargetDirection.serialize(output);
	// DashTargetEntity
	output << ((int)this->DashTargetEntity) << '\n';
}

StateAlterationParametersView StateAlterationParametersView::deserialize(std::istream& input) {
	StateAlterationParametersView _obj = StateAlterationParametersView();
	// DashTargetDirection
	Vector2 _obj_DashTargetDirection = Vector2::deserialize(input);
	_obj.DashTargetDirection = (::Vector2)_obj_DashTargetDirection;
	// DashTargetEntity
	int _obj_DashTargetEntity; input >> _obj_DashTargetEntity; input.ignore(1000, '\n');
	_obj.DashTargetEntity = (int)_obj_DashTargetEntity;
	return _obj;
}


