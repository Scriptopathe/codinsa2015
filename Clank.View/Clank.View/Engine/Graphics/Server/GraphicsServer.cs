using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Graphics.Server
{
    /// <summary>
    /// Représente le serveur graphique du jeu.
    /// Le serveur graphique récupère toutes les commandes que le serveur donne,
    /// et les regroupe dans des batch à lancer chaque frame.
    /// 
    /// Un client graphique s'occupe d'exécuter les batchs.
    /// </summary>
    public class GraphicsServer
    {
        #region Enums
        public enum CommandExecutionMode
        {
            Immediate, // la commande est exécutée dès réception
            Postponed, // toutes les commandes sont envoyées une fois la frame terminée.
        }
        #endregion

        #region Events
        public delegate void IssueCommand(Command cmd);
        /// <summary>
        /// Event lancé lorsqu'une commande est envoyée au serveur graphique.
        /// </summary>
        public event IssueCommand CommandIssued;
        #endregion

        #region Variables
        /// <summary>
        /// Représente les commandes envoyées lors de la frame en cours.
        /// </summary>
        Queue<Command> m_commands;
        
        #endregion

        #region Properties
        /// <summary>
        /// Représente la manière dont le serveur envoie les commandes au client.
        /// </summary>
        public CommandExecutionMode Mode
        {
            get;
            set;
        }
        #endregion
        #region Methods

        /// <summary>
        /// Envoie une commande au serveur graphique.
        /// </summary>
        /// <param name="command"></param>
        public void SendCommand(Command command)
        {
            if (Mode == CommandExecutionMode.Postponed)
                m_commands.Enqueue(command);
            else if (CommandIssued != null)
                CommandIssued(command);
        }

        /// <summary>
        /// Indique au serveur graphique que la fin de la frame vient d'être atteinte.
        /// </summary>
        public void Flush()
        {
            if(Mode == CommandExecutionMode.Postponed && CommandIssued != null)
            {
                while(m_commands.Count != 0)
                {
                    CommandIssued(m_commands.Dequeue());
                }
            }
        }
        #endregion


    }
}
