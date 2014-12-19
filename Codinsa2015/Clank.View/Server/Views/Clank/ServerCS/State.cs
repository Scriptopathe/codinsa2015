using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Server.Views
{

	class State
	{

		public List<EntityView> getEntitiesOnSight(int clientId)
		{
            return new List<EntityView>();
		}	
		public List<EntityView> lolol(int id, string bla, List<EntityView> carotte, int clientId)
		{
			return carotte;
		}	
		/// <summary>
		/// Génère le code pour la fonction de traitement des messages.
		/// </summary>
		public string ProcessRequest(string request, int clientId)
		{
			Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(request);
			int functionId = o.Value<int>(0);
			switch(functionId)
			{
				case 0:
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { getEntitiesOnSight(clientId) });
				case 1:
					int arg1_0 = o[1].Value<int>(0);
					string arg1_1 = o[1].Value<string>(1);
					List<EntityView> arg1_2 = (List<EntityView>)o[1][2].ToObject(typeof(List<EntityView>));
					return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { lolol(arg1_0, arg1_1, arg1_2, clientId) });
			}
			return "";
		}
	
	}
}
