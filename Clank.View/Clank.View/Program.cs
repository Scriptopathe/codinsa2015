using System;

namespace Clank.View
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ShipDemo game = new ShipDemo())
            {
                game.Run();
            }
        }
    }
#endif
}

