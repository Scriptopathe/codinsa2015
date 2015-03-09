namespace Clank.IDE
{
    partial class SettingsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_targetServerTB = new System.Windows.Forms.TextBox();
            this.m_targetClientTB = new System.Windows.Forms.TextBox();
            this.m_cancelButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Serveur cible:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Clients cibles:";
            // 
            // m_targetServerTB
            // 
            this.m_targetServerTB.Location = new System.Drawing.Point(92, 10);
            this.m_targetServerTB.Name = "m_targetServerTB";
            this.m_targetServerTB.Size = new System.Drawing.Size(439, 20);
            this.m_targetServerTB.TabIndex = 2;
            // 
            // m_targetClientTB
            // 
            this.m_targetClientTB.AcceptsReturn = true;
            this.m_targetClientTB.Location = new System.Drawing.Point(92, 35);
            this.m_targetClientTB.Multiline = true;
            this.m_targetClientTB.Name = "m_targetClientTB";
            this.m_targetClientTB.Size = new System.Drawing.Size(439, 111);
            this.m_targetClientTB.TabIndex = 3;
            // 
            // m_cancelButton
            // 
            this.m_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cancelButton.Location = new System.Drawing.Point(375, 152);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.m_cancelButton.TabIndex = 4;
            this.m_cancelButton.Text = "Annuler";
            this.m_cancelButton.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(456, 152);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.button2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(534, 187);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.m_cancelButton);
            this.Controls.Add(this.m_targetClientTB);
            this.Controls.Add(this.m_targetServerTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.Text = "Propriétés du projet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_targetServerTB;
        private System.Windows.Forms.TextBox m_targetClientTB;
        private System.Windows.Forms.Button m_cancelButton;
        private System.Windows.Forms.Button button2;
    }
}