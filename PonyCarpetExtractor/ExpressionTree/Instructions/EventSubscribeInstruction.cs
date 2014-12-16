using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree.Instructions
{
    /// <summary>
    /// Représente une instruction de souscription à un event. 
    /// </summary>
    public class EventSubscribeInstruction
    {
        #region Properties
        public IGettable Event;
        public IGettable Delegate;
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de la classe EventSubscribeInstruction.
        /// </summary>
        public EventSubscribeInstruction(IGettable evt, IGettable delegat)
        {
            Event = evt;
            Delegate = delegat;
        }
        /// <summary>
        /// Retourne l'action correspondant à cette instruction.
        /// </summary>
        public Action<Context> GetAction()
        {
            return delegate(Context c)
            {
                object evtObj = Event.GetValue(c);
                object delObj = Delegate.GetValue(c);
                
                // Vérifications :
                if (!(evtObj is InternalEventRepresentation))
                    throw new InterpreterException("The left member of the subscription must be an event.");
                if (!(delObj is Delegate))
                    throw new InterpreterException("The right member of the subscription must be a delegate.");
                // Casts :
                InternalEventRepresentation evt = (InternalEventRepresentation)evtObj;
                Delegate del = (Delegate)delObj;

                evt.Event.AddEventHandler(evt.Owner, del);
            };
        }
        #endregion
    }
}
