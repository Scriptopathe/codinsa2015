import java.lang.*;

	public class SpellCastTargetInfoView
	{

	
		public TargettingType Type;	
		public Vector2 TargetPosition;	
		public Vector2 TargetDirection;	
		public int TargetId;	
		public static SpellCastTargetInfoView Deserialize(BufferedReader input) {
		try {
			SpellCastTargetInfoView _obj =  new SpellCastTargetInfoView();
			// Type
			int _obj_Type = Integer.Parse(input.readLine());
			_obj.Type = TargettingType.fromValue(_obj_Type);
			// TargetPosition
			Vector2 _obj_TargetPosition = Vector2.deserialize(input);
			_obj.TargetPosition = _obj_TargetPosition;
			// TargetDirection
			Vector2 _obj_TargetDirection = Vector2.deserialize(input);
			_obj.TargetDirection = _obj_TargetDirection;
			// TargetId
			int _obj_TargetId = Integer.Parse(input.readLine());
			_obj.TargetId = _obj_TargetId;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// Type
			output.append(((Integer)(this.Type.getValue())).toString() + "\n");
			// TargetPosition
			this.TargetPosition.serialize(output);
			// TargetDirection
			this.TargetDirection.serialize(output);
			// TargetId
			output.append(((Integer)this.TargetId).toString() + "\n");
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
