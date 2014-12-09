using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clank.View.Engine.Entities;
namespace Clank.View.Engine.Equip
{
    /// <summary>
    /// Classe permettant la présentation de divers équipements.
    /// 
    /// Les shops sont chargés depuis des fichiers xml.
    /// </summary>
    public class Shop
    {


        /// <summary>
        /// Retourne une liste d'elixirs disponibles pour le héros donné.
        /// </summary>
        /// <param name="hero"></param>
        /// <returns></returns>
        public List<Armor> GetElixirs(EntityHero hero)
        {
            return new List<Armor>();
        }

        /// <summary>
        /// Retourne une liste d'armures disponibles pour le héros donné.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<Armor> GetArmors(EntityHero hero)
        {
            return new List<Armor>();
        }
    }
}
