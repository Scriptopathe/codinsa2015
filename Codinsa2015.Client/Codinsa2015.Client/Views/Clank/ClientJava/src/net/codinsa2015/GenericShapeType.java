package net.codinsa2015;
public enum GenericShapeType
{
	Circle(0),
	Rectangle(1);
	int _value;
	GenericShapeType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static GenericShapeType fromValue(int value) { 
		switch(value) { 
			case 0: return Circle;
			case 1: return Rectangle;
		}
		return Circle;
	}
}
