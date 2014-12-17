using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
namespace Clank.Client
{


    class Program
    {
        static void Main(string[] args)
        {

            string s = JsonConvert.SerializeObject(new List<Object> () { new Vector2() { X = 1 }, new Vector2(), 5, new Vector2[] {new Vector2() { X = 6 } }});
            var val = JsonConvert.DeserializeObject<List<Object>>(s);

            TCPHelper.Initialize(5000);
            Console.WriteLine("Enter blbl to exit, or type a command.");
            State state = new State();
            Console.WriteLine(state.SetShipPosition(0, new Vector2() { X = 5, Y = 3 }));
            Console.WriteLine(state.GetShipPosition(0));
            Console.WriteLine(state.SetShipPosition(0, new Vector2() { X = 2, Y = 8 }));
            Console.WriteLine(state.GetShipPosition(0));
            /*while(true)
            {
                Console.Write("Clank.Client #> ");
                string cmd = Console.ReadLine();
                if (cmd == "blbl")
                    break;
                TCPHelper.Send(cmd);
                Console.WriteLine("[Debug] Sent bytes.");
                string resp = TCPHelper.Receive();
                Console.WriteLine(resp);
            }*/

            TCPHelper.Close();
            Console.ReadLine();
        }
    }
}
