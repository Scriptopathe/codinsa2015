public class SpellCastTargetInfoView
{


	public TargettingType Type;
	public Vector2 TargetPosition;
	public Vector2 TargetDirection;
	public Integer TargetId;
	public static SpellCastTargetInfoView deserialize(JSONObject o)
	{
		SpellCastTargetInfoView obj = new SpellCastTargetInfoView();
		// Type
		TargettingType Type = o.getInt("Type");
		obj.Type = Type;
		// TargetPosition
		Vector2 TargetPosition = Vector2.deserialize(o.getJSONObject("TargetPosition"));
		obj.TargetPosition = TargetPosition;
		// TargetDirection
		Vector2 TargetDirection = Vector2.deserialize(o.getJSONObject("TargetDirection"));
		obj.TargetDirection = TargetDirection;
		// TargetId
		Integer TargetId = o.getInt("TargetId");
		obj.TargetId = TargetId;
		return obj;
	}

}