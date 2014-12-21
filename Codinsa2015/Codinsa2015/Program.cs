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
            bool generateClank = true;
            if (generateClank)
            {
                string root = @"C:\Users\Josué\Documents\Josue\[Projets]\Projets C#\Clank\Codinsa2015\Codinsa2015\Server\Views\Clank\";
                var views = Clank.ViewCreator.Creator.CreateViews(System.Reflection.Assembly.GetExecutingAssembly());
                views.Add("main.clank", Clank.ViewCreator.Creator.CreateMain(System.Reflection.Assembly.GetExecutingAssembly(), views));
                views.Add("autoproject.clankproject", 
                    Clank.ViewCreator.Creator.CreateProjectFile("Codinsa2015", "cs:ServerCS", "cs:ClientCS", views).Replace("encoding=\"utf-16\"", ""));
                foreach (var kvp in views)
                {
                    System.IO.File.WriteAllText(root + kvp.Key, kvp.Value);
                }
            }

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

