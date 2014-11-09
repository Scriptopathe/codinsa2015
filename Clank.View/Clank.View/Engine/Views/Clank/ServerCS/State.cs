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

		public List<EntityView> getEntitiesOnSight(int clientId)
		{
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
			}
			return "";
		}
	
	}
}
