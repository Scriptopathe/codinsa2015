import java.lang.*;

	public class SpellcastBaseView
	{

	
		public GenericShapeView Shape;	
		public static SpellcastBaseView Deserialize(BufferedReader input) {
		try {
			SpellcastBaseView _obj =  new SpellcastBaseView();
			// Shape
			GenericShapeView _obj_Shape = GenericShapeView.deserialize(input);
			_obj.Shape = _obj_Shape;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// Shape
			this.Shape.serialize(output);
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
