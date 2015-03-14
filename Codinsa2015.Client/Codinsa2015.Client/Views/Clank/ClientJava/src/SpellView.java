public class SpellView
{


	public Double CurrentCooldown;
	public Integer SourceCaster;
	public ArrayList<SpellDescriptionView> Levels;
	public Integer Level;
	public static SpellView deserialize(JSONObject o)
	{
		SpellView obj = new SpellView();
		// CurrentCooldown
		Double CurrentCooldown = o.getDouble("CurrentCooldown");
		obj.CurrentCooldown = CurrentCooldown;
		// SourceCaster
		Integer SourceCaster = o.getInt("SourceCaster");
		obj.SourceCaster = SourceCaster;
		// Levels
		ArrayList<SpellDescriptionView> Levels = new ArrayList<SpellDescriptionView>();
		JSONArray Levels_json = o.getJSONArray("Levels");
		for(int Levels_it = 0;Levels_it < o.length(); Levels_it++)
		{
			SpellDescriptionView Levels_item = SpellDescriptionView.deserialize(Levels_json.getJSONObject("Levels_it"));
			Levels.add(Levels_item);
		}
		obj.Levels = Levels;
		// Level
		Integer Level = o.getInt("Level");
		obj.Level = Level;
		return obj;
	}

}