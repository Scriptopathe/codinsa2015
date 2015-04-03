using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Spells
{
    /// <summary>
    /// Représente un modèle de spell.
    /// </summary>
    public class SpellModel
    {
        static int s_id = 10; // les IDs de 0 à 10 sont réservés.
        /// <summary>
        /// ID du modèle de spell.
        /// </summary>
        [Clank.ViewCreator.Export("int", "ID du spell permettant de le retrouver dans le tableau de sorts du jeu.")]
        public int ID
        {
            get;
            private set;
        }
         
        /// <summary>
        /// Obtient le nom du modèle de spell.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Obtient la description de chaque niveau du sort.
        /// </summary>
        [Clank.ViewCreator.Export("List<SpellLevelDescriptionView>", "Obtient la liste des descriptions des niveaux de ce sort.")]
        public List<SpellLevelDescription> Levels { get; set; }

        public SpellModel() : base() { ID = s_id++; }
        public SpellModel(List<SpellLevelDescription> levels, string name)
        {
            Levels = levels;
            Name = name;
            ID = s_id++;
        }

        public Views.SpellModelView ToView()
        {
            Views.SpellModelView view = new Views.SpellModelView();
            view.ID = ID;
            view.Levels = new List<Views.SpellLevelDescriptionView>();
            foreach (var lvl in Levels) { view.Levels.Add(lvl.ToView()); }
            return view;
        }
    }
}
