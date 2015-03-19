using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Codinsa2015.Views
{

	public class StateAlterationModelView
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		// Représente le type de l'altération d'état.
		public StateAlterationType Type;	
		// Durée de base de l'altération d'état en secondes
		public float BaseDuration;	
		// Si Type contient Dash : Obtient ou définit une valeur indiquant si le dash permet traverser les
		// murs.
		public bool DashGoThroughWall;	
		// Si Type contient Dash : type direction du dash.
		public DashDirectionType DashDirType;	
		// Valeur flat dubuff / debuff (valeur positive : buff, valeur négative : debuff). La nature du
		// buff dépend de Type.
		public float FlatValue;	
		// Même que FlatValue, mais en pourcentage de dégâts d'attaque actuels de la source.
		public float SourcePercentADValue;	
		// Même que FlatValue, mais en pourcentage des HP actuels de la source.
		public float SourcePercentHPValue;	
		// Même que FlatValue, mais en pourcentage des HP max de la source.
		public float SourcePercentMaxHPValue;	
		// Même que FlatValue mais en pourcentage de l'armure actuelle de la source.
		public float SourcePercentArmorValue;	
		// Même que FlatValue, mais en pourcentage de l'AP actuelle de l'entité source.
		public float SourcePercentAPValue;	
		// Même que FlatValue mais en pourcentage de la RM actuelle de l'entité source.
		public float SourcePercentRMValue;	
		// Même que FlatValue, mais en pourcentage dedégâts d'attaque actuels del'entité de
		// destination.
		public float DestPercentADValue;	
		// Même que FlatValue, mais en pourcentage des HP actuels de l'entité de destination.
		public float DestPercentHPValue;	
		// Même que FlatValue, mais en pourcentage des HP max de l'entité de destination.
		public float DestPercentMaxHPValue;	
		// Même que FlatValue mais en pourcentage de l'armure actuelle de l'entité de destination.
		public float DestPercentArmorValue;	
		// Même que FlatValue, mais en pourcentage de l'AP actuelle de l'entité de destination.
		public float DestPercentAPValue;	
		// Même que FlatValue mais en pourcentage de la RM actuelle de l'entité de destination.
		public float DestPercentRMValue;	
		// Obtient le multiplicateur de dégâts lorsque l'altération est appliquée à une structure.
		public float StructureBonus;	
		// Obtient le multiplicateur de dégâts lorsque l'altération est appliquée sur un monstre neute.
		public float MonsterBonus;	
		// Obtient le multiplicateur de dégâts lorsque l'altération est appliquée sur un creep.
		public float CreepBonus;	
		public static StateAlterationModelView Deserialize(System.IO.StreamReader input) {
			StateAlterationModelView _obj =  new StateAlterationModelView();
			// Type
			int _obj_Type = Int32.Parse(input.ReadLine());
			_obj.Type = (StateAlterationType)_obj_Type;
			// BaseDuration
			float _obj_BaseDuration = Single.Parse(input.ReadLine());
			_obj.BaseDuration = (float)_obj_BaseDuration;
			// DashGoThroughWall
			bool _obj_DashGoThroughWall = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			_obj.DashGoThroughWall = (bool)_obj_DashGoThroughWall;
			// DashDirType
			int _obj_DashDirType = Int32.Parse(input.ReadLine());
			_obj.DashDirType = (DashDirectionType)_obj_DashDirType;
			// FlatValue
			float _obj_FlatValue = Single.Parse(input.ReadLine());
			_obj.FlatValue = (float)_obj_FlatValue;
			// SourcePercentADValue
			float _obj_SourcePercentADValue = Single.Parse(input.ReadLine());
			_obj.SourcePercentADValue = (float)_obj_SourcePercentADValue;
			// SourcePercentHPValue
			float _obj_SourcePercentHPValue = Single.Parse(input.ReadLine());
			_obj.SourcePercentHPValue = (float)_obj_SourcePercentHPValue;
			// SourcePercentMaxHPValue
			float _obj_SourcePercentMaxHPValue = Single.Parse(input.ReadLine());
			_obj.SourcePercentMaxHPValue = (float)_obj_SourcePercentMaxHPValue;
			// SourcePercentArmorValue
			float _obj_SourcePercentArmorValue = Single.Parse(input.ReadLine());
			_obj.SourcePercentArmorValue = (float)_obj_SourcePercentArmorValue;
			// SourcePercentAPValue
			float _obj_SourcePercentAPValue = Single.Parse(input.ReadLine());
			_obj.SourcePercentAPValue = (float)_obj_SourcePercentAPValue;
			// SourcePercentRMValue
			float _obj_SourcePercentRMValue = Single.Parse(input.ReadLine());
			_obj.SourcePercentRMValue = (float)_obj_SourcePercentRMValue;
			// DestPercentADValue
			float _obj_DestPercentADValue = Single.Parse(input.ReadLine());
			_obj.DestPercentADValue = (float)_obj_DestPercentADValue;
			// DestPercentHPValue
			float _obj_DestPercentHPValue = Single.Parse(input.ReadLine());
			_obj.DestPercentHPValue = (float)_obj_DestPercentHPValue;
			// DestPercentMaxHPValue
			float _obj_DestPercentMaxHPValue = Single.Parse(input.ReadLine());
			_obj.DestPercentMaxHPValue = (float)_obj_DestPercentMaxHPValue;
			// DestPercentArmorValue
			float _obj_DestPercentArmorValue = Single.Parse(input.ReadLine());
			_obj.DestPercentArmorValue = (float)_obj_DestPercentArmorValue;
			// DestPercentAPValue
			float _obj_DestPercentAPValue = Single.Parse(input.ReadLine());
			_obj.DestPercentAPValue = (float)_obj_DestPercentAPValue;
			// DestPercentRMValue
			float _obj_DestPercentRMValue = Single.Parse(input.ReadLine());
			_obj.DestPercentRMValue = (float)_obj_DestPercentRMValue;
			// StructureBonus
			float _obj_StructureBonus = Single.Parse(input.ReadLine());
			_obj.StructureBonus = (float)_obj_StructureBonus;
			// MonsterBonus
			float _obj_MonsterBonus = Single.Parse(input.ReadLine());
			_obj.MonsterBonus = (float)_obj_MonsterBonus;
			// CreepBonus
			float _obj_CreepBonus = Single.Parse(input.ReadLine());
			_obj.CreepBonus = (float)_obj_CreepBonus;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// Type
			output.WriteLine(((int)this.Type).ToString());
			// BaseDuration
			output.WriteLine(((float)this.BaseDuration).ToString());
			// DashGoThroughWall
			output.WriteLine(this.DashGoThroughWall ? 1 : 0);
			// DashDirType
			output.WriteLine(((int)this.DashDirType).ToString());
			// FlatValue
			output.WriteLine(((float)this.FlatValue).ToString());
			// SourcePercentADValue
			output.WriteLine(((float)this.SourcePercentADValue).ToString());
			// SourcePercentHPValue
			output.WriteLine(((float)this.SourcePercentHPValue).ToString());
			// SourcePercentMaxHPValue
			output.WriteLine(((float)this.SourcePercentMaxHPValue).ToString());
			// SourcePercentArmorValue
			output.WriteLine(((float)this.SourcePercentArmorValue).ToString());
			// SourcePercentAPValue
			output.WriteLine(((float)this.SourcePercentAPValue).ToString());
			// SourcePercentRMValue
			output.WriteLine(((float)this.SourcePercentRMValue).ToString());
			// DestPercentADValue
			output.WriteLine(((float)this.DestPercentADValue).ToString());
			// DestPercentHPValue
			output.WriteLine(((float)this.DestPercentHPValue).ToString());
			// DestPercentMaxHPValue
			output.WriteLine(((float)this.DestPercentMaxHPValue).ToString());
			// DestPercentArmorValue
			output.WriteLine(((float)this.DestPercentArmorValue).ToString());
			// DestPercentAPValue
			output.WriteLine(((float)this.DestPercentAPValue).ToString());
			// DestPercentRMValue
			output.WriteLine(((float)this.DestPercentRMValue).ToString());
			// StructureBonus
			output.WriteLine(((float)this.StructureBonus).ToString());
			// MonsterBonus
			output.WriteLine(((float)this.MonsterBonus).ToString());
			// CreepBonus
			output.WriteLine(((float)this.CreepBonus).ToString());
		}

	}
}
