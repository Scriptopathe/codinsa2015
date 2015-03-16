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
import net.codinsa2015.TargettingType.*;
import net.codinsa2015.EntityTypeRelative.*;


@SuppressWarnings("unused")
public class SpellTargetInfoView
{


	public TargettingType Type;
	public Float Range;
	public Float Duration;
	public Float AoeRadius;
	public Boolean DieOnCollision;
	public EntityTypeRelative AllowedTargetTypes;
	public static SpellTargetInfoView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		SpellTargetInfoView _obj =  new SpellTargetInfoView();
		// Type
		int _obj_Type = Integer.valueOf(input.readLine());
		_obj.Type = TargettingType.fromValue(_obj_Type);
		// Range
		float _obj_Range = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.Range = _obj_Range;
		// Duration
		float _obj_Duration = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.Duration = _obj_Duration;
		// AoeRadius
		float _obj_AoeRadius = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.AoeRadius = _obj_AoeRadius;
		// DieOnCollision
		boolean _obj_DieOnCollision = Integer.valueOf(input.readLine()) == 0 ? false : true;
		_obj.DieOnCollision = _obj_DieOnCollision;
		// AllowedTargetTypes
		int _obj_AllowedTargetTypes = Integer.valueOf(input.readLine());
		_obj.AllowedTargetTypes = EntityTypeRelative.fromValue(_obj_AllowedTargetTypes);
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Type
		output.append(((Integer)(this.Type.getValue())).toString() + "\n");
		// Range
		output.append(((Float)this.Range).toString().replace('.', ',') + "\n");
		// Duration
		output.append(((Float)this.Duration).toString().replace('.', ',') + "\n");
		// AoeRadius
		output.append(((Float)this.AoeRadius).toString().replace('.', ',') + "\n");
		// DieOnCollision
		output.append((this.DieOnCollision ? 1 : 0) + "\n");
		// AllowedTargetTypes
		output.append(((Integer)(this.AllowedTargetTypes.getValue())).toString() + "\n");
	}

}
