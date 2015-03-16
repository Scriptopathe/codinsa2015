public enum VisionFlags
{
	None(0),
	Team1Vision(1),
	Team2Vision(2),
	Team1TrueVision(5),
	Team2TrueVision(10),
	Team1WardSight(17),
	Team2WardSight(18);
	int _value;
	VisionFlags(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static VisionFlags fromValue(int value) { 
		VisionFlags val = None;
		val._value = value;
		return val;
	}
}
