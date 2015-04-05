package net.codinsa2015;
public enum SpellUseResult
{
	Success(0),
	InvalidTarget(1),
	InvalidTargettingType(2),
	OnCooldown(3),
	Silenced(4),
	Blind(5),
	OutOfRange(6),
	InvalidOperation(7);
	int _value;
	SpellUseResult(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static SpellUseResult fromValue(int value) { 
		SpellUseResult val = Success;
		val._value = value;
		return val;
	}
}
