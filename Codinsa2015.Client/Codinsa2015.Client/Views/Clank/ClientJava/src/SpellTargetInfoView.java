public class SpellTargetInfoView
{


	public TargettingType Type;
	public Double Range;
	public Double Duration;
	public Double AoeRadius;
	public Boolean DieOnCollision;
	public EntityTypeRelative AllowedTargetTypes;
	public static SpellTargetInfoView deserialize(JSONObject o)
	{
		SpellTargetInfoView obj = new SpellTargetInfoView();
		// Type
		TargettingType Type = o.getInt("Type");
		obj.Type = Type;
		// Range
		Double Range = o.getDouble("Range");
		obj.Range = Range;
		// Duration
		Double Duration = o.getDouble("Duration");
		obj.Duration = Duration;
		// AoeRadius
		Double AoeRadius = o.getDouble("AoeRadius");
		obj.AoeRadius = AoeRadius;
		// DieOnCollision
		Boolean DieOnCollision = o.getBoolean("DieOnCollision");
		obj.DieOnCollision = DieOnCollision;
		// AllowedTargetTypes
		EntityTypeRelative AllowedTargetTypes = o.getInt("AllowedTargetTypes");
		obj.AllowedTargetTypes = AllowedTargetTypes;
		return obj;
	}

}