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
import net.codinsa2015.StateAlterationModelView.*;
import java.util.ArrayList;


@SuppressWarnings("unused")
public class WeaponUpgradeModelView
{


	// Obtient du sort que lance l'arme à ce niveau d'upgrade.
	public SpellLevelDescriptionView Description;
	// Obtient les altérations d'état appliquées passivement par l'arme à ce niveau d'upgrade.
	public ArrayList<StateAlterationModelView> PassiveAlterations;
	// Obtient le coût de cette upgrade.
	public Float Cost;
	public WeaponUpgradeModelView() {
		Description = new SpellLevelDescriptionView();
		PassiveAlterations = new ArrayList<StateAlterationModelView>();
	}

	public static WeaponUpgradeModelView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		WeaponUpgradeModelView _obj =  new WeaponUpgradeModelView();
		// Description
		SpellLevelDescriptionView _obj_Description = SpellLevelDescriptionView.deserialize(input);
		_obj.Description = _obj_Description;
		// PassiveAlterations
		ArrayList<StateAlterationModelView> _obj_PassiveAlterations = new ArrayList<StateAlterationModelView>();
		int _obj_PassiveAlterations_count = Integer.valueOf(input.readLine());
		for(int _obj_PassiveAlterations_i = 0; _obj_PassiveAlterations_i < _obj_PassiveAlterations_count; _obj_PassiveAlterations_i++) {
			StateAlterationModelView _obj_PassiveAlterations_e = StateAlterationModelView.deserialize(input);
			_obj_PassiveAlterations.add((StateAlterationModelView)_obj_PassiveAlterations_e);
		}
		_obj.PassiveAlterations = _obj_PassiveAlterations;
		// Cost
		float _obj_Cost = Float.valueOf(input.readLine());
		_obj.Cost = _obj_Cost;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Description
		this.Description.serialize(output);
		// PassiveAlterations
		output.append(String.valueOf(this.PassiveAlterations.size()) + "\n");
		for(int PassiveAlterations_it = 0; PassiveAlterations_it < this.PassiveAlterations.size();PassiveAlterations_it++) {
			this.PassiveAlterations.get(PassiveAlterations_it).serialize(output);
		}
		// Cost
		output.append(((Float)this.Cost).toString() + "\n");
	}

}
