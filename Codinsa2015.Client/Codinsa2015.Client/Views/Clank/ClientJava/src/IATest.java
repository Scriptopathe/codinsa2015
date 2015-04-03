import java.io.IOException;
import java.net.UnknownHostException;
import java.util.ArrayList;

import net.codinsa2015.*;

public class IATest {
	public static void main(String[] args) throws UnknownHostException, IOException {
		TCPHelper.Initialize("127.0.0.1", 5000, "IA Java mdr");
		
		State state = new State();

		SceneMode mode = state.GetMode();
		System.out.println(mode);
		ArrayList<Integer> spells = state.GetMySpells();
		int spellId = 0;
		while (true)
		{
			ArrayList<EntityBaseView> entities = state.GetEntitiesInSight();
			if (entities.size() != 0)
			{
				EntityBaseView entity = entities.get(0);
				if (!state.IsAutoMoving())
				{
					System.out.println("Moving to entity " + entity.ID + ", Position = " + entity.Position.X + ", " + entity.Position.Y);
					state.StartMoveTo(entity.Position);
				}
			}
		}	
	}

}
