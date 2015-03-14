Value EntityBaseView::serialize()
{
	Value root0(Json::objectValue);
	// GetMagicResist
	Value GetMagicResist_temp = Value(this->GetMagicResist);

	root0["GetMagicResist"] = GetMagicResist_temp;
	// GetAbilityPower
	Value GetAbilityPower_temp = Value(this->GetAbilityPower);

	root0["GetAbilityPower"] = GetAbilityPower_temp;
	// GetCooldownReduction
	Value GetCooldownReduction_temp = Value(this->GetCooldownReduction);

	root0["GetCooldownReduction"] = GetCooldownReduction_temp;
	// GetMoveSpeed
	Value GetMoveSpeed_temp = Value(this->GetMoveSpeed);

	root0["GetMoveSpeed"] = GetMoveSpeed_temp;
	// GetAttackSpeed
	Value GetAttackSpeed_temp = Value(this->GetAttackSpeed);

	root0["GetAttackSpeed"] = GetAttackSpeed_temp;
	// GetAttackDamage
	Value GetAttackDamage_temp = Value(this->GetAttackDamage);

	root0["GetAttackDamage"] = GetAttackDamage_temp;
	// GetArmor
	Value GetArmor_temp = Value(this->GetArmor);

	root0["GetArmor"] = GetArmor_temp;
	// GetHP
	Value GetHP_temp = Value(this->GetHP);

	root0["GetHP"] = GetHP_temp;
	// GetMaxHP
	Value GetMaxHP_temp = Value(this->GetMaxHP);

	root0["GetMaxHP"] = GetMaxHP_temp;
	// StateAlterations
	Value StateAlterations_temp = Value(arrayValue);
	auto StateAlterations_temp_iterator = this->StateAlterations;
	for(auto StateAlterations_temp_it = StateAlterations_temp_iterator.begin();StateAlterations_temp_it != StateAlterations_temp_iterator.end();StateAlterations_temp_it++)
	{
		Value StateAlterations_temp_item = (*StateAlterations_temp_it).serialize();
		StateAlterations_temp.append(StateAlterations_temp_item);
	}

	root0["StateAlterations"] = StateAlterations_temp;
	// BaseArmor
	Value BaseArmor_temp = Value(this->BaseArmor);

	root0["BaseArmor"] = BaseArmor_temp;
	// Direction
	Value Direction_temp = this->Direction.serialize();

	root0["Direction"] = Direction_temp;
	// Position
	Value Position_temp = this->Position.serialize();

	root0["Position"] = Position_temp;
	// ShieldPoints
	Value ShieldPoints_temp = Value(this->ShieldPoints);

	root0["ShieldPoints"] = ShieldPoints_temp;
	// HP
	Value HP_temp = Value(this->HP);

	root0["HP"] = HP_temp;
	// BaseMaxHP
	Value BaseMaxHP_temp = Value(this->BaseMaxHP);

	root0["BaseMaxHP"] = BaseMaxHP_temp;
	// BaseMoveSpeed
	Value BaseMoveSpeed_temp = Value(this->BaseMoveSpeed);

	root0["BaseMoveSpeed"] = BaseMoveSpeed_temp;
	// IsDead
	Value IsDead_temp = Value(this->IsDead);

	root0["IsDead"] = IsDead_temp;
	// Type
	Value Type_temp = Value(this->Type);

	root0["Type"] = Type_temp;
	// ID
	Value ID_temp = Value(this->ID);

	root0["ID"] = ID_temp;
	// BaseAttackDamage
	Value BaseAttackDamage_temp = Value(this->BaseAttackDamage);

	root0["BaseAttackDamage"] = BaseAttackDamage_temp;
	// BaseCooldownReduction
	Value BaseCooldownReduction_temp = Value(this->BaseCooldownReduction);

	root0["BaseCooldownReduction"] = BaseCooldownReduction_temp;
	// BaseAttackSpeed
	Value BaseAttackSpeed_temp = Value(this->BaseAttackSpeed);

	root0["BaseAttackSpeed"] = BaseAttackSpeed_temp;
	// BaseAbilityPower
	Value BaseAbilityPower_temp = Value(this->BaseAbilityPower);

	root0["BaseAbilityPower"] = BaseAbilityPower_temp;
	// BaseMagicResist
	Value BaseMagicResist_temp = Value(this->BaseMagicResist);

	root0["BaseMagicResist"] = BaseMagicResist_temp;
	// IsRooted
	Value IsRooted_temp = Value(this->IsRooted);

	root0["IsRooted"] = IsRooted_temp;
	// IsSilenced
	Value IsSilenced_temp = Value(this->IsSilenced);

	root0["IsSilenced"] = IsSilenced_temp;
	// IsStuned
	Value IsStuned_temp = Value(this->IsStuned);

	root0["IsStuned"] = IsStuned_temp;
	// IsStealthed
	Value IsStealthed_temp = Value(this->IsStealthed);

	root0["IsStealthed"] = IsStealthed_temp;
	// HasTrueVision
	Value HasTrueVision_temp = Value(this->HasTrueVision);

	root0["HasTrueVision"] = HasTrueVision_temp;
	// HasWardVision
	Value HasWardVision_temp = Value(this->HasWardVision);

	root0["HasWardVision"] = HasWardVision_temp;
	// VisionRange
	Value VisionRange_temp = Value(this->VisionRange);

	root0["VisionRange"] = VisionRange_temp;
	return root0;

}

