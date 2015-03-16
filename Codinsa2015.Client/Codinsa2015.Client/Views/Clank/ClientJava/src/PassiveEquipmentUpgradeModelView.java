import java.lang.*;

	public class PassiveEquipmentUpgradeModelView
	{

	
		public ArrayList<StateAlterationModelView> PassiveAlterations;	
		public float Cost;	
		public static PassiveEquipmentUpgradeModelView Deserialize(BufferedReader input) {
		try {
			PassiveEquipmentUpgradeModelView _obj =  new PassiveEquipmentUpgradeModelView();
			// PassiveAlterations
			ArrayList<StateAlterationModelView> _obj_PassiveAlterations = new ArrayList<StateAlterationModelView>();
			int _obj_PassiveAlterations_count = Integer.Parse(input.readLine());
			for(int _obj_PassiveAlterations_i = 0; _obj_PassiveAlterations_i < _obj_PassiveAlterations_count; _obj_PassiveAlterations_i++) {
				StateAlterationModelView _obj_PassiveAlterations_e = StateAlterationModelView.deserialize(input);
				_obj_PassiveAlterations.add((StateAlterationModelView)_obj_PassiveAlterations_e);
			}
			_obj.PassiveAlterations = _obj_PassiveAlterations;
			// Cost
			float _obj_Cost = Float.Parse(input.readLine());
			_obj.Cost = _obj_Cost;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// PassiveAlterations
			output.append(this.PassiveAlterations.size().toString() + "\n");
			for(int PassiveAlterations_it = 0; PassiveAlterations_it < this.PassiveAlterations.size();PassiveAlterations_it++) {
				this.PassiveAlterations[PassiveAlterations_it].serialize(output);
			}
			// Cost
			output.WriteLine(((Float)this.Cost).toString() + "\n");
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
