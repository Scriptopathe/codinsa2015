import java.lang.*;

	public class GenericShapeView
	{

	
		public Vector2 Position;	
		public float Radius;	
		public Vector2 Size;	
		public GenericShapeType ShapeType;	
		public static GenericShapeView Deserialize(BufferedReader input) {
		try {
			GenericShapeView _obj =  new GenericShapeView();
			// Position
			Vector2 _obj_Position = Vector2.deserialize(input);
			_obj.Position = _obj_Position;
			// Radius
			float _obj_Radius = Float.Parse(input.readLine());
			_obj.Radius = _obj_Radius;
			// Size
			Vector2 _obj_Size = Vector2.deserialize(input);
			_obj.Size = _obj_Size;
			// ShapeType
			int _obj_ShapeType = Integer.Parse(input.readLine());
			_obj.ShapeType = GenericShapeType.fromValue(_obj_ShapeType);
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
			// Size
			this.Size.serialize(output);
			// ShapeType
			output.append(((Integer)(this.ShapeType.getValue())).toString() + "\n");
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
