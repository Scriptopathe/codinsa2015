public class WeaponUpgradeModelView
{


	public SpellDescriptionView Description;
	public ArrayList<StateAlterationModelView> PassiveAlterations;
	public Double Cost;
	public static WeaponUpgradeModelView deserialize(JSONObject o)
	{
		WeaponUpgradeModelView obj = new WeaponUpgradeModelView();
		// Description
		SpellDescriptionView Description = SpellDescriptionView.deserialize(o.getJSONObject("Description"));
		obj.Description = Description;
		// PassiveAlterations
		ArrayList<StateAlterationModelView> PassiveAlterations = new ArrayList<StateAlterationModelView>();
		JSONArray PassiveAlterations_json = o.getJSONArray("PassiveAlterations");
		for(int PassiveAlterations_it = 0;PassiveAlterations_it < o.length(); PassiveAlterations_it++)
		{
			StateAlterationModelView PassiveAlterations_item = StateAlterationModelView.deserialize(PassiveAlterations_json.getJSONObject("PassiveAlterations_it"));
			PassiveAlterations.add(PassiveAlterations_item);
		}
		obj.PassiveAlterations = PassiveAlterations;
		// Cost
		Double Cost = o.getDouble("Cost");
		obj.Cost = Cost;
		return obj;
	}

}