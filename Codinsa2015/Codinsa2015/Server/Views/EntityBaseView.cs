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
	}
}
