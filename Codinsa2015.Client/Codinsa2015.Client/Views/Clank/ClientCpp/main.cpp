#include "inc/Common.h"
#include "inc/State.h"

using namespace std;
int main()
{
	try
	{

		TCPHelper::initialize("127.0.0.1", 5000, "IA en C++ !!");
		State state;
		cout << "Client started" << endl;
		vector<int> spells = state.GetMySpells();
		int spellId = 0;
		while (true)
		{
			auto entities = state.GetEntitiesInSight();
			if (entities.size() != 0)
			{
				auto entity = entities[0];
				if (!state.IsAutoMoving())
				{
					cout << "Moving to entity " << entity.ID << ", Position = " << entity.Position.X << ", " << entity.Position.Y << endl;
					state.StartMoveTo(entity.Position);
				}
			}
		}
	}
	catch ( ... )
	{
		TCPHelper::terminate();
		cout << "Une erreur est survenue :'(" << endl;
	}
}