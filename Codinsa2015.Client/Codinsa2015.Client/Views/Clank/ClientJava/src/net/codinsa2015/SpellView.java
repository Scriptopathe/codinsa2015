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


@SuppressWarnings("unused")
public class SpellView
{


	// Cooldown actuel du sort, en secondes
	public Float CurrentCooldown;
	// Id de l'entité possédant le sort.
	public Integer SourceCaster;
	// Représente l'id du modèle du spell. Ce modèle décrit les différents effets du spell pour chacun
	// de ses niveaux
	public Integer Model;
	// Niveau actuel du spell.
	public Integer Level;
	public SpellView() {
	}

	public static SpellView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		SpellView _obj =  new SpellView();
		// CurrentCooldown
		float _obj_CurrentCooldown = Float.valueOf(input.readLine());
		_obj.CurrentCooldown = _obj_CurrentCooldown;
		// SourceCaster
		int _obj_SourceCaster = Integer.valueOf(input.readLine());
		_obj.SourceCaster = _obj_SourceCaster;
		// Model
		int _obj_Model = Integer.valueOf(input.readLine());
		_obj.Model = _obj_Model;
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
		// Model
		output.append(((Integer)this.Model).toString() + "\n");
		// Level
		output.append(((Integer)this.Level).toString() + "\n");
	}

}
