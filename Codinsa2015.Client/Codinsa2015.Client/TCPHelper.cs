using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace Codinsa2015
{
    /// <summary>
    /// Contient un ensemble de fonction permettant à un client d'envoyer des données vers
    /// un socket.
    /// </summary>
    public static class TCPHelper
    {
        static Socket s_socket;
        static byte[] s_smallBuffer = new byte[1];
        static byte[] s_buffer = new byte[512];
        /// <summary>
        /// Initialise un client vers un serveur sur l'IP donnée.
        /// </summary>
        public static void Initialize(int port, string ip, string nickname)
        {
            s_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s_socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            Send(nickname);
        }

        /// <summary>
        /// Envoie une commande dans le socket.
        /// </summary>
        /// <param name="data"></param>
        public static void Send(string data)
        {
            Send(Encoding.UTF8.GetBytes(data));
        }
        /// <summary>
        /// Envoie une commande dans le socket.
        /// </summary>
        /// <param name="data"></param>
        public static void Send(byte[] data)
        {
            var tosend = Encoding.UTF8.GetString(data);
#if DEBUG
            Console.WriteLine("sent " + data.Length.ToString() + "bytes:\n" + Encoding.UTF8.GetString(data));
#endif
            s_socket.Send(Encoding.UTF8.GetBytes(data.Length.ToString() + "\n"));
            s_socket.Send(data);
        }
        /// <summary>
        /// Recoit une commande depuis le socket.
        /// </summary>
        /// <returns></returns>
        public static byte[] Receive()
        {
            // Représente le caractère '\n'.
            byte last = Encoding.UTF8.GetBytes(new char[] { '\n' })[0];

            // Récupère le nombre de données à lire
            List<byte> dataBytes = new List<byte>();
            while(true)
            {
                int bytes = s_socket.Receive(s_smallBuffer);
                if (s_smallBuffer[0] == last)
                    break;
                dataBytes.Add(s_smallBuffer[0]);
            }


            int dataLength = int.Parse(Encoding.UTF8.GetString(dataBytes.ToArray()));
            dataBytes.Clear();
            int totalBytes = 0;
            while(totalBytes < dataLength)
            {
                int bytes = s_socket.Receive(s_buffer, Math.Min(dataLength - totalBytes, s_buffer.Length), SocketFlags.None);
                totalBytes += bytes;
                for(int i = 0; i < bytes; i++)
                {
                    dataBytes.Add(s_buffer[i]);
                }
            }
#if DEBUG
            Console.WriteLine("received " + dataLength.ToString() + " bytes: " + Encoding.UTF8.GetString(dataBytes.ToArray()));
#endif
            return dataBytes.ToArray();
        }
    }
}
