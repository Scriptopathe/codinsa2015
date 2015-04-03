import clr;
clr.AddReferenceToFileAndPath("Codinsa2015.Client");
from Codinsa2015 import TCPHelper
from Codinsa2015.Views.Client import *

print "Client python started !!"
state = State()
TCPHelper.Initialize(5000, "127.0.0.1", "ia python")
print "TCP helper initialized !"

# Debut de l'IA.
while(True):
    entities = state.GetEntitiesInSight()
    print "Entities in sight"
    if entities.Count != 0:
        entity = entities[0]
        if(not state.IsAutoMoving()):
            print "Moving to entity"
            state.StartMoveTo(entity.Position)


