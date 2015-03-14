using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.Entities;
using Codinsa2015.Rendering;
namespace Codinsa2015.RemoteHumanControler
{
    /// <summary>
    /// Représente le contrôleur du lobby : 
    /// Cette classe est responsable de la maintenance de l'état du lobby, de sa mise
    /// à jour et de son affichage.
    /// </summary>
    public class LobbyControler
    {
        #region Variables
        GameClient m_client;
        int m_playerId;
        int m_teamId;
        #endregion

        #region Methods
        /// <summary>
        /// Obtient ou définit l'id du joueur 
        /// </summary>
        public int PlayerId 
        {
            get { return m_playerId; } 
            set 
            { 
                m_playerId = value;
            }
        }
        public int TeamId { get { return m_teamId; } set { m_teamId = value; } }
        #endregion


        /// <summary>
        /// Crée une nouvelle instance du lobby.
        /// </summary>
        public LobbyControler(GameClient s)
        {
            m_client = s;
            m_playerId = 0;
            m_teamId = 0;
        }

        /// <summary>
        /// Mets à jour le lobby.
        /// </summary>
        public void Update(GameTime time)
        {
            if (m_client.Renderer.GetSceneMode() != Views.SceneMode.Lobby)
                return;
            // Input de l'utilisateur : sélection de joueurs
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Up))
                m_playerId--;
            else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Down))
                m_playerId++;

            // Sélection de la team
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Left) || Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Right))
                m_teamId = m_teamId == 0 ? 1 : 0;


            // Obtient le nombre de joueurs.
            int[] playerCount = new int[2];
            switch(m_client.Renderer.Mode)
            {
                case DataMode.Direct:
                    var scene = m_client.Renderer.GameServer.GetSrvScene();
                    scene.LobbyControler.SelectedHeroId = -1;
                    lock (scene.ControlerLock)
                    {
                        foreach (var kvp in scene.Controlers)
                        {
                            EntityHero hero = kvp.Value.Hero;
                            int team = ((int)(kvp.Value.Hero.Type & EntityType.Teams) >> 1) - 1; // 0 ou 1

                            if (team == m_teamId && playerCount[team] == m_playerId)
                                scene.LobbyControler.SelectedHeroId = hero.ID;

                            playerCount[team]++;
                        }
                    }

                    // Changement d'équipe.
                    if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Space) && scene.LobbyControler.SelectedHeroId != -1)
                        scene.Controlers[scene.LobbyControler.SelectedHeroId].Hero.Type ^= EntityType.Teams;

                    // Changement de rôle
                    if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.A))
                        scene.Controlers[scene.LobbyControler.SelectedHeroId].Hero.Role = (EntityHeroRole)(((int)scene.Controlers[scene.LobbyControler.SelectedHeroId].Hero.Role + 1) % ((int)EntityHeroRole.Max + 1));

                    break;
                case DataMode.Remote:
                    throw new NotImplementedException();
            }


            if (m_playerId < 0)
                m_playerId = playerCount[m_teamId] - 1;
            if (m_playerId >= playerCount[m_teamId])
                m_playerId = 0;
        }


    }
}
