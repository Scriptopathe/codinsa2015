using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Balancing
{
    /// <summary>
    /// Système d'équilibrage dont les valeurs des niveaux sont décrits par une fonction linéaire.
    /// Dans tous les cas : this[0] => 0
    /// Sinon this[x] => startValue + x * scalingValue
    /// </summary>
    public class LinearBalanceSystem<T> : IBalanceSystem<T>
    {
        T m_startValue;
        T m_scalingValue;
        /// <summary>
        /// Récupère une valeur d'équilibrage correspondant à un niveau donné.
        /// </summary>
        public T this[int index]
        {
            get 
            {
                if (index == 0)
                    return default(T);

                dynamic startValue = m_startValue;
                dynamic scalingValue = m_scalingValue;
                return (T)(startValue + scalingValue * (index - 1));
            }
        }

        public T StartValue { get { return m_startValue; } set { m_startValue = value; } }
        public T ScalingValue { get { return m_scalingValue; } set { m_scalingValue = value; } }

        /// <summary>
        /// Crée une nouvelle instance de ArrayBalanceSystem avec le tableau donné.
        /// </summary>
        /// <param name="scalingValue"></param>
        public LinearBalanceSystem(T startValue, T scalingValue)
        {
            m_startValue = startValue;
            m_scalingValue = scalingValue;
        }

        public LinearBalanceSystem() { }
    }
}
