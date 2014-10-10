// CLIENT FULL COMMANDES

#region Auto-générées
// Client
public class TCP
{
	public static void Send(string request)
	{
		
	}
	
	public static string Receive(string request)
	{
		
	}
	
	public static string Request(string request)
	{
		
	}
}

#endregion

#region Classes contenues dans le block state
public class Carott
{
	public int carott;
}

public class Potato
{
	public int patate {get;set;}
	public int steack {get;set;}
	public List<string> lst { get;set; }
}
#endregion

#region Classes d'échange de messages
public enum _MessageType
{
	MessageType_get_carott,
	MessageType_get_potato_str,
	MessageType_set_carott,
	MessageType_set_potato_str
}
public class _Message
{
	public MessageType Type;
}

public class Access_get_carott_Message : _Message
{
	// Variables
	public int _arg;
	
	// Constructeur
	public Access_get_carott_Message(int arg)
	{
		_arg = arg;
	}
	
	// Sérialisation
	public string Serialize()
	{
	
	}
}
#endregion

public class State
{
	// Variables contenues dans le block state
	Potato potato;
	Carott carott;
	int thing;
	
	// Fonctions du block access
	Carott get_carott(int arg)
	{
		// Original : return state.carot;
		// TODO : sérialiser un objet request.
		return Serializer.Deserialize(TCP.Request(new Access_get_carott_Message(arg).Serialize()));
	}
	
	string get_potato_str(int id)
	{
		// return state.potato.lst.get(id);
		return Serializer.Deserialize(TCP.Request(new Access_get_potato_str_Message(id).Serialize())).lst.get(id);
	}
	
	// Fonctions du block write
	void set_carott(Carott carott)
	{
		// state.carot = carott;
		TCP.Send(new Write_set_carott_Message(carott).Serialize());
		
	}
	
	void set_potato_str(int id, string value)
	{
		TCP.Send(new Write_set_potato_str_Message(id, value).Serialize());
	}
}