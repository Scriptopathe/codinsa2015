package net.codinsa2015;
public enum DashDirectionType
{
	TowardsEntity(0),
	Direction(1),
	TowardsSpellPosition(2),
	BackwardsCaster(3);
	int _value;
	DashDirectionType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static DashDirectionType fromValue(int value) { 
		switch(value) { 
			case 0: return TowardsEntity;
			case 1: return Direction;
			case 2: return TowardsSpellPosition;
			case 3: return BackwardsCaster;
		}
		return TowardsEntity;
	}
}
