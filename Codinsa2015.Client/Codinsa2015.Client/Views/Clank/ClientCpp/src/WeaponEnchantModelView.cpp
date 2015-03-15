#include "../inc/WeaponEnchantModelView.h"
void WeaponEnchantModelView::serialize(std::ostream& output) {
	// OnHitEffects
	output << this->OnHitEffects.size() << '\n';
	for(int OnHitEffects_it = 0; OnHitEffects_it < this->OnHitEffects.size(); OnHitEffects_it++) {
		this->OnHitEffects[OnHitEffects_it].serialize(output);
	}

	// CastingEffects
	output << this->CastingEffects.size() << '\n';
	for(int CastingEffects_it = 0; CastingEffects_it < this->CastingEffects.size(); CastingEffects_it++) {
		this->CastingEffects[CastingEffects_it].serialize(output);
	}

	// PassiveEffects
	output << this->PassiveEffects.size() << '\n';
	for(int PassiveEffects_it = 0; PassiveEffects_it < this->PassiveEffects.size(); PassiveEffects_it++) {
		this->PassiveEffects[PassiveEffects_it].serialize(output);
	}

}

WeaponEnchantModelView WeaponEnchantModelView::deserialize(std::istream& input) {
	WeaponEnchantModelView _obj = WeaponEnchantModelView();
	// OnHitEffects
	std::vector<StateAlterationModelView> _obj_OnHitEffects = std::vector<StateAlterationModelView>();
	int _obj_OnHitEffects_count; input >> _obj_OnHitEffects_count; input.ignore(1000, '\n');
	for(int _obj_OnHitEffects_i = 0; _obj_OnHitEffects_i < _obj_OnHitEffects_count; _obj_OnHitEffects_i++) {
		StateAlterationModelView _obj_OnHitEffects_e = StateAlterationModelView::deserialize(input);
		_obj_OnHitEffects.push_back((StateAlterationModelView)_obj_OnHitEffects_e);
	}

	_obj.OnHitEffects = (::std::vector<StateAlterationModelView>)_obj_OnHitEffects;
	// CastingEffects
	std::vector<StateAlterationModelView> _obj_CastingEffects = std::vector<StateAlterationModelView>();
	int _obj_CastingEffects_count; input >> _obj_CastingEffects_count; input.ignore(1000, '\n');
	for(int _obj_CastingEffects_i = 0; _obj_CastingEffects_i < _obj_CastingEffects_count; _obj_CastingEffects_i++) {
		StateAlterationModelView _obj_CastingEffects_e = StateAlterationModelView::deserialize(input);
		_obj_CastingEffects.push_back((StateAlterationModelView)_obj_CastingEffects_e);
	}

	_obj.CastingEffects = (::std::vector<StateAlterationModelView>)_obj_CastingEffects;
	// PassiveEffects
	std::vector<StateAlterationModelView> _obj_PassiveEffects = std::vector<StateAlterationModelView>();
	int _obj_PassiveEffects_count; input >> _obj_PassiveEffects_count; input.ignore(1000, '\n');
	for(int _obj_PassiveEffects_i = 0; _obj_PassiveEffects_i < _obj_PassiveEffects_count; _obj_PassiveEffects_i++) {
		StateAlterationModelView _obj_PassiveEffects_e = StateAlterationModelView::deserialize(input);
		_obj_PassiveEffects.push_back((StateAlterationModelView)_obj_PassiveEffects_e);
	}

	_obj.PassiveEffects = (::std::vector<StateAlterationModelView>)_obj_PassiveEffects;
	return _obj;
}


