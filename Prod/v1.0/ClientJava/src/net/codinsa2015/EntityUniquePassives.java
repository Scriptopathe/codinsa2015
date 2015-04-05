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
		EntityUniquePassives val = None;
		val._value = value;
		return val;
	}
}
