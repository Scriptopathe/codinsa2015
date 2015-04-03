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

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// Retourne la résistance magique effective de cette entité.
		/// </summary>
		public float GetMagicResist;	
		/// <summary>
		/// Retourne la valeur d'AP effective de cette entité.
		/// </summary>
		public float GetAbilityPower;	
		/// <summary>
		/// Retourne la valeur de CDR effective de cette entité. (de 0 à 0.40)
		/// </summary>
		public float GetCooldownReduction;	
		/// <summary>
		/// Obtient la vitesse de déplacement de l'entité.
		/// </summary>
		public float GetMoveSpeed;	
		/// <summary>
		/// Obtient la vitesse d'attaque effective de l'entité.
		/// </summary>
		public float GetAttackSpeed;	
		/// <summary>
		/// Obtient la vitesse d'attaque effective de l'entité.
		/// </summary>
		public float GetHPRegen;	
		/// <summary>
		/// Obtient les points d'attaque effectifs de cette entité.
		/// </summary>
		public float GetAttackDamage;	
		/// <summary>
		/// Fonction utilisée pour obtenir les points d'armure effectifs sur cette unité.
		/// </summary>
		public float GetArmor;	
		/// <summary>
		/// Obtient les HP actuels de cette entité.
		/// </summary>
		public float GetHP;	
		/// <summary>
		/// Obtient les HP max actuels de cette entité.
		/// </summary>
		public float GetMaxHP;	
		/// <summary>
		/// niveau du passif unique
		/// </summary>
		public int UniquePassiveLevel;	
		/// <summary>
		/// passif unique de cette entité.
		/// </summary>
		public EntityUniquePassives UniquePassive;	
		/// <summary>
		/// Si cette entité est un héros, obtient le rôle de ce héros.
		/// </summary>
		public EntityHeroRole Role;	
		/// <summary>
		/// Obtient la liste des altérations d'état affectées à cette entité.
		/// </summary>
		public List<StateAlterationView> StateAlterations;	
		/// <summary>
		/// Représente les points d'armure de base de cette entité.
		/// </summary>
		public float BaseArmor;	
		/// <summary>
		/// Représente la direction de cette entité.
		/// </summary>
		public Vector2 Direction;	
		/// <summary>
		/// Position de l'entité sur la map.
		/// </summary>
		public Vector2 Position;	
		/// <summary>
		/// Points de bouclier de cette entité.
		/// </summary>
		public float ShieldPoints;	
		/// <summary>
		/// Obtient les points de vie actuels de l'entité
		/// </summary>
		public float HP;	
		/// <summary>
		/// régénération de HP / s de base de cette unité.
		/// </summary>
		public float BaseHPRegen;	
		/// <summary>
		/// Obtient le nombre de points de vie maximum de base de cette entité.
		/// </summary>
		public float BaseMaxHP;	
		/// <summary>
		/// Obtient la vitesse de déplacement de base de l'entité.
		/// </summary>
		public float BaseMoveSpeed;	
		/// <summary>
		/// Retourne une valeur indiquant si l'entité est morte.
		/// </summary>
		public bool IsDead;	
		/// <summary>
		/// Retourne le type de cette entité.
		/// </summary>
		public EntityType Type;	
		/// <summary>
		/// Obtient l'id de cette entité.
		/// </summary>
		public int ID;	
		/// <summary>
		/// Obtient ou définit les points d'attaque de base de cette unité.
		/// </summary>
		public float BaseAttackDamage;	
		/// <summary>
		/// Cooldown reduction de base de cette unité.
		/// </summary>
		public float BaseCooldownReduction;	
		/// <summary>
		/// Attack speed de base de cette entité.
		/// </summary>
		public float BaseAttackSpeed;	
		/// <summary>
		/// Points d'AP de base de cette entité.
		/// </summary>
		public float BaseAbilityPower;	
		/// <summary>
		/// Point de résistance magique de base de cette entité.
		/// </summary>
		public float BaseMagicResist;	
		/// <summary>
		/// Obtient une valeur indiquant si cette entité est Rootée. (ne peut plus bouger).
		/// </summary>
		public bool IsRooted;	
		/// <summary>
		/// Obtient une valeur indiquant si cette unité est Silenced (ne peut pas utiliser de sorts).
		/// </summary>
		public bool IsSilenced;	
		/// <summary>
		/// Obtient une valeur indiquant si cette unité est Stuned (ne peut pas bouger ni utiliser de
		/// sorts).
		/// </summary>
		public bool IsStuned;	
		/// <summary>
		/// Obtient une valeur indiquant si cette unité possède une immunité temporaire aux dégâts.
		/// </summary>
		public bool IsDamageImmune;	
		/// <summary>
		/// Obtient une valeur indiquant si cette unité possède une immunité temporaire aux contrôles.
		/// </summary>
		public bool IsControlImmune;	
		/// <summary>
		/// Obtient une valeur indiquant si cette unité est aveuglé (ne peut pas lancer d'attaque avec son
		/// arme).
		/// </summary>
		public bool IsBlind;	
		/// <summary>
		/// Obtient une valeur indiquant si cette unité est invisible.
		/// </summary>
		public bool IsStealthed;	
		/// <summary>
		/// Obtient une valeur indiquant si cette entité possède la vision pure.
		/// </summary>
		public bool HasTrueVision;	
		/// <summary>
		/// Obtient une valeur indiquant si cette unité peut voir les wards.
		/// </summary>
		public bool HasWardVision;	
		/// <summary>
		/// Retourne la range à laquelle cette entité donne la vision.
		/// </summary>
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
			// GetHPRegen
			float _obj_GetHPRegen = Single.Parse(input.ReadLine());
			_obj.GetHPRegen = (float)_obj_GetHPRegen;
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
			// UniquePassiveLevel
			int _obj_UniquePassiveLevel = Int32.Parse(input.ReadLine());
			_obj.UniquePassiveLevel = (int)_obj_UniquePassiveLevel;
			// UniquePassive
			EntityUniquePassives _obj_UniquePassive = (EntityUniquePassives)Int32.Parse(input.ReadLine());
			_obj.UniquePassive = (EntityUniquePassives)_obj_UniquePassive;
			// Role
			EntityHeroRole _obj_Role = (EntityHeroRole)Int32.Parse(input.ReadLine());
			_obj.Role = (EntityHeroRole)_obj_Role;
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
			// BaseHPRegen
			float _obj_BaseHPRegen = Single.Parse(input.ReadLine());
			_obj.BaseHPRegen = (float)_obj_BaseHPRegen;
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
			EntityType _obj_Type = (EntityType)Int32.Parse(input.ReadLine());
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
			// IsDamageImmune
			bool _obj_IsDamageImmune = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			_obj.IsDamageImmune = (bool)_obj_IsDamageImmune;
			// IsControlImmune
			bool _obj_IsControlImmune = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			_obj.IsControlImmune = (bool)_obj_IsControlImmune;
			// IsBlind
			bool _obj_IsBlind = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			_obj.IsBlind = (bool)_obj_IsBlind;
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
			// GetHPRegen
			output.WriteLine(((float)this.GetHPRegen).ToString());
			// GetAttackDamage
			output.WriteLine(((float)this.GetAttackDamage).ToString());
			// GetArmor
			output.WriteLine(((float)this.GetArmor).ToString());
			// GetHP
			output.WriteLine(((float)this.GetHP).ToString());
			// GetMaxHP
			output.WriteLine(((float)this.GetMaxHP).ToString());
			// UniquePassiveLevel
			output.WriteLine(((int)this.UniquePassiveLevel).ToString());
			// UniquePassive
			output.WriteLine(((int)this.UniquePassive).ToString());
			// Role
			output.WriteLine(((int)this.Role).ToString());
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
			// BaseHPRegen
			output.WriteLine(((float)this.BaseHPRegen).ToString());
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
			// IsDamageImmune
			output.WriteLine(this.IsDamageImmune ? 1 : 0);
			// IsControlImmune
			output.WriteLine(this.IsControlImmune ? 1 : 0);
			// IsBlind
			output.WriteLine(this.IsBlind ? 1 : 0);
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
