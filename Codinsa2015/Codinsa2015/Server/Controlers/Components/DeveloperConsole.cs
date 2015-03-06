using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Server.Gui;
using Codinsa2015.Graphics.Server;
namespace Codinsa2015.Server.Controlers.Components
{
    public class DeveloperConsole
    {
        /// <summary>
        /// Contrôle d'input de la console.
        /// </summary>
        Gui.GuiTextInput m_consoleInput;
        /// <summary>
        /// Affiche la sortie de la console.
        /// </summary>
        Gui.GuiMultilineTextDisplay m_consoleOutput;
        /// <summary>
        /// Gui Manager parent de la console.
        /// </summary>
        public GuiManager Manager { get; private set; }


        /// <summary>
        /// Obtient le contrôle de sortie de la console.
        /// </summary>
        public Gui.GuiMultilineTextDisplay Output
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
            }
        }
        /// <summary>
        /// Obtient ou définit une valeur indiquant si la console a le focus.
        /// </summary>
        public bool HasFocus
        {
            get { return m_consoleInput.HasFocus; }
            set { m_consoleInput.HasFocus = value; }
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

            // Console input
            m_consoleInput = new Gui.GuiTextInput();
            m_consoleInput.Position = new Vector2(0, GameServer.GetScreenSize().Y - 25);
            m_consoleInput.Size = new Point((int)GameServer.GetScreenSize().X - 200, 25);
            m_consoleInput.TextValidated += m_consoleInput_TextValidated;

            Manager.AddWidget(m_consoleInput);

            // Console output
            m_consoleOutput = new Gui.GuiMultilineTextDisplay();
            m_consoleOutput.Position = new Vector2(0, GameServer.GetScreenSize().Y - 100);
            m_consoleOutput.Size = new Point((int)GameServer.GetScreenSize().X - 200, 75);
            Manager.AddWidget(m_consoleOutput);
            GameServer.GetScene().GameInterpreter.OnPuts = new PonyCarpetExtractor.Interpreter.PutsDelegate((string s) => { m_consoleOutput.AppendLine(s); });
            GameServer.GetScene().GameInterpreter.OnError = new PonyCarpetExtractor.Interpreter.PutsDelegate((string s) => { m_consoleOutput.AppendLine("error: " + s); });
        }

        /// <summary>
        /// Se produit lorsqu'une commande est entrée dans la console.
        /// </summary>
        /// <param name="sender"></param>
        void m_consoleInput_TextValidated(Gui.GuiTextInput sender)
        {
            if (sender.Text == "")
                m_consoleInput.HasFocus = false;

            GameServer.GetScene().GameInterpreter.Eval(sender.Text);
            sender.Text = "";
        }
    }
}
