public class SpellDescriptionView
{


	public Double BaseCooldown;
	public Double CastingTime;
	public ArrayList<StateAlterationModelView> CastingTimeAlterations;
	public SpellTargetInfoView TargetType;
	public ArrayList<StateAlterationModelView> OnHitEffects;
	public static SpellDescriptionView deserialize(JSONObject o)
	{
		SpellDescriptionView obj = new SpellDescriptionView();
		// BaseCooldown
		Double BaseCooldown = o.getDouble("BaseCooldown");
		obj.BaseCooldown = BaseCooldown;
		// CastingTime
		Double CastingTime = o.getDouble("CastingTime");
		obj.CastingTime = CastingTime;
		// CastingTimeAlterations
		ArrayList<StateAlterationModelView> CastingTimeAlterations = new ArrayList<StateAlterationModelView>();
		JSONArray CastingTimeAlterations_json = o.getJSONArray("CastingTimeAlterations");
		for(int CastingTimeAlterations_it = 0;CastingTimeAlterations_it < o.length(); CastingTimeAlterations_it++)
		{
			StateAlterationModelView CastingTimeAlterations_item = StateAlterationModelView.deserialize(CastingTimeAlterations_json.getJSONObject("CastingTimeAlterations_it"));
			CastingTimeAlterations.add(CastingTimeAlterations_item);
		}
		obj.CastingTimeAlterations = CastingTimeAlterations;
		// TargetType
		SpellTargetInfoView TargetType = SpellTargetInfoView.deserialize(o.getJSONObject("TargetType"));
		obj.TargetType = TargetType;
		// OnHitEffects
		ArrayList<StateAlterationModelView> OnHitEffects = new ArrayList<StateAlterationModelView>();
		JSONArray OnHitEffects_json = o.getJSONArray("OnHitEffects");
		for(int OnHitEffects_it = 0;OnHitEffects_it < o.length(); OnHitEffects_it++)
		{
			StateAlterationModelView OnHitEffects_item = StateAlterationModelView.deserialize(OnHitEffects_json.getJSONObject("OnHitEffects_it"));
			OnHitEffects.add(OnHitEffects_item);
		}
		obj.OnHitEffects = OnHitEffects;
		return obj;
	}

}