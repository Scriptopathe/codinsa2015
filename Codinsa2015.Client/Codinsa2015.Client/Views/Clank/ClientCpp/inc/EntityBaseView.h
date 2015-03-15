#pragma once
#include "Common.h"
#include "StateAlterationView.h"
#include "Vector2.h"


class EntityBaseView
{

public: 
	float GetMagicResist;
	float GetAbilityPower;
	float GetCooldownReduction;
	float GetMoveSpeed;
	float GetAttackSpeed;
	float GetAttackDamage;
	float GetArmor;
	float GetHP;
	float GetMaxHP;
	std::vector<StateAlterationView> StateAlterations;
	float BaseArmor;
	Vector2 Direction;
	Vector2 Position;
	float ShieldPoints;
	float HP;
	float BaseMaxHP;
	float BaseMoveSpeed;
	bool IsDead;
	EntityType Type;
	int ID;
	float BaseAttackDamage;
	float BaseCooldownReduction;
	float BaseAttackSpeed;
	float BaseAbilityPower;
	float BaseMagicResist;
	bool IsRooted;
	bool IsSilenced;
	bool IsStuned;
	bool IsStealthed;
	bool HasTrueVision;
	bool HasWardVision;
	float VisionRange;
	void serialize(std::ostream& output);

	static EntityBaseView deserialize(std::istream& input);
private: 

};