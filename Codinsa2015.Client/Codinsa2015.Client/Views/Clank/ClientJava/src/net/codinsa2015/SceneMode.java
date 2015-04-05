package net.codinsa2015;
public enum SceneMode
{
	Lobby(0),
	Pick(1),
	Game(2),
	End(3);
	int _value;
	SceneMode(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static SceneMode fromValue(int value) { 
		switch(value) { 
			case 0: return Lobby;
			case 1: return Pick;
			case 2: return Game;
			case 3: return End;
		}
		return Lobby;
	}
}
