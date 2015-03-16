public enum TargettingType
{
	Targetted(1),
	Position(2),
	Direction(4);
	int _value;
	TargettingType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static TargettingType fromValue(int value) { 
		TargettingType val = Targetted;
		val._value = value;
		return val;
	}
}
