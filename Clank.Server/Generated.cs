using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clank.Server
{
    #region AUTO
    public class Vector2
    {
        public int X;
        public int Y;
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
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

        List<Ship<Vector2>> Ships;
        public State()
        {
            Ships = new List<Ship<Vector2>>() {
                new Ship<Vector2>(new Vector2(0, 0)),
                new Ship<Vector2>(new Vector2(5, 5))
            };
        }
        public Ship<Vector2> GetShip(int index)
        {
            return this.Ships[index];
        }
        public Vector2 GetShipPosition(int index)
        {
            return this.Ships[index].Position;
        }
        public bool SetShipPosition(int index, Vector2 position)
        {
            this.Ships[index].Position = position;
            return true;
        }
        public bool MoveShip(int index, Vector2 dir)
        {
            Ship<Vector2> ship = this.Ships[index];
            ship.Position.X = (ship.Position.X + dir.X);
            ship.Position.Y = (ship.Position.X + dir.X);
            return true;
        }
        /// <summary>
        /// Génère le code pour la fonction de traitement des messages.
        /// </summary>
        public string ProcessRequest(string request, int clientId)
        {
            Newtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(request);
            int functionId = o.Value<int>(0);
            switch (functionId)
            {
                case 0:
                    int arg0_0 = o[1].Value<int>(0);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { GetShip(arg0_0) });
                case 1:
                    int arg1_0 = o[1].Value<int>(0);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { GetShipPosition(arg1_0) });
                case 2:
                    int arg2_0 = o[1].Value<int>(0);
                    Vector2 arg2_1 = (Vector2)o[1][1].ToObject(typeof(Vector2));
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { SetShipPosition(arg2_0, arg2_1) });
                case 3:
                    int arg3_0 = o[1].Value<int>(0);
                    Vector2 arg3_1 = (Vector2)o[1][1].ToObject(typeof(Vector2));
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { MoveShip(arg3_0, arg3_1) });
            }
            return "";
        }

    }


    #endregion
}
