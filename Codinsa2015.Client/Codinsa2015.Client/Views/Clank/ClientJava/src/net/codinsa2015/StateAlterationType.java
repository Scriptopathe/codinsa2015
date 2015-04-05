package net.codinsa2015;
public enum StateAlterationType
{
	None(0),
	Root(1),
	Silence(2),
	Interruption(4),
	CDR(8),
	MoveSpeed(16),
	ArmorBuff(32),
	Regen(64),
	AttackDamageBuff(128),
	MaxHP(256),
	MagicDamageBuff(512),
	MagicResistBuff(1024),
	AttackSpeed(2048),
	Dash(4096),
	AttackDamage(8192),
	MagicDamage(16384),
	TrueDamage(32768),
	AllDamage(57344),
	Heal(65536),
	Stealth(131072),
	Shield(524288),
	Sight(1048576),
	TrueSight(2097152),
	Blind(4194304),
	Stun(4194311),
	DamageImmune(16777216),
	ControlImmune(33554432),
	Cleanse(67108864),
	All(268435455);
	int _value;
	StateAlterationType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static StateAlterationType fromValue(int value) { 
		switch(value) { 
			case 0: return None;
			case 1: return Root;
			case 2: return Silence;
			case 4: return Interruption;
			case 8: return CDR;
			case 16: return MoveSpeed;
			case 32: return ArmorBuff;
			case 64: return Regen;
			case 128: return AttackDamageBuff;
			case 256: return MaxHP;
			case 512: return MagicDamageBuff;
			case 1024: return MagicResistBuff;
			case 2048: return AttackSpeed;
			case 4096: return Dash;
			case 8192: return AttackDamage;
			case 16384: return MagicDamage;
			case 32768: return TrueDamage;
			case 57344: return AllDamage;
			case 65536: return Heal;
			case 131072: return Stealth;
			case 524288: return Shield;
			case 1048576: return Sight;
			case 2097152: return TrueSight;
			case 4194304: return Blind;
			case 4194311: return Stun;
			case 16777216: return DamageImmune;
			case 33554432: return ControlImmune;
			case 67108864: return Cleanse;
			case 268435455: return All;
		}
		return None;
	}
}
