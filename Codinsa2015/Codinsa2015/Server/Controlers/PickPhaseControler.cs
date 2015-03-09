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

        string m_currentMessage = "";
        #endregion

        #region Properties
        public List<List<EntityHero>> GetHeroes() { return m_heroes; }
        public List<Spells.Spell> GetActiveSpells() { return m_activeSpells; }
        public List<Spells.Spell> GetPassiveSpells() { return m_passiveSpells; }
        public int GetPickTurn() { return m_pickTurn; }
        public string CurrentMessage
        {
            get { return m_currentMessage; }
            set { m_currentMessage = value; }
        }
        public DateTime LastControlerUpdate 
        { 
            get { return m_lastControlerUpdate; }
            set { m_lastControlerUpdate = value; }
        }
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
        public float GetCurrentTimeoutSeconds()
        {
            bool controlerIsIA = m_scene.GetControlerByHeroId(GetPickingHeroId()) is Controlers.IAControler;
            if (controlerIsIA)
                return IATimeoutSeconds;
            else
                return HumanTimeoutSeconds;
        }

        #endregion

        #region Mechanics
        /// <summary>
        /// Obtient le héros dont c'est actuellement le tour.
        /// </summary>
        /// <returns></returns>
        public int GetPickingHeroId()
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
        public List<Spells.Spell> GetCurrentSpells()
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
        public bool PickSpell(int heroId, int spellId, int clientId)
        {
            // Vérification : l'appel de fonction doit venir du client correspondant au
            // héros.
            if ((m_scene.GetControlerByHeroId(heroId) != m_scene.GetControler(clientId)) &&
                clientId != GameServer.__INTERNAl_CLIENT_ID)
            {
                return false;
            }

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

        #endregion

    }
}
