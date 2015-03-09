using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.Entities;
using Codinsa2015.Rendering;
namespace Codinsa2015.Rendering
{
    /// <summary>
    /// Cette classe est responsable de l'affichage de l'état du lobby tel que décrit sur le serveur
    /// distant.
    /// </summary>
    public class LobbyRenderer
    {
        #region Variables
        SceneRenderer m_sceneRenderer;
        #endregion

        #region Methods
        #endregion


        /// <summary>
        /// Crée une nouvelle instance du lobby.
        /// </summary>
        public LobbyRenderer(SceneRenderer s)
        {
            m_sceneRenderer = s;
        }


        /// <summary>
        /// Charge le content dont a besoin le renderer.
        /// </summary>
        public void LoadContent() { }

        /// <summary>
        /// Dessine le lobby.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="time"></param>
        public void Draw(SpriteBatch batch, GameTime time, RenderTarget2D output)
        {
            int sw = (int)Ressources.ScreenSize.X;
            int sh = (int)Ressources.ScreenSize.Y;
            const int iconSize = 16;

            // Dessine l'avant plan.
            batch.GraphicsDevice.SetRenderTarget(output);
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            batch.GraphicsDevice.Clear(Color.LightGray);
            int[] playerCount = new int[2];

            // Dessine les héros connectés.
            switch(m_sceneRenderer.Mode)
            {
                case DataMode.Direct:
                    var scene = m_sceneRenderer.GameServer.GetSrvScene();
                    lock (scene.ControlerLock)
                    {
                        foreach (var kvp in scene.Controlers)
                        {
                            Server.Entities.EntityHero hero = kvp.Value.Hero;
                            int team = ((int)(hero.Type & Server.Entities.EntityType.Teams) >> 1) - 1; // 0 ou 1

                            Rectangle rect = GetDrawRect(playerCount[team], team);
                            if (hero.ID == scene.LobbyControler.SelectedHeroId)
                                EnhancedGui.Drawing.DrawRectBox(batch, Ressources.MenuItemHover, rect, Color.White, 0.2f);
                            else
                                EnhancedGui.Drawing.DrawRectBox(batch, Ressources.MenuItem, rect, Color.White, 0.2f);


                            string s = kvp.Value.HeroName + " (" + kvp.Value.GetType().Name.Replace("Controler", "") + ")";
                            Vector2 size = Ressources.CourrierFont.MeasureString(s);
                            Vector2 offset = (new Vector2(size.X, rect.Height) - size) / 2;

                            batch.Draw(GetIcon((Views.EntityHeroRole)hero.Role), new Rectangle(rect.X + 2, rect.Y + 4, iconSize, iconSize), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                            batch.DrawString(Ressources.CourrierFont, s, new Vector2(rect.X + (iconSize + 6), rect.Y) + offset, Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                            playerCount[team]++;
                        }
                    }

                    break;
                case DataMode.Remote:
                    throw new NotImplementedException();
            }


            // VS
            int oy = (1 * sh / 3);
            int scale = 6;
            string str = "VS";
            Vector2 strsize = Ressources.CourrierFont.MeasureString(str) * scale;
            batch.DrawString(Ressources.CourrierFont, str, new Vector2((sw - (int)strsize.X) / 2, (-oy / 2 + sh - (int)strsize.Y) / 2), Color.Black, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);


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
        public Rectangle GetDrawRect(int playerNumber, int team)
        {
            int sw = (int)Ressources.ScreenSize.X;
            int sh = (int)Ressources.ScreenSize.Y;

            // Largeur / hauteur des cadres.
            const int w = 250;
            const int h = 25;

            int ox = (sw / 2 - w) / 2;
            int sx = team * (sw / 2);
            int sy = 1 * sh / 3;

            return new Rectangle(sx + ox, sy + playerNumber * (h + 10), w, h);
        }        
        
        /// <summary>
        /// Obtient l'icone du rôle donné.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Texture2D GetIcon(Views.EntityHeroRole role)
        {
            switch (role)
            {
                case Views.EntityHeroRole.Fighter:
                    return Ressources.IconFighter;
                case Views.EntityHeroRole.Mage:
                    return Ressources.IconMage;
                case Views.EntityHeroRole.Tank:
                    return Ressources.IconTank;
            }
            return Ressources.IconTank;
        }

    }
}
