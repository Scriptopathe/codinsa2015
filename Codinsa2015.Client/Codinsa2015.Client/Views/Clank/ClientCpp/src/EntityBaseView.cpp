#include "../inc/EntityBaseView.h"
void EntityBaseView::serialize(std::ostream& output) {
	// GetMagicResist
	output << ((float)this->GetMagicResist) << '\n';
	// GetAbilityPower
	output << ((float)this->GetAbilityPower) << '\n';
	// GetCooldownReduction
	output << ((float)this->GetCooldownReduction) << '\n';
	// GetMoveSpeed
	output << ((float)this->GetMoveSpeed) << '\n';
	// GetAttackSpeed
	output << ((float)this->GetAttackSpeed) << '\n';
	// GetHPRegen
	output << ((float)this->GetHPRegen) << '\n';
	// GetAttackDamage
	output << ((float)this->GetAttackDamage) << '\n';
	// GetArmor
	output << ((float)this->GetArmor) << '\n';
	// GetHP
	output << ((float)this->GetHP) << '\n';
	// GetMaxHP
	output << ((float)this->GetMaxHP) << '\n';
	// UniquePassiveLevel
	output << ((int)this->UniquePassiveLevel) << '\n';
	// UniquePassive
	output << ((int)this->UniquePassive) << '\n';
	// Role
	output << ((int)this->Role) << '\n';
	// StateAlterations
	output << this->StateAlterations.size() << '\n';
	for(int StateAlterations_it = 0; StateAlterations_it < this->StateAlterations.size(); StateAlterations_it++) {
		this->StateAlterations[StateAlterations_it].serialize(output);
	}

	// BaseArmor
	output << ((float)this->BaseArmor) << '\n';
	// Direction
	this->Direction.serialize(output);
	// Position
	this->Position.serialize(output);
	// ShieldPoints
	output << ((float)this->ShieldPoints) << '\n';
	// HP
	output << ((float)this->HP) << '\n';
	// BaseHPRegen
	output << ((float)this->BaseHPRegen) << '\n';
	// BaseMaxHP
	output << ((float)this->BaseMaxHP) << '\n';
	// BaseMoveSpeed
	output << ((float)this->BaseMoveSpeed) << '\n';
	// IsDead
	output << (this->IsDead ? 1 : 0) << '\n';
	// Type
	output << ((int)this->Type) << '\n';
	// ID
	output << ((int)this->ID) << '\n';
	// BaseAttackDamage
	output << ((float)this->BaseAttackDamage) << '\n';
	// BaseCooldownReduction
	output << ((float)this->BaseCooldownReduction) << '\n';
	// BaseAttackSpeed
	output << ((float)this->BaseAttackSpeed) << '\n';
	// BaseAbilityPower
	output << ((float)this->BaseAbilityPower) << '\n';
	// BaseMagicResist
	output << ((float)this->BaseMagicResist) << '\n';
	// IsRooted
	output << (this->IsRooted ? 1 : 0) << '\n';
	// IsSilenced
	output << (this->IsSilenced ? 1 : 0) << '\n';
	// IsStuned
	output << (this->IsStuned ? 1 : 0) << '\n';
	// IsDamageImmune
	output << (this->IsDamageImmune ? 1 : 0) << '\n';
	// IsControlImmune
	output << (this->IsControlImmune ? 1 : 0) << '\n';
	// IsBlind
	output << (this->IsBlind ? 1 : 0) << '\n';
	// IsStealthed
	output << (this->IsStealthed ? 1 : 0) << '\n';
	// HasTrueVision
	output << (this->HasTrueVision ? 1 : 0) << '\n';
	// HasWardVision
	output << (this->HasWardVision ? 1 : 0) << '\n';
	// VisionRange
	output << ((float)this->VisionRange) << '\n';
}

