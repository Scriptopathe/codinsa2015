using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.DebugHumanControler
{
    /// <summary>
    /// Représente un contrôleur humain pour la phase de picks.
    /// </summary>
    public class PickPhaseControler
    {
        GameClient m_client;

        /// <summary>
        /// Obtient une valeur indiquant si ce contrôleur est en mode spectateur.
        /// </summary>
        public bool IsInSpectateMode { get; set; }

        /// <summary>
        /// Crée une nouvelle instance de PickPhaseControler.
        /// </summary>
        /// <param name="client"></param>
        public PickPhaseControler(GameClient client)
        {
            m_client = client;
        }

        /// <summary>
        /// Mets à jour ce contrôleur.
        /// </summary>
        public void Update(GameTime time)
        {
            if (Input.IsLeftClickTrigger() && !IsInSpectateMode)
                OnMouseClicked();
        }

        /// <summary>
        /// Emule un click sur la souris virtuelle.
        /// </summary>
        public void OnMouseClicked()
        {
            var ctrl = m_client.Server.GetSrvScene().PickControler;
            if (ctrl.IsReadyToGo())
                return;
            if (!ctrl.IsMyTurn(m_client.Controler.Hero.ID))
                return;

            // Sélectionne le sort donné.
            const int spellSize = 64;
            if(ctrl.IsPickingActive())
            {
                int h = (int)Ressources.ScreenSize.Y;
                int x = 5;
                var ms = Input.GetMouseState();
                foreach (int spellId in ctrl.GetActiveSpells())
                {
                    int y = h - spellSize - 4;
                    Rectangle dstRect = new Rectangle(x, y, spellSize, spellSize);

                    // Effet de surbrillance si un sort est survollé.
                    Color color = Color.White;
                    if (dstRect.Contains(new Point((int)ms.X, (int)ms.Y)))
                    {
                        ctrl.PickActiveSpell(m_client.Controler.Hero.ID, spellId);
                        break;
                    }

                    x += spellSize + 4;
                }
            }
            else
            {
                int h = (int)Ressources.ScreenSize.Y;
                int x = 5;
                var ms = Input.GetMouseState();
                foreach (Server.Entities.EntityUniquePassives spellId in ctrl.GetPassiveSpells())
                {
                    int y = h - spellSize - 4;
                    Rectangle dstRect = new Rectangle(x, y, spellSize, spellSize);

                    // Effet de surbrillance si un sort est survollé.
                    Color color = Color.White;
                    if (dstRect.Contains(new Point((int)ms.X, (int)ms.Y)))
                    {
                        ctrl.PickPassiveSpell(m_client.Controler.Hero.ID, spellId);
                        break;
                    }

                    x += spellSize + 4;
                }
            }
        }
    }
}
