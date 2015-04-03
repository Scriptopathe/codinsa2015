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
import net.codinsa2015.GenericShapeView.*;


@SuppressWarnings("unused")
public class SpellcastBaseView
{


	// Shape utilisée par ce spell cast.
	public GenericShapeView Shape;
	// nom de ce spellcast (utilisé pour le texturing notamment).
	public String Name;
	public SpellcastBaseView() {
		Shape = new GenericShapeView();
	}

	public static SpellcastBaseView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		SpellcastBaseView _obj =  new SpellcastBaseView();
		// Shape
		GenericShapeView _obj_Shape = GenericShapeView.deserialize(input);
		_obj.Shape = _obj_Shape;
		// Name
		String _obj_Name = input.readLine();
		_obj.Name = _obj_Name;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Shape
		this.Shape.serialize(output);
		// Name
		output.append(this.Name + "\n");
	}

}
