package net.codinsa2015;
public enum ConsummableUseResult
{
	Success(0),
	SuccessAndDestroyed(1),
	Fail(2),
	NotUnits(3);
	int _value;
	ConsummableUseResult(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static ConsummableUseResult fromValue(int value) { 
		switch(value) { 
			case 0: return Success;
			case 1: return SuccessAndDestroyed;
			case 2: return Fail;
			case 3: return NotUnits;
		}
		return Success;
	}
}
