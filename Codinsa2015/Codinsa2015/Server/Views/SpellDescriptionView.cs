using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class SpellDescriptionView
	{

	
		public float BaseCooldown;	
		public float CastingTime;	
		public List<StateAlterationModelView> CastingTimeAlterations;	
		public SpellTargetInfoView TargetType;	
		public List<StateAlterationModelView> OnHitEffects;	
	}
}
