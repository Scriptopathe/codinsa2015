using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Tools
{
    /// <summary>
    /// Représente un système permettant la journalisation des erreurs, warnings et autres infos.
    /// </summary>
    public class Log
    {
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
        /// Liste des entrées présentes dans le log.
        /// </summary>
        public List<Entry> Entries { get; set; }

        /// <summary>
        /// Crée une nouvelle instance de Log.
        /// </summary>
        public Log()
        {
            Entries = new List<Entry>();
        }

        public void AddError(string message)
        {
            Entries.Add(new Entry(EntryType.Error, message));
        }
        public void AddError(string message, int line, int character)
        {
            Entries.Add(new Entry(EntryType.Error, message, line, character));
        }

        public void AddError(string message, int line, int character, string source)
        {
            Entries.Add(new Entry(EntryType.Error, message, line, character, source));
        }
        public void AddWarning(string message)
        {
            Entries.Add(new Entry(EntryType.Warning, message));
        }
        public void AddWarning(string message, int line, int character)
        {
            Entries.Add(new Entry(EntryType.Warning, message, line, character));
        }
        public void AddWarning(string message, int line, int character, string source)
        {
            Entries.Add(new Entry(EntryType.Warning, message, line, character, source));
        }
        public void AddMessage(string message)
        {
            Entries.Add(new Entry(EntryType.Message, message));
        }
        public void AddMessage(string message, int line, int character)
        {
            Entries.Add(new Entry(EntryType.Message, message, line, character));
        }
        public void AddMessage(string message, int line, int character, string source)
        {
            Entries.Add(new Entry(EntryType.Message, message, line, character, source));
        }
    }
}
