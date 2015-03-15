using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Tools
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
            public string StackTrace;
            /// <summary>
            /// Caractère dans lequel est survenu l'erreur/warning.
            /// </summary>
            public Exception Ex;

            public Entry(EntryType type, string message, string stacktrace, Exception ex)
            {
                Type = type;
                Message = message;
                StackTrace = stacktrace;
                Ex = ex;
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

    }
}
