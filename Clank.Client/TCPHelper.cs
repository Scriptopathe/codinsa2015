using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace Clank.Client
{
    public static class TCPHelper
    {
        static Socket s_sock;
        /// <summary>
        /// Initialise le socket TCP sur le port passé en paramètre.
        /// </summary>
        /// <param name="port"></param>
        public static void Initialize(int port)
        {
            s_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s_sock.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
        }

        public static void Close()
        {
            s_sock.Close();
        }
        /// <summary>
        /// Reçoit un string depuis le socket donné.
        /// </summary>
        /// <param name="sock"></param>
        /// <returns></returns>
        public static string Receive()
        {
            s_sock.ReceiveTimeout = 30000;
            int bytesReceived = 0;
            byte[] buff = new byte[4048];
            StringBuilder builder = new StringBuilder();
            do
            {
                bytesReceived = s_sock.Receive(buff);
                Console.WriteLine("Received " + bytesReceived + " bytes.");
                builder.Append(Encoding.UTF8.GetString(buff, 0, bytesReceived));
            } while (bytesReceived == buff.Length);
            return builder.ToString();
        }

        /// <summary>
        /// Envoie un string vers le socket donné.
        /// </summary>
        /// <param name="sock"></param>
        public static void Send(string str)
        {
            s_sock.Send(Encoding.UTF8.GetBytes(str));
        }
    }

}
