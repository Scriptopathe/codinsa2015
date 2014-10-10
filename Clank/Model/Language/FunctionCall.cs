using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Model.Language
{
    /// <summary>
    /// Représente un appel de fonction dans le langage clank.
    /// </summary>
    public class FunctionCall : Evaluable
    {
        /// <summary>
        /// Fonction appelée.
        /// </summary>
        public Function Func { get; set; }
        /// <summary>
        /// Arguments passés à la fonction.
        /// </summary>
        public List<Evaluable> Arguments { get; set; }
        /// <summary>
        /// Objet source sur lequel est éxécuté la fonction (si méthode d'instance).
        /// </summary>
        public Evaluable Src { get; set; }

        /// <summary>
        /// Indique si cet appel de fonction est un appel de constructeur.
        /// </summary>
        public bool IsConstructor { get; set; }
    }
}
