using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Reflection;
namespace Codinsa2015.DebugHumanControler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // Chargement de toutes les assemblys au démarrage
            // pour éviter des vieux laggs.
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            float f = Vector2.Distance(new Vector2(1, 2), new Vector2(1.5f, 3.5f));
            loadedAssemblies
                .SelectMany(x => x.GetReferencedAssemblies())
                .Distinct()
                .Where(y => loadedAssemblies.Any((a) => a.FullName == y.FullName) == false)
                .ToList()
                .ForEach(x => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(x)));
            
            using(GameClient client = new GameClient())
            {

                client.Run();
            }

        }
    }
}

