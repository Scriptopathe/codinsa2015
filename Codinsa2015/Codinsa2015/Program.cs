using System;
using Microsoft.Xna.Framework;
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
            try
            {

                if (generateClank)
                {
                    string root = @"..\..\..\Server\Views\Clank\";
                    var views = Clank.ViewCreator.Creator.CreateViews(System.Reflection.Assembly.GetExecutingAssembly());
                    views.Add("main.clank", Clank.ViewCreator.Creator.CreateMain(System.Reflection.Assembly.GetExecutingAssembly(), views));
                    views.Add("autoproject.clankproject", 
                        Clank.ViewCreator.Creator.CreateProjectFile("Codinsa2015",
                        // Server
                        "cs:\"..\"", 
                        // Clients
                        "cs:\"../../../../../Codinsa2015.Client/Codinsa2015.Client/Views\"\n" +
                        "cpp:\"../../../../../Codinsa2015.Client/Codinsa2015.Client/Views/Clank/ClientCpp/src\"\n" +
                        "h:\"../../../../../Codinsa2015.Client/Codinsa2015.Client/Views/Clank/ClientCpp/inc\"\n" + 
                        "java:\"../../../../../Codinsa2015.Client/Codinsa2015.Client/Views/Clank/ClientJava/src/net/codinsa2015\"\n", 

                        views).Replace("encoding=\"utf-16\"", ""));
                    foreach (var kvp in views)
                    {
                        System.IO.File.WriteAllText(root + kvp.Key, kvp.Value);
                    }
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine("Lancez Codinsa2015.DebugHumanControler pour une sortie graphique.");
            }

            Server.GameServer serv = new Server.GameServer();
            serv.Initialize();
            int gameTime = 0;
            int oldGameTime = 0;
            Console.WriteLine("Server started");
            while(true)
            {
                GameTime time = new GameTime(
                    new TimeSpan(0, 0, 0, 0, gameTime), 
                    new TimeSpan(0, 0, 0, 0, gameTime - oldGameTime));
                serv.Update(time);
                oldGameTime = gameTime;
                gameTime += 1000/60;
            }
        }
    }
#endif
}

