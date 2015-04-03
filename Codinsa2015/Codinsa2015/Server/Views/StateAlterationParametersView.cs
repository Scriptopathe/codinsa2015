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
	
		/// <summary>
		/// Direction dans laquelle de dash doit aller. (si le targetting est Direction)
		/// </summary>
		public Vector2 DashTargetDirection;	
		/// <summary>
		/// Entité vers laquelle le dash doit se diriger (si le targetting du dash est TowardsEntity).
		/// </summary>
		public int DashTargetEntity;	
		/// <summary>
		/// Position finale du du dash (si targetting TowardsPosition)
		/// </summary>
		public Vector2 DashTargetPosition;	
		public StateAlterationParametersView() {
			DashTargetDirection = new Vector2();
			DashTargetPosition = new Vector2();
		}

		public static StateAlterationParametersView Deserialize(System.IO.StreamReader input) {
			StateAlterationParametersView _obj =  new StateAlterationParametersView();
			// DashTargetDirection
			Vector2 _obj_DashTargetDirection = Vector2.Deserialize(input);
			_obj.DashTargetDirection = (Vector2)_obj_DashTargetDirection;
			// DashTargetEntity
			int _obj_DashTargetEntity = Int32.Parse(input.ReadLine());
			_obj.DashTargetEntity = (int)_obj_DashTargetEntity;
			// DashTargetPosition
			Vector2 _obj_DashTargetPosition = Vector2.Deserialize(input);
			_obj.DashTargetPosition = (Vector2)_obj_DashTargetPosition;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// DashTargetDirection
			this.DashTargetDirection.Serialize(output);
			// DashTargetEntity
			output.WriteLine(((int)this.DashTargetEntity).ToString());
			// DashTargetPosition
			this.DashTargetPosition.Serialize(output);
		}

	}
}
