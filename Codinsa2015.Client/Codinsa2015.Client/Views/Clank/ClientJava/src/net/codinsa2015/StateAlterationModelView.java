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


	public StateAlterationType Type;
	public Float BaseDuration;
	public Boolean DashGoThroughWall;
	public DashDirectionType DashDirType;
	public Float FlatValue;
	public Float SourcePercentADValue;
	public Float SourcePercentHPValue;
	public Float SourcePercentMaxHPValue;
	public Float SourcePercentArmorValue;
	public Float SourcePercentAPValue;
	public Float SourcePercentRMValue;
	public Float DestPercentADValue;
	public Float DestPercentHPValue;
	public Float DestPercentMaxHPValue;
	public Float DestPercentArmorValue;
	public Float DestPercentAPValue;
	public Float DestPercentRMValue;
	public Float StructureBonus;
	public Float MonsterBonus;
	public Float CreepBonus;
	public static StateAlterationModelView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		StateAlterationModelView _obj =  new StateAlterationModelView();
		// Type
		int _obj_Type = Integer.valueOf(input.readLine());
		_obj.Type = StateAlterationType.fromValue(_obj_Type);
		// BaseDuration
		float _obj_BaseDuration = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.BaseDuration = _obj_BaseDuration;
		// DashGoThroughWall
		boolean _obj_DashGoThroughWall = Integer.valueOf(input.readLine()) == 0 ? false : true;
		_obj.DashGoThroughWall = _obj_DashGoThroughWall;
		// DashDirType
		int _obj_DashDirType = Integer.valueOf(input.readLine());
		_obj.DashDirType = DashDirectionType.fromValue(_obj_DashDirType);
		// FlatValue
		float _obj_FlatValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.FlatValue = _obj_FlatValue;
		// SourcePercentADValue
		float _obj_SourcePercentADValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.SourcePercentADValue = _obj_SourcePercentADValue;
		// SourcePercentHPValue
		float _obj_SourcePercentHPValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.SourcePercentHPValue = _obj_SourcePercentHPValue;
		// SourcePercentMaxHPValue
		float _obj_SourcePercentMaxHPValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.SourcePercentMaxHPValue = _obj_SourcePercentMaxHPValue;
		// SourcePercentArmorValue
		float _obj_SourcePercentArmorValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.SourcePercentArmorValue = _obj_SourcePercentArmorValue;
		// SourcePercentAPValue
		float _obj_SourcePercentAPValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.SourcePercentAPValue = _obj_SourcePercentAPValue;
		// SourcePercentRMValue
		float _obj_SourcePercentRMValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.SourcePercentRMValue = _obj_SourcePercentRMValue;
		// DestPercentADValue
		float _obj_DestPercentADValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.DestPercentADValue = _obj_DestPercentADValue;
		// DestPercentHPValue
		float _obj_DestPercentHPValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.DestPercentHPValue = _obj_DestPercentHPValue;
		// DestPercentMaxHPValue
		float _obj_DestPercentMaxHPValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.DestPercentMaxHPValue = _obj_DestPercentMaxHPValue;
		// DestPercentArmorValue
		float _obj_DestPercentArmorValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.DestPercentArmorValue = _obj_DestPercentArmorValue;
		// DestPercentAPValue
		float _obj_DestPercentAPValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.DestPercentAPValue = _obj_DestPercentAPValue;
		// DestPercentRMValue
		float _obj_DestPercentRMValue = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.DestPercentRMValue = _obj_DestPercentRMValue;
		// StructureBonus
		float _obj_StructureBonus = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.StructureBonus = _obj_StructureBonus;
		// MonsterBonus
		float _obj_MonsterBonus = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.MonsterBonus = _obj_MonsterBonus;
		// CreepBonus
		float _obj_CreepBonus = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.CreepBonus = _obj_CreepBonus;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Type
		output.append(((Integer)(this.Type.getValue())).toString() + "\n");
		// BaseDuration
		output.append(((Float)this.BaseDuration).toString().replace('.', ',') + "\n");
		// DashGoThroughWall
		output.append((this.DashGoThroughWall ? 1 : 0) + "\n");
		// DashDirType
		output.append(((Integer)(this.DashDirType.getValue())).toString() + "\n");
		// FlatValue
		output.append(((Float)this.FlatValue).toString().replace('.', ',') + "\n");
		// SourcePercentADValue
		output.append(((Float)this.SourcePercentADValue).toString().replace('.', ',') + "\n");
		// SourcePercentHPValue
		output.append(((Float)this.SourcePercentHPValue).toString().replace('.', ',') + "\n");
		// SourcePercentMaxHPValue
		output.append(((Float)this.SourcePercentMaxHPValue).toString().replace('.', ',') + "\n");
		// SourcePercentArmorValue
		output.append(((Float)this.SourcePercentArmorValue).toString().replace('.', ',') + "\n");
		// SourcePercentAPValue
		output.append(((Float)this.SourcePercentAPValue).toString().replace('.', ',') + "\n");
		// SourcePercentRMValue
		output.append(((Float)this.SourcePercentRMValue).toString().replace('.', ',') + "\n");
		// DestPercentADValue
		output.append(((Float)this.DestPercentADValue).toString().replace('.', ',') + "\n");
		// DestPercentHPValue
		output.append(((Float)this.DestPercentHPValue).toString().replace('.', ',') + "\n");
		// DestPercentMaxHPValue
		output.append(((Float)this.DestPercentMaxHPValue).toString().replace('.', ',') + "\n");
		// DestPercentArmorValue
		output.append(((Float)this.DestPercentArmorValue).toString().replace('.', ',') + "\n");
		// DestPercentAPValue
		output.append(((Float)this.DestPercentAPValue).toString().replace('.', ',') + "\n");
		// DestPercentRMValue
		output.append(((Float)this.DestPercentRMValue).toString().replace('.', ',') + "\n");
		// StructureBonus
		output.append(((Float)this.StructureBonus).toString().replace('.', ',') + "\n");
		// MonsterBonus
		output.append(((Float)this.MonsterBonus).toString().replace('.', ',') + "\n");
		// CreepBonus
		output.append(((Float)this.CreepBonus).toString().replace('.', ',') + "\n");
	}

}
