using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Default.Namespace
{

	public class String
	{

		public string Value;	
		public String()
		{
		}	
		public String(string value)
		{
			Value = value;
		}	
	public static String Deserialize(InputStream input) {
		String _obj =  new String();
		string _obj.Value = input.Readline()
		return _obj
	}

	}
}
