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
public class Vector2
{


	public Vector2()
	{
	}
	public Vector2(Float x, Float y)
	{
		this.X = x;
		this.Y = y;
	}
	public Float X;
	public Float Y;
	public Vector2() {
	}

	public static Vector2 deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		Vector2 _obj =  new Vector2();
		// X
		float _obj_X = Float.valueOf(input.readLine());
		_obj.X = _obj_X;
		// Y
		float _obj_Y = Float.valueOf(input.readLine());
		_obj.Y = _obj_Y;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// X
		output.append(((Float)this.X).toString() + "\n");
		// Y
		output.append(((Float)this.Y).toString() + "\n");
	}

}
