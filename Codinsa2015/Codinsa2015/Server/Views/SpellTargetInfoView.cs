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

	
		public TargettingType Type;	
		public float Range;	
		public float Duration;	
		public float AoeRadius;	
		public bool DieOnCollision;	
		public EntityTypeRelative AllowedTargetTypes;	
	}
}
