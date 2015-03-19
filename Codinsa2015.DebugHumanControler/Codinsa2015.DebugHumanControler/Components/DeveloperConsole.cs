using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.EnhancedGui;

namespace Codinsa2015.DebugHumanControler.Components
{
    public class DeveloperConsole
    {
        bool m_expanded = false;
        GuiButton m_expandButton;
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
                m_expandButton.IsVisible = value;
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
            int width = 600;//(int)(GameServer.GetScreenSize().X/2);
            int screenHeight = 800;
            const int h1 = 75;
            const int h2 = 25;
            const int bar = 25;
            m_window = new GuiWindow(Manager);
            m_window.Location = new Point(0, (int)(screenHeight - 125));
            m_window.Size = new Point(width, h1 + h2 + bar);
            m_window.Title = "Developer Console";

            // Expand
            m_expandButton = new GuiButton(Manager);
            m_expandButton.Parent = m_window;
            m_expandButton.Location = new Point(m_window.Size.X - 25, 5);
            m_expandButton.Size = new Point(15, 15);
            m_expandButton.Title = "+";

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

            m_expandButton.Clicked += m_expandButton_Clicked;
            Server.GameServer.GetScene().GameInterpreter.OnPuts = new PonyCarpetExtractor.Interpreter.PutsDelegate((string s) => { m_consoleOutput.AppendLine(s); m_consoleOutput.ScrollDown(); });
            Server.GameServer.GetScene().GameInterpreter.OnError = new PonyCarpetExtractor.Interpreter.PutsDelegate((string s) => { m_consoleOutput.AppendLine("error: " + s); m_consoleOutput.ScrollDown(); });
        }

        void m_expandButton_Clicked()
        {
            int expandX = 200;
            int expandY = 300;
            if(m_expanded)
            {
                expandX = -expandX;
                expandY = -expandY;
            }


            m_window.Size = new Point(m_window.Size.X+ expandX, m_window.Size.Y + expandY);
            m_consoleOutput.Size = new Point(m_consoleOutput.Size.X + expandX, m_consoleOutput.Size.Y + expandY);
            m_consoleInput.Location = new Point(m_consoleInput.Location.X, m_consoleInput.Location.Y + expandY);
            m_consoleInput.Size = new Point(m_consoleInput.Size.X + expandX, m_consoleInput.Size.Y);
            m_expandButton.Location = new Point(m_expandButton.Location.X + expandX, m_expandButton.Location.Y);
            m_expanded = !m_expanded;
        }

        /// <summary>
        /// Se produit lorsqu'une commande est entrée dans la console.
        /// </summary>
        /// <param name="sender"></param>
        void m_consoleInput_TextValidated(GuiTextInput sender)
        {
            /*if (sender.Text == "")
                m_consoleInput.HasFocus = false;*/
            m_consoleOutput.AppendLine(">> " + sender.Text);
            Server.GameServer.GetScene().GameInterpreter.Eval(sender.Text);
            sender.Text = "";
        }
    }
}
