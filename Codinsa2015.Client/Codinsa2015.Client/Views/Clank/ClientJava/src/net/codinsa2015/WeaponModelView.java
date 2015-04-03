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
import net.codinsa2015.WeaponUpgradeModelView.*;
import java.util.ArrayList;


@SuppressWarnings("unused")
public class WeaponModelView
{


	// Liste des upgrades possibles de l'arme.
	public ArrayList<WeaponUpgradeModelView> Upgrades;
	// Prix d'achat de l'arme
	public Float Price;
	public WeaponModelView() {
		Upgrades = new ArrayList<WeaponUpgradeModelView>();
	}

	public static WeaponModelView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		WeaponModelView _obj =  new WeaponModelView();
		// Upgrades
		ArrayList<WeaponUpgradeModelView> _obj_Upgrades = new ArrayList<WeaponUpgradeModelView>();
		int _obj_Upgrades_count = Integer.valueOf(input.readLine());
		for(int _obj_Upgrades_i = 0; _obj_Upgrades_i < _obj_Upgrades_count; _obj_Upgrades_i++) {
			WeaponUpgradeModelView _obj_Upgrades_e = WeaponUpgradeModelView.deserialize(input);
			_obj_Upgrades.add((WeaponUpgradeModelView)_obj_Upgrades_e);
		}
		_obj.Upgrades = _obj_Upgrades;
		// Price
		float _obj_Price = Float.valueOf(input.readLine());
		_obj.Price = _obj_Price;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Upgrades
		output.append(String.valueOf(this.Upgrades.size()) + "\n");
		for(int Upgrades_it = 0; Upgrades_it < this.Upgrades.size();Upgrades_it++) {
			this.Upgrades.get(Upgrades_it).serialize(output);
		}
		// Price
		output.append(((Float)this.Price).toString() + "\n");
	}

}
