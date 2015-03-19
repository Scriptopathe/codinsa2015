using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Classe de base pour toutes les instructions.
    /// </summary>
    public class Instruction
    {
        string m_comment;
        /// <summary>
        /// Ligne à laquelle a été crée cette instruction.
        /// </summary>
        public int Line { get; set; }
        /// <summary>
        /// Caractère où a été créé cette instruction.
        /// </summary>
        public int Character { get; set; }
        /// <summary>
        /// Fichier source à partir duquel a été créée cette instruction.
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Représente un commentaire à insérer devant l'instruction.
        /// </summary>
        public string Comment 
        {
            get { return m_comment; }
            set
            {
                if (value == null)
                    m_comment = value;
                else
                {
                    m_comment = value.Trim('\n', '\r');
                }
            }
        }
    }
}
