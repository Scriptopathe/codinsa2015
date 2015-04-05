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
		switch(value) { 
			case 2: return Team1;
			case 4: return Team2;
			case 6: return Teams;
			case 8: return Structure;
			case 24: return Tower;
			case 26: return Team1Tower;
			case 28: return Team2Tower;
			case 72: return Spawner;
			case 74: return Team1Spawner;
			case 76: return Team2Spawner;
			case 136: return Datacenter;
			case 138: return Team1Datacenter;
			case 140: return Team2Datacenter;
			case 256: return Monster;
			case 512: return Virus;
			case 514: return Team1Virus;
			case 516: return Team2Virus;
			case 1280: return MiningFarm;
			case 2304: return Router;
			case 3328: return AllTargettableNeutral;
			case 3544: return AllObjectives;
			case 16384: return Checkpoint;
			case 16386: return Team1Checkpoint;
			case 16388: return Team2Checkpoint;
			case 32768: return Player;
			case 32770: return Team1Player;
			case 32772: return Team2Player;
			case 33498: return AllTeam1;
			case 33500: return AllTeam2;
			case 65536: return WardPlacement;
			case 131072: return Ward;
			case 262144: return Shop;
			case 262146: return Team1Shop;
			case 262148: return Team2Shop;
			case 524288: return HeroSpawner;
			case 524290: return Team1HeroSpawner;
			case 524292: return Team2HeroSpawner;
			case 871896: return AllSaved;
			case 16777215: return All;
		}
		return Team1;
	}
}
