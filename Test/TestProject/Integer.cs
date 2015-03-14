using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Default.Namespace
{

	public class Integer
	{

		public int Value;	
		public Integer()
		{
		}	
		public Integer(int value)
		{
			Value = value;
		}	
	public static Integer Deserialize(InputStream input) {
		Integer _obj =  new Integer();
		int _obj.Value = Int32.Parse(input.ReadLine());
		return _obj
	}

	}
}
