/// <summary>
/// Contient toutes les informations concernant l'état du serveur.
/// </summary>
class State
{

	public Oktamer test()
	{
		// Send
		ArrayList<Object> args = new ArrayList<Object>(); 
		int funcId = 0;
		ArrayList<Object> obj = new ArrayList<Object>();
		obj.add(funcId);
		obj.add(args);
		TCPHelper.send(JsonWriter.objectToJson(obj));
		// Receive
		String str = TCPHelper.receive();
		JSONArray arr = (JSONArray)JSONReader.jsonToJava(str);
		Oktamer _ret = arr.getInt(0);
		return _ret;
	}

	public Oktamer test3(Oktamer t)
	{
		// Send
		ArrayList<Object> args = new ArrayList<Object>(); 
		args.add(t);
		int funcId = 1;
		ArrayList<Object> obj = new ArrayList<Object>();
		obj.add(funcId);
		obj.add(args);
		TCPHelper.send(JsonWriter.objectToJson(obj));
		// Receive
		String str = TCPHelper.receive();
		JSONArray arr = (JSONArray)JSONReader.jsonToJava(str);
		Oktamer _ret = arr.getInt(0);
		return _ret;
	}

	public static State deserialize(JSONObject o)
	{
		State obj = new State();
		return obj;
	}

}