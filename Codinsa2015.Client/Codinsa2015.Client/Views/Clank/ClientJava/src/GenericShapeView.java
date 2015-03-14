public class GenericShapeView
{


	public Vector2 Position;
	public Double Radius;
	public Vector2 Size;
	public GenericShapeType ShapeType;
	public static GenericShapeView deserialize(JSONObject o)
	{
		GenericShapeView obj = new GenericShapeView();
		// Position
		Vector2 Position = Vector2.deserialize(o.getJSONObject("Position"));
		obj.Position = Position;
		// Radius
		Double Radius = o.getDouble("Radius");
		obj.Radius = Radius;
		// Size
		Vector2 Size = Vector2.deserialize(o.getJSONObject("Size"));
		obj.Size = Size;
		// ShapeType
		GenericShapeType ShapeType = o.getInt("ShapeType");
		obj.ShapeType = ShapeType;
		return obj;
	}

}