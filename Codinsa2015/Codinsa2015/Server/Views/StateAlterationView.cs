using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class StateAlterationView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// Id de la source de l'altération d'état.
		/// </summary>
		public int Source;	
		/// <summary>
		/// Représente le type de source de l'altération d'état.
		/// </summary>
		public StateAlterationSource SourceType;	
		/// <summary>
		/// Représente le modèle d'altération d'état appliquée sur une entité.
		/// </summary>
		public StateAlterationModelView Model;	
		/// <summary>
		/// Représente les paramètres de l'altération d'état.
		/// </summary>
		public StateAlterationParametersView Parameters;	
		/// <summary>
		/// Temps restant en secondes pour l'altération d'état.
		/// </summary>
		public float RemainingTime;	
		public static StateAlterationView Deserialize(System.IO.StreamReader input) {
			StateAlterationView _obj =  new StateAlterationView();
			// Source
			int _obj_Source = Int32.Parse(input.ReadLine());
			_obj.Source = (int)_obj_Source;
			// SourceType
			StateAlterationSource _obj_SourceType = (StateAlterationSource)Int32.Parse(input.ReadLine());
			_obj.SourceType = (StateAlterationSource)_obj_SourceType;
			// Model
			StateAlterationModelView _obj_Model = StateAlterationModelView.Deserialize(input);
			_obj.Model = (StateAlterationModelView)_obj_Model;
			// Parameters
			StateAlterationParametersView _obj_Parameters = StateAlterationParametersView.Deserialize(input);
			_obj.Parameters = (StateAlterationParametersView)_obj_Parameters;
			// RemainingTime
			float _obj_RemainingTime = Single.Parse(input.ReadLine());
			_obj.RemainingTime = (float)_obj_RemainingTime;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Source
			output.WriteLine(((int)this.Source).ToString());
			// SourceType
			output.WriteLine(((int)this.SourceType).ToString());
			// Model
			this.Model.Serialize(output);
			// Parameters
			this.Parameters.Serialize(output);
			// RemainingTime
			output.WriteLine(((float)this.RemainingTime).ToString());
		}

	}
}
