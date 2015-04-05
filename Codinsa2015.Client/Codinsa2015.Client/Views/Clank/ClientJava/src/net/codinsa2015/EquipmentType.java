package net.codinsa2015;
public enum EquipmentType
{
	Consummable(0),
	Armor(1),
	Weapon(2),
	WeaponEnchant(3),
	Boots(4);
	int _value;
	EquipmentType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static EquipmentType fromValue(int value) { 
		switch(value) { 
			case 0: return Consummable;
			case 1: return Armor;
			case 2: return Weapon;
			case 3: return WeaponEnchant;
			case 4: return Boots;
		}
		return Consummable;
	}
}
