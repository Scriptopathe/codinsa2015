package net.codinsa2015;
public enum ConsummableType
{
	Empty(0),
	Ward(1),
	Unward(2);
	int _value;
	ConsummableType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static ConsummableType fromValue(int value) { 
		switch(value) { 
			case 0: return Empty;
			case 1: return Ward;
			case 2: return Unward;
		}
		return Empty;
	}
}
