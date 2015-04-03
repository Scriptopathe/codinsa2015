using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views.Client
{

	public class SpellCastTargetInfoView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// Type de ciblage de cet objet TargetInfo.
		/// </summary>
		public TargettingType Type;	
		/// <summary>
		/// Retourne la position de la cible, si le type de ciblage (Type) est TargettingType.Position.
		/// </summary>
		public Vector2 TargetPosition;	
		/// <summary>
		/// Retourne la direction de la cible, si le type de ciblage (Type) est TargettingType.Direction.
		/// Ce vecteur est transformé automatiquement en vecteur unitaire.
		/// </summary>
		public Vector2 TargetDirection;	
		/// <summary>
		/// Retourne l'id de la cible, si le type de cibale (Type) est TargettingType.Targetted.
		/// </summary>
		public int TargetId;	
		public static SpellCastTargetInfoView Deserialize(System.IO.StreamReader input) {
			SpellCastTargetInfoView _obj =  new SpellCastTargetInfoView();
			// Type
			TargettingType _obj_Type = (TargettingType)Int32.Parse(input.ReadLine());
			_obj.Type = (TargettingType)_obj_Type;
			// TargetPosition
			Vector2 _obj_TargetPosition = Vector2.Deserialize(input);
			_obj.TargetPosition = (Vector2)_obj_TargetPosition;
			// TargetDirection
			Vector2 _obj_TargetDirection = Vector2.Deserialize(input);
			_obj.TargetDirection = (Vector2)_obj_TargetDirection;
			// TargetId
			int _obj_TargetId = Int32.Parse(input.ReadLine());
			_obj.TargetId = (int)_obj_TargetId;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Type
			output.WriteLine(((int)this.Type).ToString());
			// TargetPosition
			this.TargetPosition.Serialize(output);
			// TargetDirection
			this.TargetDirection.Serialize(output);
			// TargetId
			output.WriteLine(((int)this.TargetId).ToString());
		}

	}
}
