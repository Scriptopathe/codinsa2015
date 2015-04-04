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
	Spawner(72),
	Team1Spawner(74),
	Team2Spawner(76),
	Datacenter(136),
	Team1Datacenter(138),
	Team2Datacenter(140),
	Monster(256),
	Virus(512),
	Team1Virus(514),
	Team2Virus(516),
	MiningFarm(1280),
	Router(2304),
	AllTargettableNeutral(3328),
	AllObjectives(3544),
	Checkpoint(16384),
	Team1Checkpoint(16386),
	Team2Checkpoint(16388),
	Player(32768),
	Team1Player(32770),
	Team2Player(32772),
	AllTeam1(33498),
	AllTeam2(33500),
	WardPlacement(65536),
	Ward(131072),
	Shop(262144),
	Team1Shop(262146),
	Team2Shop(262148),
	HeroSpawner(524288),
	Team1HeroSpawner(524290),
	Team2HeroSpawner(524292),
	AllSaved(871896),
	All(16777215);
	int _value;
	EntityType(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static EntityType fromValue(int value) { 
		EntityType val = Team1;
		val._value = value;
		return val;
	}
}
