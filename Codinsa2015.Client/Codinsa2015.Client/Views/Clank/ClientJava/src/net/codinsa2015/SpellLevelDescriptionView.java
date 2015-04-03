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
import net.codinsa2015.SpellTargetInfoView.*;


@SuppressWarnings("unused")
public class SpellLevelDescriptionView
{


	// Cooldown de base du sort.
	public Float BaseCooldown;
	// Casting time du sort.
	public Float CastingTime;
	// Altération d'état appliquée pendant le casting time.
	public ArrayList<StateAlterationModelView> CastingTimeAlterations;
	// Indique la manière dont le ciblage du sort est effectué.
	public SpellTargetInfoView TargetType;
	// Effets à l'impact du sort. Ils sont appliqués une fois le casting time terminé.
	public ArrayList<StateAlterationModelView> OnHitEffects;
	public SpellLevelDescriptionView() {
		CastingTimeAlterations = new ArrayList<StateAlterationModelView>();
		TargetType = new SpellTargetInfoView();
		OnHitEffects = new ArrayList<StateAlterationModelView>();
	}

	public static SpellLevelDescriptionView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		SpellLevelDescriptionView _obj =  new SpellLevelDescriptionView();
		// BaseCooldown
		float _obj_BaseCooldown = Float.valueOf(input.readLine());
		_obj.BaseCooldown = _obj_BaseCooldown;
		// CastingTime
		float _obj_CastingTime = Float.valueOf(input.readLine());
		_obj.CastingTime = _obj_CastingTime;
		// CastingTimeAlterations
		ArrayList<StateAlterationModelView> _obj_CastingTimeAlterations = new ArrayList<StateAlterationModelView>();
		int _obj_CastingTimeAlterations_count = Integer.valueOf(input.readLine());
		for(int _obj_CastingTimeAlterations_i = 0; _obj_CastingTimeAlterations_i < _obj_CastingTimeAlterations_count; _obj_CastingTimeAlterations_i++) {
			StateAlterationModelView _obj_CastingTimeAlterations_e = StateAlterationModelView.deserialize(input);
			_obj_CastingTimeAlterations.add((StateAlterationModelView)_obj_CastingTimeAlterations_e);
		}
		_obj.CastingTimeAlterations = _obj_CastingTimeAlterations;
		// TargetType
		SpellTargetInfoView _obj_TargetType = SpellTargetInfoView.deserialize(input);
		_obj.TargetType = _obj_TargetType;
		// OnHitEffects
		ArrayList<StateAlterationModelView> _obj_OnHitEffects = new ArrayList<StateAlterationModelView>();
		int _obj_OnHitEffects_count = Integer.valueOf(input.readLine());
		for(int _obj_OnHitEffects_i = 0; _obj_OnHitEffects_i < _obj_OnHitEffects_count; _obj_OnHitEffects_i++) {
			StateAlterationModelView _obj_OnHitEffects_e = StateAlterationModelView.deserialize(input);
			_obj_OnHitEffects.add((StateAlterationModelView)_obj_OnHitEffects_e);
		}
		_obj.OnHitEffects = _obj_OnHitEffects;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// BaseCooldown
		output.append(((Float)this.BaseCooldown).toString() + "\n");
		// CastingTime
		output.append(((Float)this.CastingTime).toString() + "\n");
		// CastingTimeAlterations
		output.append(String.valueOf(this.CastingTimeAlterations.size()) + "\n");
		for(int CastingTimeAlterations_it = 0; CastingTimeAlterations_it < this.CastingTimeAlterations.size();CastingTimeAlterations_it++) {
			this.CastingTimeAlterations.get(CastingTimeAlterations_it).serialize(output);
		}
		// TargetType
		this.TargetType.serialize(output);
		// OnHitEffects
		output.append(String.valueOf(this.OnHitEffects.size()) + "\n");
		for(int OnHitEffects_it = 0; OnHitEffects_it < this.OnHitEffects.size();OnHitEffects_it++) {
			this.OnHitEffects.get(OnHitEffects_it).serialize(output);
		}
	}

}
