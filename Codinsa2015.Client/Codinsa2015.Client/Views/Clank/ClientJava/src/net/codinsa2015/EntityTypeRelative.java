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
	Idol(136),
	AllyIdol(138),
	EnnemyIdol(140),
	Monster(256),
	Creep(512),
	AllyCreep(514),
	EnnemyCreep(516),
	Boss(1280),
	Miniboss(2304),
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
		EntityTypeRelative val = Me;
		val._value = value;
		return val;
	}
}
