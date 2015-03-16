package net.codinsa2015;
import java.lang.*;
import java.util.ArrayList;
import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;
import net.codinsa2015.StateAlterationView.*;
import java.util.ArrayList;
import net.codinsa2015.Vector2.*;
import net.codinsa2015.EntityType.*;


@SuppressWarnings("unused")
public class EntityBaseView
{


	public Float GetMagicResist;
	public Float GetAbilityPower;
	public Float GetCooldownReduction;
	public Float GetMoveSpeed;
	public Float GetAttackSpeed;
	public Float GetAttackDamage;
	public Float GetArmor;
	public Float GetHP;
	public Float GetMaxHP;
	public ArrayList<StateAlterationView> StateAlterations;
	public Float BaseArmor;
	public Vector2 Direction;
	public Vector2 Position;
	public Float ShieldPoints;
	public Float HP;
	public Float BaseMaxHP;
	public Float BaseMoveSpeed;
	public Boolean IsDead;
	public EntityType Type;
	public Integer ID;
	public Float BaseAttackDamage;
	public Float BaseCooldownReduction;
	public Float BaseAttackSpeed;
	public Float BaseAbilityPower;
	public Float BaseMagicResist;
	public Boolean IsRooted;
	public Boolean IsSilenced;
	public Boolean IsStuned;
	public Boolean IsStealthed;
	public Boolean HasTrueVision;
	public Boolean HasWardVision;
	public Float VisionRange;
	public static EntityBaseView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		EntityBaseView _obj =  new EntityBaseView();
		// GetMagicResist
		float _obj_GetMagicResist = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.GetMagicResist = _obj_GetMagicResist;
		// GetAbilityPower
		float _obj_GetAbilityPower = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.GetAbilityPower = _obj_GetAbilityPower;
		// GetCooldownReduction
		float _obj_GetCooldownReduction = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.GetCooldownReduction = _obj_GetCooldownReduction;
		// GetMoveSpeed
		float _obj_GetMoveSpeed = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.GetMoveSpeed = _obj_GetMoveSpeed;
		// GetAttackSpeed
		float _obj_GetAttackSpeed = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.GetAttackSpeed = _obj_GetAttackSpeed;
		// GetAttackDamage
		float _obj_GetAttackDamage = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.GetAttackDamage = _obj_GetAttackDamage;
		// GetArmor
		float _obj_GetArmor = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.GetArmor = _obj_GetArmor;
		// GetHP
		float _obj_GetHP = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.GetHP = _obj_GetHP;
		// GetMaxHP
		float _obj_GetMaxHP = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.GetMaxHP = _obj_GetMaxHP;
		// StateAlterations
		ArrayList<StateAlterationView> _obj_StateAlterations = new ArrayList<StateAlterationView>();
		int _obj_StateAlterations_count = Integer.valueOf(input.readLine());
		for(int _obj_StateAlterations_i = 0; _obj_StateAlterations_i < _obj_StateAlterations_count; _obj_StateAlterations_i++) {
			StateAlterationView _obj_StateAlterations_e = StateAlterationView.deserialize(input);
			_obj_StateAlterations.add((StateAlterationView)_obj_StateAlterations_e);
		}
		_obj.StateAlterations = _obj_StateAlterations;
		// BaseArmor
		float _obj_BaseArmor = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.BaseArmor = _obj_BaseArmor;
		// Direction
		Vector2 _obj_Direction = Vector2.deserialize(input);
		_obj.Direction = _obj_Direction;
		// Position
		Vector2 _obj_Position = Vector2.deserialize(input);
		_obj.Position = _obj_Position;
		// ShieldPoints
		float _obj_ShieldPoints = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.ShieldPoints = _obj_ShieldPoints;
		// HP
		float _obj_HP = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.HP = _obj_HP;
		// BaseMaxHP
		float _obj_BaseMaxHP = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.BaseMaxHP = _obj_BaseMaxHP;
		// BaseMoveSpeed
		float _obj_BaseMoveSpeed = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.BaseMoveSpeed = _obj_BaseMoveSpeed;
		// IsDead
		boolean _obj_IsDead = Integer.valueOf(input.readLine()) == 0 ? false : true;
		_obj.IsDead = _obj_IsDead;
		// Type
		int _obj_Type = Integer.valueOf(input.readLine());
		_obj.Type = EntityType.fromValue(_obj_Type);
		// ID
		int _obj_ID = Integer.valueOf(input.readLine());
		_obj.ID = _obj_ID;
		// BaseAttackDamage
		float _obj_BaseAttackDamage = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.BaseAttackDamage = _obj_BaseAttackDamage;
		// BaseCooldownReduction
		float _obj_BaseCooldownReduction = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.BaseCooldownReduction = _obj_BaseCooldownReduction;
		// BaseAttackSpeed
		float _obj_BaseAttackSpeed = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.BaseAttackSpeed = _obj_BaseAttackSpeed;
		// BaseAbilityPower
		float _obj_BaseAbilityPower = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.BaseAbilityPower = _obj_BaseAbilityPower;
		// BaseMagicResist
		float _obj_BaseMagicResist = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.BaseMagicResist = _obj_BaseMagicResist;
		// IsRooted
		boolean _obj_IsRooted = Integer.valueOf(input.readLine()) == 0 ? false : true;
		_obj.IsRooted = _obj_IsRooted;
		// IsSilenced
		boolean _obj_IsSilenced = Integer.valueOf(input.readLine()) == 0 ? false : true;
		_obj.IsSilenced = _obj_IsSilenced;
		// IsStuned
		boolean _obj_IsStuned = Integer.valueOf(input.readLine()) == 0 ? false : true;
		_obj.IsStuned = _obj_IsStuned;
		// IsStealthed
		boolean _obj_IsStealthed = Integer.valueOf(input.readLine()) == 0 ? false : true;
		_obj.IsStealthed = _obj_IsStealthed;
		// HasTrueVision
		boolean _obj_HasTrueVision = Integer.valueOf(input.readLine()) == 0 ? false : true;
		_obj.HasTrueVision = _obj_HasTrueVision;
		// HasWardVision
		boolean _obj_HasWardVision = Integer.valueOf(input.readLine()) == 0 ? false : true;
		_obj.HasWardVision = _obj_HasWardVision;
		// VisionRange
		float _obj_VisionRange = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.VisionRange = _obj_VisionRange;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// GetMagicResist
		output.append(((Float)this.GetMagicResist).toString().replace('.', ',') + "\n");
		// GetAbilityPower
		output.append(((Float)this.GetAbilityPower).toString().replace('.', ',') + "\n");
		// GetCooldownReduction
		output.append(((Float)this.GetCooldownReduction).toString().replace('.', ',') + "\n");
		// GetMoveSpeed
		output.append(((Float)this.GetMoveSpeed).toString().replace('.', ',') + "\n");
		// GetAttackSpeed
		output.append(((Float)this.GetAttackSpeed).toString().replace('.', ',') + "\n");
		// GetAttackDamage
		output.append(((Float)this.GetAttackDamage).toString().replace('.', ',') + "\n");
		// GetArmor
		output.append(((Float)this.GetArmor).toString().replace('.', ',') + "\n");
		// GetHP
		output.append(((Float)this.GetHP).toString().replace('.', ',') + "\n");
		// GetMaxHP
		output.append(((Float)this.GetMaxHP).toString().replace('.', ',') + "\n");
		// StateAlterations
		output.append(String.valueOf(this.StateAlterations.size()) + "\n");
		for(int StateAlterations_it = 0; StateAlterations_it < this.StateAlterations.size();StateAlterations_it++) {
			this.StateAlterations.get(StateAlterations_it).serialize(output);
		}
		// BaseArmor
		output.append(((Float)this.BaseArmor).toString().replace('.', ',') + "\n");
		// Direction
		this.Direction.serialize(output);
		// Position
		this.Position.serialize(output);
		// ShieldPoints
		output.append(((Float)this.ShieldPoints).toString().replace('.', ',') + "\n");
		// HP
		output.append(((Float)this.HP).toString().replace('.', ',') + "\n");
		// BaseMaxHP
		output.append(((Float)this.BaseMaxHP).toString().replace('.', ',') + "\n");
		// BaseMoveSpeed
		output.append(((Float)this.BaseMoveSpeed).toString().replace('.', ',') + "\n");
		// IsDead
		output.append((this.IsDead ? 1 : 0) + "\n");
		// Type
		output.append(((Integer)(this.Type.getValue())).toString() + "\n");
		// ID
		output.append(((Integer)this.ID).toString() + "\n");
		// BaseAttackDamage
		output.append(((Float)this.BaseAttackDamage).toString().replace('.', ',') + "\n");
		// BaseCooldownReduction
		output.append(((Float)this.BaseCooldownReduction).toString().replace('.', ',') + "\n");
		// BaseAttackSpeed
		output.append(((Float)this.BaseAttackSpeed).toString().replace('.', ',') + "\n");
		// BaseAbilityPower
		output.append(((Float)this.BaseAbilityPower).toString().replace('.', ',') + "\n");
		// BaseMagicResist
		output.append(((Float)this.BaseMagicResist).toString().replace('.', ',') + "\n");
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
		output.append(((Float)this.VisionRange).toString().replace('.', ',') + "\n");
	}

}
