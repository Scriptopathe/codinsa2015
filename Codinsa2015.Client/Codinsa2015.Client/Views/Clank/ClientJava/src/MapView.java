public class MapView
{


	public ArrayList<ArrayList<Boolean>> Passability;
	public static MapView deserialize(JSONObject o)
	{
		MapView obj = new MapView();
		// Passability
		ArrayList<ArrayList<Boolean>> Passability = new ArrayList<ArrayList<Boolean>>();
		JSONArray Passability_json = o.getJSONArray("Passability");
		for(int Passability_it = 0;Passability_it < o.length(); Passability_it++)
		{
			ArrayList<Boolean> Passability_item = new ArrayList<Boolean>();
			JSONArray Passability_item_json = Passability_json.getJSONArray("Passability_it");
			for(int Passability_item_it = 0;Passability_item_it < Passability_json.length(); Passability_item_it++)
			{
				Boolean Passability_item_item = Passability_item_json.getBoolean("Passability_item_it");
				Passability_item.add(Passability_item_item);
			}
			Passability.add(Passability_item);
		}
		obj.Passability = Passability;
		return obj;
	}

}