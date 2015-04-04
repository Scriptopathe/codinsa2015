namespace Codinsa2015.DebugHumanControler
{
    partial class Launcher
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_spectateCb = new System.Windows.Forms.CheckBox();
            this.m_goButton = new System.Windows.Forms.Button();
            this.m_resolutions = new System.Windows.Forms.ComboBox();
            this.m_portNb = new System.Windows.Forms.NumericUpDown();
            this.m_portCb = new System.Windows.Forms.Label();
            this.m_debugLogsCb = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_portNb)).BeginInit();
            this.SuspendLayout();
            // 
            // m_spectateCb
            // 
            this.m_spectateCb.AutoSize = true;
            this.m_spectateCb.Location = new System.Drawing.Point(13, 13);
            this.m_spectateCb.Name = "m_spectateCb";
            this.m_spectateCb.Size = new System.Drawing.Size(106, 17);
            this.m_spectateCb.TabIndex = 0;
            this.m_spectateCb.Text = "Mode spectateur";
            this.m_spectateCb.UseVisualStyleBackColor = true;
            // 
            // m_goButton
            // 
            this.m_goButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_goButton.Location = new System.Drawing.Point(12, 87);
            this.m_goButton.Name = "m_goButton";
            this.m_goButton.Size = new System.Drawing.Size(260, 49);
            this.m_goButton.TabIndex = 1;
            this.m_goButton.Text = "Go !";
            this.m_goButton.UseVisualStyleBackColor = true;
            this.m_goButton.Click += new System.EventHandler(this.m_goButton_Click);
            // 
            // m_resolutions
            // 
            this.m_resolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_resolutions.FormattingEnabled = true;
            this.m_resolutions.Items.AddRange(new object[] {
            "1366x868",
            "960x640"});
            this.m_resolutions.Location = new System.Drawing.Point(13, 37);
            this.m_resolutions.Name = "m_resolutions";
            this.m_resolutions.Size = new System.Drawing.Size(259, 21);
            this.m_resolutions.TabIndex = 2;
            this.m_resolutions.SelectedIndexChanged += new System.EventHandler(this.m_resolutions_SelectedIndexChanged);
            // 
            // m_portNb
            // 
            this.m_portNb.Location = new System.Drawing.Point(50, 64);
            this.m_portNb.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.m_portNb.Name = "m_portNb";
            this.m_portNb.Size = new System.Drawing.Size(222, 20);
            this.m_portNb.TabIndex = 3;
            this.m_portNb.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.m_portNb.ValueChanged += new System.EventHandler(this.m_portNb_ValueChanged);
            // 
            // m_portCb
            // 
            this.m_portCb.AutoSize = true;
            this.m_portCb.Location = new System.Drawing.Point(12, 66);
            this.m_portCb.Name = "m_portCb";
            this.m_portCb.Size = new System.Drawing.Size(32, 13);
            this.m_portCb.TabIndex = 4;
            this.m_portCb.Text = "Port :";
            // 
            // m_debugLogsCb
            // 
            this.m_debugLogsCb.AutoSize = true;
            this.m_debugLogsCb.Location = new System.Drawing.Point(125, 12);
            this.m_debugLogsCb.Name = "m_debugLogsCb";
            this.m_debugLogsCb.Size = new System.Drawing.Size(140, 17);
            this.m_debugLogsCb.TabIndex = 5;
            this.m_debugLogsCb.Text = "Activer le logs de debug";
            this.m_debugLogsCb.UseVisualStyleBackColor = true;
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 148);
            this.Controls.Add(this.m_debugLogsCb);
            this.Controls.Add(this.m_portCb);
            this.Controls.Add(this.m_portNb);
            this.Controls.Add(this.m_resolutions);
            this.Controls.Add(this.m_goButton);
            this.Controls.Add(this.m_spectateCb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Launcher";
            this.Text = "Codinsa2015.Launcher";
            ((System.ComponentModel.ISupportInitialize)(this.m_portNb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox m_spectateCb;
        private System.Windows.Forms.Button m_goButton;
        private System.Windows.Forms.ComboBox m_resolutions;
        private System.Windows.Forms.NumericUpDown m_portNb;
        private System.Windows.Forms.Label m_portCb;
        private System.Windows.Forms.CheckBox m_debugLogsCb;
    }
}

