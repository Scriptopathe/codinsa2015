package net.codinsa2015;
public enum StateAlterationSource
{
	Consumable(0),
	Armor(1),
	Weapon(2),
	Amulet(3),
	Boots(4),
	SpellActive(5),
	UniquePassive(6);
	int _value;
	StateAlterationSource(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static StateAlterationSource fromValue(int value) { 
		switch(value) { 
			case 0: return Consumable;
			case 1: return Armor;
			case 2: return Weapon;
			case 3: return Amulet;
			case 4: return Boots;
			case 5: return SpellActive;
			case 6: return UniquePassive;
		}
		return Consumable;
	}
}
