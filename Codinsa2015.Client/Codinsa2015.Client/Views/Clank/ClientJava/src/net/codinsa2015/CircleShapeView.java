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
public class CircleShapeView
{


	public Vector2 Position;
	public Float Radius;
	public static CircleShapeView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		CircleShapeView _obj =  new CircleShapeView();
		// Position
		Vector2 _obj_Position = Vector2.deserialize(input);
		_obj.Position = _obj_Position;
		// Radius
		float _obj_Radius = Float.valueOf(input.readLine());
		_obj.Radius = _obj_Radius;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Position
		this.Position.serialize(output);
		// Radius
		output.append(((Float)this.Radius).toString() + "\n");
	}

}
