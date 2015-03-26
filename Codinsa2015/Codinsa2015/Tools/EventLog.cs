using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Tools
{
    /// <summary>
    /// Représente un système permettant la journalisation des erreurs, warnings et autres infos.
    /// </summary>
    public class EventLog
    {
        public delegate void EventLogHandler(Entry entry);
        /// <summary>
        /// Event envoyé lorsqu'un élément est ajouté dans le log.
        /// </summary>
        public event EventLogHandler OnEvent;
        /// <summary>
        /// Représente les types d'entrées au log.
        /// </summary>
        public enum EntryType
        {
            Error,
            Warning,
            Message
        }
        /// <summary>
        /// Représente une entrée du journal.
        /// </summary>
        public class Entry
        {
            /// <summary>
            /// Type de l'entrée (erreur, warning etc...)
            /// </summary>
            public EntryType Type;
            /// <summary>
            /// Message porté par l'entrée.
            /// </summary>
            public string Message;
            public Entry(EntryType type, string message) { Message = message; Type = type; }

        }


        /// <summary>
        /// Crée une nouvelle instance de Log.
        /// </summary>
        public EventLog()
        {
        }

        public void AddError(string message)
        {
            OnEvent(new Entry(EntryType.Error, message));
        }
        public void AddWarning(string message)
        {
            OnEvent(new Entry(EntryType.Warning, message));
        }
        public void AddMessage(string message)
        {
            OnEvent(new Entry(EntryType.Message, message));
        }
    }
}