static EntityBaseView EntityBaseView::deserialize(Value& val)
{
	EntityBaseView obj0 = EntityBaseView();
	// GetMagicResist
	float GetMagicResist = val["GetMagicResist"].asDouble();

	obj0.GetMagicResist = GetMagicResist;

	// GetAbilityPower
	float GetAbilityPower = val["GetAbilityPower"].asDouble();

	obj0.GetAbilityPower = GetAbilityPower;

	// GetCooldownReduction
	float GetCooldownReduction = val["GetCooldownReduction"].asDouble();

	obj0.GetCooldownReduction = GetCooldownReduction;

	// GetMoveSpeed
	float GetMoveSpeed = val["GetMoveSpeed"].asDouble();

	obj0.GetMoveSpeed = GetMoveSpeed;

	// GetAttackSpeed
	float GetAttackSpeed = val["GetAttackSpeed"].asDouble();

	obj0.GetAttackSpeed = GetAttackSpeed;

	// GetAttackDamage
	float GetAttackDamage = val["GetAttackDamage"].asDouble();

	obj0.GetAttackDamage = GetAttackDamage;

	// GetArmor
	float GetArmor = val["GetArmor"].asDouble();

	obj0.GetArmor = GetArmor;

	// GetHP
	float GetHP = val["GetHP"].asDouble();

	obj0.GetHP = GetHP;

	// GetMaxHP
	float GetMaxHP = val["GetMaxHP"].asDouble();

	obj0.GetMaxHP = GetMaxHP;

	// StateAlterations
	vector<StateAlterationView> StateAlterations = vector<StateAlterationView>();
	auto StateAlterations_iterator = val["StateAlterations"];
	for(auto StateAlterations_it = StateAlterations_iterator.begin();StateAlterations_it != StateAlterations_iterator.end(); StateAlterations_it++)
	{
		StateAlterationView StateAlterations_item = StateAlterationView::deserialize((*StateAlterations_it));
		StateAlterations.push_back(StateAlterations_item);
	}

	obj0.StateAlterations = StateAlterations;

	// BaseArmor
	float BaseArmor = val["BaseArmor"].asDouble();

	obj0.BaseArmor = BaseArmor;

	// Direction
	Vector2 Direction = Vector2::deserialize(val["Direction"]);

	obj0.Direction = Direction;

	// Position
	Vector2 Position = Vector2::deserialize(val["Position"]);

	obj0.Position = Position;

	// ShieldPoints
	float ShieldPoints = val["ShieldPoints"].asDouble();

	obj0.ShieldPoints = ShieldPoints;

	// HP
	float HP = val["HP"].asDouble();

	obj0.HP = HP;

	// BaseMaxHP
	float BaseMaxHP = val["BaseMaxHP"].asDouble();

	obj0.BaseMaxHP = BaseMaxHP;

	// BaseMoveSpeed
	float BaseMoveSpeed = val["BaseMoveSpeed"].asDouble();

	obj0.BaseMoveSpeed = BaseMoveSpeed;

	// IsDead
	bool IsDead = val["IsDead"].asBool();

	obj0.IsDead = IsDead;

	// Type
	EntityType Type = val["Type"].asInt();

	obj0.Type = Type;

	// ID
	int ID = val["ID"].asInt();

	obj0.ID = ID;

	// BaseAttackDamage
	float BaseAttackDamage = val["BaseAttackDamage"].asDouble();

	obj0.BaseAttackDamage = BaseAttackDamage;

	// BaseCooldownReduction
	float BaseCooldownReduction = val["BaseCooldownReduction"].asDouble();

	obj0.BaseCooldownReduction = BaseCooldownReduction;

	// BaseAttackSpeed
	float BaseAttackSpeed = val["BaseAttackSpeed"].asDouble();

	obj0.BaseAttackSpeed = BaseAttackSpeed;

	// BaseAbilityPower
	float BaseAbilityPower = val["BaseAbilityPower"].asDouble();

	obj0.BaseAbilityPower = BaseAbilityPower;

	// BaseMagicResist
	float BaseMagicResist = val["BaseMagicResist"].asDouble();

	obj0.BaseMagicResist = BaseMagicResist;

	// IsRooted
	bool IsRooted = val["IsRooted"].asBool();

	obj0.IsRooted = IsRooted;

	// IsSilenced
	bool IsSilenced = val["IsSilenced"].asBool();

	obj0.IsSilenced = IsSilenced;

	// IsStuned
	bool IsStuned = val["IsStuned"].asBool();

	obj0.IsStuned = IsStuned;

	// IsStealthed
	bool IsStealthed = val["IsStealthed"].asBool();

	obj0.IsStealthed = IsStealthed;

	// HasTrueVision
	bool HasTrueVision = val["HasTrueVision"].asBool();

	obj0.HasTrueVision = HasTrueVision;

	// HasWardVision
	bool HasWardVision = val["HasWardVision"].asBool();

	obj0.HasWardVision = HasWardVision;

	// VisionRange
	float VisionRange = val["VisionRange"].asDouble();

	obj0.VisionRange = VisionRange;

	return obj0;

}



