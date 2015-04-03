using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views.Client
{

	public class VisionMapView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// Représente la vision qu'ont les 2 équipes sur l'ensemble de la map.
		/// </summary>
		public List<List<VisionFlags>> Vision;	
		public static VisionMapView Deserialize(System.IO.StreamReader input) {
			VisionMapView _obj =  new VisionMapView();
			// Vision
			List<List<VisionFlags>> _obj_Vision = new List<List<VisionFlags>>();
			int _obj_Vision_count = Int32.Parse(input.ReadLine());
			for(int _obj_Vision_i = 0; _obj_Vision_i < _obj_Vision_count; _obj_Vision_i++) {
				List<VisionFlags> _obj_Vision_e = new List<VisionFlags>();
				int _obj_Vision_e_count = Int32.Parse(input.ReadLine());
				for(int _obj_Vision_e_i = 0; _obj_Vision_e_i < _obj_Vision_e_count; _obj_Vision_e_i++) {
					VisionFlags _obj_Vision_e_e = (VisionFlags)Int32.Parse(input.ReadLine());
					_obj_Vision_e.Add((VisionFlags)_obj_Vision_e_e);
				}
				_obj_Vision.Add((List<VisionFlags>)_obj_Vision_e);
			}
			_obj.Vision = (List<List<VisionFlags>>)_obj_Vision;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Vision
			output.WriteLine(this.Vision.Count.ToString());
			for(int Vision_it = 0; Vision_it < this.Vision.Count;Vision_it++) {
				output.WriteLine(this.Vision[Vision_it].Count.ToString());
				for(int VisionVision_it_it = 0; VisionVision_it_it < this.Vision[Vision_it].Count;VisionVision_it_it++) {
					output.WriteLine(((int)this.Vision[Vision_it][VisionVision_it_it]).ToString());
				}
			}
		}

	}
}
