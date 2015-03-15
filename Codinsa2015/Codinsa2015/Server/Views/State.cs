using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Codinsa2015.Views
{

	public class State
	{

static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
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
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
				System.IO.StreamWriter output;
			int functionId = Int32.Parse(input.ReadLine());
			switch(functionId)
			{
			case 0:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				EntityBaseView retValue0 = GetHero(clientId);
				retValue0.Serialize(output);
				output.Close();
				return s.ToArray();
			case 1:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				Vector2 retValue1 = GetPosition(clientId);
				retValue1.Serialize(output);
				output.Close();
				return s.ToArray();
			case 2:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				MapView retValue2 = GetMapView(clientId);
				retValue2.Serialize(output);
				output.Close();
				return s.ToArray();
			case 3:
				Vector2 arg3_0 = Vector2.Deserialize(input);
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue3 = StartMoveTo(arg3_0, clientId);
				output.WriteLine(retValue3 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 4:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue4 = IsAutoMoving(clientId);
				output.WriteLine(retValue4 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 5:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue5 = EndMoveTo(clientId);
				output.WriteLine(retValue5 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 6:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
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
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				EntityBaseView retValue7 = GetEntityById(arg7_0, clientId);
				retValue7.Serialize(output);
				output.Close();
				return s.ToArray();
			case 8:
				int arg8_0 = Int32.Parse(input.ReadLine());
				SpellCastTargetInfoView arg8_1 = SpellCastTargetInfoView.Deserialize(input);
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue8 = UseSpell(arg8_0, arg8_1, clientId);
				output.WriteLine(retValue8 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 9:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SceneMode retValue9 = GetMode(clientId);
				output.WriteLine(((int)retValue9).ToString());
				output.Close();
				return s.ToArray();
			case 10:
				int arg10_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellDescriptionView retValue10 = GetSpellCurrentLevelDescription(arg10_0, clientId);
				retValue10.Serialize(output);
				output.Close();
				return s.ToArray();
			case 11:
				int arg11_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				SpellView retValue11 = GetSpell(arg11_0, clientId);
				retValue11.Serialize(output);
				output.Close();
				return s.ToArray();
			case 12:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
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
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
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
