using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Rendering
{
    /// <summary>
    /// Classe chargée du rendu de la scène lors de la phase de picks.
    /// </summary>
    public class PickPhaseRenderer
    {
        /// <summary>
        /// Renderer de la scène.
        /// </summary>
        SceneRenderer m_sceneRenderer;

        /// <summary>
        /// Crée une nouvelle instance de PickPhaseRenderer.
        /// </summary>
        /// <param name="renderer"></param>
        public PickPhaseRenderer(SceneRenderer renderer)
        {
            m_sceneRenderer = renderer;
        }

        /// <summary>
        /// Charge les éventuelles ressources dont a besoin de renderer.
        /// </summary>
        public void LoadContent()
        {

        }

        /// <summary>
        /// Dessine le lobby.
        /// </summary>
        public void Draw(SpriteBatch batch, GameTime time, RenderTarget2D output)
        {
            batch.GraphicsDevice.SetRenderTarget(output);
            // Obtention des données depuis le serveur
            switch(m_sceneRenderer.Mode)
            {
                case DataMode.Direct:
                    DrawDirect(time, batch);
                    break;
                case DataMode.Remote:
                    DrawRemote(time, batch);
                    break;
            }
        }

        /// <summary>
        /// Dessine le lobby en ayant une connexion à distance avec le serveur.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="batch"></param>
        void DrawRemote(GameTime time, SpriteBatch batch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dessine le lobby en ayant une connexion mémoire directe avec le serveur.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="batch"></param>
        void DrawDirect(GameTime time, SpriteBatch batch)
        {
            int w = (int)Ressources.ScreenSize.X;
            int h = (int)Ressources.ScreenSize.Y;
            var ctrl = m_sceneRenderer.GameServer.GetSrvScene().PickControler;

            batch.Begin();
            batch.GraphicsDevice.Clear(Color.LightGray);
            // Dessine les héros.
            int x = 5;
            int y;
            for (int team = 0; team <= 1; team++)
            {
                y = 20;
                foreach (var hero in ctrl.GetHeroes()[team])
                {
                    DrawHero(batch, new Rectangle(x, y, w / 2 - 10, 100), hero);
                    y += 110;
                }
                x += w / 2;
            }

            // Dessine les diverses informations sur l'état de la phase de picks.
            if (!ctrl.IsReadyToGo())
            {
                // Dessine le temps restant.
                y = h - 300;
                string s = ((int)(ctrl.GetCurrentTimeoutSeconds() - (DateTime.Now - ctrl.LastControlerUpdate).TotalSeconds)).ToString();
                float scale = 4.0f;
                Vector2 sz = Ressources.CourrierFont.MeasureString(s) * scale;
                x = (w - (int)sz.X) / 2;
                batch.DrawString(Ressources.CourrierFont, s, new Vector2(x, y), Color.Black, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.5f);

                y += 70;
                // Dessine les messages.
                x = 5;
                scale = 1.0f;
                string msg = m_sceneRenderer.GameServer.GetSrvScene().GetControlerByHeroId(ctrl.GetPickingHeroId()).HeroName + " : choisissez une compétence " + (ctrl.GetCurrentSpells() == ctrl.GetActiveSpells() ? "active" : "passive") + ".";
                sz = Ressources.CourrierFont.MeasureString(msg) * scale;
                x = (w - (int)sz.X) / 2;
                batch.DrawString(Ressources.CourrierFont, msg, new Vector2(x, y), Color.Black, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.5f);

                x = 5;
                y += 100;
                batch.DrawString(Ressources.CourrierFont, ctrl.CurrentMessage, new Vector2(x, y), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            }

            // Dessine les icones des compétences proposées.
            if (ctrl.IsReadyToGo())
            {
                string str = "Appuyez sur entrée pour démarer le jeu.";
                Vector2 size = Ressources.CourrierFont.MeasureString(str);
                y = h - 50;
                x = (w - (int)size.X) / 2;
                batch.DrawString(Ressources.CourrierFont, str, new Vector2(x, y), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            }
            else
            {
                x = 5;
                foreach (Server.Spells.Spell spell in ctrl.GetCurrentSpells())
                {
                    const int spellSize = 32;
                    y = h - spellSize - 4;
                    Rectangle dstRect = new Rectangle(x, y, spellSize, spellSize);

                    // Effet de surbrillance si un sort est survollé.
                    Color color = Color.White;
                    var ms = Input.GetMouseState();
                    if (dstRect.Contains(new Point((int)ms.X, (int)ms.Y)))
                    {
                        color = Color.Red;
                    }

                    batch.Draw(Ressources.GetSpellTexture(spell.Name),
                               dstRect,
                               null,
                               color,
                               0.0f,
                               Vector2.Zero,
                               SpriteEffects.None,
                               0.5f);
                    x += spellSize + 4;
                }
            }


            batch.End();
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

        /// <summary>
        /// Dessine le slot du héros donné à la position donnée.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="hero"></param>
        void DrawHero(SpriteBatch batch, Rectangle rect, Server.Entities.EntityHero hero)
        {
            float layerDepth = 0.5f;
            const int iconSize = 16;
            const int spellSize = 32;
            var ctrl = m_sceneRenderer.GameServer.GetSrvScene().PickControler;
            var scene = m_sceneRenderer.GameServer.GetSrvScene();
            var texture = ctrl.IsMyTurn(hero.ID) ? Ressources.MenuItemHover : Ressources.MenuItem;
            EnhancedGui.Drawing.DrawRectBox(batch, texture, rect, Color.White, layerDepth + 0.1f);

            int x = rect.X + 20;
            int y = rect.Y + 20;
            // Dessine l'icone du rôle du héros
            batch.Draw(GetIcon((Views.EntityHeroRole)hero.Role), new Rectangle(x, y, iconSize, iconSize), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);
            x += iconSize + 10;

            // Dessine le nom du héros.
            batch.DrawString(Ressources.CourrierFont, scene.GetControlerByHeroId(hero.ID).HeroName, new Vector2(x, y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);

            y += 25;
            x = rect.X + 20;
            // Dessine les sorts du héros.
            for (int i = 0; i < hero.Spells.Count; i++)
            {
                Rectangle iconRect = new Rectangle(x, y, spellSize, spellSize);
                batch.Draw(
                    Ressources.GetSpellTexture(hero.Spells[i].Name),
                    iconRect,
                    null,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    layerDepth);

                x += spellSize + 4;
            }
        }
    }
}
