using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Equip
{
    /// <summary>
    /// Classe de base pour représenter une armure.
    /// </summary>
    public class Armor
    {
        /// <summary>
        /// Obtient ou définit la liste des altérations d'état données par cette
        /// armure.
        /// </summary>
        public List<Entities.StateAlterationModel> Alterations { get; set; }

        /// <summary>
        /// Nom de l'armure.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Prix de l'armure en points d'amélioration.
        /// </summary>
        public float Price { get; set; }


        /// <summary>
        /// Crée une nouvelle instance d'armor.
        /// </summary>
        public Armor()
        {
            Alterations = new List<Entities.StateAlterationModel>();
        }
    }
}
