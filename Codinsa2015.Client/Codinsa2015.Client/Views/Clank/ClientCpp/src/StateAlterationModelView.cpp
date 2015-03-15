#include "../inc/StateAlterationModelView.h"
void StateAlterationModelView::serialize(std::ostream& output) {
	// Type
	output << ((int)this->Type) << '\n';
	// BaseDuration
	output << ((float)this->BaseDuration) << '\n';
	// DashGoThroughWall
	output << (this->DashGoThroughWall ? 1 : 0) << '\n';
	// DashDirectionType
	output << ((int)this->DashDirectionType) << '\n';
	// FlatValue
	output << ((float)this->FlatValue) << '\n';
	// SourcePercentADValue
	output << ((float)this->SourcePercentADValue) << '\n';
	// SourcePercentHPValue
	output << ((float)this->SourcePercentHPValue) << '\n';
	// SourcePercentMaxHPValue
	output << ((float)this->SourcePercentMaxHPValue) << '\n';
	// SourcePercentArmorValue
	output << ((float)this->SourcePercentArmorValue) << '\n';
	// SourcePercentAPValue
	output << ((float)this->SourcePercentAPValue) << '\n';
	// SourcePercentRMValue
	output << ((float)this->SourcePercentRMValue) << '\n';
	// DestPercentADValue
	output << ((float)this->DestPercentADValue) << '\n';
	// DestPercentHPValue
	output << ((float)this->DestPercentHPValue) << '\n';
	// DestPercentMaxHPValue
	output << ((float)this->DestPercentMaxHPValue) << '\n';
	// DestPercentArmorValue
	output << ((float)this->DestPercentArmorValue) << '\n';
	// DestPercentAPValue
	output << ((float)this->DestPercentAPValue) << '\n';
	// DestPercentRMValue
	output << ((float)this->DestPercentRMValue) << '\n';
	// StructureBonus
	output << ((float)this->StructureBonus) << '\n';
	// MonsterBonus
	output << ((float)this->MonsterBonus) << '\n';
	// CreepBonus
	output << ((float)this->CreepBonus) << '\n';
}

StateAlterationModelView StateAlterationModelView::deserialize(std::istream& input) {
	StateAlterationModelView _obj = StateAlterationModelView();
	// Type
	int _obj_Type; input >> _obj_Type; input.ignore(1000, '\n');
	_obj.Type = (::StateAlterationType)_obj_Type;
	// BaseDuration
	float _obj_BaseDuration; input >> _obj_BaseDuration; input.ignore(1000, '\n');
	_obj.BaseDuration = (float)_obj_BaseDuration;
	// DashGoThroughWall
	bool _obj_DashGoThroughWall; input >> _obj_DashGoThroughWall; input.ignore(1000, '\n');
	_obj.DashGoThroughWall = (bool)_obj_DashGoThroughWall;
	// DashDirectionType
	int _obj_DashDirectionType; input >> _obj_DashDirectionType; input.ignore(1000, '\n');
	_obj.DashDirectionType = (::DashDirectionType)_obj_DashDirectionType;
	// FlatValue
	float _obj_FlatValue; input >> _obj_FlatValue; input.ignore(1000, '\n');
	_obj.FlatValue = (float)_obj_FlatValue;
	// SourcePercentADValue
	float _obj_SourcePercentADValue; input >> _obj_SourcePercentADValue; input.ignore(1000, '\n');
	_obj.SourcePercentADValue = (float)_obj_SourcePercentADValue;
	// SourcePercentHPValue
	float _obj_SourcePercentHPValue; input >> _obj_SourcePercentHPValue; input.ignore(1000, '\n');
	_obj.SourcePercentHPValue = (float)_obj_SourcePercentHPValue;
	// SourcePercentMaxHPValue
	float _obj_SourcePercentMaxHPValue; input >> _obj_SourcePercentMaxHPValue; input.ignore(1000, '\n');
	_obj.SourcePercentMaxHPValue = (float)_obj_SourcePercentMaxHPValue;
	// SourcePercentArmorValue
	float _obj_SourcePercentArmorValue; input >> _obj_SourcePercentArmorValue; input.ignore(1000, '\n');
	_obj.SourcePercentArmorValue = (float)_obj_SourcePercentArmorValue;
	// SourcePercentAPValue
	float _obj_SourcePercentAPValue; input >> _obj_SourcePercentAPValue; input.ignore(1000, '\n');
	_obj.SourcePercentAPValue = (float)_obj_SourcePercentAPValue;
	// SourcePercentRMValue
	float _obj_SourcePercentRMValue; input >> _obj_SourcePercentRMValue; input.ignore(1000, '\n');
	_obj.SourcePercentRMValue = (float)_obj_SourcePercentRMValue;
	// DestPercentADValue
	float _obj_DestPercentADValue; input >> _obj_DestPercentADValue; input.ignore(1000, '\n');
	_obj.DestPercentADValue = (float)_obj_DestPercentADValue;
	// DestPercentHPValue
	float _obj_DestPercentHPValue; input >> _obj_DestPercentHPValue; input.ignore(1000, '\n');
	_obj.DestPercentHPValue = (float)_obj_DestPercentHPValue;
	// DestPercentMaxHPValue
	float _obj_DestPercentMaxHPValue; input >> _obj_DestPercentMaxHPValue; input.ignore(1000, '\n');
	_obj.DestPercentMaxHPValue = (float)_obj_DestPercentMaxHPValue;
	// DestPercentArmorValue
	float _obj_DestPercentArmorValue; input >> _obj_DestPercentArmorValue; input.ignore(1000, '\n');
	_obj.DestPercentArmorValue = (float)_obj_DestPercentArmorValue;
	// DestPercentAPValue
	float _obj_DestPercentAPValue; input >> _obj_DestPercentAPValue; input.ignore(1000, '\n');
	_obj.DestPercentAPValue = (float)_obj_DestPercentAPValue;
	// DestPercentRMValue
	float _obj_DestPercentRMValue; input >> _obj_DestPercentRMValue; input.ignore(1000, '\n');
	_obj.DestPercentRMValue = (float)_obj_DestPercentRMValue;
	// StructureBonus
	float _obj_StructureBonus; input >> _obj_StructureBonus; input.ignore(1000, '\n');
	_obj.StructureBonus = (float)_obj_StructureBonus;
	// MonsterBonus
	float _obj_MonsterBonus; input >> _obj_MonsterBonus; input.ignore(1000, '\n');
	_obj.MonsterBonus = (float)_obj_MonsterBonus;
	// CreepBonus
	float _obj_CreepBonus; input >> _obj_CreepBonus; input.ignore(1000, '\n');
	_obj.CreepBonus = (float)_obj_CreepBonus;
	return _obj;
}


