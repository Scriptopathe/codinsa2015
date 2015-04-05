import java.io.IOException;
import java.net.UnknownHostException;
import java.util.ArrayList;

import net.codinsa2015.*;

public class IATest {
	public static void main(String[] args) throws UnknownHostException, IOException, InterruptedException {
		TCPHelper.Initialize("127.0.0.1", 5000, "IA Java mdr");
		Thread.sleep(1000);
		State state = new State();
		GameStaticDataView data = state.GetStaticData();
		System.out.println(data.Armors.size());
		
		SceneMode mode = state.GetMode();
		
		while(mode.getValue() == SceneMode.Pick.getValue())
			mode = state.GetMode();
		
		data = state.GetStaticData();
		System.out.println(data.RouterPositions.size());
		state.ShopGetWeapons();
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
