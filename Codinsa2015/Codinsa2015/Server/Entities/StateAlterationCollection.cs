using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Entities
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
        public StateAlterationCollection GetInteractionsByType(StateAlterationType type)
        {
            StateAlterationCollection alterations = new StateAlterationCollection();
            foreach(StateAlteration alteration in this)
            {
                if ((alteration.Model.Type & type) == type)
                    alterations.Add(alteration);
            }
            return alterations;
        }

        /// <summary>
        /// Termine toutes les altérations dont le type de source correspond au type
        /// donné.
        /// </summary>
        /// <param name="type"></param>
        public void EndAlterations(StateAlterationSource type)
        {
            foreach(StateAlteration alt in this)
            {
                if (alt.SourceType == type)
                    alt.EndNow();
            }
        }


        /// <summary>
        /// Retourne une liste de vues de cette collection d'altération.
        /// </summary>
        public List<Views.StateAlterationView> ToView()
        {
            List<Views.StateAlterationView> views = new List<Views.StateAlterationView>();
            foreach(StateAlteration alt in this)
            {
                views.Add(alt.ToView());
            }
            return views;
        }

    }
}
