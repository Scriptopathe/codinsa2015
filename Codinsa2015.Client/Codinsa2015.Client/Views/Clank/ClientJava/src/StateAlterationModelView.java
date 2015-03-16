import java.lang.*;

	public class StateAlterationModelView
	{

	
		public StateAlterationType Type;	
		public float BaseDuration;	
		public bool DashGoThroughWall;	
		public DashDirectionType DashDirectionType;	
		public float FlatValue;	
		public float SourcePercentADValue;	
		public float SourcePercentHPValue;	
		public float SourcePercentMaxHPValue;	
		public float SourcePercentArmorValue;	
		public float SourcePercentAPValue;	
		public float SourcePercentRMValue;	
		public float DestPercentADValue;	
		public float DestPercentHPValue;	
		public float DestPercentMaxHPValue;	
		public float DestPercentArmorValue;	
		public float DestPercentAPValue;	
		public float DestPercentRMValue;	
		public float StructureBonus;	
		public float MonsterBonus;	
		public float CreepBonus;	
		public static StateAlterationModelView Deserialize(BufferedReader input) {
		try {
			StateAlterationModelView _obj =  new StateAlterationModelView();
			// Type
			int _obj_Type = Integer.Parse(input.readLine());
			_obj.Type = StateAlterationType.fromValue(_obj_Type);
			// BaseDuration
			float _obj_BaseDuration = Float.Parse(input.readLine());
			_obj.BaseDuration = _obj_BaseDuration;
			// DashGoThroughWall
			boolean _obj_DashGoThroughWall = Integer.valueof(input.readLine()) == 0 ? false : true;
			_obj.DashGoThroughWall = _obj_DashGoThroughWall;
			// DashDirectionType
			int _obj_DashDirectionType = Integer.Parse(input.readLine());
			_obj.DashDirectionType = DashDirectionType.fromValue(_obj_DashDirectionType);
			// FlatValue
			float _obj_FlatValue = Float.Parse(input.readLine());
			_obj.FlatValue = _obj_FlatValue;
			// SourcePercentADValue
			float _obj_SourcePercentADValue = Float.Parse(input.readLine());
			_obj.SourcePercentADValue = _obj_SourcePercentADValue;
			// SourcePercentHPValue
			float _obj_SourcePercentHPValue = Float.Parse(input.readLine());
			_obj.SourcePercentHPValue = _obj_SourcePercentHPValue;
			// SourcePercentMaxHPValue
			float _obj_SourcePercentMaxHPValue = Float.Parse(input.readLine());
			_obj.SourcePercentMaxHPValue = _obj_SourcePercentMaxHPValue;
			// SourcePercentArmorValue
			float _obj_SourcePercentArmorValue = Float.Parse(input.readLine());
			_obj.SourcePercentArmorValue = _obj_SourcePercentArmorValue;
			// SourcePercentAPValue
			float _obj_SourcePercentAPValue = Float.Parse(input.readLine());
			_obj.SourcePercentAPValue = _obj_SourcePercentAPValue;
			// SourcePercentRMValue
			float _obj_SourcePercentRMValue = Float.Parse(input.readLine());
			_obj.SourcePercentRMValue = _obj_SourcePercentRMValue;
			// DestPercentADValue
			float _obj_DestPercentADValue = Float.Parse(input.readLine());
			_obj.DestPercentADValue = _obj_DestPercentADValue;
			// DestPercentHPValue
			float _obj_DestPercentHPValue = Float.Parse(input.readLine());
			_obj.DestPercentHPValue = _obj_DestPercentHPValue;
			// DestPercentMaxHPValue
			float _obj_DestPercentMaxHPValue = Float.Parse(input.readLine());
			_obj.DestPercentMaxHPValue = _obj_DestPercentMaxHPValue;
			// DestPercentArmorValue
			float _obj_DestPercentArmorValue = Float.Parse(input.readLine());
			_obj.DestPercentArmorValue = _obj_DestPercentArmorValue;
			// DestPercentAPValue
			float _obj_DestPercentAPValue = Float.Parse(input.readLine());
			_obj.DestPercentAPValue = _obj_DestPercentAPValue;
			// DestPercentRMValue
			float _obj_DestPercentRMValue = Float.Parse(input.readLine());
			_obj.DestPercentRMValue = _obj_DestPercentRMValue;
			// StructureBonus
			float _obj_StructureBonus = Float.Parse(input.readLine());
			_obj.StructureBonus = _obj_StructureBonus;
			// MonsterBonus
			float _obj_MonsterBonus = Float.Parse(input.readLine());
			_obj.MonsterBonus = _obj_MonsterBonus;
			// CreepBonus
			float _obj_CreepBonus = Float.Parse(input.readLine());
			_obj.CreepBonus = _obj_CreepBonus;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// Type
			output.append(((Integer)(this.Type.getValue())).toString() + "\n");
			// BaseDuration
			output.WriteLine(((Float)this.BaseDuration).toString() + "\n");
			// DashGoThroughWall
			output.append((this.DashGoThroughWall ? 1 : 0) + "\n");
			// DashDirectionType
			output.append(((Integer)(this.DashDirectionType.getValue())).toString() + "\n");
			// FlatValue
			output.WriteLine(((Float)this.FlatValue).toString() + "\n");
			// SourcePercentADValue
			output.WriteLine(((Float)this.SourcePercentADValue).toString() + "\n");
			// SourcePercentHPValue
			output.WriteLine(((Float)this.SourcePercentHPValue).toString() + "\n");
			// SourcePercentMaxHPValue
			output.WriteLine(((Float)this.SourcePercentMaxHPValue).toString() + "\n");
			// SourcePercentArmorValue
			output.WriteLine(((Float)this.SourcePercentArmorValue).toString() + "\n");
			// SourcePercentAPValue
			output.WriteLine(((Float)this.SourcePercentAPValue).toString() + "\n");
			// SourcePercentRMValue
			output.WriteLine(((Float)this.SourcePercentRMValue).toString() + "\n");
			// DestPercentADValue
			output.WriteLine(((Float)this.DestPercentADValue).toString() + "\n");
			// DestPercentHPValue
			output.WriteLine(((Float)this.DestPercentHPValue).toString() + "\n");
			// DestPercentMaxHPValue
			output.WriteLine(((Float)this.DestPercentMaxHPValue).toString() + "\n");
			// DestPercentArmorValue
			output.WriteLine(((Float)this.DestPercentArmorValue).toString() + "\n");
			// DestPercentAPValue
			output.WriteLine(((Float)this.DestPercentAPValue).toString() + "\n");
			// DestPercentRMValue
			output.WriteLine(((Float)this.DestPercentRMValue).toString() + "\n");
			// StructureBonus
			output.WriteLine(((Float)this.StructureBonus).toString() + "\n");
			// MonsterBonus
			output.WriteLine(((Float)this.MonsterBonus).toString() + "\n");
			// CreepBonus
			output.WriteLine(((Float)this.CreepBonus).toString() + "\n");
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
