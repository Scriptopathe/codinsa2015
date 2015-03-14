using System;
using Microsoft.Xna.Framework;
namespace Codinsa2015.RemoteHumanControler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using(GameClient client = new GameClient())
            {

                client.Run();
            }

        }
    }
}

