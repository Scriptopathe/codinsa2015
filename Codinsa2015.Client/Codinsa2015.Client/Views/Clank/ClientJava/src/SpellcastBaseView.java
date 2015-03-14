public class SpellcastBaseView
{


	public GenericShapeView Shape;
	public static SpellcastBaseView deserialize(JSONObject o)
	{
		SpellcastBaseView obj = new SpellcastBaseView();
		// Shape
		GenericShapeView Shape = GenericShapeView.deserialize(o.getJSONObject("Shape"));
		obj.Shape = Shape;
		return obj;
	}

}