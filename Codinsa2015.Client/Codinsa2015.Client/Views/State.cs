using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Codinsa2015.Views
{

	public class State
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
		public EntityBaseView GetHero()
		{
			Console.WriteLine("[GetHero]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)0).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			EntityBaseView returnValue = EntityBaseView.Deserialize(input);
			return (EntityBaseView)returnValue;
		}
	
		public Vector2 GetPosition()
		{
			Console.WriteLine("[GetPosition]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)1).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			Vector2 returnValue = Vector2.Deserialize(input);
			return (Vector2)returnValue;
		}
	
		public MapView GetMapView()
		{
			Console.WriteLine("[GetMapView]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)2).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			MapView returnValue = MapView.Deserialize(input);
			return (MapView)returnValue;
		}
	
		public bool StartMoveTo(Vector2 position)
		{
			Console.WriteLine("[StartMoveTo]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)3).ToString());
			position.Serialize(output);
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			bool returnValue = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		public bool IsAutoMoving()
		{
			Console.WriteLine("[IsAutoMoving]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)4).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			bool returnValue = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		public bool EndMoveTo()
		{
			Console.WriteLine("[EndMoveTo]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)5).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			bool returnValue = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		public List<EntityBaseView> GetEntitiesInSight()
		{
			Console.WriteLine("[GetEntitiesInSight]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)6).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<EntityBaseView> returnValue = new List<EntityBaseView>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				EntityBaseView returnValue_e = EntityBaseView.Deserialize(input);
				returnValue.Add((EntityBaseView)returnValue_e);
			}
			return (List<EntityBaseView>)returnValue;
		}
	
		public EntityBaseView GetEntityById(int entityId)
		{
			Console.WriteLine("[GetEntityById]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)7).ToString());
			output.WriteLine(((int)entityId).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			EntityBaseView returnValue = EntityBaseView.Deserialize(input);
			return (EntityBaseView)returnValue;
		}
	
		public bool UseSpell(int spellId,SpellCastTargetInfoView target)
		{
			Console.WriteLine("[UseSpell]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)8).ToString());
			output.WriteLine(((int)spellId).ToString());
			target.Serialize(output);
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			bool returnValue = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		public SceneMode GetMode()
		{
			Console.WriteLine("[GetMode]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)9).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Int32.Parse(input.ReadLine());
			return (SceneMode)returnValue;
		}
	
		public SpellDescriptionView GetSpellCurrentLevelDescription(int spellId)
		{
			Console.WriteLine("[GetSpellCurrentLevelDescription]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)10).ToString());
			output.WriteLine(((int)spellId).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			SpellDescriptionView returnValue = SpellDescriptionView.Deserialize(input);
			return (SpellDescriptionView)returnValue;
		}
	
		public SpellView GetSpell(int spellId)
		{
			Console.WriteLine("[GetSpell]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)11).ToString());
			output.WriteLine(((int)spellId).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			SpellView returnValue = SpellView.Deserialize(input);
			return (SpellView)returnValue;
		}
	
		public List<SpellView> GetSpells()
		{
			Console.WriteLine("[GetSpells]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)12).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<SpellView> returnValue = new List<SpellView>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				SpellView returnValue_e = SpellView.Deserialize(input);
				returnValue.Add((SpellView)returnValue_e);
			}
			return (List<SpellView>)returnValue;
		}
	
		public List<SpellView> GetHeroSpells(int entityId)
		{
			Console.WriteLine("[GetHeroSpells]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)13).ToString());
			output.WriteLine(((int)entityId).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<SpellView> returnValue = new List<SpellView>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				SpellView returnValue_e = SpellView.Deserialize(input);
				returnValue.Add((SpellView)returnValue_e);
			}
			return (List<SpellView>)returnValue;
		}
	
		public static State Deserialize(System.IO.StreamReader input) {
			State _obj =  new State();
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
		}

	}
}
