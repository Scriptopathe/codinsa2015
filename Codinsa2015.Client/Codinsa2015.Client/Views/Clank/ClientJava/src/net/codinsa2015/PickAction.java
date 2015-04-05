package net.codinsa2015;
public enum PickAction
{
	Wait(0),
	PickActive(1),
	PickPassive(2);
	int _value;
	PickAction(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static PickAction fromValue(int value) { 
		switch(value) { 
			case 0: return Wait;
			case 1: return PickActive;
			case 2: return PickPassive;
		}
		return Wait;
	}
}
