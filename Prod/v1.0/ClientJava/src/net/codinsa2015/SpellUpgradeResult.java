package net.codinsa2015;
public enum SpellUpgradeResult
{
	Success(0),
	AlreadyMaxLevel(1),
	NotEnoughPA(2),
	InvalidOperation(3);
	int _value;
	SpellUpgradeResult(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static SpellUpgradeResult fromValue(int value) { 
		SpellUpgradeResult val = Success;
		val._value = value;
		return val;
	}
}
