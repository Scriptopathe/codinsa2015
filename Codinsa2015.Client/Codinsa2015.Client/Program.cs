using System;

namespace Codinsa2015.Client
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            /*
            using (Game1 game = new Game1())
            {
                game.Run();
            }*/
            Views.TCPHelper.Initialize(Codinsa2015.Graphics.Client.RemoteClient.__DEBUG_PORT, "127.0.0.1");
            Views.State state = new Views.State();
            Console.WriteLine("Client started...");
            while (true)
            {
                var entities = state.GetEntitiesInSight();

                if(entities.Count != 0)
                {
                    var entity = entities[0];
                    if(!state.IsAutoMoving())
                    {
                        Console.WriteLine("Moving to entity " + entity.ID + ", position = " + entity.Position);
                        state.StartMoveTo(entity.Position);
                    }
                }
            }
        }
    }
#endif
}

