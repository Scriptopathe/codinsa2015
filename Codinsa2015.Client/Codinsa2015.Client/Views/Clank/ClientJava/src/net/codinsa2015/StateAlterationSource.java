package net.codinsa2015;
public enum StateAlterationSource
{
	Consumable(0),
	Armor(1),
	Weapon(2),
	Amulet(3),
	Boots(4),
	SpellActive(5),
	SpellPassive(6);
	int _value;
	StateAlterationSource(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static StateAlterationSource fromValue(int value) { 
		StateAlterationSource val = Consumable;
		val._value = value;
		return val;
	}
}
