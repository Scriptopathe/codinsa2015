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
            this.m_goButton.Location = new System.Drawing.Point(13, 37);
            this.m_goButton.Name = "m_goButton";
            this.m_goButton.Size = new System.Drawing.Size(260, 49);
            this.m_goButton.TabIndex = 1;
            this.m_goButton.Text = "Go !";
            this.m_goButton.UseVisualStyleBackColor = true;
            this.m_goButton.Click += new System.EventHandler(this.m_goButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 98);
            this.Controls.Add(this.m_goButton);
            this.Controls.Add(this.m_spectateCb);
            this.Name = "Form1";
            this.Text = "Codinsa2015.Launcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox m_spectateCb;
        private System.Windows.Forms.Button m_goButton;
    }
}

