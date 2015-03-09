using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.Entities;

namespace Codinsa2015.Server.Controlers
{
    /// <summary>
    /// Représente le contrôleur du lobby : 
    /// Cette classe est responsable de la maintenance de l'état du lobby, de sa mise
    /// à jour et de son affichage.
    /// </summary>
    public class PickPhaseControler
    {
        const float HumanTimeoutSeconds = 30;
        const float IATimeoutSeconds = 2;

        #region Variables
        Scene m_scene;
        /// <summary>
        /// Liste des héros de chaque équipe.
        /// </summary>
        List<List<EntityHero>> m_heroes;
        List<Spells.Spell> m_activeSpells;
        List<Spells.Spell> m_passiveSpells;

        /// <summary>
        /// Représente le numéro du tour de pick actuel.
        /// </summary>
        int m_pickTurn;
        /// <summary>
        /// Obtient la date de dernière réponse du contrôleur dont c'est le tour.
        /// </summary>
        DateTime m_lastControlerUpdate;
        /// <summary>
        /// Position de la "souris" virtuelle.
        /// La souris virtuelle est utilisée par le contrôleur humain pour intéragir avec ce contrôleur.
        /// </summary>
        Vector2 m_virtualMousePos;

        string m_currentMessage = "";
        #endregion

        #region Properties

        
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance du lobby.
        /// 
        /// Assume que le nombre de héros présents est égal dans chaque équipe.
        /// </summary>
        public PickPhaseControler(Scene s, List<EntityHero> heroes)
        {
            m_scene = s;
            m_lastControlerUpdate = DateTime.Now;

            // Ajoute les héros au contrôleur.
            m_heroes = new List<List<EntityHero>>();
            m_heroes.Add(heroes.Where(new Func<EntityHero, bool>((EntityHero h) =>
            {
                return h.Type.HasFlag(EntityType.Team1);
            })).ToList());
            m_heroes.Add(heroes.Where(new Func<EntityHero, bool>((EntityHero h) =>
            {
                return h.Type.HasFlag(EntityType.Team2);
            })).ToList());


            // Passive spells
            m_passiveSpells = new List<Spells.Spell>() { new Spells.DashForwardSpell(null), new Spells.FireballSpell(null), new Spells.MovementSpeedBuffSpell(null), new Spells.TargettedTowerSpell(null),
                                                         new Spells.DashForwardSpell(null), new Spells.FireballSpell(null), new Spells.MovementSpeedBuffSpell(null), new Spells.TargettedTowerSpell(null) };

            m_activeSpells = new List<Spells.Spell>() { new Spells.DashForwardSpell(null), new Spells.FireballSpell(null), new Spells.MovementSpeedBuffSpell(null), new Spells.TargettedTowerSpell(null),
                                                         new Spells.DashForwardSpell(null), new Spells.FireballSpell(null), new Spells.MovementSpeedBuffSpell(null), new Spells.TargettedTowerSpell(null) };
        }

        /// <summary>
        /// Mets à jour le lobby.
        /// </summary>
        public void Update(GameTime timeh)
        {
            bool controlerIsIA = m_scene.GetControlerByHeroId(GetPickingHeroId()) is Controlers.IAControler;
            // Vérifie que l'IA ne timeout pas.
            float elapsedTimeSinceLastUpdate = (float)(DateTime.Now - m_lastControlerUpdate).TotalSeconds;

            if (controlerIsIA)
            {
                if (elapsedTimeSinceLastUpdate > IATimeoutSeconds)
                {

                    // Affiche le timeout.
                    string iaName = m_scene.GetControlerByHeroId(GetPickingHeroId()).HeroName;
                    m_currentMessage = iaName + " : temps expiré ! Aucune compétence choisie !";


                    m_lastControlerUpdate = DateTime.Now;
                    m_pickTurn++;

                }
            }
            else
                if (elapsedTimeSinceLastUpdate > HumanTimeoutSeconds)
                {
                    // Affiche le timeout
                    string iaName = m_scene.GetControlerByHeroId(GetPickingHeroId()).HeroName;
                    m_currentMessage = iaName + " : temps expiré ! Aucune compétence choisie !";


                    m_lastControlerUpdate = DateTime.Now;
                    m_pickTurn++;
                }

        }

