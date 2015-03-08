using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.EnhancedGui;
using Codinsa2015.Graphics.Server;
namespace Codinsa2015.Server.Controlers.Components
{
    public class DeveloperConsole
    {
        GuiWindow m_window;
        /// <summary>
        /// Contrôle d'input de la console.
        /// </summary>
        GuiTextInput m_consoleInput;
        /// <summary>
        /// Affiche la sortie de la console.
        /// </summary>
        GuiMultilineTextDisplay m_consoleOutput;
        /// <summary>
        /// Gui Manager parent de la console.
        /// </summary>
        public GuiManager Manager { get; private set; }


        /// <summary>
        /// Obtient le contrôle de sortie de la console.
        /// </summary>
        public GuiMultilineTextDisplay Output
        {
            get { return m_consoleOutput; }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la console est visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                return m_consoleOutput.IsVisible;
            }
            set
            {
                m_consoleOutput.IsVisible = value;
                m_consoleInput.IsVisible = value;
                m_window.IsVisible = value;
            }
        }
        /// <summary>
        /// Obtient ou définit une valeur indiquant si la console a le focus.
        /// </summary>
        public bool HasFocus
        {
            get { return m_consoleInput.HasFocus(); }
            set { if (value == true) { m_consoleInput.Focus(); } }
        }
        /// <summary>
        /// Crée une nouvelle instance de Developper Console.
        /// </summary>
        public DeveloperConsole(GuiManager mgr)
        {
            Manager = mgr;
        }

        /// <summary>
        /// Charge les ressources dont a besoin la console.
        /// </summary>
        public void LoadContent()
        {
            int width = (int)(GameServer.GetScreenSize().X/2);
            const int h1 = 75;
            const int h2 = 25;
            const int bar = 25;
            m_window = new GuiWindow(Manager);
            m_window.Location = new Point(0, (int)(GameServer.GetScreenSize().Y - 125));
            m_window.Size = new Point(width, h1 + h2 + bar);
            m_window.Title = "Developer Console";
            // Console input
            m_consoleInput = new GuiTextInput(Manager);
            m_consoleInput.Parent = m_window;
            m_consoleInput.Location = new Point(0, bar + h1);
            m_consoleInput.Size = new Point(width, h2);
            m_consoleInput.TextValidated += m_consoleInput_TextValidated;

            // Console output
            m_consoleOutput = new GuiMultilineTextDisplay(Manager);
            m_consoleOutput.Parent = m_window;
            m_consoleOutput.Location = new Point(0, bar);
            m_consoleOutput.Size = new Point(width, h1);
            GameServer.GetScene().GameInterpreter.OnPuts = new PonyCarpetExtractor.Interpreter.PutsDelegate((string s) => { m_consoleOutput.AppendLine(s); });
            GameServer.GetScene().GameInterpreter.OnError = new PonyCarpetExtractor.Interpreter.PutsDelegate((string s) => { m_consoleOutput.AppendLine("error: " + s); });
        }

        /// <summary>
        /// Se produit lorsqu'une commande est entrée dans la console.
        /// </summary>
        /// <param name="sender"></param>
        void m_consoleInput_TextValidated(GuiTextInput sender)
        {
            /*if (sender.Text == "")
                m_consoleInput.HasFocus = false;*/

            GameServer.GetScene().GameInterpreter.Eval(sender.Text);
            sender.Text = "";
        }
    }
}
