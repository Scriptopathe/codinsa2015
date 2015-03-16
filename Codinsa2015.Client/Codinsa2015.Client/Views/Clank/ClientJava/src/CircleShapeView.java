import java.lang.*;

	public class CircleShapeView
	{

	
		public Vector2 Position;	
		public float Radius;	
		public static CircleShapeView Deserialize(BufferedReader input) {
		try {
			CircleShapeView _obj =  new CircleShapeView();
			// Position
			Vector2 _obj_Position = Vector2.deserialize(input);
			_obj.Position = _obj_Position;
			// Radius
			float _obj_Radius = Float.Parse(input.readLine());
			_obj.Radius = _obj_Radius;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// Position
			this.Position.serialize(output);
			// Radius
			output.WriteLine(((Float)this.Radius).toString() + "\n");
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
