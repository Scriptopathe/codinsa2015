package net.codinsa2015;
public enum TargettingType
{
	Targetted(1),
	Position(2),
	Direction(4);
	int _value;
	TargettingType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static TargettingType fromValue(int value) { 
		switch(value) { 
			case 1: return Targetted;
			case 2: return Position;
			case 4: return Direction;
		}
		return Targetted;
	}
}
