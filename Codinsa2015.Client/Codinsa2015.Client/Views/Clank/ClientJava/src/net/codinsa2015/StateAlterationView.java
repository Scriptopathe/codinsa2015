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
import net.codinsa2015.StateAlterationSource.*;
import net.codinsa2015.StateAlterationModelView.*;
import net.codinsa2015.StateAlterationParametersView.*;


@SuppressWarnings("unused")
public class StateAlterationView
{


	public Integer Source;
	public StateAlterationSource SourceType;
	public StateAlterationModelView Model;
	public StateAlterationParametersView Parameters;
	public Float RemainingTime;
	public static StateAlterationView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		StateAlterationView _obj =  new StateAlterationView();
		// Source
		int _obj_Source = Integer.valueOf(input.readLine());
		_obj.Source = _obj_Source;
		// SourceType
		int _obj_SourceType = Integer.valueOf(input.readLine());
		_obj.SourceType = StateAlterationSource.fromValue(_obj_SourceType);
		// Model
		StateAlterationModelView _obj_Model = StateAlterationModelView.deserialize(input);
		_obj.Model = _obj_Model;
		// Parameters
		StateAlterationParametersView _obj_Parameters = StateAlterationParametersView.deserialize(input);
		_obj.Parameters = _obj_Parameters;
		// RemainingTime
		float _obj_RemainingTime = Float.valueOf(input.readLine());
		_obj.RemainingTime = _obj_RemainingTime;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// Source
		output.append(((Integer)this.Source).toString() + "\n");
		// SourceType
		output.append(((Integer)(this.SourceType.getValue())).toString() + "\n");
		// Model
		this.Model.serialize(output);
		// Parameters
		this.Parameters.serialize(output);
		// RemainingTime
		output.append(((Float)this.RemainingTime).toString() + "\n");
	}

}
