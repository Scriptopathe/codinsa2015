import java.lang.*;

	public class StateAlterationView
	{

	
		public int Source;	
		public StateAlterationSource SourceType;	
		public StateAlterationModelView Model;	
		public StateAlterationParametersView Parameters;	
		public float RemainingTime;	
		public static StateAlterationView Deserialize(BufferedReader input) {
		try {
			StateAlterationView _obj =  new StateAlterationView();
			// Source
			int _obj_Source = Integer.Parse(input.readLine());
			_obj.Source = _obj_Source;
			// SourceType
			int _obj_SourceType = Integer.Parse(input.readLine());
			_obj.SourceType = StateAlterationSource.fromValue(_obj_SourceType);
			// Model
			StateAlterationModelView _obj_Model = StateAlterationModelView.deserialize(input);
			_obj.Model = _obj_Model;
			// Parameters
			StateAlterationParametersView _obj_Parameters = StateAlterationParametersView.deserialize(input);
			_obj.Parameters = _obj_Parameters;
			// RemainingTime
			float _obj_RemainingTime = Float.Parse(input.readLine());
			_obj.RemainingTime = _obj_RemainingTime;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// Source
			output.append(((Integer)this.Source).toString() + "\n");
			// SourceType
			output.append(((Integer)(this.SourceType.getValue())).toString() + "\n");
			// Model
			this.Model.serialize(output);
			// Parameters
			this.Parameters.serialize(output);
			// RemainingTime
			output.WriteLine(((Float)this.RemainingTime).toString() + "\n");
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
