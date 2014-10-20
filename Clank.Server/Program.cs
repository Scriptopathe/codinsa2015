using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clank.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // 
            Engine serv = new Engine();
            serv.InitializeServer(5000);
            serv.WaitForConnectionsAsync();
            Console.WriteLine("Server started. Press enter to stop listening for connexion.");
            Console.ReadLine();
            serv.StopWaitingForConnections();
            Console.WriteLine("Clients connected, starting server.");
            serv.StartServerAsync();
            while(true)
            {
                Console.WriteLine("Running frame...");
                serv.RunOneFrame();
                System.Threading.Thread.Sleep(5000);
            }
            Console.WriteLine("Test ok");
            Console.ReadLine();
        }
    }
}
