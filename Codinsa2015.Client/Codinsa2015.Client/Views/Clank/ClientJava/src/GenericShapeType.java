public enum GenericShapeType
{
	Circle(0),
	Rectangle(1);
	int _value;
	GenericShapeType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static GenericShapeType fromValue(int value) { 
		GenericShapeType val = Circle;
		val._value = value;
		return val;
	}
}
