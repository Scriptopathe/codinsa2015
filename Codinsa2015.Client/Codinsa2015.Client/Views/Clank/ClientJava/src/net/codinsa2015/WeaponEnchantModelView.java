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
import net.codinsa2015.StateAlterationModelView.*;
import java.util.ArrayList;


@SuppressWarnings("unused")
public class WeaponEnchantModelView
{


	// Obtient les altértions d'état appliquées à l'impact de l'attaque sur la cible.
	public ArrayList<StateAlterationModelView> OnHitEffects;
	// Obtient les altérations d'état appliquées lors de l'attaque sur le caster.
	public ArrayList<StateAlterationModelView> CastingEffects;
	// Obtient les effets passifs appliqués par l'enchantement.
	public ArrayList<StateAlterationModelView> PassiveEffects;
	public static WeaponEnchantModelView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		WeaponEnchantModelView _obj =  new WeaponEnchantModelView();
		// OnHitEffects
		ArrayList<StateAlterationModelView> _obj_OnHitEffects = new ArrayList<StateAlterationModelView>();
		int _obj_OnHitEffects_count = Integer.valueOf(input.readLine());
		for(int _obj_OnHitEffects_i = 0; _obj_OnHitEffects_i < _obj_OnHitEffects_count; _obj_OnHitEffects_i++) {
			StateAlterationModelView _obj_OnHitEffects_e = StateAlterationModelView.deserialize(input);
			_obj_OnHitEffects.add((StateAlterationModelView)_obj_OnHitEffects_e);
		}
		_obj.OnHitEffects = _obj_OnHitEffects;
		// CastingEffects
		ArrayList<StateAlterationModelView> _obj_CastingEffects = new ArrayList<StateAlterationModelView>();
		int _obj_CastingEffects_count = Integer.valueOf(input.readLine());
		for(int _obj_CastingEffects_i = 0; _obj_CastingEffects_i < _obj_CastingEffects_count; _obj_CastingEffects_i++) {
			StateAlterationModelView _obj_CastingEffects_e = StateAlterationModelView.deserialize(input);
			_obj_CastingEffects.add((StateAlterationModelView)_obj_CastingEffects_e);
		}
		_obj.CastingEffects = _obj_CastingEffects;
		// PassiveEffects
		ArrayList<StateAlterationModelView> _obj_PassiveEffects = new ArrayList<StateAlterationModelView>();
		int _obj_PassiveEffects_count = Integer.valueOf(input.readLine());
		for(int _obj_PassiveEffects_i = 0; _obj_PassiveEffects_i < _obj_PassiveEffects_count; _obj_PassiveEffects_i++) {
			StateAlterationModelView _obj_PassiveEffects_e = StateAlterationModelView.deserialize(input);
			_obj_PassiveEffects.add((StateAlterationModelView)_obj_PassiveEffects_e);
		}
		_obj.PassiveEffects = _obj_PassiveEffects;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// OnHitEffects
		output.append(String.valueOf(this.OnHitEffects.size()) + "\n");
		for(int OnHitEffects_it = 0; OnHitEffects_it < this.OnHitEffects.size();OnHitEffects_it++) {
			this.OnHitEffects.get(OnHitEffects_it).serialize(output);
		}
		// CastingEffects
		output.append(String.valueOf(this.CastingEffects.size()) + "\n");
		for(int CastingEffects_it = 0; CastingEffects_it < this.CastingEffects.size();CastingEffects_it++) {
			this.CastingEffects.get(CastingEffects_it).serialize(output);
		}
		// PassiveEffects
		output.append(String.valueOf(this.PassiveEffects.size()) + "\n");
		for(int PassiveEffects_it = 0; PassiveEffects_it < this.PassiveEffects.size();PassiveEffects_it++) {
			this.PassiveEffects.get(PassiveEffects_it).serialize(output);
		}
	}

}
