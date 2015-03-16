import java.lang.*;

	public class StateAlterationParametersView
	{

	
		public Vector2 DashTargetDirection;	
		public int DashTargetEntity;	
		public static StateAlterationParametersView Deserialize(BufferedReader input) {
		try {
			StateAlterationParametersView _obj =  new StateAlterationParametersView();
			// DashTargetDirection
			Vector2 _obj_DashTargetDirection = Vector2.deserialize(input);
			_obj.DashTargetDirection = _obj_DashTargetDirection;
			// DashTargetEntity
			int _obj_DashTargetEntity = Integer.Parse(input.readLine());
			_obj.DashTargetEntity = _obj_DashTargetEntity;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// DashTargetDirection
			this.DashTargetDirection.serialize(output);
			// DashTargetEntity
			output.append(((Integer)this.DashTargetEntity).toString() + "\n");
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
