using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clank.IDE
{
    public partial class SettingsDialog : Form
    {
        ProjectSettings m_settings;
        /// <summary>
        /// Obtient ou définit les paramètres du projets associés à ce contrôle.
        /// </summary>
        public ProjectSettings Settings
        {
            get
            {
                ProjectSettings settings = new ProjectSettings();
                settings.ServerTarget = Core.Generation.GenerationTarget.FromString(m_targetServerTB.Text);
                settings.ClientTargets = Core.Generation.GenerationTarget.TargetsFromString(m_targetClientTB.Text, '\n');
                return settings;
            }
            set
            {
                m_targetClientTB.Text = Core.Generation.GenerationTarget.TargetsToString(value.ClientTargets, '\n');
                m_targetServerTB.Text = value.ServerTarget.ToString();
            }
        }
        /// <summary>
        /// Crée une nouvelle instance du dialogue.
        /// </summary>
        public SettingsDialog()
        {
            InitializeComponent();
        }
    }
}
