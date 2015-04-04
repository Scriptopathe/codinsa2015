using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Xna.Framework;
namespace Codinsa2015.DebugHumanControler
{
    public partial class Launcher : Form
    {
        public bool Spectate
        {
            get;
            set;
        }

        public Vector2 Resolution
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public bool UseDebugLog { get; set; }

        public Launcher()
        {
            InitializeComponent();
            m_resolutions.SelectedIndex = 0;
        }

        private void m_goButton_Click(object sender, EventArgs e)
        {
            Spectate = m_spectateCb.Checked;
            Port = (int)m_portNb.Value;
            string resolutionStr = (string)m_resolutions.Items[m_resolutions.SelectedIndex];
            string[] v = resolutionStr.Split('x');
            Resolution = new Vector2(Int32.Parse(v[0]), Int32.Parse(v[1]));
            UseDebugLog = m_debugLogsCb.Checked;
            Close();
        }

        private void m_resolutions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void m_portNb_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}
