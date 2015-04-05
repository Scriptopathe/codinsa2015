package net.codinsa2015;
public enum EntityHeroRole
{
	Fighter(0),
	Mage(1),
	Tank(2),
	Max(2);
	int _value;
	EntityHeroRole(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static EntityHeroRole fromValue(int value) { 
		EntityHeroRole val = Fighter;
		val._value = value;
		return val;
	}
}
