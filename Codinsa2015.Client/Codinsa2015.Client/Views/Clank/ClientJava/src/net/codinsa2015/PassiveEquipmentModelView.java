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
import net.codinsa2015.PassiveEquipmentUpgradeModelView.*;
import java.util.ArrayList;


@SuppressWarnings("unused")
public class PassiveEquipmentModelView
{


	// prix d'achat de l'équipement
	public Float Price;
	// liste des upgrades de cet équipement.
	public ArrayList<PassiveEquipmentUpgradeModelView> Upgrades;
	public PassiveEquipmentModelView() {
		Upgrades = new ArrayList<PassiveEquipmentUpgradeModelView>();
	}

	public static PassiveEquipmentModelView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		PassiveEquipmentModelView _obj =  new PassiveEquipmentModelView();
		// Price
		float _obj_Price = Float.valueOf(input.readLine());
		_obj.Price = _obj_Price;
		// Upgrades
		ArrayList<PassiveEquipmentUpgradeModelView> _obj_Upgrades = new ArrayList<PassiveEquipmentUpgradeModelView>();
		int _obj_Upgrades_count = Integer.valueOf(input.readLine());
		for(int _obj_Upgrades_i = 0; _obj_Upgrades_i < _obj_Upgrades_count; _obj_Upgrades_i++) {
			PassiveEquipmentUpgradeModelView _obj_Upgrades_e = PassiveEquipmentUpgradeModelView.deserialize(input);
			_obj_Upgrades.add((PassiveEquipmentUpgradeModelView)_obj_Upgrades_e);
		}
		_obj.Upgrades = _obj_Upgrades;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Price
		output.append(((Float)this.Price).toString() + "\n");
		// Upgrades
		output.append(String.valueOf(this.Upgrades.size()) + "\n");
		for(int Upgrades_it = 0; Upgrades_it < this.Upgrades.size();Upgrades_it++) {
			this.Upgrades.get(Upgrades_it).serialize(output);
		}
	}

}
