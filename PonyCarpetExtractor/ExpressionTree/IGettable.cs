using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree
{
    /// <summary>
    /// Représente une structure de donnée qui peut retourner une valeur.
    /// </summary>
    public interface IGettable
    {
        /// <summary>
        /// Retourne la valeur représentée par cet objet.
        /// </summary>
        object GetValue(Context context);
    }
}
