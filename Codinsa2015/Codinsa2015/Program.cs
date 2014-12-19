using System;

namespace Codinsa2015
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameClient game = new GameClient())
            {
                game.Run();
            }

            // todo :
            // - dash : projection en arriere etc... (direction -> mais blocage)
            // - aggro des creeps
        }
    }
#endif
}

