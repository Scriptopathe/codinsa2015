using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class WeaponEnchantModelView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// ID unique de l'enchantement.
		/// </summary>
		public int ID;	
		/// <summary>
		/// Obtient les altértions d'état appliquées à l'impact de l'attaque sur la cible.
		/// </summary>
		public List<StateAlterationModelView> OnHitEffects;	
		/// <summary>
		/// Obtient les altérations d'état appliquées lors de l'attaque sur le caster.
		/// </summary>
		public List<StateAlterationModelView> CastingEffects;	
		/// <summary>
		/// Obtient les effets passifs appliqués par l'enchantement.
		/// </summary>
		public List<StateAlterationModelView> PassiveEffects;	
		public WeaponEnchantModelView() {
			OnHitEffects = new List<StateAlterationModelView>();
			CastingEffects = new List<StateAlterationModelView>();
			PassiveEffects = new List<StateAlterationModelView>();
		}

		public static WeaponEnchantModelView Deserialize(System.IO.StreamReader input) {
			WeaponEnchantModelView _obj =  new WeaponEnchantModelView();
			// ID
			int _obj_ID = Int32.Parse(input.ReadLine());
			_obj.ID = (int)_obj_ID;
			// OnHitEffects
			List<StateAlterationModelView> _obj_OnHitEffects = new List<StateAlterationModelView>();
			int _obj_OnHitEffects_count = Int32.Parse(input.ReadLine());
			for(int _obj_OnHitEffects_i = 0; _obj_OnHitEffects_i < _obj_OnHitEffects_count; _obj_OnHitEffects_i++) {
				StateAlterationModelView _obj_OnHitEffects_e = StateAlterationModelView.Deserialize(input);
				_obj_OnHitEffects.Add((StateAlterationModelView)_obj_OnHitEffects_e);
			}
			_obj.OnHitEffects = (List<StateAlterationModelView>)_obj_OnHitEffects;
			// CastingEffects
			List<StateAlterationModelView> _obj_CastingEffects = new List<StateAlterationModelView>();
			int _obj_CastingEffects_count = Int32.Parse(input.ReadLine());
			for(int _obj_CastingEffects_i = 0; _obj_CastingEffects_i < _obj_CastingEffects_count; _obj_CastingEffects_i++) {
				StateAlterationModelView _obj_CastingEffects_e = StateAlterationModelView.Deserialize(input);
				_obj_CastingEffects.Add((StateAlterationModelView)_obj_CastingEffects_e);
			}
			_obj.CastingEffects = (List<StateAlterationModelView>)_obj_CastingEffects;
			// PassiveEffects
			List<StateAlterationModelView> _obj_PassiveEffects = new List<StateAlterationModelView>();
			int _obj_PassiveEffects_count = Int32.Parse(input.ReadLine());
			for(int _obj_PassiveEffects_i = 0; _obj_PassiveEffects_i < _obj_PassiveEffects_count; _obj_PassiveEffects_i++) {
				StateAlterationModelView _obj_PassiveEffects_e = StateAlterationModelView.Deserialize(input);
				_obj_PassiveEffects.Add((StateAlterationModelView)_obj_PassiveEffects_e);
			}
			_obj.PassiveEffects = (List<StateAlterationModelView>)_obj_PassiveEffects;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// ID
			output.WriteLine(((int)this.ID).ToString());
			// OnHitEffects
			output.WriteLine(this.OnHitEffects.Count.ToString());
			for(int OnHitEffects_it = 0; OnHitEffects_it < this.OnHitEffects.Count;OnHitEffects_it++) {
				this.OnHitEffects[OnHitEffects_it].Serialize(output);
			}
			// CastingEffects
			output.WriteLine(this.CastingEffects.Count.ToString());
			for(int CastingEffects_it = 0; CastingEffects_it < this.CastingEffects.Count;CastingEffects_it++) {
				this.CastingEffects[CastingEffects_it].Serialize(output);
			}
			// PassiveEffects
			output.WriteLine(this.PassiveEffects.Count.ToString());
			for(int PassiveEffects_it = 0; PassiveEffects_it < this.PassiveEffects.Count;PassiveEffects_it++) {
				this.PassiveEffects[PassiveEffects_it].Serialize(output);
			}
		}

	}
}
