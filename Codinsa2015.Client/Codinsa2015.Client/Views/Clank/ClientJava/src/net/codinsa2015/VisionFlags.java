package net.codinsa2015;
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
		switch(value) { 
			case 0: return None;
			case 1: return Team1Vision;
			case 2: return Team2Vision;
			case 5: return Team1TrueVision;
			case 10: return Team2TrueVision;
			case 17: return Team1WardSight;
			case 18: return Team2WardSight;
		}
		return None;
	}
}
