public class WeaponEnchantModelView
{


	public ArrayList<StateAlterationModelView> OnHitEffects;
	public ArrayList<StateAlterationModelView> CastingEffects;
	public ArrayList<StateAlterationModelView> PassiveEffects;
	public static WeaponEnchantModelView deserialize(JSONObject o)
	{
		WeaponEnchantModelView obj = new WeaponEnchantModelView();
		// OnHitEffects
		ArrayList<StateAlterationModelView> OnHitEffects = new ArrayList<StateAlterationModelView>();
		JSONArray OnHitEffects_json = o.getJSONArray("OnHitEffects");
		for(int OnHitEffects_it = 0;OnHitEffects_it < o.length(); OnHitEffects_it++)
		{
			StateAlterationModelView OnHitEffects_item = StateAlterationModelView.deserialize(OnHitEffects_json.getJSONObject("OnHitEffects_it"));
			OnHitEffects.add(OnHitEffects_item);
		}
		obj.OnHitEffects = OnHitEffects;
		// CastingEffects
		ArrayList<StateAlterationModelView> CastingEffects = new ArrayList<StateAlterationModelView>();
		JSONArray CastingEffects_json = o.getJSONArray("CastingEffects");
		for(int CastingEffects_it = 0;CastingEffects_it < o.length(); CastingEffects_it++)
		{
			StateAlterationModelView CastingEffects_item = StateAlterationModelView.deserialize(CastingEffects_json.getJSONObject("CastingEffects_it"));
			CastingEffects.add(CastingEffects_item);
		}
		obj.CastingEffects = CastingEffects;
		// PassiveEffects
		ArrayList<StateAlterationModelView> PassiveEffects = new ArrayList<StateAlterationModelView>();
		JSONArray PassiveEffects_json = o.getJSONArray("PassiveEffects");
		for(int PassiveEffects_it = 0;PassiveEffects_it < o.length(); PassiveEffects_it++)
		{
			StateAlterationModelView PassiveEffects_item = StateAlterationModelView.deserialize(PassiveEffects_json.getJSONObject("PassiveEffects_it"));
			PassiveEffects.add(PassiveEffects_item);
		}
		obj.PassiveEffects = PassiveEffects;
		return obj;
	}

}