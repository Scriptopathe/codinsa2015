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
		ConsummableType val = Empty;
		val._value = value;
		return val;
	}
}
