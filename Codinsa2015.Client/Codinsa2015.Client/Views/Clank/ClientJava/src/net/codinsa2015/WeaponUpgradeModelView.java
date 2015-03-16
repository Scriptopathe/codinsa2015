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
import net.codinsa2015.StateAlterationModelView.*;
import java.util.ArrayList;


@SuppressWarnings("unused")
public class WeaponUpgradeModelView
{


	public SpellDescriptionView Description;
	public ArrayList<StateAlterationModelView> PassiveAlterations;
	public Float Cost;
	public static WeaponUpgradeModelView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		WeaponUpgradeModelView _obj =  new WeaponUpgradeModelView();
		// Description
		SpellDescriptionView _obj_Description = SpellDescriptionView.deserialize(input);
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
		float _obj_Cost = Float.valueOf(input.readLine().replace(',', '.'));
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
		output.append(((Float)this.Cost).toString().replace('.', ',') + "\n");
	}

}
