using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Views;
namespace Codinsa2015.Display
{    
    public enum DataMode
    {
        // Indique que le renderer doit faire un rendu d'un objet correspondant
        // exactement à l'objet détenu par le serveur.
        Direct,
        // Indique que le renderer doit faire un rendu d'une vue de l'objet détenu
        // par le serveur.
        Remote,
    }
    /// <summary>
    /// Représente une vue paramétrable de la scène du jeu.
    /// Cette scène doit posséder une instance de l'état du jeu à jour,
    /// et l'appel à draw dessine sur une texture renvoyée en sortie.
    /// </summary>
    public class SceneRenderer
    {
        #region Variables
        bool __dirty = true;
        /// <summary>
        /// Viewport de rendu de la scène.
        /// </summary>
        Rectangle m_viewport;

        /// <summary>
        /// Mode dans lequel se trouve la scène.
        /// </summary>
        Codinsa2015.Views.SceneMode m_sceneMode;

        /// <summary>
        /// Renderer de la map.
        /// </summary>
        MapRenderer m_mapRenderer;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit la résolution de la texture sur laquelle va être rendue la scène.
        /// </summary>
        public Rectangle Viewport
        {
            get { return m_viewport; }
            set { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Obtient une donnée représentant la manière dont vont être récupérées les informations à dessiner
        /// (à distance, ou directement sur le moteur).
        /// </summary>
        public DataMode Mode { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de Scene Renderer avec les paramètres donnés.
        /// </summary>
        /// <param name="mode"></param>
        public SceneRenderer(DataMode mode)
        {
            m_mapRenderer = new MapRenderer(this);
        }

        /// <summary>
        /// Mets à jour l'état de la scène à partir de l'objet state communiquant avec
        /// le serveur.
        /// </summary>
        /// <param name="state"></param>
        public void UpdateRemoteState(Codinsa2015.Views.Client.State state)
        {
            __dirty = false;
            throw new NotImplementedException();
        }
        /// <summary>
        /// Dessine cette scène.
        /// </summary>
        public void Draw()
        {
            if (Mode == DataMode.Remote && __dirty)
                throw new Exception("Appel à UpdateRemoteState manquant pour un renderer en mode Remote.");

            switch (m_sceneMode)
            {
                case SceneMode.Game:
                    DrawGameMode();
                    break;
                case SceneMode.Lobby:
                    DrawLobby();
                    break;
                case SceneMode.Pick:
                    DrawPickPhase();
                    break;
            }

            __dirty = true;
        }

        #region Draw
        /// <summary>
        /// Dessine la scène lorsqu'elle est en mode "jeu".
        /// </summary>
        void DrawGameMode()
        {

        }

        /// <summary>
        /// Dessine la scène pendant le lobby.
        /// </summary>
        void DrawLobby()
        {

        }

        /// <summary>
        /// Dessine la scène pendant la phase de pick.
        /// </summary>
        void DrawPickPhase()
        {

        }
        #endregion
        #endregion
    }
}
