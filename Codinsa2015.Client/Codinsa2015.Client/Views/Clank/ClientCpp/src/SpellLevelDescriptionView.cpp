#include "../inc/SpellLevelDescriptionView.h"
void SpellLevelDescriptionView::serialize(std::ostream& output) {
	// BaseCooldown
	output << ((float)this->BaseCooldown) << '\n';
	// CastingTime
	output << ((float)this->CastingTime) << '\n';
	// CastingTimeAlterations
	output << this->CastingTimeAlterations.size() << '\n';
	for(int CastingTimeAlterations_it = 0; CastingTimeAlterations_it < this->CastingTimeAlterations.size(); CastingTimeAlterations_it++) {
		this->CastingTimeAlterations[CastingTimeAlterations_it].serialize(output);
	}

	// TargetType
	this->TargetType.serialize(output);
	// OnHitEffects
	output << this->OnHitEffects.size() << '\n';
	for(int OnHitEffects_it = 0; OnHitEffects_it < this->OnHitEffects.size(); OnHitEffects_it++) {
		this->OnHitEffects[OnHitEffects_it].serialize(output);
	}

}

SpellLevelDescriptionView SpellLevelDescriptionView::deserialize(std::istream& input) {
	SpellLevelDescriptionView _obj = SpellLevelDescriptionView();
	// BaseCooldown
	float _obj_BaseCooldown; input >> _obj_BaseCooldown; input.ignore(1000, '\n');
	_obj.BaseCooldown = (float)_obj_BaseCooldown;
	// CastingTime
	float _obj_CastingTime; input >> _obj_CastingTime; input.ignore(1000, '\n');
	_obj.CastingTime = (float)_obj_CastingTime;
	// CastingTimeAlterations
	std::vector<StateAlterationModelView> _obj_CastingTimeAlterations = std::vector<StateAlterationModelView>();
	int _obj_CastingTimeAlterations_count; input >> _obj_CastingTimeAlterations_count; input.ignore(1000, '\n');
	for(int _obj_CastingTimeAlterations_i = 0; _obj_CastingTimeAlterations_i < _obj_CastingTimeAlterations_count; _obj_CastingTimeAlterations_i++) {
		StateAlterationModelView _obj_CastingTimeAlterations_e = StateAlterationModelView::deserialize(input);
		_obj_CastingTimeAlterations.push_back((StateAlterationModelView)_obj_CastingTimeAlterations_e);
	}

	_obj.CastingTimeAlterations = (::std::vector<StateAlterationModelView>)_obj_CastingTimeAlterations;
	// TargetType
	SpellTargetInfoView _obj_TargetType = SpellTargetInfoView::deserialize(input);
	_obj.TargetType = (::SpellTargetInfoView)_obj_TargetType;
	// OnHitEffects
	std::vector<StateAlterationModelView> _obj_OnHitEffects = std::vector<StateAlterationModelView>();
	int _obj_OnHitEffects_count; input >> _obj_OnHitEffects_count; input.ignore(1000, '\n');
	for(int _obj_OnHitEffects_i = 0; _obj_OnHitEffects_i < _obj_OnHitEffects_count; _obj_OnHitEffects_i++) {
		StateAlterationModelView _obj_OnHitEffects_e = StateAlterationModelView::deserialize(input);
		_obj_OnHitEffects.push_back((StateAlterationModelView)_obj_OnHitEffects_e);
	}

	_obj.OnHitEffects = (::std::vector<StateAlterationModelView>)_obj_OnHitEffects;
	return _obj;
}

SpellLevelDescriptionView::SpellLevelDescriptionView() {
}


