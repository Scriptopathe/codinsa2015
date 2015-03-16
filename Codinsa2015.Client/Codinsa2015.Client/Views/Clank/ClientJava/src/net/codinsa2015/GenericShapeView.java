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
import net.codinsa2015.GenericShapeType.*;


@SuppressWarnings("unused")
public class GenericShapeView
{


	public Vector2 Position;
	public Float Radius;
	public Vector2 Size;
	public GenericShapeType ShapeType;
	public static GenericShapeView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		GenericShapeView _obj =  new GenericShapeView();
		// Position
		Vector2 _obj_Position = Vector2.deserialize(input);
		_obj.Position = _obj_Position;
		// Radius
		float _obj_Radius = Float.valueOf(input.readLine().replace(',', '.'));
		_obj.Radius = _obj_Radius;
		// Size
		Vector2 _obj_Size = Vector2.deserialize(input);
		_obj.Size = _obj_Size;
		// ShapeType
		int _obj_ShapeType = Integer.valueOf(input.readLine());
		_obj.ShapeType = GenericShapeType.fromValue(_obj_ShapeType);
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Position
		this.Position.serialize(output);
		// Radius
		output.append(((Float)this.Radius).toString().replace('.', ',') + "\n");
		// Size
		this.Size.serialize(output);
		// ShapeType
		output.append(((Integer)(this.ShapeType.getValue())).toString() + "\n");
	}

}
