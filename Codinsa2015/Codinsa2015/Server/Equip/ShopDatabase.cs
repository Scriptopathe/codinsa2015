using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Equip
{
    /// <summary>
    /// Représente une base de données contenant des instances d'armes, armures etc...
    /// </summary>
    public class ShopDatabase
    {
        /// <summary>
        /// Armes contenues dans la base de données.
        /// </summary>
        public List<WeaponModel> Weapons { get; set; }
        /// <summary>
        /// Enchantements contenus dans la base de données.
        /// </summary>
        public List<WeaponEnchantModel> Enchants { get; set; }
        /// <summary>
        /// Armures contenues dans la base de données.
        /// </summary>
        public List<PassiveEquipmentModel> Armors { get; set; }

        /// <summary>
        /// Bottes contenues dans la base de données.
        /// </summary>
        public List<PassiveEquipmentModel> Boots { get; set; }
        /// <summary>
        /// Crée une nouvelle instance de ShopDatabase vide.
        /// </summary>
        public ShopDatabase()
        {
            Weapons = new List<WeaponModel>();
            Enchants = new List<WeaponEnchantModel>();
            Armors = new List<PassiveEquipmentModel>();
            Boots = new List<PassiveEquipmentModel>();
        }

        /// <summary>
        /// Charge une base de données depuis un fichier dont le chemin d'accès est passé en paramètre.
        /// </summary>
        /// <returns></returns>
        public static ShopDatabase Load(string file)
        {
            return Tools.Serializer.Deserialize<ShopDatabase>(System.IO.File.ReadAllText(file));
        }
        
        /// <summary>
        /// Sauvegarde la base de données dans le fichier dont le chemin d'accès est passé en paramètre.
        /// </summary>
        /// <param name="file"></param>
        public void Save(string file)
        {
            System.IO.File.WriteAllText(file, Tools.Serializer.Serialize<ShopDatabase>(this));
        }
    }
}
