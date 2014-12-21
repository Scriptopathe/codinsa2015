using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente un accès à un membre d'enum.
    /// </summary>
    public class EnumAccess : Evaluable
    {
        /// <summary>
        /// Nom du membre de l'énumération.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Expression évaluable sur laquelle on va vouloir faire un accès.
        /// </summary>
        public Evaluable Left { get; set; }
        /// <summary>
        /// Crée une nouvelle instance de EnumAccess.
        /// </summary>
        public EnumAccess()
        {

        }
    }
}
