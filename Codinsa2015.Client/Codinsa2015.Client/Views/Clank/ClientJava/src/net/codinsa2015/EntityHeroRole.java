package net.codinsa2015;
public enum EntityHeroRole
{
	Fighter(0),
	Mage(1),
	Tank(2);
	int _value;
	EntityHeroRole(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static EntityHeroRole fromValue(int value) { 
		switch(value) { 
			case 0: return Fighter;
			case 1: return Mage;
			case 2: return Tank;
		}
		return Fighter;
	}
}
