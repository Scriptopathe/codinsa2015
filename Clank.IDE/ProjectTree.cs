using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clank.IDE
{
    public class ProjectTreeNode : TreeNode
    {
        public string File { get; set; }
    }
    public partial class ProjectTree : TreeView
    {
        public ProjectNode m_projectNode;

        public ProjectTree()
        {
            InitializeComponent();
            NodeMouseClick += OnNodeClicked;

            // Création de l'image list.
            ImageList lst = new ImageList();
            lst.Images.Add(Properties.Resources.folder);
            lst.Images.Add(Properties.Resources.file);
            ImageList = lst;
        }



        #region API
        /// <summary>
        /// Sélectionne le projet à afficher.
        /// </summary>
        /// <param name="node"></param>
        public void SetProject(ProjectNode node)
        {
            m_projectNode = node;
            RefreshTree();
        }

        /// <summary>
        /// Se produit lorsque l'utilisateur clique sur un noued avec la souris.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnNodeClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                SelectedNode = e.Node;

                if (e.Node is ProjectTreeNode)
                {
                    ProjectTreeNode node = ((ProjectTreeNode)e.Node);
                    // Noeud de fichier.
                    // Affiche le menu
                    ContextMenuStrip menu = new ContextMenuStrip();

                    // Ajouter un fichier existant.
                    ToolStripItem removeFile = new ToolStripMenuItem("Supprimer...");
                    removeFile.Click += (object s, EventArgs ev) =>
                    {
                        // Tentative de suppression du main.
                        if(node.File == m_projectNode.MainFile)
                        {
                            MessageBox.Show("Impossible de supprimer le fichier principal du projet.", "Opération impossible", MessageBoxButtons.OK);
                            return;
                        }

                        m_projectNode.SourceFiles.Remove(node.File);
                        Nodes["project"].Nodes[node.Name].Remove();
                    };
                    menu.Items.Add(removeFile);

                    // Separateur
                    menu.Items.Add(new ToolStripSeparator());

                    // Propriétés
                    ToolStripItem compilationTarget = new ToolStripMenuItem("Définir comme cible de compilation.");
                    compilationTarget.Click += (object s, EventArgs ev) =>
                    {
                        string old = System.IO.Path.GetFileName(m_projectNode.MainFile);
                        if(old != null)
                            if(Nodes["project"].Nodes[old] != null)
                                Nodes["project"].Nodes[old].Text = Nodes["project"].Nodes[old].Name; ;
                        m_projectNode.SetMainfile(node.File);
                        node.Text = node.Name + " (main)";
                    };
                    menu.Items.Add(compilationTarget);



                    menu.Show(this, e.X, e.Y);
                }
                else  // Noeud principal.
                {
                    // Affiche le menu
                    ContextMenuStrip menu = new ContextMenuStrip();

                    // Ajouter un nouveau fichier
                    ToolStripItem addNew = new ToolStripMenuItem("Ajouter un nouveau fichier...");
                    addNew.Click += AddNewFile;
                    menu.Items.Add(addNew);

                    // Ajouter un fichier existant.
                    ToolStripItem addExisting = new ToolStripMenuItem("Ajouter un fichier existant...");
                    addExisting.Click += AddExistingFile;
                    menu.Items.Add(addExisting);

                    // Separateur
                    menu.Items.Add(new ToolStripSeparator());

                    // Propriétés
                    ToolStripItem item = new ToolStripMenuItem("Propriétés...");
                    item.Click += OpenPropertiesWindow;
                    menu.Items.Add(item);



                    menu.Show(this, e.X, e.Y);
                }
            }
            
        }

        /// <summary>
        /// Ajoute un fichier existant au projet.
        /// </summary>
        void AddNewFile(object sender, EventArgs e)
        {
            
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.Filter = "Clank source (*.clank)|*.clank";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(dlg.FileName, "main\r\n{\r\n\r\n}\r\n");
                m_projectNode.AddSource(dlg.FileName);

                ProjectTreeNode node = new ProjectTreeNode();
                node.Text = System.IO.Path.GetFileName(dlg.FileName);
                node.File = dlg.FileName;
                node.Name = node.Text;
                node.ImageIndex = 1;
                node.SelectedImageIndex = 1;
                Nodes["project"].Nodes.Add(node);
            }
        }

        /// <summary>
        /// Ajoute un fichier existant au projet.
        /// </summary>
        void AddExistingFile(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                m_projectNode.AddSource(dlg.FileName);

                ProjectTreeNode node = new ProjectTreeNode();
                node.Text =  System.IO.Path.GetFileName(dlg.FileName);
                node.File = dlg.FileName;
                node.Name = node.Text;
                node.ImageIndex = 1;
                node.SelectedImageIndex = 1;
                Nodes["project"].Nodes.Add(node);
            }
        }

        /// <summary>
        /// Ouvre la fenêtre de dialogue des propriétés.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OpenPropertiesWindow(object sender, EventArgs e)
        {
            SettingsDialog dlg = new SettingsDialog();
            dlg.Settings = m_projectNode.Settings;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_projectNode.Settings = dlg.Settings;
            }
        }

        /// <summary>
        /// Mets à jour l'arbre avec les éléments du projet.
        /// </summary>
        public void RefreshTree()
        {
            this.Nodes.Clear();
            this.Nodes.Add("project", m_projectNode.Name, 0);
            
            foreach(string str in m_projectNode.SourceFiles)
            {
                ProjectTreeNode node = new ProjectTreeNode();
                node.Text =  System.IO.Path.GetFileName(str);
                node.File = m_projectNode.GetFullFilename(str);
                node.Name = node.Text;
                node.ImageIndex = 1;
                node.SelectedImageIndex = 1;
                if (str == m_projectNode.MainFile)
                    node.Text += " (main)";
                this.Nodes["project"].Nodes.Add(node);
            }

            Nodes["project"].Expand();
        }
        #endregion

    }
}
