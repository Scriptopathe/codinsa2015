using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Default.Namespace
{

	public class NonSerializableClass<T>
	{

		WorkIfTSerializable<T> var;	
	public static NonSerializableClass Deserialize(InputStream input) {
		NonSerializableClass _obj =  new NonSerializableClass();
		WorkIfTSerializable<T> _obj.var = WorkIfTSerializable<T>.Deserialize(input)
		return _obj
	}

	}
}
