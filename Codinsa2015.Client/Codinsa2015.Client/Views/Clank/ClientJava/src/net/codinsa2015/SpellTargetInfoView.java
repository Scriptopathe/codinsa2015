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


	// Type de ciblage du sort.
	public TargettingType Type;
	// Range du sort en unités métriques.
	public Float Range;
	// Durée en secondes que met le sort à atteindre la position donnée.
	public Float Duration;
	// Rayon du sort. (non utilisé pour les sort targetted)
	public Float AoeRadius;
	// Obtient une valeur indiquant si le sort est détruit lors d'une collision avec une entité
	public Boolean DieOnCollision;
	// Retourne le type de cibles pouvant être touchées par ce sort.
	public EntityTypeRelative AllowedTargetTypes;
	public static SpellTargetInfoView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		SpellTargetInfoView _obj =  new SpellTargetInfoView();
		// Type
		TargettingType _obj_Type = TargettingType.fromValue(Integer.valueOf(input.readLine()));
		_obj.Type = TargettingType.fromValue(_obj_Type);
		// Range
		float _obj_Range = Float.valueOf(input.readLine());
		_obj.Range = _obj_Range;
		// Duration
		float _obj_Duration = Float.valueOf(input.readLine());
		_obj.Duration = _obj_Duration;
		// AoeRadius
		float _obj_AoeRadius = Float.valueOf(input.readLine());
		_obj.AoeRadius = _obj_AoeRadius;
		// DieOnCollision
		boolean _obj_DieOnCollision = Integer.valueOf(input.readLine()) == 0 ? false : true;
		_obj.DieOnCollision = _obj_DieOnCollision;
		// AllowedTargetTypes
		EntityTypeRelative _obj_AllowedTargetTypes = EntityTypeRelative.fromValue(Integer.valueOf(input.readLine()));
		_obj.AllowedTargetTypes = EntityTypeRelative.fromValue(_obj_AllowedTargetTypes);
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Type
		output.append(((Integer)(this.Type.getValue())).toString() + "\n");
		// Range
		output.append(((Float)this.Range).toString() + "\n");
		// Duration
		output.append(((Float)this.Duration).toString() + "\n");
		// AoeRadius
		output.append(((Float)this.AoeRadius).toString() + "\n");
		// DieOnCollision
		output.append((this.DieOnCollision ? 1 : 0) + "\n");
		// AllowedTargetTypes
		output.append(((Integer)(this.AllowedTargetTypes.getValue())).toString() + "\n");
	}

}
