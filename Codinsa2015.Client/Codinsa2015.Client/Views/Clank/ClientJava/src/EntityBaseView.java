public class EntityBaseView
{


	public Double GetMagicResist;
	public Double GetAbilityPower;
	public Double GetCooldownReduction;
	public Double GetMoveSpeed;
	public Double GetAttackSpeed;
	public Double GetAttackDamage;
	public Double GetArmor;
	public Double GetHP;
	public Double GetMaxHP;
	public ArrayList<StateAlterationView> StateAlterations;
	public Double BaseArmor;
	public Vector2 Direction;
	public Vector2 Position;
	public Double ShieldPoints;
	public Double HP;
	public Double BaseMaxHP;
	public Double BaseMoveSpeed;
	public Boolean IsDead;
	public EntityType Type;
	public Integer ID;
	public Double BaseAttackDamage;
	public Double BaseCooldownReduction;
	public Double BaseAttackSpeed;
	public Double BaseAbilityPower;
	public Double BaseMagicResist;
	public Boolean IsRooted;
	public Boolean IsSilenced;
	public Boolean IsStuned;
	public Boolean IsStealthed;
	public Boolean HasTrueVision;
	public Boolean HasWardVision;
	public Double VisionRange;
	public static EntityBaseView deserialize(JSONObject o)
	{
		EntityBaseView obj = new EntityBaseView();
		// GetMagicResist
		Double GetMagicResist = o.getDouble("GetMagicResist");
		obj.GetMagicResist = GetMagicResist;
		// GetAbilityPower
		Double GetAbilityPower = o.getDouble("GetAbilityPower");
		obj.GetAbilityPower = GetAbilityPower;
		// GetCooldownReduction
		Double GetCooldownReduction = o.getDouble("GetCooldownReduction");
		obj.GetCooldownReduction = GetCooldownReduction;
		// GetMoveSpeed
		Double GetMoveSpeed = o.getDouble("GetMoveSpeed");
		obj.GetMoveSpeed = GetMoveSpeed;
		// GetAttackSpeed
		Double GetAttackSpeed = o.getDouble("GetAttackSpeed");
		obj.GetAttackSpeed = GetAttackSpeed;
		// GetAttackDamage
		Double GetAttackDamage = o.getDouble("GetAttackDamage");
		obj.GetAttackDamage = GetAttackDamage;
		// GetArmor
		Double GetArmor = o.getDouble("GetArmor");
		obj.GetArmor = GetArmor;
		// GetHP
		Double GetHP = o.getDouble("GetHP");
		obj.GetHP = GetHP;
		// GetMaxHP
		Double GetMaxHP = o.getDouble("GetMaxHP");
		obj.GetMaxHP = GetMaxHP;
		// StateAlterations
		ArrayList<StateAlterationView> StateAlterations = new ArrayList<StateAlterationView>();
		JSONArray StateAlterations_json = o.getJSONArray("StateAlterations");
		for(int StateAlterations_it = 0;StateAlterations_it < o.length(); StateAlterations_it++)
		{
			StateAlterationView StateAlterations_item = StateAlterationView.deserialize(StateAlterations_json.getJSONObject("StateAlterations_it"));
			StateAlterations.add(StateAlterations_item);
		}
		obj.StateAlterations = StateAlterations;
		// BaseArmor
		Double BaseArmor = o.getDouble("BaseArmor");
		obj.BaseArmor = BaseArmor;
		// Direction
		Vector2 Direction = Vector2.deserialize(o.getJSONObject("Direction"));
		obj.Direction = Direction;
		// Position
		Vector2 Position = Vector2.deserialize(o.getJSONObject("Position"));
		obj.Position = Position;
		// ShieldPoints
		Double ShieldPoints = o.getDouble("ShieldPoints");
		obj.ShieldPoints = ShieldPoints;
		// HP
		Double HP = o.getDouble("HP");
		obj.HP = HP;
		// BaseMaxHP
		Double BaseMaxHP = o.getDouble("BaseMaxHP");
		obj.BaseMaxHP = BaseMaxHP;
		// BaseMoveSpeed
		Double BaseMoveSpeed = o.getDouble("BaseMoveSpeed");
		obj.BaseMoveSpeed = BaseMoveSpeed;
		// IsDead
		Boolean IsDead = o.getBoolean("IsDead");
		obj.IsDead = IsDead;
		// Type
		EntityType Type = o.getInt("Type");
		obj.Type = Type;
		// ID
		Integer ID = o.getInt("ID");
		obj.ID = ID;
		// BaseAttackDamage
		Double BaseAttackDamage = o.getDouble("BaseAttackDamage");
		obj.BaseAttackDamage = BaseAttackDamage;
		// BaseCooldownReduction
		Double BaseCooldownReduction = o.getDouble("BaseCooldownReduction");
		obj.BaseCooldownReduction = BaseCooldownReduction;
		// BaseAttackSpeed
		Double BaseAttackSpeed = o.getDouble("BaseAttackSpeed");
		obj.BaseAttackSpeed = BaseAttackSpeed;
		// BaseAbilityPower
		Double BaseAbilityPower = o.getDouble("BaseAbilityPower");
		obj.BaseAbilityPower = BaseAbilityPower;
		// BaseMagicResist
		Double BaseMagicResist = o.getDouble("BaseMagicResist");
		obj.BaseMagicResist = BaseMagicResist;
		// IsRooted
		Boolean IsRooted = o.getBoolean("IsRooted");
		obj.IsRooted = IsRooted;
		// IsSilenced
		Boolean IsSilenced = o.getBoolean("IsSilenced");
		obj.IsSilenced = IsSilenced;
		// IsStuned
		Boolean IsStuned = o.getBoolean("IsStuned");
		obj.IsStuned = IsStuned;
		// IsStealthed
		Boolean IsStealthed = o.getBoolean("IsStealthed");
		obj.IsStealthed = IsStealthed;
		// HasTrueVision
		Boolean HasTrueVision = o.getBoolean("HasTrueVision");
		obj.HasTrueVision = HasTrueVision;
		// HasWardVision
		Boolean HasWardVision = o.getBoolean("HasWardVision");
		obj.HasWardVision = HasWardVision;
		// VisionRange
		Double VisionRange = o.getDouble("VisionRange");
		obj.VisionRange = VisionRange;
		return obj;
	}

}