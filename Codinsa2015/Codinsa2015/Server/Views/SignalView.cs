using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class SignalView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		/// <summary>
		/// id de l'entité émétrice du signal
		/// </summary>
		public int SourceEntity;	
		/// <summary>
		/// ID de l'entité que cible le signal (pour les signaux AttackEntity, DefendEntity)
		/// </summary>
		public int DestinationEntity;	
		/// <summary>
		/// Position que cible le signal (pour les signaux ComingToPosition)
		/// </summary>
		public Vector2 DestinationPosition;	
		public SignalView() {
			DestinationPosition = new Vector2();
		}

		public static SignalView Deserialize(System.IO.StreamReader input) {
			SignalView _obj =  new SignalView();
			// SourceEntity
			int _obj_SourceEntity = Int32.Parse(input.ReadLine());
			_obj.SourceEntity = (int)_obj_SourceEntity;
			// DestinationEntity
			int _obj_DestinationEntity = Int32.Parse(input.ReadLine());
			_obj.DestinationEntity = (int)_obj_DestinationEntity;
			// DestinationPosition
			Vector2 _obj_DestinationPosition = Vector2.Deserialize(input);
			_obj.DestinationPosition = (Vector2)_obj_DestinationPosition;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// SourceEntity
			output.WriteLine(((int)this.SourceEntity).ToString());
			// DestinationEntity
			output.WriteLine(((int)this.DestinationEntity).ToString());
			// DestinationPosition
			this.DestinationPosition.Serialize(output);
		}

	}
}
