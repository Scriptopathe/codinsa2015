package net.codinsa2015;
public enum EntityUniquePassives
{
	None(0),
	Hunter(1),
	Rugged(2),
	Unshakable(4),
	Strategist(16),
	Soldier(32),
	Altruistic(64),
	All(65535);
	int _value;
	EntityUniquePassives(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static EntityUniquePassives fromValue(int value) { 
		switch(value) { 
			case 0: return None;
			case 1: return Hunter;
			case 2: return Rugged;
			case 4: return Unshakable;
			case 16: return Strategist;
			case 32: return Soldier;
			case 64: return Altruistic;
			case 65535: return All;
		}
		return None;
	}
}
