package net.codinsa2015;
public enum SignalType
{
	ComingToPosition(0),
	AttackEntity(1),
	DefendEntity(2);
	int _value;
	SignalType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static SignalType fromValue(int value) { 
		switch(value) { 
			case 0: return ComingToPosition;
			case 1: return AttackEntity;
			case 2: return DefendEntity;
		}
		return ComingToPosition;
	}
}