        /// <summary>
        /// Obtient la valeur de timeout actuelle en secondes.
        /// </summary>
        /// <returns></returns>
        float GetCurrentTimeoutSeconds()
        {
            bool controlerIsIA = m_scene.GetControlerByHeroId(GetPickingHeroId()) is Controlers.IAControler;
            if (controlerIsIA)
                return IATimeoutSeconds;
            else
                return HumanTimeoutSeconds;
        }

#if false
        /// <summary>
        /// Dessine le lobby.
        /// </summary>
        public void Draw(GameTime time, SpriteBatch batch)
        {
            int w = (int)GameServer.GetScreenSize().X;
            int h = (int)GameServer.GetScreenSize().Y;
            batch.Begin();
            batch.GraphicsDevice.Clear(Color.LightGray);

            // Dessine les héros.
            int x = 5;
            int y;
            for (int team = 0; team <= 1; team++)
            {
                y = 20;
                foreach (var hero in m_heroes[team])
                {
                    DrawHero(batch, new Rectangle(x, y, w/2 - 10, 100), hero);
                    y += 110;
                }
                x += w / 2;
            }

            // Dessine les diverses informations sur l'état de la phase de picks.
            if (!IsReadyToGo())
            {
                // Dessine le temps restant.
                y = h - 300;
                string s = ((int)(GetCurrentTimeoutSeconds() - (DateTime.Now - m_lastControlerUpdate).TotalSeconds)).ToString();
                float scale = 4.0f;
                Vector2 sz = Ressources.CourrierFont.MeasureString(s) * scale;
                x = (w - (int)sz.X)/2;
                batch.DrawString(Ressources.CourrierFont, s, new Vector2(x, y), Color.Black, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.5f);

                y += 70;
                // Dessine les messages.
                x = 5;
                scale = 1.0f;
                string msg = m_scene.GetControlerByHeroId(GetPickingHeroId()).HeroName + " : choisissez une compétence " + (GetCurrentSpells() == m_activeSpells ? "active" : "passive") + ".";
                sz = Ressources.CourrierFont.MeasureString(msg) * scale;
                x = (w - (int)sz.X) / 2;
                batch.DrawString(Ressources.CourrierFont, msg, new Vector2(x, y), Color.Black, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.5f);

                x = 5;
                y += 100;
                batch.DrawString(Ressources.CourrierFont, m_currentMessage, new Vector2(x, y), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            }

            // Dessine les icones des compétences proposées.
            if(IsReadyToGo())
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
                foreach (Spells.Spell spell in GetCurrentSpells())
                {
                    const int spellSize = 32;
                    y = h - spellSize - 4;
                    Rectangle dstRect = new Rectangle(x, y, spellSize, spellSize);

                    // Effet de surbrillance si un sort est survollé.
                    Color color = Color.White;
                    if (dstRect.Contains(new Point((int)m_virtualMousePos.X, (int)m_virtualMousePos.Y)))
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
        /// Dessine le slot du héros donné à la position donnée.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="hero"></param>
        void DrawHero(SpriteBatch batch, Rectangle rect, EntityHero hero)
        {
            float layerDepth = 0.5f;
            const int iconSize = 16;
            const int spellSize = 32;

            var texture = IsMyTurn(hero.ID) ? Ressources.MenuItemHover : Ressources.MenuItem;
            EnhancedGui.Drawing.DrawRectBox(batch, texture, rect, Color.White, layerDepth+0.1f);

            int x = rect.X + 20;
            int y = rect.Y + 20;
            // Dessine l'icone du rôle du héros
            batch.Draw(GetIcon(hero.Role), new Rectangle(x, y, iconSize, iconSize), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);
            x += iconSize + 10;

            // Dessine le nom du héros.
            batch.DrawString(Ressources.CourrierFont, m_scene.GetControlerByHeroId(hero.ID).HeroName, new Vector2(x, y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);

            y += 25;
            x = rect.X + 20;
            // Dessine les sorts du héros.
            for (int i = 0; i < hero.Spells.Count; i++ )
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
#endif

        #endregion
        #region Mechanics
        /// <summary>
        /// Obtient le héros dont c'est actuellement le tour.
        /// </summary>
        /// <returns></returns>
        int GetPickingHeroId()
        {
            int heroesCount = m_heroes[0].Count * 2;
            int turnId = m_pickTurn / heroesCount;
            int teamId = 0;
            int playerId = 0;
            switch (turnId)
            {
                case 0:
                    teamId = m_pickTurn % 2;
                    playerId = (m_pickTurn % heroesCount) / 2;
                    break;
                case 1:
                    teamId = (m_pickTurn + 1) % 2;
                    playerId = (heroesCount / 2 - 1) - (m_pickTurn % heroesCount) / 2;
                    break;
                case 2:
                    teamId = m_pickTurn % 2;
                    playerId = (m_pickTurn % heroesCount) / 2;
                    break;
            }

            return m_heroes[teamId][playerId].ID;
        }


        /// <summary>
        /// Obtient la liste des spells actuellement proposés.
        /// </summary>
        /// <returns></returns>
        List<Spells.Spell> GetCurrentSpells()
        {
            int heroesCount = m_heroes[0].Count * 2;
            int turnId = m_pickTurn / heroesCount;
            switch (turnId)
            {
                case 0:
                    return m_passiveSpells;
                case 1:
                    return m_activeSpells;
                case 2:
                    return m_activeSpells;
                   
            }
            return new List<Spells.Spell>();
        }
        #endregion

        #region API
        /// <summary>
        /// Définit la position de la souris virtuelle.
        /// </summary>
        /// <param name="pos"></param>
        public void SetVirtualMousePos(Vector2 pos)
        {
            m_virtualMousePos = pos;
        }

        /// <summary>
        /// Emule un click sur la souris virtuelle.
        /// </summary>
        public void VirtualMouseClick(int controlerId)
        {
            if (IsReadyToGo())
                return;

            // Sélectionne le sort donné.
            int h = (int)GameServer.GetScreenSize().Y;
            int x = 5;
            int spellId = 0;
            foreach (Spells.Spell spell in GetCurrentSpells())
            {
                const int spellSize = 32;
                int y = h - spellSize - 4;
                Rectangle dstRect = new Rectangle(x, y, spellSize, spellSize);

                // Effet de surbrillance si un sort est survollé.
                Color color = Color.White;
                if (dstRect.Contains(new Point((int)m_virtualMousePos.X, (int)m_virtualMousePos.Y)))
                {
                    PickSpell(controlerId, spellId);
                    m_lastControlerUpdate = DateTime.Now;
                    break;
                }

                x += spellSize + 4;
                spellId++;
            }
        }

        /// <summary>
        /// Retourne vrai si c'est le tour du héro dont l'id est donné.
        /// </summary>
        public bool IsMyTurn(int heroId)
        {
            return GetPickingHeroId() == heroId && !IsReadyToGo();
        }

        /// <summary>
        /// Si c'est le tour du héros donné, pick le spell donné pour ce héros et retourne true.
        /// Si ce n'est pas le tour du héros donné, ou que le spell dont l'id est donné n'existe pas,
        /// retourne false.
        /// </summary>
        public bool PickSpell(int heroId, int spellId)
        {
            if (heroId != GetPickingHeroId() || IsReadyToGo())
                return false;

            List<Spells.Spell> spells = GetCurrentSpells();
            if (spellId >= spells.Count)
                return false;

            // Marque le dernier temps de réponse.
            m_lastControlerUpdate = DateTime.Now;

            // Ajoute le spell au héros
            m_scene.GetControlerByHeroId(heroId).Hero.Spells.Add(spells[spellId]);
            spells[spellId].SourceCaster = m_scene.GetControlerByHeroId(heroId).Hero;

            // Affiche la compétence choisie.
            string iaName = m_scene.GetControlerByHeroId(heroId).HeroName;
            m_currentMessage = iaName + " a choisi la compétence '" + spells[spellId].Name + "'.";

            // Supprime le spell de la liste.
            spells.RemoveAt(spellId);

            m_pickTurn++;
            return true;
        }

        /// <summary>
        /// Retourne true si la phase de picks est terminée.
        /// </summary>
        public bool IsReadyToGo()
        {
            int heroesCount = m_heroes[0].Count * 2;
            return m_pickTurn / heroesCount >= 3;
        }

        #region Click API

        #endregion
        #endregion

        #region Misc
        /// <summary>
        /// Obtient l'icone du rôle donné.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Texture2D GetIcon(EntityHeroRole role)
        {
            switch (role)
            {
                case EntityHeroRole.Fighter:
                    return Ressources.IconFighter;
                case EntityHeroRole.Mage:
                    return Ressources.IconMage;
                case EntityHeroRole.Tank:
                    return Ressources.IconTank;
            }
            return Ressources.IconTank;
        }
        #endregion
    }
}
