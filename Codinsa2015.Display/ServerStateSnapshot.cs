using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Views;
using State = Codinsa2015.Views.Client.State;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Rendering
{
    /// <summary>
    /// Représente un agglomérat de données disponibles provenant du serveur distant.
    /// </summary>
    public class ServerStateSnapshot
    {
        State m_state;
        public MapView Map { get; private set; }
        public List<EntityBaseView> EntitiesInSight { get; private set; }
        public SceneMode SceneMode { get; private set; }

        /// <summary>
        /// Crée une nouvelle instance de snapshot à partir du serveur state donné.
        /// </summary>
        public ServerStateSnapshot(State state)
        {
            m_state = state;
        }

        /// <summary>
        /// Mets à jour le snapshot du serveur.
        /// </summary>
        public void UpdateSnapshot()
        {
            EntitiesInSight = m_state.GetEntitiesInSight();
            Map = m_state.GetMapView();

        }

        #region API
        /// <summary>
        /// Retourne la passabilité de la map à la position donnée en unités métriques.
        /// </summary>
        public bool GetPassabilityAt(float x, float y)
        {
            if (x < 0 || y < 0 || x >= Map.Passability.Count || y >= Map.Passability[0].Count ||
                float.IsNaN(x) || float.IsNaN(y))
                return false;

            return Map.Passability[(int)x][(int)y];
        }
        public bool GetPassabilityAt(Vector2 pos) { return GetPassabilityAt(pos.X, pos.Y); }
        #endregion
    }
}
