public class CircleShapeView
{


	public Vector2 Position;
	public Double Radius;
	public static CircleShapeView deserialize(JSONObject o)
	{
		CircleShapeView obj = new CircleShapeView();
		// Position
		Vector2 Position = Vector2.deserialize(o.getJSONObject("Position"));
		obj.Position = Position;
		// Radius
		Double Radius = o.getDouble("Radius");
		obj.Radius = Radius;
		return obj;
	}

}