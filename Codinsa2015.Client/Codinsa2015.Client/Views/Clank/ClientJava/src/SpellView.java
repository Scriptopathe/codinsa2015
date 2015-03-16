import java.lang.*;

	public class SpellView
	{

	
		public float CurrentCooldown;	
		public int SourceCaster;	
		public ArrayList<SpellDescriptionView> Levels;	
		public int Level;	
		public static SpellView Deserialize(BufferedReader input) {
		try {
			SpellView _obj =  new SpellView();
			// CurrentCooldown
			float _obj_CurrentCooldown = Float.Parse(input.readLine());
			_obj.CurrentCooldown = _obj_CurrentCooldown;
			// SourceCaster
			int _obj_SourceCaster = Integer.Parse(input.readLine());
			_obj.SourceCaster = _obj_SourceCaster;
			// Levels
			ArrayList<SpellDescriptionView> _obj_Levels = new ArrayList<SpellDescriptionView>();
			int _obj_Levels_count = Integer.Parse(input.readLine());
			for(int _obj_Levels_i = 0; _obj_Levels_i < _obj_Levels_count; _obj_Levels_i++) {
				SpellDescriptionView _obj_Levels_e = SpellDescriptionView.deserialize(input);
				_obj_Levels.add((SpellDescriptionView)_obj_Levels_e);
			}
			_obj.Levels = _obj_Levels;
			// Level
			int _obj_Level = Integer.Parse(input.readLine());
			_obj.Level = _obj_Level;
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
			return _obj;
		}

		public void serialize(OutputStreamWriter output) {
			try {
			// CurrentCooldown
			output.WriteLine(((Float)this.CurrentCooldown).toString() + "\n");
			// SourceCaster
			output.append(((Integer)this.SourceCaster).toString() + "\n");
			// Levels
			output.append(this.Levels.size().toString() + "\n");
			for(int Levels_it = 0; Levels_it < this.Levels.size();Levels_it++) {
				this.Levels[Levels_it].serialize(output);
			}
			// Level
			output.append(((Integer)this.Level).toString() + "\n");
			} catch (UnsupportedEncodingExceptio e) { 
			} catch (IOException e) { }
		}

	}
}
