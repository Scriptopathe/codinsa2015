public class VisionMapView
{


	public ArrayList<ArrayList<VisionFlags>> Vision;
	public static VisionMapView deserialize(JSONObject o)
	{
		VisionMapView obj = new VisionMapView();
		// Vision
		ArrayList<ArrayList<VisionFlags>> Vision = new ArrayList<ArrayList<VisionFlags>>();
		JSONArray Vision_json = o.getJSONArray("Vision");
		for(int Vision_it = 0;Vision_it < o.length(); Vision_it++)
		{
			ArrayList<VisionFlags> Vision_item = new ArrayList<VisionFlags>();
			JSONArray Vision_item_json = Vision_json.getJSONArray("Vision_it");
			for(int Vision_item_it = 0;Vision_item_it < Vision_json.length(); Vision_item_it++)
			{
				VisionFlags Vision_item_item = Vision_item_json.getInt("Vision_item_it");
				Vision_item.add(Vision_item_item);
			}
			Vision.add(Vision_item);
		}
		obj.Vision = Vision;
		return obj;
	}

}