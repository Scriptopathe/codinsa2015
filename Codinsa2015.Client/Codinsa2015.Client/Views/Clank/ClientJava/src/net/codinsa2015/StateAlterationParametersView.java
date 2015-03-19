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
public class StateAlterationParametersView
{


	// Position finale que le dash doit atteindre (si le targetting est Direction)
	public Vector2 DashTargetDirection;
	// Entité vers laquelle le dash doit se diriger (si le targetting du dash est Entity).
	public Integer DashTargetEntity;
	public static StateAlterationParametersView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		StateAlterationParametersView _obj =  new StateAlterationParametersView();
		// DashTargetDirection
		Vector2 _obj_DashTargetDirection = Vector2.deserialize(input);
		_obj.DashTargetDirection = _obj_DashTargetDirection;
		// DashTargetEntity
		int _obj_DashTargetEntity = Integer.valueOf(input.readLine());
		_obj.DashTargetEntity = _obj_DashTargetEntity;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// DashTargetDirection
		this.DashTargetDirection.serialize(output);
		// DashTargetEntity
		output.append(((Integer)this.DashTargetEntity).toString() + "\n");
	}

}
