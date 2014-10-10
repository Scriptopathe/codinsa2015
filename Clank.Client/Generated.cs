using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clank.Client
{
    public class Vector2
    {
        public int X;
        public int Y;
        public override string ToString()
        {
            return "{ " + X.ToString() + ", " + Y.ToString() + " }";
        }
    }

    #region Auto


    public class Ship<T>
    {

        public int Speed;
        public string Name;
        public T Position;
        public Ship(T pos)
        {
            Speed = 5;
            Name = "Spitfire";
            Position = pos;
        }
    }
    /// <summary>
    /// Contient toutes les informations concernant l'état du serveur.
    /// </summary>
    public class State
    {

        public Ship<Vector2> GetShip(int index)
        {
            // Send
            List<object> args = new List<object>() { index };
            int funcId = 0;
            List<object> obj = new List<object>() { funcId, args };
            TCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            // Receive
            string str = TCPHelper.Receive();
            Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
            return (Ship<Vector2>)o[0].ToObject(typeof(Ship<Vector2>));
        }

        public Vector2 GetShipPosition(int index)
        {
            // Send
            List<object> args = new List<object>() { index };
            int funcId = 1;
            List<object> obj = new List<object>() { funcId, args };
            TCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            // Receive
            string str = TCPHelper.Receive();
            Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
            return (Vector2)o[0].ToObject(typeof(Vector2));
        }

        public bool SetShipPosition(int index, Vector2 position)
        {
            // Send
            List<object> args = new List<object>() { index, position };
            int funcId = 2;
            List<object> obj = new List<object>() { funcId, args };
            TCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            // Receive
            string str = TCPHelper.Receive();
            Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
            return o.Value<bool>(0);
        }

        public bool MoveShip(int index, Vector2 dir)
        {
            // Send
            List<object> args = new List<object>() { index, dir };
            int funcId = 3;
            List<object> obj = new List<object>() { funcId, args };
            TCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            // Receive
            string str = TCPHelper.Receive();
            Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
            return o.Value<bool>(0);
        }

    }


    #endregion

}
