public class Vector2
{


	public Vector2()
	{
	}
	public Vector2(Double x, Double y)
	{
		this.X = x;
		this.Y = y;
	}
	public Double X;
	public Double Y;
	public static Vector2 deserialize(JSONObject o)
	{
		Vector2 obj = new Vector2();
		// X
		Double X = o.getDouble("X");
		obj.X = X;
		// Y
		Double Y = o.getDouble("Y");
		obj.Y = Y;
		return obj;
	}

}