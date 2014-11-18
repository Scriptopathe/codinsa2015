using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Clank.View.Engine.Entities
{
    /// <summary>
    /// Représente une collection d'altération d'état.
    /// 
    /// Contient des fonctions utiles pour détecter facilement une alteration
    /// d'état en particulier.
    /// </summary>
    public class StateAlterationCollection : List<StateAlteration>
    {

        /// <summary>
        /// Mets à jour toutes les altérations d'état de cette collection.
        /// </summary>
        public void UpdateStateAlterations(GameTime time, EntityBase dstEntity)
        {
            List<StateAlteration> toDelete = new List<StateAlteration>();
            foreach(StateAlteration alteration in this)
            {
                alteration.Update(time);
                if (alteration.HasEnded(dstEntity, time))
                    toDelete.Add(alteration);
            }


            // Supprime les altérations d'état obsolètes.
            foreach (StateAlteration alterations in toDelete)
                Remove(alterations);
        }

        /// <summary>
        /// Retourne la liste des altérations d'état ayant le type donné.
        /// </summary>
        public List<StateAlteration> GetInteractionsByType(StateAlterationType type)
        {
            List<StateAlteration> alterations = new List<StateAlteration>();
            foreach(StateAlteration alteration in this)
            {
                if ((alteration.Model.Type & type) == type)
                    alterations.Add(alteration);
            }
            return alterations;
        }
    }
}
