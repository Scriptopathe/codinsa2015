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
import net.codinsa2015.Vector2.*;


@SuppressWarnings("unused")
public class SpellCastTargetInfoView
{


	// Type de ciblage de cet objet TargetInfo.
	public TargettingType Type;
	// Retourne la position de la cible, si le type de ciblage (Type) est TargettingType.Position.
	public Vector2 TargetPosition;
	// Retourne la direction de la cible, si le type de ciblage (Type) est TargettingType.Direction.
	// Ce vecteur est transformé automatiquement en vecteur unitaire.
	public Vector2 TargetDirection;
	// Retourne l'id de la cible, si le type de cibale (Type) est TargettingType.Targetted.
	public Integer TargetId;
	public static SpellCastTargetInfoView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		SpellCastTargetInfoView _obj =  new SpellCastTargetInfoView();
		// Type
		TargettingType _obj_Type = TargettingType.fromValue(Integer.valueOf(input.readLine()));
		_obj.Type = TargettingType.fromValue(_obj_Type);
		// TargetPosition
		Vector2 _obj_TargetPosition = Vector2.deserialize(input);
		_obj.TargetPosition = _obj_TargetPosition;
		// TargetDirection
		Vector2 _obj_TargetDirection = Vector2.deserialize(input);
		_obj.TargetDirection = _obj_TargetDirection;
		// TargetId
		int _obj_TargetId = Integer.valueOf(input.readLine());
		_obj.TargetId = _obj_TargetId;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Type
		output.append(((Integer)(this.Type.getValue())).toString() + "\n");
		// TargetPosition
		this.TargetPosition.serialize(output);
		// TargetDirection
		this.TargetDirection.serialize(output);
		// TargetId
		output.append(((Integer)this.TargetId).toString() + "\n");
	}

}
