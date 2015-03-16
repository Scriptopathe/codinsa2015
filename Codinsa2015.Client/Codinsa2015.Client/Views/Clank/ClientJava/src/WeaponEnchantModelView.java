import java.lang.*;

	public class WeaponEnchantModelView
	{

	
		public ArrayList<StateAlterationModelView> OnHitEffects;	
		public ArrayList<StateAlterationModelView> CastingEffects;	
		public ArrayList<StateAlterationModelView> PassiveEffects;	
		public static WeaponEnchantModelView Deserialize(BufferedReader input) {
		try {
			WeaponEnchantModelView _obj =  new WeaponEnchantModelView();
			// OnHitEffects
			ArrayList<StateAlterationModelView> _obj_OnHitEffects = new ArrayList<StateAlterationModelView>();
			int _obj_OnHitEffects_count = Integer.Parse(input.readLine());
			for(int _obj_OnHitEffects_i = 0; _obj_OnHitEffects_i < _obj_OnHitEffects_count; _obj_OnHitEffects_i++) {
				StateAlterationModelView _obj_OnHitEffects_e = StateAlterationModelView.deserialize(input);
				_obj_OnHitEffects.add((StateAlterationModelView)_obj_OnHitEffects_e);
			}
			_obj.OnHitEffects = _obj_OnHitEffects;
			// CastingEffects
			ArrayList<StateAlterationModelView> _obj_CastingEffects = new ArrayList<StateAlterationModelView>();
			int _obj_CastingEffects_count = Integer.Parse(input.readLine());
			for(int _obj_CastingEffects_i = 0; _obj_CastingEffects_i < _obj_CastingEffects_count; _obj_CastingEffects_i++) {
				StateAlterationModelView _obj_CastingEffects_e = StateAlterationModelView.deserialize(input);
				_obj_CastingEffects.add((StateAlterationModelView)_obj_CastingEffects_e);
			}
			_obj.CastingEffects = _obj_CastingEffects;
			// PassiveEffects
			ArrayList<StateAlterationModelView> _obj_PassiveEffects = new ArrayList<StateAlterationModelView>();
			int _obj_PassiveEffects_count = Integer.Parse(input.readLine());
			for(int _obj_PassiveEffects_i = 0; _obj_PassiveEffects_i < _obj_PassiveEffects_count; _obj_PassiveEffects_i++) {
				StateAlterationModelView _obj_PassiveEffects_e = StateAlterationModelView.deserialize(input);
				_obj_PassiveEffects.add((StateAlterationModelView)_obj_PassiveEffects_e);
			}
			_obj.PassiveEffects = _obj_PassiveEffects;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// OnHitEffects
			output.append(this.OnHitEffects.size().toString() + "\n");
			for(int OnHitEffects_it = 0; OnHitEffects_it < this.OnHitEffects.size();OnHitEffects_it++) {
				this.OnHitEffects[OnHitEffects_it].serialize(output);
			}
			// CastingEffects
			output.append(this.CastingEffects.size().toString() + "\n");
			for(int CastingEffects_it = 0; CastingEffects_it < this.CastingEffects.size();CastingEffects_it++) {
				this.CastingEffects[CastingEffects_it].serialize(output);
			}
			// PassiveEffects
			output.append(this.PassiveEffects.size().toString() + "\n");
			for(int PassiveEffects_it = 0; PassiveEffects_it < this.PassiveEffects.size();PassiveEffects_it++) {
				this.PassiveEffects[PassiveEffects_it].serialize(output);
			}
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
