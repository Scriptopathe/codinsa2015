using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Codinsa2015.Views.Client
{

	public enum PickAction
	{
		Wait = 0,
		PickActive = 1,
		PickPassive = 2
	}
	
	public enum PickResult
	{
		Success = 0,
		NotYourTurn = 1,
		SpellNotAvailable = 2,
		InvalidOperation = 3
	}
	
	public enum EntityUniquePassives
	{
		None = 0,
		Hunter = 1,
		Rugged = 2,
		Unshakable = 4,
		Strategist = 16,
		Soldier = 32,
		Altruistic = 64,
		All = 65535
	}
	
	public enum EntityHeroRole
	{
		Fighter = 0,
		Mage = 1,
		Tank = 2,
		Max = 2
	}
	
	public enum EntityType
	{
		Team1 = 2,
		Team2 = 4,
		Teams = 6,
		Structure = 8,
		Tower = 24,
		Team1Tower = 26,
		Team2Tower = 28,
		Spawner = 72,
		Team1Spawner = 74,
		Team2Spawner = 76,
		Datacenter = 136,
		Team1Datacenter = 138,
		Team2Datacenter = 140,
		Monster = 256,
		Virus = 512,
		Team1Virus = 514,
		Team2Virus = 516,
		MiningFarm = 1280,
		Router = 2304,
		AllTargettableNeutral = 3328,
		AllObjectives = 3544,
		Checkpoint = 16384,
		Team1Checkpoint = 16386,
		Team2Checkpoint = 16388,
		Player = 32768,
		Team1Player = 32770,
		Team2Player = 32772,
		AllTeam1 = 33498,
		AllTeam2 = 33500,
		WardPlacement = 65536,
		Ward = 131072,
		Shop = 262144,
		Team1Shop = 262146,
		Team2Shop = 262148,
		HeroSpawner = 524288,
		Team1HeroSpawner = 524290,
		Team2HeroSpawner = 524292,
		AllSaved = 871896,
		All = 16777215
	}
	
	public enum EntityTypeRelative
	{
		Me = 1,
		Ally = 2,
		Ennemy = 4,
		Structure = 8,
		Tower = 24,
		AllyTower = 26,
		EnnemyTower = 28,
		Spawner = 72,
		AllySpawner = 74,
		EnnemySpawner = 76,
		Datacenter = 136,
		AllyDatacenter = 138,
		EnnemyDatacenter = 140,
		Monster = 256,
		Virus = 512,
		AllyVirus = 514,
		EnnemyVirus = 516,
		MiningFarm = 1280,
		Router = 2304,
		AllObjectives = 3544,
		AllTargettableNeutral = 3840,
		Checkpoint = 16384,
		AllyCheckpoint = 16386,
		EnnemyCheckpoint = 16388,
		Player = 32768,
		AllyPlayer = 32770,
		EnnemyPlayer = 32772,
		AllAlly = 33498,
		AllEnnemy = 33500,
		WardPlacement = 65536,
		Ward = 131072,
		Shop = 262144,
		HeroSpawner = 524288,
		AllyHeroSpawner = 524290,
		EnnemyHeroSpawner = 524292,
		AllSaved = 609752,
		All = 16777215
	}
	
	public enum StateAlterationSource
	{
		Consumable = 0,
		Armor = 1,
		Weapon = 2,
		Amulet = 3,
		Boots = 4,
		SpellActive = 5,
		UniquePassive = 6
	}
	
	public enum StateAlterationType
	{
		None = 0,
		Root = 1,
		Silence = 2,
		Interruption = 4,
		CDR = 8,
		MoveSpeed = 16,
		ArmorBuff = 32,
		Regen = 64,
		AttackDamageBuff = 128,
		MaxHP = 256,
		MagicDamageBuff = 512,
		MagicResistBuff = 1024,
		AttackSpeed = 2048,
		Dash = 4096,
		AttackDamage = 8192,
		MagicDamage = 16384,
		TrueDamage = 32768,
		AllDamage = 57344,
		Heal = 65536,
		Stealth = 131072,
		Shield = 524288,
		Sight = 1048576,
		WardSight = 1048576,
		TrueSight = 2097152,
		Blind = 4194304,
		Stun = 4194311,
		AllCC = 4194311,
		DamageImmune = 16777216,
		ControlImmune = 33554432,
		Cleanse = 67108864,
		All = 268435455
	}
	
	public enum DashDirectionType
	{
		TowardsEntity = 0,
		Direction = 1,
		TowardsSpellPosition = 2,
		BackwardsCaster = 3
	}
	
	public enum ConsummableType
	{
		Empty = 0,
		Ward = 1,
		Unward = 2
	}
	
	public enum ConsummableUseResult
	{
		Success = 0,
		SuccessAndDestroyed = 1,
		Fail = 2,
		NotUnits = 3
	}
	
	public enum EquipmentType
	{
		Consummable = 0,
		Armor = 1,
		Weapon = 2,
		WeaponEnchant = 3,
		Boots = 4
	}
	
	public enum ShopTransactionResult
	{
		ItemDoesNotExist = 0,
		ItemIsNotAConsummable = 1,
		NoItemToSell = 2,
		NotEnoughMoney = 3,
		NotInShopRange = 4,
		UnavailableItem = 5,
		ProvidedSlotDoesNotExist = 6,
		NoSlotAvailableOnHero = 7,
		EnchantForNoWeapon = 8,
		StackOverflow = 9,
		Success = 10,
		AlreadyMaxLevel = 11
	}
	
	public enum GenericShapeType
	{
		Circle = 0,
		Rectangle = 1
	}
	
	public enum SignalType
	{
		ComingToPosition = 0,
		AttackEntity = 1,
		DefendEntity = 2
	}
	
	public enum SpellUseResult
	{
		Success = 0,
		InvalidTarget = 1,
		InvalidTargettingType = 2,
		OnCooldown = 3,
		Silenced = 4,
		Blind = 5,
		OutOfRange = 6,
		InvalidOperation = 7
	}
	
	public enum SpellUpgradeResult
	{
		Success = 0,
		AlreadyMaxLevel = 1,
		NotEnoughPA = 2,
		InvalidOperation = 3
	}
	
	public enum TargettingType
	{
		Targetted = 1,
		Position = 2,
		Direction = 4
	}
	
	public enum VisionFlags
	{
		None = 0,
		Team1Vision = 1,
		Team2Vision = 2,
		Team1TrueVision = 5,
		Team2TrueVision = 10,
		Team1WardSight = 17,
		Team2WardSight = 18
	}
	
	public enum SceneMode
	{
		Lobby = 0,
		Pick = 1,
		Game = 2,
		End = 3
	}
	
}
