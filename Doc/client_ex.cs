// Commun : client / serveur
public class JSON
{
	
}
// Cette classe est donnée au client quoi qu'il en soit.
public class Carott
{
	public int number;
	public string Serialize()
	{
		return JSON.Serialize(number);
	}
	public Carott Deserialize(string str)
	{
		return JSON.Deserialize(str);
	}
}

// Classe donnée au client que si en mode full_snapshot.
public class Potato
{
	public int patate;
	public int steack;
	public List<string> lst;
	public string Serialize();
	public Carott Deserialize();
}

// Client
public class TCP
{
	public static void Send()
	{
		
	}
	
	public static void Receive()
	{
	
	}
	
	public static void Request()
	{
	
	}
}

// Full snapshot
public class State
{
	Potato potato;
	Carott carott;
	int thing;
	
	public Carott get_carott()
	{
		return this.carrot;
	}
	
	public void set_carott(Carott c)
	{
		this.carott = c;
	}
	
	public string get_potato_str(int id)
	{
		return this.potato.lst[id]
	}
	
	public void set_potato_str(int id, string str)
	{
		this.potato.lst[id] = str;
	}
	public void send_state()
	{
		TCP.Send(JSON.Serialize(this));
	}
	
	public State get_state()
	{
		return JSON.Deserialize(TCP.Receive());
	}
}

// Commands only
public class State
{
	Potato potato;
	Carott carott;
	int thing;
	public Carott get_carott()
	{
		return Carott.Deserialize(TCP.request("get state.carott"));
	}
	
	public string get_potato_str(int id)
	{
		return StringType.Deserialize(TCP.request("get state.potato_str " +
												IntType.Serialize(id)));
	}
	
	public void set_potato_str(int id, string value)
	{
		TCP.request("set state.potato_str " + IntType.Serialize(id) + " " + 
			StringType.Serialize(value));
	}
}


// Server
public class State
{
	Potato potato;
	Carott carott;
	int thing;
	
	public void ReceiveMsg()
	{
		if(msg.Contains("get state.carott"))
			send_get_carott(msg.Remove("get state.carott"));
		else if(msg.Contains("get state.potato_str"))
			send_get_potato_str(msg.Remove("get state.potato_str"));
		else if(msg.Contains("set state.potato_str"))
			set_potato_str(msg.Remove("set state.potato_str"));
	}
	
	public void send_get_carott(string msg)
	{
		TCP.send(carott.Serialize());
	}
	
	public void send_get_state_potato_str(string msg)
	{
		string[] args = msg.Split(ARG_SEPARATOR);
		int arg1 = JSON.ParseInt(args[0]);
		TCP.send(StringType.Serialize(potato.lst[arg1]));
	}
	
	public void set_potato_str(string msg)
	{
		string[] args = msg.Split(ARG_SEPARATOR);
		int arg1 = JSON.ParseInt(args[0]);
		string arg2 = JSON.ParseString(args[1]);
		this.potato[arg1] = arg2;
		TCP.send("OK"); // TCP.Send("FAIL");
	}
}