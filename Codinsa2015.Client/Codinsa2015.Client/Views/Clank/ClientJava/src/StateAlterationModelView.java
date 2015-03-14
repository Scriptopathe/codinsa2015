public class StateAlterationModelView
{


	public StateAlterationType Type;
	public Double BaseDuration;
	public Boolean DashGoThroughWall;
	public DashDirectionType DashDirectionType;
	public Double FlatValue;
	public Double SourcePercentADValue;
	public Double SourcePercentHPValue;
	public Double SourcePercentMaxHPValue;
	public Double SourcePercentArmorValue;
	public Double SourcePercentAPValue;
	public Double SourcePercentRMValue;
	public Double DestPercentADValue;
	public Double DestPercentHPValue;
	public Double DestPercentMaxHPValue;
	public Double DestPercentArmorValue;
	public Double DestPercentAPValue;
	public Double DestPercentRMValue;
	public Double StructureBonus;
	public Double MonsterBonus;
	public Double CreepBonus;
	public static StateAlterationModelView deserialize(JSONObject o)
	{
		StateAlterationModelView obj = new StateAlterationModelView();
		// Type
		StateAlterationType Type = o.getInt("Type");
		obj.Type = Type;
		// BaseDuration
		Double BaseDuration = o.getDouble("BaseDuration");
		obj.BaseDuration = BaseDuration;
		// DashGoThroughWall
		Boolean DashGoThroughWall = o.getBoolean("DashGoThroughWall");
		obj.DashGoThroughWall = DashGoThroughWall;
		// DashDirectionType
		DashDirectionType DashDirectionType = o.getInt("DashDirectionType");
		obj.DashDirectionType = DashDirectionType;
		// FlatValue
		Double FlatValue = o.getDouble("FlatValue");
		obj.FlatValue = FlatValue;
		// SourcePercentADValue
		Double SourcePercentADValue = o.getDouble("SourcePercentADValue");
		obj.SourcePercentADValue = SourcePercentADValue;
		// SourcePercentHPValue
		Double SourcePercentHPValue = o.getDouble("SourcePercentHPValue");
		obj.SourcePercentHPValue = SourcePercentHPValue;
		// SourcePercentMaxHPValue
		Double SourcePercentMaxHPValue = o.getDouble("SourcePercentMaxHPValue");
		obj.SourcePercentMaxHPValue = SourcePercentMaxHPValue;
		// SourcePercentArmorValue
		Double SourcePercentArmorValue = o.getDouble("SourcePercentArmorValue");
		obj.SourcePercentArmorValue = SourcePercentArmorValue;
		// SourcePercentAPValue
		Double SourcePercentAPValue = o.getDouble("SourcePercentAPValue");
		obj.SourcePercentAPValue = SourcePercentAPValue;
		// SourcePercentRMValue
		Double SourcePercentRMValue = o.getDouble("SourcePercentRMValue");
		obj.SourcePercentRMValue = SourcePercentRMValue;
		// DestPercentADValue
		Double DestPercentADValue = o.getDouble("DestPercentADValue");
		obj.DestPercentADValue = DestPercentADValue;
		// DestPercentHPValue
		Double DestPercentHPValue = o.getDouble("DestPercentHPValue");
		obj.DestPercentHPValue = DestPercentHPValue;
		// DestPercentMaxHPValue
		Double DestPercentMaxHPValue = o.getDouble("DestPercentMaxHPValue");
		obj.DestPercentMaxHPValue = DestPercentMaxHPValue;
		// DestPercentArmorValue
		Double DestPercentArmorValue = o.getDouble("DestPercentArmorValue");
		obj.DestPercentArmorValue = DestPercentArmorValue;
		// DestPercentAPValue
		Double DestPercentAPValue = o.getDouble("DestPercentAPValue");
		obj.DestPercentAPValue = DestPercentAPValue;
		// DestPercentRMValue
		Double DestPercentRMValue = o.getDouble("DestPercentRMValue");
		obj.DestPercentRMValue = DestPercentRMValue;
		// StructureBonus
		Double StructureBonus = o.getDouble("StructureBonus");
		obj.StructureBonus = StructureBonus;
		// MonsterBonus
		Double MonsterBonus = o.getDouble("MonsterBonus");
		obj.MonsterBonus = MonsterBonus;
		// CreepBonus
		Double CreepBonus = o.getDouble("CreepBonus");
		obj.CreepBonus = CreepBonus;
		return obj;
	}

}