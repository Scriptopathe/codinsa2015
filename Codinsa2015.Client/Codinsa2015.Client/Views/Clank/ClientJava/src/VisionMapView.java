import java.lang.*;

	public class VisionMapView
	{

	
		public ArrayList<ArrayList<VisionFlags>> Vision;	
		public static VisionMapView Deserialize(BufferedReader input) {
		try {
			VisionMapView _obj =  new VisionMapView();
			// Vision
			ArrayList<ArrayList<VisionFlags>> _obj_Vision = new ArrayList<ArrayList<VisionFlags>>();
			int _obj_Vision_count = Integer.Parse(input.readLine());
			for(int _obj_Vision_i = 0; _obj_Vision_i < _obj_Vision_count; _obj_Vision_i++) {
				ArrayList<VisionFlags> _obj_Vision_e = new ArrayList<VisionFlags>();
				int _obj_Vision_e_count = Integer.Parse(input.readLine());
				for(int _obj_Vision_e_i = 0; _obj_Vision_e_i < _obj_Vision_e_count; _obj_Vision_e_i++) {
					int _obj_Vision_e_e = Integer.Parse(input.readLine());
					_obj_Vision_e.add((VisionFlags)_obj_Vision_e_e);
				}
				_obj_Vision.add((ArrayList<VisionFlags>)_obj_Vision_e);
			}
			_obj.Vision = _obj_Vision;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// Vision
			output.append(this.Vision.size().toString() + "\n");
			for(int Vision_it = 0; Vision_it < this.Vision.size();Vision_it++) {
				output.append(this.Vision[Vision_it].size().toString() + "\n");
				for(int VisionVision_it_it = 0; VisionVision_it_it < this.Vision[Vision_it].size();VisionVision_it_it++) {
					output.append(((Integer)(this.Vision[Vision_it][VisionVision_it_it].getValue())).toString() + "\n");
				}
			}
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
