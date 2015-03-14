using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Default.Namespace
{

	public class Double
	{

		public float Value;	
		public Double()
		{
		}	
		public Double(float value)
		{
			Value = value;
		}	
	public static Double Deserialize(InputStream input) {
		Double _obj =  new Double();
		float _obj.Value = Single.Parse(input.ReadLine());
		return _obj
	}

	}
}
