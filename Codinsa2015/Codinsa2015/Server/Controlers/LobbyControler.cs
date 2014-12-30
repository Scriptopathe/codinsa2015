using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.Entities;
using Codinsa2015.Graphics.Server;
namespace Codinsa2015.Server.Controlers
{
    /// <summary>
    /// Représente le contrôleur du lobby : 
    /// Cette classe est responsable de la maintenance de l'état du lobby, de sa mise
    /// à jour et de son affichage.
    /// </summary>
    public class LobbyControler
    {
        Scene m_scene;
        int m_playerId;
        int m_teamId;
        int m_controlledId;
        /// <summary>
        /// Crée une nouvelle instance du lobby.
        /// </summary>
        public LobbyControler(Scene s)
        {
            m_scene = s;
            m_playerId = 0;
            m_teamId = 0;
        }

        /// <summary>
        /// Mets à jour le lobby.
        /// </summary>
        public void Update(GameTime timeh)
        {
            m_controlledId = -1;

            // Obtient le nombre de joueurs.
            int[] playerCount = new int[2];

            lock (m_scene.ControlerLock)
            {
                foreach (var kvp in m_scene.Controlers)
                {
                    EntityHero hero = kvp.Value.Hero;
                    int team = ((int)(kvp.Value.Hero.Type & EntityType.Teams) >> 1) - 1; // 0 ou 1

                    if (team == m_teamId && playerCount[team] == m_playerId)
                        m_controlledId = hero.ID;

                    playerCount[team]++;
                }
            }

            // Input de l'utilisateur : sélection de joueurs
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Up))
                m_playerId--;
            else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Down))
                m_playerId++;

            // Sélection de la team
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Left) || Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Right))
                m_teamId = m_teamId == 0 ? 1 : 0;

            // Changement d'équipe.
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Space) && m_controlledId != -1)
                m_scene.Controlers[m_controlledId].Hero.Type ^= EntityType.Teams;

            if (m_playerId < 0)
                m_playerId = playerCount[m_teamId] - 1;
            if (m_playerId >= playerCount[m_teamId])
                m_playerId = 0;
        }

        /// <summary>
        /// Dessine le lobby.
        /// </summary>
        public void Draw(GameTime time, RemoteSpriteBatch batch)
        {
            int sw = (int)GameServer.GetScreenSize().X;
            int sh = (int)GameServer.GetScreenSize().Y;
            

            // Dessine l'avant plan.
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            batch.Server.Clear(Color.LightGray);
            int[] playerCount = new int[2];

            lock (m_scene.ControlerLock)
            {
                foreach (var kvp in m_scene.Controlers)
                {
                    EntityHero hero = kvp.Value.Hero;
                    int team = ((int)(kvp.Value.Hero.Type & EntityType.Teams) >> 1) - 1; // 0 ou 1

                    Rectangle rect = GetDrawRect(playerCount[team], team);
                    if (m_scene.Controlers[hero.ID].Hero.ID == m_controlledId)
                        Gui.Drawing.DrawRectBox(batch, Ressources.MenuItemHover, rect, Color.White, 0.2f);
                    else
                        Gui.Drawing.DrawRectBox(batch, Ressources.MenuItem, rect, Color.White, 0.2f);


                    string s = kvp.Value.HeroName + " (" + kvp.Value.GetType().Name.Replace("Controler", "") + ")";
                    Vector2 size = Ressources.CourrierFont.MeasureString(s);
                    Vector2 offset = (new Vector2(rect.Width, rect.Height) - size) / 2;


                    batch.DrawString(Ressources.CourrierFont, s, new Vector2(rect.X, rect.Y) + offset, Color.White, 0.0f, Vector2.Zero, 1f, 0.0f);
                    playerCount[team]++;
                }
            }
            // VS
            int oy = (1 * sh / 3);
            int scale = 8;
            string str = "VS";
            Vector2 strsize = Ressources.CourrierFont.MeasureString(str) * scale;
            batch.DrawString(Ressources.CourrierFont, str, new Vector2((sw - (int)strsize.X) / 2, (-oy/2 + sh - (int)strsize.Y) / 2), Color.Black, 0.0f, Vector2.Zero, scale, 0.0f);


            // Instructions
            str = "Appuyez sur Entrée pour lancer le jeu.";
            strsize = Ressources.CourrierFont.MeasureString(str);
            batch.DrawString(Ressources.CourrierFont, str, new Vector2((sw - (int)strsize.X) / 2, sh - 25), Color.Black);
            batch.End();
        }


        /// <summary>
        /// Obtient le rectangle sur lequel dessiner cadre du joueur d'id donné dans une équipe.
        /// </summary>
        /// <param name="playerNumber"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        Rectangle GetDrawRect(int playerNumber, int team)
        {
            int sw = (int)GameServer.GetScreenSize().X;
            int sh = (int)GameServer.GetScreenSize().Y;

            // Largeur / hauteur des cadres.
            const int w = 200;
            const int h = 25;

            int ox = (sw / 2 - w) / 2;
            int sx = team * (sw / 2);
            int sy = 1 * sh / 3;

            return new Rectangle(sx + ox, sy + playerNumber * (h + 10), w, h);
        }
    }
}
