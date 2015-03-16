import java.lang.*;

	public class SpellDescriptionView
	{

	
		public float BaseCooldown;	
		public float CastingTime;	
		public ArrayList<StateAlterationModelView> CastingTimeAlterations;	
		public SpellTargetInfoView TargetType;	
		public ArrayList<StateAlterationModelView> OnHitEffects;	
		public static SpellDescriptionView Deserialize(BufferedReader input) {
		try {
			SpellDescriptionView _obj =  new SpellDescriptionView();
			// BaseCooldown
			float _obj_BaseCooldown = Float.Parse(input.readLine());
			_obj.BaseCooldown = _obj_BaseCooldown;
			// CastingTime
			float _obj_CastingTime = Float.Parse(input.readLine());
			_obj.CastingTime = _obj_CastingTime;
			// CastingTimeAlterations
			ArrayList<StateAlterationModelView> _obj_CastingTimeAlterations = new ArrayList<StateAlterationModelView>();
			int _obj_CastingTimeAlterations_count = Integer.Parse(input.readLine());
			for(int _obj_CastingTimeAlterations_i = 0; _obj_CastingTimeAlterations_i < _obj_CastingTimeAlterations_count; _obj_CastingTimeAlterations_i++) {
				StateAlterationModelView _obj_CastingTimeAlterations_e = StateAlterationModelView.deserialize(input);
				_obj_CastingTimeAlterations.add((StateAlterationModelView)_obj_CastingTimeAlterations_e);
			}
			_obj.CastingTimeAlterations = _obj_CastingTimeAlterations;
			// TargetType
			SpellTargetInfoView _obj_TargetType = SpellTargetInfoView.deserialize(input);
			_obj.TargetType = _obj_TargetType;
			// OnHitEffects
			ArrayList<StateAlterationModelView> _obj_OnHitEffects = new ArrayList<StateAlterationModelView>();
			int _obj_OnHitEffects_count = Integer.Parse(input.readLine());
			for(int _obj_OnHitEffects_i = 0; _obj_OnHitEffects_i < _obj_OnHitEffects_count; _obj_OnHitEffects_i++) {
				StateAlterationModelView _obj_OnHitEffects_e = StateAlterationModelView.deserialize(input);
				_obj_OnHitEffects.add((StateAlterationModelView)_obj_OnHitEffects_e);
			}
			_obj.OnHitEffects = _obj_OnHitEffects;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// BaseCooldown
			output.WriteLine(((Float)this.BaseCooldown).toString() + "\n");
			// CastingTime
			output.WriteLine(((Float)this.CastingTime).toString() + "\n");
			// CastingTimeAlterations
			output.append(this.CastingTimeAlterations.size().toString() + "\n");
			for(int CastingTimeAlterations_it = 0; CastingTimeAlterations_it < this.CastingTimeAlterations.size();CastingTimeAlterations_it++) {
				this.CastingTimeAlterations[CastingTimeAlterations_it].serialize(output);
			}
			// TargetType
			this.TargetType.serialize(output);
			// OnHitEffects
			output.append(this.OnHitEffects.size().toString() + "\n");
			for(int OnHitEffects_it = 0; OnHitEffects_it < this.OnHitEffects.size();OnHitEffects_it++) {
				this.OnHitEffects[OnHitEffects_it].serialize(output);
			}
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
