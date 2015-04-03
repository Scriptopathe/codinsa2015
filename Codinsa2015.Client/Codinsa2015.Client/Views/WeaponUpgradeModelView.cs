using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views.Client
{

	public class WeaponUpgradeModelView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// Obtient la description de l'upgrade
		/// </summary>
		public SpellDescriptionView Description;	
		/// <summary>
		/// 
		/// </summary>
		public List<StateAlterationModelView> PassiveAlterations;	
		/// <summary>
		/// Obtient le coût de l'upgrade.
		/// </summary>
		public float Cost;	
		public static WeaponUpgradeModelView Deserialize(System.IO.StreamReader input) {
			WeaponUpgradeModelView _obj =  new WeaponUpgradeModelView();
			// Description
			SpellDescriptionView _obj_Description = SpellDescriptionView.Deserialize(input);
			_obj.Description = (SpellDescriptionView)_obj_Description;
			// PassiveAlterations
			List<StateAlterationModelView> _obj_PassiveAlterations = new List<StateAlterationModelView>();
			int _obj_PassiveAlterations_count = Int32.Parse(input.ReadLine());
			for(int _obj_PassiveAlterations_i = 0; _obj_PassiveAlterations_i < _obj_PassiveAlterations_count; _obj_PassiveAlterations_i++) {
				StateAlterationModelView _obj_PassiveAlterations_e = StateAlterationModelView.Deserialize(input);
				_obj_PassiveAlterations.Add((StateAlterationModelView)_obj_PassiveAlterations_e);
			}
			_obj.PassiveAlterations = (List<StateAlterationModelView>)_obj_PassiveAlterations;
			// Cost
			float _obj_Cost = Single.Parse(input.ReadLine());
			_obj.Cost = (float)_obj_Cost;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Description
			this.Description.Serialize(output);
			// PassiveAlterations
			output.WriteLine(this.PassiveAlterations.Count.ToString());
			for(int PassiveAlterations_it = 0; PassiveAlterations_it < this.PassiveAlterations.Count;PassiveAlterations_it++) {
				this.PassiveAlterations[PassiveAlterations_it].Serialize(output);
			}
			// Cost
			output.WriteLine(((float)this.Cost).ToString());
		}

	}
}
