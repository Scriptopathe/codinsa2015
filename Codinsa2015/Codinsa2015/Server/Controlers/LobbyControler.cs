using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Controlers
{
    /// <summary>
    /// Représente le contrôleur du lobby.
    /// </summary>
    public class LobbyControler
    {
        #region Variables

        #endregion

        #region Properties
        /// <summary>
        /// Obtient l'id du héros sélectionné dans le lobby.
        /// </summary>
        public int SelectedHeroId
        {
            get;
            set;
        }

        #endregion

        public LobbyControler() { }
    }
}
