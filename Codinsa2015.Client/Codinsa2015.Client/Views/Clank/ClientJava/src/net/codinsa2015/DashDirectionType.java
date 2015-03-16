package net.codinsa2015;
public enum DashDirectionType
{
	TowardsEntity(0),
	Direction(1),
	BackwardsCaster(2);
	int _value;
	DashDirectionType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static DashDirectionType fromValue(int value) { 
		DashDirectionType val = TowardsEntity;
		val._value = value;
		return val;
	}
}
