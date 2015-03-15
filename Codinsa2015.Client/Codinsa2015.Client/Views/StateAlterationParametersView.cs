using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class StateAlterationParametersView
	{

static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		public Vector2 DashTargetDirection;	
		public int DashTargetEntity;	
		public static StateAlterationParametersView Deserialize(System.IO.StreamReader input) {
			StateAlterationParametersView _obj =  new StateAlterationParametersView();
			// DashTargetDirection
			Vector2 _obj_DashTargetDirection = Vector2.Deserialize(input);
			_obj.DashTargetDirection = (Vector2)_obj_DashTargetDirection;
			// DashTargetEntity
			int _obj_DashTargetEntity = Int32.Parse(input.ReadLine());
			_obj.DashTargetEntity = (int)_obj_DashTargetEntity;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// DashTargetDirection
			this.DashTargetDirection.Serialize(output);
			// DashTargetEntity
			output.WriteLine(((int)this.DashTargetEntity).ToString());
		}

	}
}