EntityBaseView EntityBaseView::deserialize(std::istream& input) {
	EntityBaseView _obj = EntityBaseView();
	// GetMagicResist
	float _obj_GetMagicResist; input >> _obj_GetMagicResist; input.ignore(1000, '\n');
	_obj.GetMagicResist = (float)_obj_GetMagicResist;
	// GetAbilityPower
	float _obj_GetAbilityPower; input >> _obj_GetAbilityPower; input.ignore(1000, '\n');
	_obj.GetAbilityPower = (float)_obj_GetAbilityPower;
	// GetCooldownReduction
	float _obj_GetCooldownReduction; input >> _obj_GetCooldownReduction; input.ignore(1000, '\n');
	_obj.GetCooldownReduction = (float)_obj_GetCooldownReduction;
	// GetMoveSpeed
	float _obj_GetMoveSpeed; input >> _obj_GetMoveSpeed; input.ignore(1000, '\n');
	_obj.GetMoveSpeed = (float)_obj_GetMoveSpeed;
	// GetAttackSpeed
	float _obj_GetAttackSpeed; input >> _obj_GetAttackSpeed; input.ignore(1000, '\n');
	_obj.GetAttackSpeed = (float)_obj_GetAttackSpeed;
	// GetHPRegen
	float _obj_GetHPRegen; input >> _obj_GetHPRegen; input.ignore(1000, '\n');
	_obj.GetHPRegen = (float)_obj_GetHPRegen;
	// GetAttackDamage
	float _obj_GetAttackDamage; input >> _obj_GetAttackDamage; input.ignore(1000, '\n');
	_obj.GetAttackDamage = (float)_obj_GetAttackDamage;
	// GetArmor
	float _obj_GetArmor; input >> _obj_GetArmor; input.ignore(1000, '\n');
	_obj.GetArmor = (float)_obj_GetArmor;
	// GetHP
	float _obj_GetHP; input >> _obj_GetHP; input.ignore(1000, '\n');
	_obj.GetHP = (float)_obj_GetHP;
	// GetMaxHP
	float _obj_GetMaxHP; input >> _obj_GetMaxHP; input.ignore(1000, '\n');
	_obj.GetMaxHP = (float)_obj_GetMaxHP;
	// UniquePassiveLevel
	int _obj_UniquePassiveLevel; input >> _obj_UniquePassiveLevel; input.ignore(1000, '\n');
	_obj.UniquePassiveLevel = (int)_obj_UniquePassiveLevel;
	// UniquePassive
	int _obj_UniquePassive_asInt; input >> _obj_UniquePassive; input.ignore(1000, '\n');
	EntityUniquePassives _obj_UniquePassive = (EntityUniquePassives)_obj_UniquePassive_asInt;
	_obj.UniquePassive = (::EntityUniquePassives)_obj_UniquePassive;
	// Role
	int _obj_Role_asInt; input >> _obj_Role; input.ignore(1000, '\n');
	EntityHeroRole _obj_Role = (EntityHeroRole)_obj_Role_asInt;
	_obj.Role = (::EntityHeroRole)_obj_Role;
	// StateAlterations
	std::vector<StateAlterationView> _obj_StateAlterations = std::vector<StateAlterationView>();
	int _obj_StateAlterations_count; input >> _obj_StateAlterations_count; input.ignore(1000, '\n');
	for(int _obj_StateAlterations_i = 0; _obj_StateAlterations_i < _obj_StateAlterations_count; _obj_StateAlterations_i++) {
		StateAlterationView _obj_StateAlterations_e = StateAlterationView::deserialize(input);
		_obj_StateAlterations.push_back((StateAlterationView)_obj_StateAlterations_e);
	}

	_obj.StateAlterations = (::std::vector<StateAlterationView>)_obj_StateAlterations;
	// BaseArmor
	float _obj_BaseArmor; input >> _obj_BaseArmor; input.ignore(1000, '\n');
	_obj.BaseArmor = (float)_obj_BaseArmor;
	// Direction
	Vector2 _obj_Direction = Vector2::deserialize(input);
	_obj.Direction = (::Vector2)_obj_Direction;
	// Position
	Vector2 _obj_Position = Vector2::deserialize(input);
	_obj.Position = (::Vector2)_obj_Position;
	// ShieldPoints
	float _obj_ShieldPoints; input >> _obj_ShieldPoints; input.ignore(1000, '\n');
	_obj.ShieldPoints = (float)_obj_ShieldPoints;
	// HP
	float _obj_HP; input >> _obj_HP; input.ignore(1000, '\n');
	_obj.HP = (float)_obj_HP;
	// BaseHPRegen
	float _obj_BaseHPRegen; input >> _obj_BaseHPRegen; input.ignore(1000, '\n');
	_obj.BaseHPRegen = (float)_obj_BaseHPRegen;
	// BaseMaxHP
	float _obj_BaseMaxHP; input >> _obj_BaseMaxHP; input.ignore(1000, '\n');
	_obj.BaseMaxHP = (float)_obj_BaseMaxHP;
	// BaseMoveSpeed
	float _obj_BaseMoveSpeed; input >> _obj_BaseMoveSpeed; input.ignore(1000, '\n');
	_obj.BaseMoveSpeed = (float)_obj_BaseMoveSpeed;
	// IsDead
	bool _obj_IsDead; input >> _obj_IsDead; input.ignore(1000, '\n');
	_obj.IsDead = (bool)_obj_IsDead;
	// Type
	int _obj_Type_asInt; input >> _obj_Type; input.ignore(1000, '\n');
	EntityType _obj_Type = (EntityType)_obj_Type_asInt;
	_obj.Type = (::EntityType)_obj_Type;
	// ID
	int _obj_ID; input >> _obj_ID; input.ignore(1000, '\n');
	_obj.ID = (int)_obj_ID;
	// BaseAttackDamage
	float _obj_BaseAttackDamage; input >> _obj_BaseAttackDamage; input.ignore(1000, '\n');
	_obj.BaseAttackDamage = (float)_obj_BaseAttackDamage;
	// BaseCooldownReduction
	float _obj_BaseCooldownReduction; input >> _obj_BaseCooldownReduction; input.ignore(1000, '\n');
	_obj.BaseCooldownReduction = (float)_obj_BaseCooldownReduction;
	// BaseAttackSpeed
	float _obj_BaseAttackSpeed; input >> _obj_BaseAttackSpeed; input.ignore(1000, '\n');
	_obj.BaseAttackSpeed = (float)_obj_BaseAttackSpeed;
	// BaseAbilityPower
	float _obj_BaseAbilityPower; input >> _obj_BaseAbilityPower; input.ignore(1000, '\n');
	_obj.BaseAbilityPower = (float)_obj_BaseAbilityPower;
	// BaseMagicResist
	float _obj_BaseMagicResist; input >> _obj_BaseMagicResist; input.ignore(1000, '\n');
	_obj.BaseMagicResist = (float)_obj_BaseMagicResist;
	// IsRooted
	bool _obj_IsRooted; input >> _obj_IsRooted; input.ignore(1000, '\n');
	_obj.IsRooted = (bool)_obj_IsRooted;
	// IsSilenced
	bool _obj_IsSilenced; input >> _obj_IsSilenced; input.ignore(1000, '\n');
	_obj.IsSilenced = (bool)_obj_IsSilenced;
	// IsStuned
	bool _obj_IsStuned; input >> _obj_IsStuned; input.ignore(1000, '\n');
	_obj.IsStuned = (bool)_obj_IsStuned;
	// IsDamageImmune
	bool _obj_IsDamageImmune; input >> _obj_IsDamageImmune; input.ignore(1000, '\n');
	_obj.IsDamageImmune = (bool)_obj_IsDamageImmune;
	// IsControlImmune
	bool _obj_IsControlImmune; input >> _obj_IsControlImmune; input.ignore(1000, '\n');
	_obj.IsControlImmune = (bool)_obj_IsControlImmune;
	// IsBlind
	bool _obj_IsBlind; input >> _obj_IsBlind; input.ignore(1000, '\n');
	_obj.IsBlind = (bool)_obj_IsBlind;
	// IsStealthed
	bool _obj_IsStealthed; input >> _obj_IsStealthed; input.ignore(1000, '\n');
	_obj.IsStealthed = (bool)_obj_IsStealthed;
	// HasTrueVision
	bool _obj_HasTrueVision; input >> _obj_HasTrueVision; input.ignore(1000, '\n');
	_obj.HasTrueVision = (bool)_obj_HasTrueVision;
	// HasWardVision
	bool _obj_HasWardVision; input >> _obj_HasWardVision; input.ignore(1000, '\n');
	_obj.HasWardVision = (bool)_obj_HasWardVision;
	// VisionRange
	float _obj_VisionRange; input >> _obj_VisionRange; input.ignore(1000, '\n');
	_obj.VisionRange = (float)_obj_VisionRange;
	return _obj;
}


