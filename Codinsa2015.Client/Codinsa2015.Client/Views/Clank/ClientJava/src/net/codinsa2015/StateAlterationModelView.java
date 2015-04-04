package net.codinsa2015;
import java.lang.*;
import java.util.ArrayList;
import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;
import net.codinsa2015.StateAlterationType.*;
import net.codinsa2015.DashDirectionType.*;


@SuppressWarnings("unused")
public class StateAlterationModelView
{


	// Représente le type de l'altération d'état.
	public StateAlterationType Type;
	// Durée de base de l'altération d'état en secondes
	public Float BaseDuration;
	// Si Type contient Dash : Obtient ou définit une valeur indiquant si le dash permet traverser les
	// murs.
	public Boolean DashGoThroughWall;
	// Si Type contient Dash : type direction du dash.
	public DashDirectionType DashDirType;
	// Valeur flat dubuff / debuff (valeur positive : buff, valeur négative : debuff). La nature du
	// buff dépend de Type.
	public Float FlatValue;
	// Même que FlatValue, mais en pourcentage de dégâts d'attaque actuels de la source.
	public Float SourcePercentADValue;
	// Même que FlatValue, mais en pourcentage des HP actuels de la source.
	public Float SourcePercentHPValue;
	// Même que FlatValue, mais en pourcentage des HP max de la source.
	public Float SourcePercentMaxHPValue;
	// Même que FlatValue mais en pourcentage de l'armure actuelle de la source.
	public Float SourcePercentArmorValue;
	// Même que FlatValue, mais en pourcentage de l'AP actuelle de l'entité source.
	public Float SourcePercentAPValue;
	// Même que FlatValue mais en pourcentage de la RM actuelle de l'entité source.
	public Float SourcePercentRMValue;
	// Même que FlatValue, mais en pourcentage dedégâts d'attaque actuels del'entité de
	// destination.
	public Float DestPercentADValue;
	// Même que FlatValue, mais en pourcentage des HP actuels de l'entité de destination.
	public Float DestPercentHPValue;
	// Même que FlatValue, mais en pourcentage des HP max de l'entité de destination.
	public Float DestPercentMaxHPValue;
	// Même que FlatValue mais en pourcentage de l'armure actuelle de l'entité de destination.
	public Float DestPercentArmorValue;
	// Même que FlatValue, mais en pourcentage de l'AP actuelle de l'entité de destination.
	public Float DestPercentAPValue;
	// Même que FlatValue mais en pourcentage de la RM actuelle de l'entité de destination.
	public Float DestPercentRMValue;
	// Obtient le multiplicateur de dégâts lorsque l'altération est appliquée à une structure.
	public Float StructureBonus;
	// Obtient le multiplicateur de dégâts lorsque l'altération est appliquée sur un monstre neute.
	public Float MonsterBonus;
	// Obtient le multiplicateur de dégâts lorsque l'altération est appliquée sur un Virus.
	public Float VirusBonus;
	public StateAlterationModelView() {
	}

