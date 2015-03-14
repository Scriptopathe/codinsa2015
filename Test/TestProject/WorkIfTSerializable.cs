using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Default.Namespace
{

	public class WorkIfTSerializable<T>
	{

		T var;	
	public static WorkIfTSerializable Deserialize(InputStream input) {
		WorkIfTSerializable _obj =  new WorkIfTSerializable();
		T _obj.var = T.Deserialize(input)
		return _obj
	}

	}
}
