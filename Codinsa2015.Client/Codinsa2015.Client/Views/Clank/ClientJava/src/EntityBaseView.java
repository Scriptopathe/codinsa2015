import java.lang.*;

	public class EntityBaseView
	{

	
		public float GetMagicResist;	
		public float GetAbilityPower;	
		public float GetCooldownReduction;	
		public float GetMoveSpeed;	
		public float GetAttackSpeed;	
		public float GetAttackDamage;	
		public float GetArmor;	
		public float GetHP;	
		public float GetMaxHP;	
		public ArrayList<StateAlterationView> StateAlterations;	
		public float BaseArmor;	
		public Vector2 Direction;	
		public Vector2 Position;	
		public float ShieldPoints;	
		public float HP;	
		public float BaseMaxHP;	
		public float BaseMoveSpeed;	
		public bool IsDead;	
		public EntityType Type;	
		public int ID;	
		public float BaseAttackDamage;	
		public float BaseCooldownReduction;	
		public float BaseAttackSpeed;	
		public float BaseAbilityPower;	
		public float BaseMagicResist;	
		public bool IsRooted;	
		public bool IsSilenced;	
		public bool IsStuned;	
		public bool IsStealthed;	
		public bool HasTrueVision;	
		public bool HasWardVision;	
		public float VisionRange;	
		public static EntityBaseView Deserialize(BufferedReader input) {
		try {
			EntityBaseView _obj =  new EntityBaseView();
			// GetMagicResist
			float _obj_GetMagicResist = Float.Parse(input.readLine());
			_obj.GetMagicResist = _obj_GetMagicResist;
			// GetAbilityPower
			float _obj_GetAbilityPower = Float.Parse(input.readLine());
			_obj.GetAbilityPower = _obj_GetAbilityPower;
			// GetCooldownReduction
			float _obj_GetCooldownReduction = Float.Parse(input.readLine());
			_obj.GetCooldownReduction = _obj_GetCooldownReduction;
			// GetMoveSpeed
			float _obj_GetMoveSpeed = Float.Parse(input.readLine());
			_obj.GetMoveSpeed = _obj_GetMoveSpeed;
			// GetAttackSpeed
			float _obj_GetAttackSpeed = Float.Parse(input.readLine());
			_obj.GetAttackSpeed = _obj_GetAttackSpeed;
			// GetAttackDamage
			float _obj_GetAttackDamage = Float.Parse(input.readLine());
			_obj.GetAttackDamage = _obj_GetAttackDamage;
			// GetArmor
			float _obj_GetArmor = Float.Parse(input.readLine());
			_obj.GetArmor = _obj_GetArmor;
			// GetHP
			float _obj_GetHP = Float.Parse(input.readLine());
			_obj.GetHP = _obj_GetHP;
			// GetMaxHP
			float _obj_GetMaxHP = Float.Parse(input.readLine());
			_obj.GetMaxHP = _obj_GetMaxHP;
			// StateAlterations
			ArrayList<StateAlterationView> _obj_StateAlterations = new ArrayList<StateAlterationView>();
			int _obj_StateAlterations_count = Integer.Parse(input.readLine());
			for(int _obj_StateAlterations_i = 0; _obj_StateAlterations_i < _obj_StateAlterations_count; _obj_StateAlterations_i++) {
				StateAlterationView _obj_StateAlterations_e = StateAlterationView.deserialize(input);
				_obj_StateAlterations.add((StateAlterationView)_obj_StateAlterations_e);
			}
			_obj.StateAlterations = _obj_StateAlterations;
			// BaseArmor
			float _obj_BaseArmor = Float.Parse(input.readLine());
			_obj.BaseArmor = _obj_BaseArmor;
			// Direction
			Vector2 _obj_Direction = Vector2.deserialize(input);
			_obj.Direction = _obj_Direction;
			// Position
			Vector2 _obj_Position = Vector2.deserialize(input);
			_obj.Position = _obj_Position;
			// ShieldPoints
			float _obj_ShieldPoints = Float.Parse(input.readLine());
			_obj.ShieldPoints = _obj_ShieldPoints;
			// HP
			float _obj_HP = Float.Parse(input.readLine());
			_obj.HP = _obj_HP;
			// BaseMaxHP
			float _obj_BaseMaxHP = Float.Parse(input.readLine());
			_obj.BaseMaxHP = _obj_BaseMaxHP;
			// BaseMoveSpeed
			float _obj_BaseMoveSpeed = Float.Parse(input.readLine());
			_obj.BaseMoveSpeed = _obj_BaseMoveSpeed;
			// IsDead
			boolean _obj_IsDead = Integer.valueof(input.readLine()) == 0 ? false : true;
			_obj.IsDead = _obj_IsDead;
			// Type
			int _obj_Type = Integer.Parse(input.readLine());
			_obj.Type = EntityType.fromValue(_obj_Type);
			// ID
			int _obj_ID = Integer.Parse(input.readLine());
			_obj.ID = _obj_ID;
			// BaseAttackDamage
			float _obj_BaseAttackDamage = Float.Parse(input.readLine());
			_obj.BaseAttackDamage = _obj_BaseAttackDamage;
			// BaseCooldownReduction
			float _obj_BaseCooldownReduction = Float.Parse(input.readLine());
			_obj.BaseCooldownReduction = _obj_BaseCooldownReduction;
			// BaseAttackSpeed
			float _obj_BaseAttackSpeed = Float.Parse(input.readLine());
			_obj.BaseAttackSpeed = _obj_BaseAttackSpeed;
			// BaseAbilityPower
			float _obj_BaseAbilityPower = Float.Parse(input.readLine());
			_obj.BaseAbilityPower = _obj_BaseAbilityPower;
			// BaseMagicResist
			float _obj_BaseMagicResist = Float.Parse(input.readLine());
			_obj.BaseMagicResist = _obj_BaseMagicResist;
			// IsRooted
			boolean _obj_IsRooted = Integer.valueof(input.readLine()) == 0 ? false : true;
			_obj.IsRooted = _obj_IsRooted;
			// IsSilenced
			boolean _obj_IsSilenced = Integer.valueof(input.readLine()) == 0 ? false : true;
			_obj.IsSilenced = _obj_IsSilenced;
			// IsStuned
			boolean _obj_IsStuned = Integer.valueof(input.readLine()) == 0 ? false : true;
			_obj.IsStuned = _obj_IsStuned;
			// IsStealthed
			boolean _obj_IsStealthed = Integer.valueof(input.readLine()) == 0 ? false : true;
			_obj.IsStealthed = _obj_IsStealthed;
			// HasTrueVision
			boolean _obj_HasTrueVision = Integer.valueof(input.readLine()) == 0 ? false : true;
			_obj.HasTrueVision = _obj_HasTrueVision;
			// HasWardVision
			boolean _obj_HasWardVision = Integer.valueof(input.readLine()) == 0 ? false : true;
			_obj.HasWardVision = _obj_HasWardVision;
			// VisionRange
			float _obj_VisionRange = Float.Parse(input.readLine());
			_obj.VisionRange = _obj_VisionRange;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// GetMagicResist
			output.WriteLine(((Float)this.GetMagicResist).toString() + "\n");
			// GetAbilityPower
			output.WriteLine(((Float)this.GetAbilityPower).toString() + "\n");
			// GetCooldownReduction
			output.WriteLine(((Float)this.GetCooldownReduction).toString() + "\n");
			// GetMoveSpeed
			output.WriteLine(((Float)this.GetMoveSpeed).toString() + "\n");
			// GetAttackSpeed
			output.WriteLine(((Float)this.GetAttackSpeed).toString() + "\n");
			// GetAttackDamage
			output.WriteLine(((Float)this.GetAttackDamage).toString() + "\n");
			// GetArmor
			output.WriteLine(((Float)this.GetArmor).toString() + "\n");
			// GetHP
			output.WriteLine(((Float)this.GetHP).toString() + "\n");
			// GetMaxHP
			output.WriteLine(((Float)this.GetMaxHP).toString() + "\n");
			// StateAlterations
			output.append(this.StateAlterations.size().toString() + "\n");
			for(int StateAlterations_it = 0; StateAlterations_it < this.StateAlterations.size();StateAlterations_it++) {
				this.StateAlterations[StateAlterations_it].serialize(output);
			}
			// BaseArmor
			output.WriteLine(((Float)this.BaseArmor).toString() + "\n");
			// Direction
			this.Direction.serialize(output);
			// Position
			this.Position.serialize(output);
			// ShieldPoints
			output.WriteLine(((Float)this.ShieldPoints).toString() + "\n");
			// HP
			output.WriteLine(((Float)this.HP).toString() + "\n");
			// BaseMaxHP
			output.WriteLine(((Float)this.BaseMaxHP).toString() + "\n");
			// BaseMoveSpeed
			output.WriteLine(((Float)this.BaseMoveSpeed).toString() + "\n");
			// IsDead
			output.append((this.IsDead ? 1 : 0) + "\n");
			// Type
			output.append(((Integer)(this.Type.getValue())).toString() + "\n");
			// ID
			output.append(((Integer)this.ID).toString() + "\n");
			// BaseAttackDamage
			output.WriteLine(((Float)this.BaseAttackDamage).toString() + "\n");
			// BaseCooldownReduction
			output.WriteLine(((Float)this.BaseCooldownReduction).toString() + "\n");
			// BaseAttackSpeed
			output.WriteLine(((Float)this.BaseAttackSpeed).toString() + "\n");
			// BaseAbilityPower
			output.WriteLine(((Float)this.BaseAbilityPower).toString() + "\n");
			// BaseMagicResist
			output.WriteLine(((Float)this.BaseMagicResist).toString() + "\n");
			// IsRooted
			output.append((this.IsRooted ? 1 : 0) + "\n");
			// IsSilenced
			output.append((this.IsSilenced ? 1 : 0) + "\n");
			// IsStuned
			output.append((this.IsStuned ? 1 : 0) + "\n");
			// IsStealthed
			output.append((this.IsStealthed ? 1 : 0) + "\n");
			// HasTrueVision
			output.append((this.HasTrueVision ? 1 : 0) + "\n");
			// HasWardVision
			output.append((this.HasWardVision ? 1 : 0) + "\n");
			// VisionRange
			output.WriteLine(((Float)this.VisionRange).toString() + "\n");
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