	public static StateAlterationModelView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		StateAlterationModelView _obj =  new StateAlterationModelView();
		// Type
		StateAlterationType _obj_Type = StateAlterationType.fromValue(Integer.valueOf(input.readLine()));
		_obj.Type = _obj_Type;
		// BaseDuration
		float _obj_BaseDuration = Float.valueOf(input.readLine());
		_obj.BaseDuration = _obj_BaseDuration;
		// DashGoThroughWall
		boolean _obj_DashGoThroughWall = Integer.valueOf(input.readLine()) == 0 ? false : true;
		_obj.DashGoThroughWall = _obj_DashGoThroughWall;
		// DashDirType
		DashDirectionType _obj_DashDirType = DashDirectionType.fromValue(Integer.valueOf(input.readLine()));
		_obj.DashDirType = _obj_DashDirType;
		// FlatValue
		float _obj_FlatValue = Float.valueOf(input.readLine());
		_obj.FlatValue = _obj_FlatValue;
		// SourcePercentADValue
		float _obj_SourcePercentADValue = Float.valueOf(input.readLine());
		_obj.SourcePercentADValue = _obj_SourcePercentADValue;
		// SourcePercentHPValue
		float _obj_SourcePercentHPValue = Float.valueOf(input.readLine());
		_obj.SourcePercentHPValue = _obj_SourcePercentHPValue;
		// SourcePercentMaxHPValue
		float _obj_SourcePercentMaxHPValue = Float.valueOf(input.readLine());
		_obj.SourcePercentMaxHPValue = _obj_SourcePercentMaxHPValue;
		// SourcePercentArmorValue
		float _obj_SourcePercentArmorValue = Float.valueOf(input.readLine());
		_obj.SourcePercentArmorValue = _obj_SourcePercentArmorValue;
		// SourcePercentAPValue
		float _obj_SourcePercentAPValue = Float.valueOf(input.readLine());
		_obj.SourcePercentAPValue = _obj_SourcePercentAPValue;
		// SourcePercentRMValue
		float _obj_SourcePercentRMValue = Float.valueOf(input.readLine());
		_obj.SourcePercentRMValue = _obj_SourcePercentRMValue;
		// DestPercentADValue
		float _obj_DestPercentADValue = Float.valueOf(input.readLine());
		_obj.DestPercentADValue = _obj_DestPercentADValue;
		// DestPercentHPValue
		float _obj_DestPercentHPValue = Float.valueOf(input.readLine());
		_obj.DestPercentHPValue = _obj_DestPercentHPValue;
		// DestPercentMaxHPValue
		float _obj_DestPercentMaxHPValue = Float.valueOf(input.readLine());
		_obj.DestPercentMaxHPValue = _obj_DestPercentMaxHPValue;
		// DestPercentArmorValue
		float _obj_DestPercentArmorValue = Float.valueOf(input.readLine());
		_obj.DestPercentArmorValue = _obj_DestPercentArmorValue;
		// DestPercentAPValue
		float _obj_DestPercentAPValue = Float.valueOf(input.readLine());
		_obj.DestPercentAPValue = _obj_DestPercentAPValue;
		// DestPercentRMValue
		float _obj_DestPercentRMValue = Float.valueOf(input.readLine());
		_obj.DestPercentRMValue = _obj_DestPercentRMValue;
		// StructureBonus
		float _obj_StructureBonus = Float.valueOf(input.readLine());
		_obj.StructureBonus = _obj_StructureBonus;
		// MonsterBonus
		float _obj_MonsterBonus = Float.valueOf(input.readLine());
		_obj.MonsterBonus = _obj_MonsterBonus;
		// VirusBonus
		float _obj_VirusBonus = Float.valueOf(input.readLine());
		_obj.VirusBonus = _obj_VirusBonus;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Type
		output.append(((Integer)(this.Type.getValue())).toString() + "\n");
		// BaseDuration
		output.append(((Float)this.BaseDuration).toString() + "\n");
		// DashGoThroughWall
		output.append((this.DashGoThroughWall ? 1 : 0) + "\n");
		// DashDirType
		output.append(((Integer)(this.DashDirType.getValue())).toString() + "\n");
		// FlatValue
		output.append(((Float)this.FlatValue).toString() + "\n");
		// SourcePercentADValue
		output.append(((Float)this.SourcePercentADValue).toString() + "\n");
		// SourcePercentHPValue
		output.append(((Float)this.SourcePercentHPValue).toString() + "\n");
		// SourcePercentMaxHPValue
		output.append(((Float)this.SourcePercentMaxHPValue).toString() + "\n");
		// SourcePercentArmorValue
		output.append(((Float)this.SourcePercentArmorValue).toString() + "\n");
		// SourcePercentAPValue
		output.append(((Float)this.SourcePercentAPValue).toString() + "\n");
		// SourcePercentRMValue
		output.append(((Float)this.SourcePercentRMValue).toString() + "\n");
		// DestPercentADValue
		output.append(((Float)this.DestPercentADValue).toString() + "\n");
		// DestPercentHPValue
		output.append(((Float)this.DestPercentHPValue).toString() + "\n");
		// DestPercentMaxHPValue
		output.append(((Float)this.DestPercentMaxHPValue).toString() + "\n");
		// DestPercentArmorValue
		output.append(((Float)this.DestPercentArmorValue).toString() + "\n");
		// DestPercentAPValue
		output.append(((Float)this.DestPercentAPValue).toString() + "\n");
		// DestPercentRMValue
		output.append(((Float)this.DestPercentRMValue).toString() + "\n");
		// StructureBonus
		output.append(((Float)this.StructureBonus).toString() + "\n");
		// MonsterBonus
		output.append(((Float)this.MonsterBonus).toString() + "\n");
		// VirusBonus
		output.append(((Float)this.VirusBonus).toString() + "\n");
	}

}
