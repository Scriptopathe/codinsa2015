#include "../inc/StateAlterationView.h"
void StateAlterationView::serialize(std::ostream& output) {
	// Source
	output << ((int)this->Source) << '\n';
	// SourceType
	output << ((int)this->SourceType) << '\n';
	// Model
	this->Model.serialize(output);
	// Parameters
	this->Parameters.serialize(output);
	// RemainingTime
	output << ((float)this->RemainingTime) << '\n';
}

StateAlterationView StateAlterationView::deserialize(std::istream& input) {
	StateAlterationView _obj = StateAlterationView();
	// Source
	int _obj_Source; input >> _obj_Source; input.ignore(1000, '\n');
	_obj.Source = (int)_obj_Source;
	// SourceType
	int _obj_SourceType; input >> _obj_SourceType; input.ignore(1000, '\n');
	_obj.SourceType = (::StateAlterationSource)_obj_SourceType;
	// Model
	StateAlterationModelView _obj_Model = StateAlterationModelView::deserialize(input);
	_obj.Model = (::StateAlterationModelView)_obj_Model;
	// Parameters
	StateAlterationParametersView _obj_Parameters = StateAlterationParametersView::deserialize(input);
	_obj.Parameters = (::StateAlterationParametersView)_obj_Parameters;
	// RemainingTime
	float _obj_RemainingTime; input >> _obj_RemainingTime; input.ignore(1000, '\n');
	_obj.RemainingTime = (float)_obj_RemainingTime;
	return _obj;
}


