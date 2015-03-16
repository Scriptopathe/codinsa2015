package net.codinsa2015;
public enum SceneMode
{
	Lobby(0),
	Pick(1),
	Game(2);
	int _value;
	SceneMode(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static SceneMode fromValue(int value) { 
		SceneMode val = Lobby;
		val._value = value;
		return val;
	}
}
