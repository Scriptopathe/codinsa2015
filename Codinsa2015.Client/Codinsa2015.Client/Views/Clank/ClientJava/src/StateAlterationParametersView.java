public class StateAlterationParametersView
{


	public Vector2 DashTargetDirection;
	public Integer DashTargetEntity;
	public static StateAlterationParametersView deserialize(JSONObject o)
	{
		StateAlterationParametersView obj = new StateAlterationParametersView();
		// DashTargetDirection
		Vector2 DashTargetDirection = Vector2.deserialize(o.getJSONObject("DashTargetDirection"));
		obj.DashTargetDirection = DashTargetDirection;
		// DashTargetEntity
		Integer DashTargetEntity = o.getInt("DashTargetEntity");
		obj.DashTargetEntity = DashTargetEntity;
		return obj;
	}

}