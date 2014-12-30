using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public enum EntityType
	{
		Team1 = 2,
		Team2 = 4,
		Teams = 6,
		Structure = 8,
		Tower = 24,
		Team1Tower = 26,
		Team2Tower = 28,
		Inhibitor = 40,
		Team1Inhibitor = 42,
		Team2Inhibitor = 44,
		Spawner = 72,
		Team1Spawner = 74,
		Team2Spawner = 76,
		Idol = 136,
		Team1Idol = 138,
		Team2Idol = 140,
		Monster = 256,
		Creep = 512,
		Team1Creep = 514,
		Team2Creep = 516,
		Boss = 1280,
		Miniboss = 2304,
		AllObjectives = 3576,
		Checkpoint = 16384,
		Team1Checkpoint = 16386,
		Team2CheckPoint = 16388,
		Player = 32768,
		Team1Player = 32770,
		Team2Player = 32772,
		AllTeam1 = 33530,
		AllTeam2 = 33532,
		WardPlacement = 65536,
		AllSaved = 85496,
		Ward = 131072,
		Shop = 262144
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
		Inhibitor = 40,
		AllyInhibitor = 42,
		EnnemyInhibitor = 44,
		Spawner = 72,
		AllySpawner = 74,
		EnnemySpawner = 76,
		Idol = 136,
		AllyIdol = 138,
		EnnemyIdol = 140,
		Monster = 256,
		Creep = 512,
		AllyCreep = 514,
		EnnemyCreep = 516,
		Boss = 1280,
		Miniboss = 2304,
		Checkpoint = 16384,
		AllyCheckpoint = 16386,
		EnnemyCheckpoint = 16388,
		Player = 32768,
		AllyPlayer = 32770,
		EnnemyPlayer = 32772,
		AllAlly = 33530,
		AllEnnemy = 33532,
		WardPlacement = 65536,
		Ward = 131072,
		Shop = 262144
	}
	
	public enum StateAlterationSource
	{
		Consumable = 0,
		Armor = 1,
		Weapon = 2,
		Boots = 3,
		Self = 4,
		Spell = 5
	}
	
	public enum StateAlterationType
	{
		None = 0,
		Root = 1,
		Silence = 2,
		Interruption = 4,
		Stun = 7,
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
		Heal = 65536,
		Stealth = 131072,
		Shield = 524288,
		Sight = 1048576,
		WardSight = 1048576,
		TrueSight = 2097152
	}
	
	public enum DashDirectionType
	{
		TowardsEntity = 0,
		Direction = 1,
		BackwardsCaster = 2
	}
	
	public enum TargettingType
	{
		Targetted = 1,
		Position = 2,
		Direction = 4
	}
	
	public enum SceneMode
	{
		Lobby = 0,
		Pick = 1,
		Game = 2
	}
	
	public class State
	{

		public MapView GetMapView()
		{
			// Send
			List<object> args = new List<object>() { };
			int funcId = 0;
			List<object> obj = new List<object>() { funcId, args };
			TCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
			// Receive
			string str = TCPHelper.Receive();
			Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
			return (MapView)o[0].ToObject(typeof(MapView));
		}
	
		public bool StartMoveTo(Vector2 position)
		{
			// Send
			List<object> args = new List<object>() { position};
			int funcId = 1;
			List<object> obj = new List<object>() { funcId, args };
			TCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
			// Receive
			string str = TCPHelper.Receive();
			Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
			return o.Value<bool>(0);
		}
	
		public bool IsAutoMoving()
		{
			// Send
			List<object> args = new List<object>() { };
			int funcId = 2;
			List<object> obj = new List<object>() { funcId, args };
			TCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
			// Receive
			string str = TCPHelper.Receive();
			Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
			return o.Value<bool>(0);
		}
	
		public bool EndMoveTo()
		{
			// Send
			List<object> args = new List<object>() { };
			int funcId = 3;
			List<object> obj = new List<object>() { funcId, args };
			TCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
			// Receive
			string str = TCPHelper.Receive();
			Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
			return o.Value<bool>(0);
		}
	
		public List<EntityBaseView> GetEntitiesInSight()
		{
			// Send
			List<object> args = new List<object>() { };
			int funcId = 4;
			List<object> obj = new List<object>() { funcId, args };
			TCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
			// Receive
			string str = TCPHelper.Receive();
			Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
			return (List<EntityBaseView>)o[0].ToObject(typeof(List<EntityBaseView>));
		}
	
	}
}