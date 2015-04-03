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
import net.codinsa2015.SpellLevelDescriptionView.*;
import java.util.ArrayList;


@SuppressWarnings("unused")
public class SpellModelView
{


	// ID du spell permettant de le retrouver dans le tableau de sorts du jeu.
	public Integer ID;
	// Obtient la liste des descriptions des niveaux de ce sort.
	public ArrayList<SpellLevelDescriptionView> Levels;
	public static SpellModelView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		SpellModelView _obj =  new SpellModelView();
		// ID
		int _obj_ID = Integer.valueOf(input.readLine());
		_obj.ID = _obj_ID;
		// Levels
		ArrayList<SpellLevelDescriptionView> _obj_Levels = new ArrayList<SpellLevelDescriptionView>();
		int _obj_Levels_count = Integer.valueOf(input.readLine());
		for(int _obj_Levels_i = 0; _obj_Levels_i < _obj_Levels_count; _obj_Levels_i++) {
			SpellLevelDescriptionView _obj_Levels_e = SpellLevelDescriptionView.deserialize(input);
			_obj_Levels.add((SpellLevelDescriptionView)_obj_Levels_e);
		}
		_obj.Levels = _obj_Levels;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// ID
		output.append(((Integer)this.ID).toString() + "\n");
		// Levels
		output.append(String.valueOf(this.Levels.size()) + "\n");
		for(int Levels_it = 0; Levels_it < this.Levels.size();Levels_it++) {
			this.Levels.get(Levels_it).serialize(output);
		}
	}

}
