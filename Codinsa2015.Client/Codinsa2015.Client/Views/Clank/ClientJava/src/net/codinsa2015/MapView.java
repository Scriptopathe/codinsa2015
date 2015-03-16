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


@SuppressWarnings("unused")
public class MapView
{


	public ArrayList<ArrayList<Boolean>> Passability;
	public static MapView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		MapView _obj =  new MapView();
		// Passability
		ArrayList<ArrayList<Boolean>> _obj_Passability = new ArrayList<ArrayList<Boolean>>();
		int _obj_Passability_count = Integer.valueOf(input.readLine());
		for(int _obj_Passability_i = 0; _obj_Passability_i < _obj_Passability_count; _obj_Passability_i++) {
			ArrayList<Boolean> _obj_Passability_e = new ArrayList<Boolean>();
			int _obj_Passability_e_count = Integer.valueOf(input.readLine());
			for(int _obj_Passability_e_i = 0; _obj_Passability_e_i < _obj_Passability_e_count; _obj_Passability_e_i++) {
				boolean _obj_Passability_e_e = Integer.valueOf(input.readLine()) == 0 ? false : true;
				_obj_Passability_e.add((Boolean)_obj_Passability_e_e);
			}
			_obj_Passability.add((ArrayList<Boolean>)_obj_Passability_e);
		}
		_obj.Passability = _obj_Passability;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Passability
		output.append(String.valueOf(this.Passability.size()) + "\n");
		for(int Passability_it = 0; Passability_it < this.Passability.size();Passability_it++) {
			output.append(String.valueOf(this.Passability.get(Passability_it).size()) + "\n");
			for(int PassabilityPassability_it_it = 0; PassabilityPassability_it_it < this.Passability.get(Passability_it).size();PassabilityPassability_it_it++) {
				output.append((this.Passability.get(Passability_it).get(PassabilityPassability_it_it) ? 1 : 0) + "\n");
			}
		}
	}

}
