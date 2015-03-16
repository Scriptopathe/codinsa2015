import java.lang.*;

	public class SpellTargetInfoView
	{

	
		public TargettingType Type;	
		public float Range;	
		public float Duration;	
		public float AoeRadius;	
		public bool DieOnCollision;	
		public EntityTypeRelative AllowedTargetTypes;	
		public static SpellTargetInfoView Deserialize(BufferedReader input) {
		try {
			SpellTargetInfoView _obj =  new SpellTargetInfoView();
			// Type
			int _obj_Type = Integer.Parse(input.readLine());
			_obj.Type = TargettingType.fromValue(_obj_Type);
			// Range
			float _obj_Range = Float.Parse(input.readLine());
			_obj.Range = _obj_Range;
			// Duration
			float _obj_Duration = Float.Parse(input.readLine());
			_obj.Duration = _obj_Duration;
			// AoeRadius
			float _obj_AoeRadius = Float.Parse(input.readLine());
			_obj.AoeRadius = _obj_AoeRadius;
			// DieOnCollision
			boolean _obj_DieOnCollision = Integer.valueof(input.readLine()) == 0 ? false : true;
			_obj.DieOnCollision = _obj_DieOnCollision;
			// AllowedTargetTypes
			int _obj_AllowedTargetTypes = Integer.Parse(input.readLine());
			_obj.AllowedTargetTypes = EntityTypeRelative.fromValue(_obj_AllowedTargetTypes);
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// Type
			output.append(((Integer)(this.Type.getValue())).toString() + "\n");
			// Range
			output.WriteLine(((Float)this.Range).toString() + "\n");
			// Duration
			output.WriteLine(((Float)this.Duration).toString() + "\n");
			// AoeRadius
			output.WriteLine(((Float)this.AoeRadius).toString() + "\n");
			// DieOnCollision
			output.append((this.DieOnCollision ? 1 : 0) + "\n");
			// AllowedTargetTypes
			output.append(((Integer)(this.AllowedTargetTypes.getValue())).toString() + "\n");
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
