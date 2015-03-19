using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class SpellTargetInfoView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		// Type de ciblage du sort.
		public TargettingType Type;	
		// Range du sort en unités métriques.
		public float Range;	
		// Durée en secondes que met le sort à atteindre la position donnée.
		public float Duration;	
		// Rayon du sort. (non utilisé pour les sort targetted)
		public float AoeRadius;	
		// Obtient une valeur indiquant si le sort est détruit lors d'une collision avec une entité
		public bool DieOnCollision;	
		// Retourne le type de cibles pouvant être touchées par ce sort.
		public EntityTypeRelative AllowedTargetTypes;	
		public static SpellTargetInfoView Deserialize(System.IO.StreamReader input) {
			SpellTargetInfoView _obj =  new SpellTargetInfoView();
			// Type
			int _obj_Type = Int32.Parse(input.ReadLine());
			_obj.Type = (TargettingType)_obj_Type;
			// Range
			float _obj_Range = Single.Parse(input.ReadLine());
			_obj.Range = (float)_obj_Range;
			// Duration
			float _obj_Duration = Single.Parse(input.ReadLine());
			_obj.Duration = (float)_obj_Duration;
			// AoeRadius
			float _obj_AoeRadius = Single.Parse(input.ReadLine());
			_obj.AoeRadius = (float)_obj_AoeRadius;
			// DieOnCollision
			bool _obj_DieOnCollision = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			_obj.DieOnCollision = (bool)_obj_DieOnCollision;
			// AllowedTargetTypes
			int _obj_AllowedTargetTypes = Int32.Parse(input.ReadLine());
			_obj.AllowedTargetTypes = (EntityTypeRelative)_obj_AllowedTargetTypes;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Type
			output.WriteLine(((int)this.Type).ToString());
			// Range
			output.WriteLine(((float)this.Range).ToString());
			// Duration
			output.WriteLine(((float)this.Duration).ToString());
			// AoeRadius
			output.WriteLine(((float)this.AoeRadius).ToString());
			// DieOnCollision
			output.WriteLine(this.DieOnCollision ? 1 : 0);
			// AllowedTargetTypes
			output.WriteLine(((int)this.AllowedTargetTypes).ToString());
		}

	}
}
