using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
namespace Codinsa2015.Graphics.Server
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

        /// <summary>
        /// Représente le content manager utilisé par ce serveur.
        /// </summary>
        public ContentManager Content
        {
            get;
            set;
        }
        #endregion


        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de graphics server.
        /// </summary>
        public GraphicsServer(CommandExecutionMode mode, ContentManager content)
        {
            Content = content;
            Mode = mode;
        }
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
            SendCommand(new CommandEndFrame());

            if(Mode == CommandExecutionMode.Postponed && CommandIssued != null)
            {
                while(m_commands.Count != 0)
                {
                    CommandIssued(m_commands.Dequeue());
                }
            }
        }

        /// <summary>
        /// Change le render target en cours d'utilisation.
        /// Null pour le back buffer.
        /// </summary>
        public void SetRenderTarget(RemoteRenderTarget target)
        {
            SendCommand(new CommandGraphicsDeviceSetRenderTarget(target));
        }

        /// <summary>
        /// Remplit le render target actuel avec la couleur donnée.
        /// </summary>
        public void Clear(Microsoft.Xna.Framework.Color color)
        {
            SendCommand(new CommandGraphicsDeviceClear(color));
        }
        #endregion


    }
}
