public class StateAlterationView
{


	public Integer Source;
	public StateAlterationSource SourceType;
	public StateAlterationModelView Model;
	public StateAlterationParametersView Parameters;
	public Double RemainingTime;
	public static StateAlterationView deserialize(JSONObject o)
	{
		StateAlterationView obj = new StateAlterationView();
		// Source
		Integer Source = o.getInt("Source");
		obj.Source = Source;
		// SourceType
		StateAlterationSource SourceType = o.getInt("SourceType");
		obj.SourceType = SourceType;
		// Model
		StateAlterationModelView Model = StateAlterationModelView.deserialize(o.getJSONObject("Model"));
		obj.Model = Model;
		// Parameters
		StateAlterationParametersView Parameters = StateAlterationParametersView.deserialize(o.getJSONObject("Parameters"));
		obj.Parameters = Parameters;
		// RemainingTime
		Double RemainingTime = o.getDouble("RemainingTime");
		obj.RemainingTime = RemainingTime;
		return obj;
	}

}