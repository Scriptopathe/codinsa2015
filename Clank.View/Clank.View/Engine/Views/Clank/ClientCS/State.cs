using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Clank.View.Engine.Views
{

	class State
	{

		public List<EntityView> getEntitiesOnSight()
		{
			// Send
			List<object> args = new List<object>() { };
			int funcId = 0;
			List<object> obj = new List<object>() { funcId, args };
			TCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
			// Receive
			string str = TCPHelper.Receive();
			Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
			return (List<EntityView>)o[0].ToObject(typeof(List<EntityView>));
		}
	
		public List<EntityView> lolol(int id,string bla,List<EntityView> carotte)
		{
			// Send
			List<object> args = new List<object>() { id,bla,carotte};
			int funcId = 1;
			List<object> obj = new List<object>() { funcId, args };
			TCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
			// Receive
			string str = TCPHelper.Receive();
			Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
			return (List<EntityView>)o[0].ToObject(typeof(List<EntityView>));
		}
	
	}
}
