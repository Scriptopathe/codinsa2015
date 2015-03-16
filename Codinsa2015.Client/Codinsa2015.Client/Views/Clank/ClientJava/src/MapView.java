import java.lang.*;

	public class MapView
	{

	
		public ArrayList<ArrayList<bool>> Passability;	
		public static MapView Deserialize(BufferedReader input) {
		try {
			MapView _obj =  new MapView();
			// Passability
			ArrayList<ArrayList<bool>> _obj_Passability = new ArrayList<ArrayList<bool>>();
			int _obj_Passability_count = Integer.Parse(input.readLine());
			for(int _obj_Passability_i = 0; _obj_Passability_i < _obj_Passability_count; _obj_Passability_i++) {
				ArrayList<bool> _obj_Passability_e = new ArrayList<bool>();
				int _obj_Passability_e_count = Integer.Parse(input.readLine());
				for(int _obj_Passability_e_i = 0; _obj_Passability_e_i < _obj_Passability_e_count; _obj_Passability_e_i++) {
					boolean _obj_Passability_e_e = Integer.valueof(input.readLine()) == 0 ? false : true;
					_obj_Passability_e.add((bool)_obj_Passability_e_e);
				}
				_obj_Passability.add((ArrayList<bool>)_obj_Passability_e);
			}
			_obj.Passability = _obj_Passability;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// Passability
			output.append(this.Passability.size().toString() + "\n");
			for(int Passability_it = 0; Passability_it < this.Passability.size();Passability_it++) {
				output.append(this.Passability[Passability_it].size().toString() + "\n");
				for(int PassabilityPassability_it_it = 0; PassabilityPassability_it_it < this.Passability[Passability_it].size();PassabilityPassability_it_it++) {
					output.append((this.Passability[Passability_it][PassabilityPassability_it_it] ? 1 : 0) + "\n");
				}
			}
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
