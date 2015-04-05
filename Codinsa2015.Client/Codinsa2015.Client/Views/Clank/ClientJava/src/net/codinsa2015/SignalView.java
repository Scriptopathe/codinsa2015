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
import net.codinsa2015.Vector2.*;


@SuppressWarnings("unused")
public class SignalView
{


	// id de l'entité émétrice du signal
	public Integer SourceEntity;
	// ID de l'entité que cible le signal (pour les signaux AttackEntity, DefendEntity)
	public Integer DestinationEntity;
	// Position que cible le signal (pour les signaux ComingToPosition)
	public Vector2 DestinationPosition;
	public SignalView() {
		DestinationPosition = new Vector2();
	}

	public static SignalView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		SignalView _obj =  new SignalView();
		// SourceEntity
		int _obj_SourceEntity = Integer.valueOf(input.readLine());
		_obj.SourceEntity = _obj_SourceEntity;
		// DestinationEntity
		int _obj_DestinationEntity = Integer.valueOf(input.readLine());
		_obj.DestinationEntity = _obj_DestinationEntity;
		// DestinationPosition
		Vector2 _obj_DestinationPosition = Vector2.deserialize(input);
		_obj.DestinationPosition = _obj_DestinationPosition;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// SourceEntity
		output.append(((Integer)this.SourceEntity).toString() + "\n");
		// DestinationEntity
		output.append(((Integer)this.DestinationEntity).toString() + "\n");
		// DestinationPosition
		this.DestinationPosition.serialize(output);
	}

}
