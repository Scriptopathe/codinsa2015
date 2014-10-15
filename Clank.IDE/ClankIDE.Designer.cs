namespace Clank.IDE
{
    partial class ClankIDE
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "1",
            "2",
            "3",
            "4"}, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClankIDE));
            this.m_status = new System.Windows.Forms.StatusStrip();
            this.m_statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_menu = new System.Windows.Forms.MenuStrip();
            this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_new = new System.Windows.Forms.ToolStripMenuItem();
            this.m_open = new System.Windows.Forms.ToolStripMenuItem();
            this.m_save = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.m_newProject = new System.Windows.Forms.ToolStripMenuItem();
            this.m_openProject = new System.Windows.Forms.ToolStripMenuItem();
            this.m_saveProject = new System.Windows.Forms.ToolStripMenuItem();
            this.m_recentProjectStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.générationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_generate = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_setAsMain = new System.Windows.Forms.ToolStripMenuItem();
            this.ongletsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_closeTab = new System.Windows.Forms.ToolStripMenuItem();
            this.m_mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.m_lateralSpliter = new System.Windows.Forms.SplitContainer();
            this.m_verticalSplit = new System.Windows.Forms.SplitContainer();
            this.m_codeTabs = new System.Windows.Forms.TabControl();
            this.m_errorList = new System.Windows.Forms.ListView();
            this.m_typeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_messageColumn2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_lineColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_sourceColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.m_openButton = new System.Windows.Forms.ToolStripButton();
            this.m_saveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_compileButton = new System.Windows.Forms.ToolStripButton();
            this.m_setAsMainButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_closeTabButton = new System.Windows.Forms.ToolStripButton();
            this.m_projectTree = new Clank.IDE.ProjectTree();
            this.m_status.SuspendLayout();
            this.m_menu.SuspendLayout();
            this.m_mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_lateralSpliter)).BeginInit();
            this.m_lateralSpliter.Panel1.SuspendLayout();
            this.m_lateralSpliter.Panel2.SuspendLayout();
            this.m_lateralSpliter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_verticalSplit)).BeginInit();
            this.m_verticalSplit.Panel1.SuspendLayout();
            this.m_verticalSplit.Panel2.SuspendLayout();
            this.m_verticalSplit.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_status
            // 
            this.m_status.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.m_status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_statusLabel});
            this.m_status.Location = new System.Drawing.Point(0, 375);
            this.m_status.Name = "m_status";
            this.m_status.Size = new System.Drawing.Size(1093, 22);
            this.m_status.TabIndex = 0;
            this.m_status.Text = "statusStrip1";
            // 
            // m_statusLabel
            // 
            this.m_statusLabel.Name = "m_statusLabel";
            this.m_statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // m_menu
            // 
            this.m_menu.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.m_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fichierToolStripMenuItem,
            this.générationToolStripMenuItem,
            this.ongletsToolStripMenuItem});
            this.m_menu.Location = new System.Drawing.Point(0, 0);
            this.m_menu.Name = "m_menu";
            this.m_menu.Size = new System.Drawing.Size(1093, 24);
            this.m_menu.TabIndex = 1;
            this.m_menu.Text = "menuStrip1";
            // 
            // fichierToolStripMenuItem
            // 
            this.fichierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_new,
            this.m_open,
            this.m_save,
            this.toolStripSeparator3,
            this.m_newProject,
            this.m_openProject,
            this.m_saveProject,
            this.m_recentProjectStrip});
            this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            this.fichierToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fichierToolStripMenuItem.Text = "Fichier";
            // 
            // m_new
            // 
            this.m_new.Name = "m_new";
            this.m_new.ShortcutKeyDisplayString = "";
            this.m_new.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.m_new.Size = new System.Drawing.Size(240, 22);
            this.m_new.Text = "Nouveau";
            // 
            // m_open
            // 
            this.m_open.Name = "m_open";
            this.m_open.ShortcutKeyDisplayString = "";
            this.m_open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.m_open.Size = new System.Drawing.Size(240, 22);
            this.m_open.Text = "Ouvrir...";
            // 
            // m_save
            // 
            this.m_save.Name = "m_save";
            this.m_save.ShortcutKeyDisplayString = "";
            this.m_save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.m_save.Size = new System.Drawing.Size(240, 22);
            this.m_save.Text = "Enregistrer";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(237, 6);
            // 
            // m_newProject
            // 
            this.m_newProject.Name = "m_newProject";
            this.m_newProject.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.m_newProject.Size = new System.Drawing.Size(240, 22);
            this.m_newProject.Text = "Nouveau projet";
            this.m_newProject.Click += new System.EventHandler(this.m_newProject_Click);
            // 
            // m_openProject
            // 
            this.m_openProject.Name = "m_openProject";
            this.m_openProject.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.m_openProject.Size = new System.Drawing.Size(240, 22);
            this.m_openProject.Text = "Ouvrir un projet";
            this.m_openProject.Click += new System.EventHandler(this.m_openProject_Click);
            // 
            // m_saveProject
            // 
            this.m_saveProject.Name = "m_saveProject";
            this.m_saveProject.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.m_saveProject.Size = new System.Drawing.Size(240, 22);
            this.m_saveProject.Text = "Enregister le projet";
            this.m_saveProject.Click += new System.EventHandler(this.m_saveProject_Click);
            // 
            // m_recentProjectStrip
            // 
            this.m_recentProjectStrip.Name = "m_recentProjectStrip";
            this.m_recentProjectStrip.Size = new System.Drawing.Size(240, 22);
            this.m_recentProjectStrip.Text = "Projets récents";
            // 
            // générationToolStripMenuItem
            // 
            this.générationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_generate,
            this.configurationToolStripMenuItem,
            this.m_setAsMain});
            this.générationToolStripMenuItem.Name = "générationToolStripMenuItem";
            this.générationToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.générationToolStripMenuItem.Text = "Génération";
            // 
            // m_generate
            // 
            this.m_generate.Name = "m_generate";
            this.m_generate.ShortcutKeyDisplayString = "";
            this.m_generate.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.m_generate.Size = new System.Drawing.Size(178, 22);
            this.m_generate.Text = "Générer";
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.configurationToolStripMenuItem.Text = "Propriétés du projet";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripMenuItem_Click);
            // 
            // m_setAsMain
            // 
            this.m_setAsMain.Name = "m_setAsMain";
            this.m_setAsMain.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.m_setAsMain.Size = new System.Drawing.Size(178, 22);
            this.m_setAsMain.Text = "Set as main";
            this.m_setAsMain.Click += new System.EventHandler(this.m_setAsMain_Click);
            // 
            // ongletsToolStripMenuItem
            // 
            this.ongletsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_closeTab});
            this.ongletsToolStripMenuItem.Name = "ongletsToolStripMenuItem";
            this.ongletsToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.ongletsToolStripMenuItem.Text = "Onglets";
            // 
            // m_closeTab
            // 
            this.m_closeTab.Name = "m_closeTab";
            this.m_closeTab.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.m_closeTab.Size = new System.Drawing.Size(154, 22);
            this.m_closeTab.Text = "Fermer";
            // 
            // m_mainLayout
            // 
            this.m_mainLayout.ColumnCount = 1;
            this.m_mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_mainLayout.Controls.Add(this.m_lateralSpliter, 0, 1);
            this.m_mainLayout.Controls.Add(this.toolStrip1, 0, 0);
            this.m_mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_mainLayout.Location = new System.Drawing.Point(0, 24);
            this.m_mainLayout.Name = "m_mainLayout";
            this.m_mainLayout.RowCount = 2;
            this.m_mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.m_mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_mainLayout.Size = new System.Drawing.Size(1093, 351);
            this.m_mainLayout.TabIndex = 2;
            // 
            // m_lateralSpliter
            // 
            this.m_lateralSpliter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lateralSpliter.Location = new System.Drawing.Point(3, 28);
            this.m_lateralSpliter.Name = "m_lateralSpliter";
            // 
            // m_lateralSpliter.Panel1
            // 
            this.m_lateralSpliter.Panel1.Controls.Add(this.m_verticalSplit);
            this.m_lateralSpliter.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // m_lateralSpliter.Panel2
            // 
            this.m_lateralSpliter.Panel2.Controls.Add(this.m_projectTree);
            this.m_lateralSpliter.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.m_lateralSpliter.Size = new System.Drawing.Size(1087, 320);
            this.m_lateralSpliter.SplitterDistance = 970;
            this.m_lateralSpliter.TabIndex = 3;
            // 
            // m_verticalSplit
            // 
            this.m_verticalSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_verticalSplit.Location = new System.Drawing.Point(0, 0);
            this.m_verticalSplit.Name = "m_verticalSplit";
            this.m_verticalSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // m_verticalSplit.Panel1
            // 
            this.m_verticalSplit.Panel1.Controls.Add(this.m_codeTabs);
            this.m_verticalSplit.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // m_verticalSplit.Panel2
            // 
            this.m_verticalSplit.Panel2.Controls.Add(this.m_errorList);
            this.m_verticalSplit.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.m_verticalSplit.Size = new System.Drawing.Size(970, 320);
            this.m_verticalSplit.SplitterDistance = 213;
            this.m_verticalSplit.TabIndex = 0;
            // 
            // m_codeTabs
            // 
            this.m_codeTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_codeTabs.Location = new System.Drawing.Point(0, 0);
            this.m_codeTabs.Name = "m_codeTabs";
            this.m_codeTabs.SelectedIndex = 0;
            this.m_codeTabs.Size = new System.Drawing.Size(970, 213);
            this.m_codeTabs.TabIndex = 0;
            // 
            // m_errorList
            // 
            this.m_errorList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_typeColumn,
            this.m_messageColumn2,
            this.m_lineColumn,
            this.m_sourceColumn});
            this.m_errorList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_errorList.FullRowSelect = true;
            this.m_errorList.GridLines = true;
            this.m_errorList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.m_errorList.Location = new System.Drawing.Point(0, 0);
            this.m_errorList.Name = "m_errorList";
            this.m_errorList.Size = new System.Drawing.Size(970, 103);
            this.m_errorList.TabIndex = 0;
            this.m_errorList.UseCompatibleStateImageBehavior = false;
            this.m_errorList.View = System.Windows.Forms.View.Details;
            // 
            // m_typeColumn
            // 
            this.m_typeColumn.Text = "Type";
            // 
            // m_messageColumn2
            // 
            this.m_messageColumn2.Text = "Message";
            this.m_messageColumn2.Width = 515;
            // 
            // m_lineColumn
            // 
            this.m_lineColumn.Text = "Ligne";
            this.m_lineColumn.Width = 42;
            // 
            // m_sourceColumn
            // 
            this.m_sourceColumn.Text = "Fichier";
            this.m_sourceColumn.Width = 232;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_openButton,
            this.m_saveButton,
            this.toolStripSeparator1,
            this.m_compileButton,
            this.m_setAsMainButton,
            this.toolStripSeparator2,
            this.m_closeTabButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1093, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // m_openButton
            // 
            this.m_openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_openButton.Image = global::Clank.IDE.Properties.Resources.open;
            this.m_openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_openButton.Name = "m_openButton";
            this.m_openButton.Size = new System.Drawing.Size(23, 22);
            this.m_openButton.Text = "Open";
            this.m_openButton.Click += new System.EventHandler(this.m_openButton_Click);
            // 
            // m_saveButton
            // 
            this.m_saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_saveButton.Image = global::Clank.IDE.Properties.Resources.save;
            this.m_saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_saveButton.Name = "m_saveButton";
            this.m_saveButton.Size = new System.Drawing.Size(23, 22);
            this.m_saveButton.Text = "Save";
            this.m_saveButton.ToolTipText = "Save";
            this.m_saveButton.Click += new System.EventHandler(this.m_saveButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // m_compileButton
            // 
            this.m_compileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_compileButton.Image = global::Clank.IDE.Properties.Resources.start;
            this.m_compileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_compileButton.Name = "m_compileButton";
            this.m_compileButton.Size = new System.Drawing.Size(23, 22);
            this.m_compileButton.Text = "Compile";
            this.m_compileButton.Click += new System.EventHandler(this.m_compileButton_Click);
            // 
            // m_setAsMainButton
            // 
            this.m_setAsMainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_setAsMainButton.Image = global::Clank.IDE.Properties.Resources.select;
            this.m_setAsMainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_setAsMainButton.Name = "m_setAsMainButton";
            this.m_setAsMainButton.Size = new System.Drawing.Size(23, 22);
            this.m_setAsMainButton.Text = "Set as main";
            this.m_setAsMainButton.Click += new System.EventHandler(this.m_setAsMainButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // m_closeTabButton
            // 
            this.m_closeTabButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_closeTabButton.Image = global::Clank.IDE.Properties.Resources.close;
            this.m_closeTabButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_closeTabButton.Name = "m_closeTabButton";
            this.m_closeTabButton.Size = new System.Drawing.Size(23, 22);
            this.m_closeTabButton.Text = "Ferme l\'onglet.";
            this.m_closeTabButton.Click += new System.EventHandler(this.m_closeTabButton_Click);
            // 
            // m_projectTree
            // 
            this.m_projectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_projectTree.ImageIndex = 0;
            this.m_projectTree.Location = new System.Drawing.Point(0, 0);
            this.m_projectTree.Name = "m_projectTree";
            this.m_projectTree.SelectedImageIndex = 0;
            this.m_projectTree.Size = new System.Drawing.Size(113, 320);
            this.m_projectTree.TabIndex = 0;
            // 
            // ClankIDE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1093, 397);
            this.Controls.Add(this.m_mainLayout);
            this.Controls.Add(this.m_status);
            this.Controls.Add(this.m_menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.m_menu;
            this.Name = "ClankIDE";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Clank.IDE";
            this.m_status.ResumeLayout(false);
            this.m_status.PerformLayout();
            this.m_menu.ResumeLayout(false);
            this.m_menu.PerformLayout();
            this.m_mainLayout.ResumeLayout(false);
            this.m_mainLayout.PerformLayout();
            this.m_lateralSpliter.Panel1.ResumeLayout(false);
            this.m_lateralSpliter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_lateralSpliter)).EndInit();
            this.m_lateralSpliter.ResumeLayout(false);
            this.m_verticalSplit.Panel1.ResumeLayout(false);
            this.m_verticalSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_verticalSplit)).EndInit();
            this.m_verticalSplit.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip m_status;
        private System.Windows.Forms.MenuStrip m_menu;
        private System.Windows.Forms.ToolStripMenuItem fichierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_new;
        private System.Windows.Forms.ToolStripMenuItem m_open;
        private System.Windows.Forms.ToolStripMenuItem m_save;
        private System.Windows.Forms.ToolStripMenuItem générationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_generate;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ongletsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_closeTab;
        private System.Windows.Forms.ToolStripStatusLabel m_statusLabel;
        private System.Windows.Forms.TableLayoutPanel m_mainLayout;
        private System.Windows.Forms.SplitContainer m_lateralSpliter;
        private System.Windows.Forms.SplitContainer m_verticalSplit;
        private System.Windows.Forms.TabControl m_codeTabs;
        private System.Windows.Forms.ListView m_errorList;
        private System.Windows.Forms.ColumnHeader m_typeColumn;
        private System.Windows.Forms.ColumnHeader m_lineColumn;
        private System.Windows.Forms.ColumnHeader m_messageColumn2;
        private System.Windows.Forms.ColumnHeader m_sourceColumn;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton m_openButton;
        private System.Windows.Forms.ToolStripButton m_saveButton;
        private System.Windows.Forms.ToolStripButton m_compileButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem m_setAsMain;
        private System.Windows.Forms.ToolStripButton m_setAsMainButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton m_closeTabButton;
        private ProjectTree m_projectTree;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem m_newProject;
        private System.Windows.Forms.ToolStripMenuItem m_openProject;
        private System.Windows.Forms.ToolStripMenuItem m_saveProject;
        private System.Windows.Forms.ToolStripMenuItem m_recentProjectStrip;
    }
}

