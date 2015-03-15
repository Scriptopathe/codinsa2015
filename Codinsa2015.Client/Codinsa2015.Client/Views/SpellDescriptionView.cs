using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class SpellDescriptionView
	{

static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		public float BaseCooldown;	
		public float CastingTime;	
		public List<StateAlterationModelView> CastingTimeAlterations;	
		public SpellTargetInfoView TargetType;	
		public List<StateAlterationModelView> OnHitEffects;	
		public static SpellDescriptionView Deserialize(System.IO.StreamReader input) {
			SpellDescriptionView _obj =  new SpellDescriptionView();
			// BaseCooldown
			float _obj_BaseCooldown = Single.Parse(input.ReadLine());
			_obj.BaseCooldown = (float)_obj_BaseCooldown;
			// CastingTime
			float _obj_CastingTime = Single.Parse(input.ReadLine());
			_obj.CastingTime = (float)_obj_CastingTime;
			// CastingTimeAlterations
			List<StateAlterationModelView> _obj_CastingTimeAlterations = new List<StateAlterationModelView>();
			int _obj_CastingTimeAlterations_count = Int32.Parse(input.ReadLine());
			for(int _obj_CastingTimeAlterations_i = 0; _obj_CastingTimeAlterations_i < _obj_CastingTimeAlterations_count; _obj_CastingTimeAlterations_i++) {
				StateAlterationModelView _obj_CastingTimeAlterations_e = StateAlterationModelView.Deserialize(input);
				_obj_CastingTimeAlterations.Add((StateAlterationModelView)_obj_CastingTimeAlterations_e);
			}
			_obj.CastingTimeAlterations = (List<StateAlterationModelView>)_obj_CastingTimeAlterations;
			// TargetType
			SpellTargetInfoView _obj_TargetType = SpellTargetInfoView.Deserialize(input);
			_obj.TargetType = (SpellTargetInfoView)_obj_TargetType;
			// OnHitEffects
			List<StateAlterationModelView> _obj_OnHitEffects = new List<StateAlterationModelView>();
			int _obj_OnHitEffects_count = Int32.Parse(input.ReadLine());
			for(int _obj_OnHitEffects_i = 0; _obj_OnHitEffects_i < _obj_OnHitEffects_count; _obj_OnHitEffects_i++) {
				StateAlterationModelView _obj_OnHitEffects_e = StateAlterationModelView.Deserialize(input);
				_obj_OnHitEffects.Add((StateAlterationModelView)_obj_OnHitEffects_e);
			}
			_obj.OnHitEffects = (List<StateAlterationModelView>)_obj_OnHitEffects;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// BaseCooldown
			output.WriteLine(((float)this.BaseCooldown).ToString());
			// CastingTime
			output.WriteLine(((float)this.CastingTime).ToString());
			// CastingTimeAlterations
			output.WriteLine(this.CastingTimeAlterations.Count.ToString());
			for(int CastingTimeAlterations_it = 0; CastingTimeAlterations_it < this.CastingTimeAlterations.Count;CastingTimeAlterations_it++) {
				this.CastingTimeAlterations[CastingTimeAlterations_it].Serialize(output);
			}
			// TargetType
			this.TargetType.Serialize(output);
			// OnHitEffects
			output.WriteLine(this.OnHitEffects.Count.ToString());
			for(int OnHitEffects_it = 0; OnHitEffects_it < this.OnHitEffects.Count;OnHitEffects_it++) {
				this.OnHitEffects[OnHitEffects_it].Serialize(output);
			}
		}

	}
}
