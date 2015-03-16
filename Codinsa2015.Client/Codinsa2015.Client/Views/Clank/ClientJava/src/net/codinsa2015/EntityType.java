package net.codinsa2015;
public enum EntityType
{
	Team1(2),
	Team2(4),
	Teams(6),
	Structure(8),
	Tower(24),
	Team1Tower(26),
	Team2Tower(28),
	Inhibitor(40),
	Team1Inhibitor(42),
	Team2Inhibitor(44),
	Spawner(72),
	Team1Spawner(74),
	Team2Spawner(76),
	Idol(136),
	Team1Idol(138),
	Team2Idol(140),
	Monster(256),
	Creep(512),
	Team1Creep(514),
	Team2Creep(516),
	Boss(1280),
	Miniboss(2304),
	AllObjectives(3576),
	AllTargettableNeutral(4088),
	Checkpoint(16384),
	Team1Checkpoint(16386),
	Team2CheckPoint(16388),
	Player(32768),
	Team1Player(32770),
	Team2Player(32772),
	AllTeam1(33530),
	AllTeam2(33532),
	WardPlacement(65536),
	Ward(131072),
	Shop(262144),
	HeroSpawner(524288),
	AllSaved(609784);
	int _value;
	EntityType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static EntityType fromValue(int value) { 
		EntityType val = Team1;
		val._value = value;
		return val;
	}
}
