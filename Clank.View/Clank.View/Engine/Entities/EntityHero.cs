using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Entities
{
    /// <summary>
    /// Classe de base pour toutes les entités héros.
    /// </summary>
    public class EntityHero : EntityBase
    {
        #region Variables
        /// <summary>
        /// Liste de spells accessibles pour ce héros.
        /// </summary>
        List<Spells.SpellBase> m_spells;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit la liste des spells accessibles pour ce héros.
        /// </summary>
        public List<Spells.SpellBase> Spells
        {
            get { return m_spells; }
            set { m_spells = value; }
        }
        #endregion

        #region Methods

        #endregion

    }
}
