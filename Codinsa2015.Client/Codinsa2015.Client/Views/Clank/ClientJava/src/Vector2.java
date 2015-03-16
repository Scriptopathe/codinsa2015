import java.lang.*;

	public class Vector2
	{

	
		public Vector2()
		{
		}	
		public Vector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}	
		public float X;	
		public float Y;	
		public static Vector2 Deserialize(BufferedReader input) {
		try {
			Vector2 _obj =  new Vector2();
			// X
			float _obj_X = Float.Parse(input.readLine());
			_obj.X = _obj_X;
			// Y
			float _obj_Y = Float.Parse(input.readLine());
			_obj.Y = _obj_Y;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// X
			output.WriteLine(((Float)this.X).toString() + "\n");
			// Y
			output.WriteLine(((Float)this.Y).toString() + "\n");
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
