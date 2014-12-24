using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace Codinsa2015.Views
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
        public static void Initialize(int port, string ip)
        {
            s_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s_socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
        }

        /// <summary>
        /// Envoie une commande dans le socket.
        /// </summary>
        /// <param name="data"></param>
        public static void Send(string data)
        {
            s_socket.Send(Encoding.UTF8.GetBytes(data.Length.ToString() + "\n" + data));
        }

        /// <summary>
        /// Recoit une commande depuis le socket.
        /// </summary>
        /// <returns></returns>
        public static string Receive()
        {
            // Représente le caractère '\n'.
            byte last = Encoding.UTF8.GetBytes(new char[] { '\n' })[0];

            // Récupère le nombre de données à lire
            string dataLengthStr = "";
            while(true)
            {
                int bytes = s_socket.Receive(s_smallBuffer);
                if (s_smallBuffer[0] == last)
                    break;
                dataLengthStr += Encoding.UTF8.GetString(s_smallBuffer);
            }


            string data = "";
            int dataLength = int.Parse(dataLengthStr);
            int totalBytes = 0;
            while(totalBytes < dataLength)
            {
                int bytes = s_socket.Receive(s_buffer, Math.Min(dataLength - totalBytes, s_buffer.Length), SocketFlags.None);
                totalBytes += bytes;
                data += Encoding.UTF8.GetString(s_buffer, 0, bytes);
            }

            return data;
        }
    }
}
