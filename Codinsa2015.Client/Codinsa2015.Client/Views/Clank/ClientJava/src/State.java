import java.lang.*;

	public class State
	{

		public EntityBaseView GetHero()
		{
			Console.WriteLine("[GetHero]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)0).toString() + "\n");
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			EntityBaseView returnValue = EntityBaseView.deserialize(input);
			return (EntityBaseView)returnValue;
		}
	
		public Vector2 GetPosition()
		{
			Console.WriteLine("[GetPosition]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)1).toString() + "\n");
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			Vector2 returnValue = Vector2.deserialize(input);
			return (Vector2)returnValue;
		}
	
		public MapView GetMapView()
		{
			Console.WriteLine("[GetMapView]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)2).toString() + "\n");
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			MapView returnValue = MapView.deserialize(input);
			return (MapView)returnValue;
		}
	
		public bool StartMoveTo(Vector2 position)
		{
			Console.WriteLine("[StartMoveTo]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)3).toString() + "\n");
			position.serialize(output);
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			boolean returnValue = Integer.valueof(input.readLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		public bool IsAutoMoving()
		{
			Console.WriteLine("[IsAutoMoving]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)4).toString() + "\n");
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			boolean returnValue = Integer.valueof(input.readLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		public bool EndMoveTo()
		{
			Console.WriteLine("[EndMoveTo]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)5).toString() + "\n");
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			boolean returnValue = Integer.valueof(input.readLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		public ArrayList<EntityBaseView> GetEntitiesInSight()
		{
			Console.WriteLine("[GetEntitiesInSight]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)6).toString() + "\n");
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			ArrayList<EntityBaseView> returnValue = new ArrayList<EntityBaseView>();
			int returnValue_count = Integer.Parse(input.readLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				EntityBaseView returnValue_e = EntityBaseView.deserialize(input);
				returnValue.add((EntityBaseView)returnValue_e);
			}
			return (ArrayList<EntityBaseView>)returnValue;
		}
	
		public EntityBaseView GetEntityById(int entityId)
		{
			Console.WriteLine("[GetEntityById]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)7).toString() + "\n");
			output.append(((Integer)entityId).toString() + "\n");
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			EntityBaseView returnValue = EntityBaseView.deserialize(input);
			return (EntityBaseView)returnValue;
		}
	
		public bool UseSpell(int spellId,SpellCastTargetInfoView target)
		{
			Console.WriteLine("[UseSpell]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)8).toString() + "\n");
			output.append(((Integer)spellId).toString() + "\n");
			target.serialize(output);
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			boolean returnValue = Integer.valueof(input.readLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		public SceneMode GetMode()
		{
			Console.WriteLine("[GetMode]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)9).toString() + "\n");
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Integer.Parse(input.readLine());
			return (SceneMode)returnValue;
		}
	
		public SpellDescriptionView GetSpellCurrentLevelDescription(int spellId)
		{
			Console.WriteLine("[GetSpellCurrentLevelDescription]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)10).toString() + "\n");
			output.append(((Integer)spellId).toString() + "\n");
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			SpellDescriptionView returnValue = SpellDescriptionView.deserialize(input);
			return (SpellDescriptionView)returnValue;
		}
	
		public SpellView GetSpell(int spellId)
		{
			Console.WriteLine("[GetSpell]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)11).toString() + "\n");
			output.append(((Integer)spellId).toString() + "\n");
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			SpellView returnValue = SpellView.deserialize(input);
			return (SpellView)returnValue;
		}
	
		public ArrayList<SpellView> GetSpells()
		{
			Console.WriteLine("[GetSpells]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)12).toString() + "\n");
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			ArrayList<SpellView> returnValue = new ArrayList<SpellView>();
			int returnValue_count = Integer.Parse(input.readLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				SpellView returnValue_e = SpellView.deserialize(input);
				returnValue.add((SpellView)returnValue_e);
			}
			return (ArrayList<SpellView>)returnValue;
		}
	
		public ArrayList<SpellView> GetHeroSpells(int entityId)
		{
			Console.WriteLine("[GetHeroSpells]");
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.append(((Integer)13).toString() + "\n");
			output.append(((Integer)entityId).toString() + "\n");
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			ArrayList<SpellView> returnValue = new ArrayList<SpellView>();
			int returnValue_count = Integer.Parse(input.readLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				SpellView returnValue_e = SpellView.deserialize(input);
				returnValue.add((SpellView)returnValue_e);
			}
			return (ArrayList<SpellView>)returnValue;
		}
	
		public static State Deserialize(BufferedReader input) {
		try {
			State _obj =  new State();
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
