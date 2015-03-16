public enum SpellUseResult
{
	Success(0),
	InvalidTarget(1),
	InvalidTargettingType(2),
	OnCooldown(3),
	Silenced(4),
	OutOfRange(5);
	int _value;
	SpellUseResult(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static SpellUseResult fromValue(int value) { 
		SpellUseResult val = Success;
		val._value = value;
		return val;
	}
}
