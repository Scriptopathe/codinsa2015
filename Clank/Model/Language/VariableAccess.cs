using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente l'accès à une variable d'un type.
    /// </summary>
    public class VariableAccess : Evaluable
    {
        /// <summary>
        /// Expression évaluable sur laquelle on va vouloir faire un accès.
        /// </summary>
        public Evaluable Left { get; set; }
        /// <summary>
        /// Nom de la variable à laquelle accéder.
        /// </summary>
        public string VariableName { get; set; }

        /// <summary>
        /// Crée une nouvelle instance de VariableAccess.
        /// </summary>
        public VariableAccess()
        {
            
        }
    }
}
