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
import net.codinsa2015.VisionFlags.*;


@SuppressWarnings("unused")
public class VisionMapView
{


	// Représente la vision qu'ont les 2 équipes sur l'ensemble de la map.
	public ArrayList<ArrayList<VisionFlags>> Vision;
	public static VisionMapView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		VisionMapView _obj =  new VisionMapView();
		// Vision
		ArrayList<ArrayList<VisionFlags>> _obj_Vision = new ArrayList<ArrayList<VisionFlags>>();
		int _obj_Vision_count = Integer.valueOf(input.readLine());
		for(int _obj_Vision_i = 0; _obj_Vision_i < _obj_Vision_count; _obj_Vision_i++) {
			ArrayList<VisionFlags> _obj_Vision_e = new ArrayList<VisionFlags>();
			int _obj_Vision_e_count = Integer.valueOf(input.readLine());
			for(int _obj_Vision_e_i = 0; _obj_Vision_e_i < _obj_Vision_e_count; _obj_Vision_e_i++) {
				VisionFlags _obj_Vision_e_e = VisionFlags.fromValue(Integer.valueOf(input.readLine()));
				_obj_Vision_e.add(VisionFlags.fromValue(_obj_Vision_e_e));
			}
			_obj_Vision.add((ArrayList<VisionFlags>)_obj_Vision_e);
		}
		_obj.Vision = _obj_Vision;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Vision
		output.append(String.valueOf(this.Vision.size()) + "\n");
		for(int Vision_it = 0; Vision_it < this.Vision.size();Vision_it++) {
			output.append(String.valueOf(this.Vision.get(Vision_it).size()) + "\n");
			for(int VisionVision_it_it = 0; VisionVision_it_it < this.Vision.get(Vision_it).size();VisionVision_it_it++) {
				output.append(((Integer)(this.Vision.get(Vision_it).get(VisionVision_it_it).getValue())).toString() + "\n");
			}
		}
	}

}
