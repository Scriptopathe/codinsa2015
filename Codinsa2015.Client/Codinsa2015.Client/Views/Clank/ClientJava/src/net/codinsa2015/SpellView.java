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
import net.codinsa2015.SpellDescriptionView.*;
import java.util.ArrayList;


@SuppressWarnings("unused")
public class SpellView
{


	// Cooldown actuel du sort, en secondes
	public Float CurrentCooldown;
	// Id de l'entité possédant le sort.
	public Integer SourceCaster;
	// Représente les descriptions du spell pour les différents niveaux.
	public ArrayList<SpellDescriptionView> Levels;
	// Niveau actuel du spell.
	public Integer Level;
	public static SpellView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		SpellView _obj =  new SpellView();
		// CurrentCooldown
		float _obj_CurrentCooldown = Float.valueOf(input.readLine());
		_obj.CurrentCooldown = _obj_CurrentCooldown;
		// SourceCaster
		int _obj_SourceCaster = Integer.valueOf(input.readLine());
		_obj.SourceCaster = _obj_SourceCaster;
		// Levels
		ArrayList<SpellDescriptionView> _obj_Levels = new ArrayList<SpellDescriptionView>();
		int _obj_Levels_count = Integer.valueOf(input.readLine());
		for(int _obj_Levels_i = 0; _obj_Levels_i < _obj_Levels_count; _obj_Levels_i++) {
			SpellDescriptionView _obj_Levels_e = SpellDescriptionView.deserialize(input);
			_obj_Levels.add((SpellDescriptionView)_obj_Levels_e);
		}
		_obj.Levels = _obj_Levels;
		// Level
		int _obj_Level = Integer.valueOf(input.readLine());
		_obj.Level = _obj_Level;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// CurrentCooldown
		output.append(((Float)this.CurrentCooldown).toString() + "\n");
		// SourceCaster
		output.append(((Integer)this.SourceCaster).toString() + "\n");
		// Levels
		output.append(String.valueOf(this.Levels.size()) + "\n");
		for(int Levels_it = 0; Levels_it < this.Levels.size();Levels_it++) {
			this.Levels.get(Levels_it).serialize(output);
		}
		// Level
		output.append(((Integer)this.Level).toString() + "\n");
	}

}
