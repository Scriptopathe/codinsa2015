using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

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
		public List<StateAlterationView> StateAlterations;	
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
		public static EntityBaseView Deserialize(System.IO.StreamReader input) {
			EntityBaseView _obj =  new EntityBaseView();
			// GetMagicResist
			float _obj_GetMagicResist = Single.Parse(input.ReadLine());
			_obj.GetMagicResist = (float)_obj_GetMagicResist;
			// GetAbilityPower
			float _obj_GetAbilityPower = Single.Parse(input.ReadLine());
			_obj.GetAbilityPower = (float)_obj_GetAbilityPower;
			// GetCooldownReduction
			float _obj_GetCooldownReduction = Single.Parse(input.ReadLine());
			_obj.GetCooldownReduction = (float)_obj_GetCooldownReduction;
			// GetMoveSpeed
			float _obj_GetMoveSpeed = Single.Parse(input.ReadLine());
			_obj.GetMoveSpeed = (float)_obj_GetMoveSpeed;
			// GetAttackSpeed
			float _obj_GetAttackSpeed = Single.Parse(input.ReadLine());
			_obj.GetAttackSpeed = (float)_obj_GetAttackSpeed;
			// GetAttackDamage
			float _obj_GetAttackDamage = Single.Parse(input.ReadLine());
			_obj.GetAttackDamage = (float)_obj_GetAttackDamage;
			// GetArmor
			float _obj_GetArmor = Single.Parse(input.ReadLine());
			_obj.GetArmor = (float)_obj_GetArmor;
			// GetHP
			float _obj_GetHP = Single.Parse(input.ReadLine());
			_obj.GetHP = (float)_obj_GetHP;
			// GetMaxHP
			float _obj_GetMaxHP = Single.Parse(input.ReadLine());
			_obj.GetMaxHP = (float)_obj_GetMaxHP;
			// StateAlterations
			List<StateAlterationView> _obj_StateAlterations = new List<StateAlterationView>();
			int _obj_StateAlterations_count = Int32.Parse(input.ReadLine());
			for(int _obj_StateAlterations_i = 0; _obj_StateAlterations_i < _obj_StateAlterations_count; _obj_StateAlterations_i++) {
				StateAlterationView _obj_StateAlterations_e = StateAlterationView.Deserialize(input);
				_obj_StateAlterations.Add((StateAlterationView)_obj_StateAlterations_e);
			}
			_obj.StateAlterations = (List<StateAlterationView>)_obj_StateAlterations;
			// BaseArmor
			float _obj_BaseArmor = Single.Parse(input.ReadLine());
			_obj.BaseArmor = (float)_obj_BaseArmor;
			// Direction
			Vector2 _obj_Direction = Vector2.Deserialize(input);
			_obj.Direction = (Vector2)_obj_Direction;
			// Position
			Vector2 _obj_Position = Vector2.Deserialize(input);
			_obj.Position = (Vector2)_obj_Position;
			// ShieldPoints
			float _obj_ShieldPoints = Single.Parse(input.ReadLine());
			_obj.ShieldPoints = (float)_obj_ShieldPoints;
			// HP
			float _obj_HP = Single.Parse(input.ReadLine());
			_obj.HP = (float)_obj_HP;
			// BaseMaxHP
			float _obj_BaseMaxHP = Single.Parse(input.ReadLine());
			_obj.BaseMaxHP = (float)_obj_BaseMaxHP;
			// BaseMoveSpeed
			float _obj_BaseMoveSpeed = Single.Parse(input.ReadLine());
			_obj.BaseMoveSpeed = (float)_obj_BaseMoveSpeed;
			// IsDead
			bool _obj_IsDead = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			_obj.IsDead = (bool)_obj_IsDead;
			// Type
			int _obj_Type = Int32.Parse(input.ReadLine());
			_obj.Type = (EntityType)_obj_Type;
			// ID
			int _obj_ID = Int32.Parse(input.ReadLine());
			_obj.ID = (int)_obj_ID;
			// BaseAttackDamage
			float _obj_BaseAttackDamage = Single.Parse(input.ReadLine());
			_obj.BaseAttackDamage = (float)_obj_BaseAttackDamage;
			// BaseCooldownReduction
			float _obj_BaseCooldownReduction = Single.Parse(input.ReadLine());
			_obj.BaseCooldownReduction = (float)_obj_BaseCooldownReduction;
			// BaseAttackSpeed
			float _obj_BaseAttackSpeed = Single.Parse(input.ReadLine());
			_obj.BaseAttackSpeed = (float)_obj_BaseAttackSpeed;
			// BaseAbilityPower
			float _obj_BaseAbilityPower = Single.Parse(input.ReadLine());
			_obj.BaseAbilityPower = (float)_obj_BaseAbilityPower;
			// BaseMagicResist
			float _obj_BaseMagicResist = Single.Parse(input.ReadLine());
			_obj.BaseMagicResist = (float)_obj_BaseMagicResist;
			// IsRooted
			bool _obj_IsRooted = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			_obj.IsRooted = (bool)_obj_IsRooted;
			// IsSilenced
			bool _obj_IsSilenced = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			_obj.IsSilenced = (bool)_obj_IsSilenced;
			// IsStuned
			bool _obj_IsStuned = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			_obj.IsStuned = (bool)_obj_IsStuned;
			// IsStealthed
			bool _obj_IsStealthed = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			_obj.IsStealthed = (bool)_obj_IsStealthed;
			// HasTrueVision
			bool _obj_HasTrueVision = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			_obj.HasTrueVision = (bool)_obj_HasTrueVision;
			// HasWardVision
			bool _obj_HasWardVision = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			_obj.HasWardVision = (bool)_obj_HasWardVision;
			// VisionRange
			float _obj_VisionRange = Single.Parse(input.ReadLine());
			_obj.VisionRange = (float)_obj_VisionRange;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// GetMagicResist
			output.WriteLine(((float)this.GetMagicResist).ToString());
			// GetAbilityPower
			output.WriteLine(((float)this.GetAbilityPower).ToString());
			// GetCooldownReduction
			output.WriteLine(((float)this.GetCooldownReduction).ToString());
			// GetMoveSpeed
			output.WriteLine(((float)this.GetMoveSpeed).ToString());
			// GetAttackSpeed
			output.WriteLine(((float)this.GetAttackSpeed).ToString());
			// GetAttackDamage
			output.WriteLine(((float)this.GetAttackDamage).ToString());
			// GetArmor
			output.WriteLine(((float)this.GetArmor).ToString());
			// GetHP
			output.WriteLine(((float)this.GetHP).ToString());
			// GetMaxHP
			output.WriteLine(((float)this.GetMaxHP).ToString());
			// StateAlterations
			output.WriteLine(this.StateAlterations.Count.ToString());
			for(int StateAlterations_it = 0; StateAlterations_it < this.StateAlterations.Count;StateAlterations_it++) {
				this.StateAlterations[StateAlterations_it].Serialize(output);
			}
			// BaseArmor
			output.WriteLine(((float)this.BaseArmor).ToString());
			// Direction
			this.Direction.Serialize(output);
			// Position
			this.Position.Serialize(output);
			// ShieldPoints
			output.WriteLine(((float)this.ShieldPoints).ToString());
			// HP
			output.WriteLine(((float)this.HP).ToString());
			// BaseMaxHP
			output.WriteLine(((float)this.BaseMaxHP).ToString());
			// BaseMoveSpeed
			output.WriteLine(((float)this.BaseMoveSpeed).ToString());
			// IsDead
			output.WriteLine(this.IsDead ? 1 : 0);
			// Type
			output.WriteLine(((int)this.Type).ToString());
			// ID
			output.WriteLine(((int)this.ID).ToString());
			// BaseAttackDamage
			output.WriteLine(((float)this.BaseAttackDamage).ToString());
			// BaseCooldownReduction
			output.WriteLine(((float)this.BaseCooldownReduction).ToString());
			// BaseAttackSpeed
			output.WriteLine(((float)this.BaseAttackSpeed).ToString());
			// BaseAbilityPower
			output.WriteLine(((float)this.BaseAbilityPower).ToString());
			// BaseMagicResist
			output.WriteLine(((float)this.BaseMagicResist).ToString());
			// IsRooted
			output.WriteLine(this.IsRooted ? 1 : 0);
			// IsSilenced
			output.WriteLine(this.IsSilenced ? 1 : 0);
			// IsStuned
			output.WriteLine(this.IsStuned ? 1 : 0);
			// IsStealthed
			output.WriteLine(this.IsStealthed ? 1 : 0);
			// HasTrueVision
			output.WriteLine(this.HasTrueVision ? 1 : 0);
			// HasWardVision
			output.WriteLine(this.HasWardVision ? 1 : 0);
			// VisionRange
			output.WriteLine(((float)this.VisionRange).ToString());
		}

	}
}
