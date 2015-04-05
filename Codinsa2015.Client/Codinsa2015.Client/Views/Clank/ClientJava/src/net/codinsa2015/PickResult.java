package net.codinsa2015;
public enum PickResult
{
	Success(0),
	NotYourTurn(1),
	SpellNotAvailable(2),
	InvalidOperation(3);
	int _value;
	PickResult(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static PickResult fromValue(int value) { 
		switch(value) { 
			case 0: return Success;
			case 1: return NotYourTurn;
			case 2: return SpellNotAvailable;
			case 3: return InvalidOperation;
		}
		return Success;
	}
}
