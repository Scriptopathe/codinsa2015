public class TestMePls
{

	public Oktamer test;
	public static TestMePls deserialize(JSONObject o)
	{
		TestMePls obj = new TestMePls();
		// test
		Oktamer test = o.getInt("test");
		obj.test = test;
		return obj;
	}

}