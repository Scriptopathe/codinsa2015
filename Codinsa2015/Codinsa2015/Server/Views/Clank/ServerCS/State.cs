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
		AllTargettableNeutral = 4088,
		Checkpoint = 16384,
		Team1Checkpoint = 16386,
		Team2CheckPoint = 16388,
		Player = 32768,
		Team1Player = 32770,
		Team2Player = 32772,
		AllTeam1 = 33530,
		AllTeam2 = 33532,
		WardPlacement = 65536,
		Ward = 131072,
		Shop = 262144,
		HeroSpawner = 524288,
		AllSaved = 609784
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
		Shop = 262144,
		HeroSpawner = 524288
	}
	
	public enum StateAlterationSource
	{
		Consumable = 0,
		Armor = 1,
		Weapon = 2,
		Amulet = 3,
		Boots = 4,
		Self = 5,
		SpellActive = 6,
		SpellPassive = 7
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
	
	public enum SpellUseResult
	{
		Success = 0,
		InvalidTarget = 1,
		InvalidTargettingType = 2,
		OnCooldown = 3,
		Silenced = 4,
		OutOfRange = 5
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

		public EntityBaseView GetHero(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetHero();
		}	
		public Vector2 GetPosition(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetPosition();
		}	
		public MapView GetMapView(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMapView();
		}	
		public bool StartMoveTo(Vector2 position, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).StartMoveTo(position);
		}	
		public bool IsAutoMoving(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).IsAutoMoving();
		}	
		public bool EndMoveTo(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).EndMoveTo();
		}	
		public List<EntityBaseView> GetEntitiesInSight(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetEntitiesInSight();
		}	
		public EntityBaseView GetEntityById(int entityId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetEntityById(entityId);
		}	
		public bool UseSpell(int spellId, SpellCastTargetInfoView target, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).UseSpell(spellId,target);
		}	
		public SceneMode GetMode(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetMode();
		}	
		public SpellDescriptionView GetSpellCurrentLevelDescription(int spellId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetSpellCurrentLevelDescription(spellId);
		}	
		public SpellView GetSpell(int spellId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetSpell(spellId);
		}	
		public List<SpellView> GetSpells(int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetSpells();
		}	
		public List<SpellView> GetHeroSpells(int entityId, int clientId)
		{
			return Codinsa2015.Server.GameServer.GetScene().GetControler(clientId).GetHeroSpells(entityId);
		}	
		/// <summary>
		/// Génère le code pour la fonction de traitement des messages.
		/// </summary>
		public string ProcessRequest(string request, int clientId)
		{
			Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(request);
			int functionId = o.Value<int>(0);
			switch(functionId)
			{
				case 0:
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { GetHero(clientId) });
				case 1:
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { GetPosition(clientId) });
				case 2:
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { GetMapView(clientId) });
				case 3:
					Vector2 arg3_0 = (Vector2)o[1][0].ToObject(typeof(Vector2));
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { StartMoveTo(arg3_0, clientId) });
				case 4:
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { IsAutoMoving(clientId) });
				case 5:
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { EndMoveTo(clientId) });
				case 6:
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { GetEntitiesInSight(clientId) });
				case 7:
					int arg7_0 = o[1].Value<int>(0);
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { GetEntityById(arg7_0, clientId) });
				case 8:
					int arg8_0 = o[1].Value<int>(0);
					SpellCastTargetInfoView arg8_1 = (SpellCastTargetInfoView)o[1][1].ToObject(typeof(SpellCastTargetInfoView));
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { UseSpell(arg8_0, arg8_1, clientId) });
				case 9:
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { GetMode(clientId) });
				case 10:
					int arg10_0 = o[1].Value<int>(0);
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { GetSpellCurrentLevelDescription(arg10_0, clientId) });
				case 11:
					int arg11_0 = o[1].Value<int>(0);
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { GetSpell(arg11_0, clientId) });
				case 12:
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { GetSpells(clientId) });
				case 13:
					int arg13_0 = o[1].Value<int>(0);
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { GetHeroSpells(arg13_0, clientId) });
			}
			return "";
		}
	
	}
}
