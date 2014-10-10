using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Tools
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
            /// <summary>
            /// Ligne à laquelle est survenue l'erreur/warning.
            /// </summary>
            public int Line;
            /// <summary>
            /// Caractère dans lequel est survenu l'erreur/warning.
            /// </summary>
            public int Character;
            /// <summary>
            /// Nom du fichier source d'où provient le message.
            /// </summary>
            public string Source;
            public Entry(EntryType type, string message) { Message = message; Type = type; }
            public Entry(EntryType type, string message, int line, int character) { Type = type; Message = message; Line = line; Character = character; }
            public Entry(EntryType type, string message, int line, int character, string source)
            {
                Type = type; Message = message; Line = line; Character = character;
                Source = source;
            }
            
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
        public void AddError(string message, int line, int character)
        {
            OnEvent(new Entry(EntryType.Error, message, line, character));
        }

        public void AddError(string message, int line, int character, string source)
        {
            OnEvent(new Entry(EntryType.Error, message, line, character, source));
        }
        public void AddWarning(string message)
        {
            OnEvent(new Entry(EntryType.Warning, message));
        }
        public void AddWarning(string message, int line, int character)
        {
            OnEvent(new Entry(EntryType.Warning, message, line, character));
        }
        public void AddWarning(string message, int line, int character, string source)
        {
            OnEvent(new Entry(EntryType.Warning, message, line, character, source));
        }
        public void AddMessage(string message)
        {
            OnEvent(new Entry(EntryType.Message, message));
        }
        public void AddMessage(string message, int line, int character)
        {
            OnEvent(new Entry(EntryType.Message, message, line, character));
        }
        public void AddMessage(string message, int line, int character, string source)
        {
            OnEvent(new Entry(EntryType.Message, message, line, character, source));
        }
    }
}
