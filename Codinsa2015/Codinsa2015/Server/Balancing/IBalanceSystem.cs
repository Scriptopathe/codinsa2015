using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Balancing
{
    /// <summary>
    /// Interface que doivent implémenter tous les systèmes d'équilibrage.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBalanceSystem<T>
    {
        /// <summary>
        /// Récupère une valeur d'équilibrage correspondant à un niveau donné.
        /// </summary>
        T this[int lvl] { get; }
    }
}
