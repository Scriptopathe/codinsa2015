public enum StateAlterationType
{
	None(0),
	Root(1),
	Silence(2),
	Interruption(4),
	Stun(7),
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
	Heal(65536),
	Stealth(131072),
	Shield(524288),
	Sight(1048576),
	WardSight(1048576),
	TrueSight(2097152);
	int _value;
	StateAlterationType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static StateAlterationType fromValue(int value) { 
		StateAlterationType val = None;
		val._value = value;
		return val;
	}
}
