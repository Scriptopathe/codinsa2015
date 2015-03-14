using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Codinsa2015.Views
{

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
		AllObjectives = 3576,
		AllTargettableNeutral = 4088,
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
		HeroSpawner = 524288,
		AllSaved = 609784
	}
	
	public enum StateAlterationSource
	{
		Consumable = 0,
		Armor = 1,
		Weapon = 2,
		Amulet = 3,
		Boots = 4,
		SpellActive = 5,
		SpellPassive = 6
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
		Success = 10
	}
	
	public enum GenericShapeType
	{
		Circle = 0,
		Rectangle = 1
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
		public byte[] ProcessRequest(byte[] request, int clientId)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream(request);
			System.IO.StreamReader input = new System.IO.StreamReader(s, Encoding.UTF8);
				System.IO.StreamWriter output;
			int functionId = Int32.Parse(input.ReadLine());
			switch(functionId)
			{
			case 0:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				EntityBaseView retValue0 = GetHero(clientId);
				retValue0.Serialize(output);
				output.Close();
				return s.ToArray();
			case 1:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				Vector2 retValue1 = GetPosition(clientId);
				retValue1.Serialize(output);
				output.Close();
				return s.ToArray();
			case 2:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				MapView retValue2 = GetMapView(clientId);
				retValue2.Serialize(output);
				output.Close();
				return s.ToArray();
			case 3:
				Vector2 arg3_0 = Vector2.Deserialize(input);
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				bool retValue3 = StartMoveTo(arg3_0, clientId);
				output.WriteLine(retValue3 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 4:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				bool retValue4 = IsAutoMoving(clientId);
				output.WriteLine(retValue4 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 5:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				bool retValue5 = EndMoveTo(clientId);
				output.WriteLine(retValue5 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 6:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				List<EntityBaseView> retValue6 = GetEntitiesInSight(clientId);
				output.WriteLine(retValue6.Count.ToString());
				for(int retValue6_it = 0; retValue6_it < retValue6.Count;retValue6_it++) {
					retValue6[retValue6_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 7:
				int arg7_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				EntityBaseView retValue7 = GetEntityById(arg7_0, clientId);
				retValue7.Serialize(output);
				output.Close();
				return s.ToArray();
			case 8:
				int arg8_0 = Int32.Parse(input.ReadLine());
				SpellCastTargetInfoView arg8_1 = SpellCastTargetInfoView.Deserialize(input);
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				bool retValue8 = UseSpell(arg8_0, arg8_1, clientId);
				output.WriteLine(retValue8 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 9:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				SceneMode retValue9 = GetMode(clientId);
				output.WriteLine(((int)retValue9).ToString());
				output.Close();
				return s.ToArray();
			case 10:
				int arg10_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				SpellDescriptionView retValue10 = GetSpellCurrentLevelDescription(arg10_0, clientId);
				retValue10.Serialize(output);
				output.Close();
				return s.ToArray();
			case 11:
				int arg11_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				SpellView retValue11 = GetSpell(arg11_0, clientId);
				retValue11.Serialize(output);
				output.Close();
				return s.ToArray();
			case 12:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				List<SpellView> retValue12 = GetSpells(clientId);
				output.WriteLine(retValue12.Count.ToString());
				for(int retValue12_it = 0; retValue12_it < retValue12.Count;retValue12_it++) {
					retValue12[retValue12_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			case 13:
				int arg13_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, Encoding.UTF8);
				output.NewLine = "\n";
				List<SpellView> retValue13 = GetHeroSpells(arg13_0, clientId);
				output.WriteLine(retValue13.Count.ToString());
				for(int retValue13_it = 0; retValue13_it < retValue13.Count;retValue13_it++) {
					retValue13[retValue13_it].Serialize(output);
				}
				output.Close();
				return s.ToArray();
			}
			return new byte[0];
		}
	
		public static State Deserialize(System.IO.StreamReader input) {
			State _obj =  new State();
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
		}

	}
}
