package net.codinsa2015;
public enum EntityTypeRelative
{
	Me(1),
	Ally(2),
	Ennemy(4),
	Structure(8),
	Tower(24),
	AllyTower(26),
	EnnemyTower(28),
	Spawner(72),
	AllySpawner(74),
	EnnemySpawner(76),
	Datacenter(136),
	AllyDatacenter(138),
	EnnemyDatacenter(140),
	Monster(256),
	Virus(512),
	AllyVirus(514),
	EnnemyVirus(516),
	MiningFarm(1280),
	Router(2304),
	AllObjectives(3544),
	AllTargettableNeutral(3840),
	Checkpoint(16384),
	AllyCheckpoint(16386),
	EnnemyCheckpoint(16388),
	Player(32768),
	AllyPlayer(32770),
	EnnemyPlayer(32772),
	AllAlly(33498),
	AllEnnemy(33500),
	WardPlacement(65536),
	Ward(131072),
	Shop(262144),
	HeroSpawner(524288),
	AllyHeroSpawner(524290),
	EnnemyHeroSpawner(524292),
	AllSaved(609752),
	All(16777215);
	int _value;
	EntityTypeRelative(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static EntityTypeRelative fromValue(int value) { 
		switch(value) { 
			case 1: return Me;
			case 2: return Ally;
			case 4: return Ennemy;
			case 8: return Structure;
			case 24: return Tower;
			case 26: return AllyTower;
			case 28: return EnnemyTower;
			case 72: return Spawner;
			case 74: return AllySpawner;
			case 76: return EnnemySpawner;
			case 136: return Datacenter;
			case 138: return AllyDatacenter;
			case 140: return EnnemyDatacenter;
			case 256: return Monster;
			case 512: return Virus;
			case 514: return AllyVirus;
			case 516: return EnnemyVirus;
			case 1280: return MiningFarm;
			case 2304: return Router;
			case 3544: return AllObjectives;
			case 3840: return AllTargettableNeutral;
			case 16384: return Checkpoint;
			case 16386: return AllyCheckpoint;
			case 16388: return EnnemyCheckpoint;
			case 32768: return Player;
			case 32770: return AllyPlayer;
			case 32772: return EnnemyPlayer;
			case 33498: return AllAlly;
			case 33500: return AllEnnemy;
			case 65536: return WardPlacement;
			case 131072: return Ward;
			case 262144: return Shop;
			case 524288: return HeroSpawner;
			case 524290: return AllyHeroSpawner;
			case 524292: return EnnemyHeroSpawner;
			case 609752: return AllSaved;
			case 16777215: return All;
		}
		return Me;
	}
}
