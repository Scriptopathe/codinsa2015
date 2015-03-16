public enum ConsummableUseResult
{
	Success(0),
	SuccessAndDestroyed(1),
	Fail(2),
	NotUnits(3);
	int _value;
	ConsummableUseResult(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static ConsummableUseResult fromValue(int value) { 
		ConsummableUseResult val = Success;
		val._value = value;
		return val;
	}
}
