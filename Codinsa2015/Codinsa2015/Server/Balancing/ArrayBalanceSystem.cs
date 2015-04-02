using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
namespace Codinsa2015.Server.Balancing
{
    /// <summary>
    /// Système d'équilibrage dont les valeurs des niveaux sont décrit dans un tableau.
    /// Le niveau 0 correspond à default(T) => 0 pour les float / int.
    /// Le niveau 1 correspond à la première valeur du tableau.
    /// Le niveau -1 correspond à l'opposé de la première valeur du tableau.
    /// </summary>
    public class ArrayBalanceSystem<T> : IBalanceSystem<T>
    {
        T[] m_array;

        /// <summary>
        /// Obtient le tableau interne de valeurs de l'array.
        /// </summary>
        public T[] Array { get {return m_array; } set {m_array = value;}}

        /// <summary>
        /// Récupère une valeur d'équilibrage correspondant à un niveau donné.
        /// </summary>
        public T this[int index]
        {
            get 
            {
                if (index == 0)
                    return default(T);
                else
                {
                    dynamic val = m_array[Math.Abs(index) - 1];
                    dynamic sign = Math.Sign(index);
                    return (T)(sign * val);
                }
            }
        }

        /// <summary>
        /// Conversion implicite d'un array en ArrayBalanceSystem.
        /// </summary>
        public static implicit operator ArrayBalanceSystem<T>(T[] t)
        {
            return new ArrayBalanceSystem<T>(t);
        }


        /// <summary>
        /// Crée une nouvelle instance de ArrayBalanceSystem avec le tableau donné.
        /// </summary>
        /// <param name="array"></param>
        public ArrayBalanceSystem(T[] array)
        {
            m_array = array;
        }

        public ArrayBalanceSystem() { }

    }
}
