using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree
{
    /// <summary>
    /// Représente un objet dont la valeur est mutable.
    /// Il s'agit de la classe de Gettable/Settable la plus simple.
    /// </summary>
    public class Mutable : IGettable, ISettable
    {
        /// <summary>
        /// Valeur de cet objet Gettable.
        /// </summary>
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Retourne la valeur de l'objet Gettable.
        /// </summary>
        /// <param name="context">ignoré</param>
        /// <returns></returns>
        public object GetValue(Context context)
        {
            return Value;
        }
        /// <summary>
        /// Change la valeur représentée par cet ISettable.
        /// </summary>
        public void SetValue(Context context, object value)
        {
            Value = value;
        }
        /// <summary>
        /// Constructeur pour le type MutableGettable.
        /// </summary>
        /// <param name="val"></param>
        public Mutable(object val)
        {
            Value = val;
        }
    }
}
