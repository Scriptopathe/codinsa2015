using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class MapView
	{

	
		public List<List<bool>> Passability;	
		public static MapView Deserialize(System.IO.StreamReader input) {
			MapView _obj =  new MapView();
			// Passability
			List<List<bool>> _obj_Passability = new List<List<bool>>();
			int _obj_Passability_count = Int32.Parse(input.ReadLine());
			for(int _obj_Passability_i = 0; _obj_Passability_i < _obj_Passability_count; _obj_Passability_i++) {
				List<bool> _obj_Passability_e = new List<bool>();
				int _obj_Passability_e_count = Int32.Parse(input.ReadLine());
				for(int _obj_Passability_e_i = 0; _obj_Passability_e_i < _obj_Passability_e_count; _obj_Passability_e_i++) {
					bool _obj_Passability_e_e = Int32.Parse(input.ReadLine()) == 0 ? false : true;
					_obj_Passability_e.Add((bool)_obj_Passability_e_e);
				}
				_obj_Passability.Add((List<bool>)_obj_Passability_e);
			}
			_obj.Passability = (List<List<bool>>)_obj_Passability;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Passability
			output.WriteLine(this.Passability.Count.ToString());
			for(int Passability_it = 0; Passability_it < this.Passability.Count;Passability_it++) {
				output.WriteLine(this.Passability[Passability_it].Count.ToString());
				for(int PassabilityPassability_it_it = 0; PassabilityPassability_it_it < this.Passability[Passability_it].Count;PassabilityPassability_it_it++) {
					output.WriteLine(this.Passability[Passability_it][PassabilityPassability_it_it] ? 1 : 0);
				}
			}
		}

	}
}
