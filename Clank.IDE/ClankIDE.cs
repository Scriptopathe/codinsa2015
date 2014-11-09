using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using System.IO;
namespace Clank.IDE
{


    public partial class ClankIDE : Form
    {
        #region Variables
        List<PageInfo> m_pageInfos;
        ProjectNode m_project;
        Core.Tools.EventLog.EventLogHandler m_handlerFunction;
        List<Core.Tools.EventLog.Entry> m_errors;
        Timer m_generationTimer;
        Configuration m_configuration;
        
        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de la fenêtre d'IDE.
        /// </summary>
        public ClankIDE()
        {
            InitializeComponent();

            // Création des events.
            m_pageInfos = new List<PageInfo>();
            m_new.Click += (object sender, EventArgs e) =>
            {
                CreateNewPage("new (" + m_pageInfos.Count + ")", false);
            };
            m_closeTab.Click += (object sender, EventArgs e) =>
            {
                CloseCurrentPage();
            };
            m_open.Click += OpenPage;
            m_generate.Click += (object sender, EventArgs e) =>
            {
                GenerateCurrent();
            };
            m_save.Click += (object sender, EventArgs e) =>
            {
                SaveCurrent();
            };

            m_projectTree.NodeMouseDoubleClick += OnProjectNodeDoubleClick;

            // Setup des logs
            m_errors = new List<Core.Tools.EventLog.Entry>();
            m_handlerFunction = new Core.Tools.EventLog.EventLogHandler(AddEntry);

            InitErrorList();

            // Simule l'ouverture d'un projet.
            m_project = new ProjectNode();
            m_project.MainFile = null;
            m_project.Settings.ServerTarget = new Core.Generation.GenerationTarget("CS", "server.cs");
            m_project.Settings.ClientTargets = new List<Core.Generation.GenerationTarget>() { new Core.Generation.GenerationTarget("CS", "client.cs") };
            SetStatusMessage("Prêt.");
            m_projectTree.SetProject(m_project);
            

            // Démarre le timer de compilation
            m_generationTimer = new Timer() { Interval = 1000 };
            m_generationTimer.Tick += OnCompileTimer;

            // Charge la config
            m_configuration = Configuration.Load("config.xml");
            SetupRecentProjectsList();

        }

        /// <summary>
        /// Compile le projet pour afficher les erreurs à intervalle de temps régulier
        /// si le joueur modifie un fichier à compiler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnCompileTimer(object sender, EventArgs e)
        {
            GenerateCurrent(false, true);
        }

        /// <summary>
        /// Mets en place la liste des projets récents.
        /// </summary>
        void SetupRecentProjectsList()
        {
            m_recentProjectStrip.DropDownItems.Clear();
            foreach(string str in m_configuration.RecentProjects)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(str);
                item.Click += (object sender, EventArgs e) =>
                {
                    OpenProjectFile(str);
                };
                m_recentProjectStrip.DropDownItems.Add(item);
            }
        }


        #region Main functions
        /// <summary>
        /// Ouvre une page en demandant à l'utilisateur de charger un fichier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OpenPage(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.InitialDirectory = "./";
            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file = dlg.FileName;
                CreateNewPage(file, true);
            }
        }
        /// <summary>
        /// Crée une nouvelle page.
        /// </summary>
        void CreateNewPage(string fullpath, bool load)
        {
            // Création de l'éditeur de code.
            ICSharpCode.TextEditor.TextEditorControl editor = new ICSharpCode.TextEditor.TextEditorControl();
            string dir = "res\\";
            FileSyntaxModeProvider fsmProvider;
            if (Directory.Exists(dir))
            {
                fsmProvider = new FileSyntaxModeProvider(dir);
                
                HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider);
                editor.SetHighlighting("clank");
            }
            editor.ConvertTabsToSpaces = true;
            editor.Dock = System.Windows.Forms.DockStyle.Fill;
            editor.IsReadOnly = false;
            editor.Location = new System.Drawing.Point(0, 0);
            editor.Size = new System.Drawing.Size(591, 249);
            editor.Font = new System.Drawing.Font("Consolas", 10);
            editor.TabIndex = 0;
            editor.Text = "main\r\n{\r\n}\r\n";
            editor.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // Ajout d'un nouveau tab page.
            int index = m_codeTabs.TabCount;
            string display = fullpath;
            if (File.Exists(fullpath))
            {
                display = Path.GetFileName(fullpath);
            }
            TabPage page = new TabPage(display);
            page.Controls.Add(editor);
            PageInfo info = new PageInfo() { SourceFile = fullpath, Page = page, Editor = editor };
            m_pageInfos.Add(info);

            editor.TextChanged += (object sender, EventArgs e) => { OnEditorTextChanged(info); };
            
            // Chargement ?
            if (load && File.Exists(fullpath))
            {
                string text = File.ReadAllText(fullpath);
                editor.Text = text;
            }
            
            m_codeTabs.TabPages.Add(page);
            m_codeTabs.SelectedTab = page;
        }

        /// <summary>
        /// Se produit lorsque l'utilisateur modifie le texte d'un des éditeurs, cela
        /// a pour effet de relancer le timer pour la génération automatique.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnEditorTextChanged(PageInfo info)
        {
            m_generationTimer.Stop();
            m_generationTimer.Start();
            info.Page.Text = Path.GetFileName(info.SourceFile) + "*";
        }
        
        /// <summary>
        /// Crée une map nom de fichier (par rapport au dossier contenant le main file) / contenu en mémoire pour une compilation
        /// à la volée (afin de vérifier les erreurs).
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> MapFilesInMemory()
        {
            Dictionary<string, string> files = new Dictionary<string, string>();
            string mainFilePath = m_project.GetFullFilename(m_project.MainFile);
            string baseDir = Path.GetDirectoryName(mainFilePath);
            foreach(string file in m_project.SourceFiles)
            {
                // Chemin absolu
                string absolutePath = m_project.GetFullFilename(file);
                string dirName = Path.GetDirectoryName(m_project.GetFullFilename(m_project.MainFile));
                string relativeToMainfile = m_project.GetFilenameRelativeTo(dirName + "\\", absolutePath);

                // Si le fichier est déjà ouvert.
                var page = GetPageByFullFilename(absolutePath);
                string content;
                if (page == null)
                {
                    if (File.Exists(absolutePath))
                        content = File.ReadAllText(absolutePath, Encoding.UTF8);
                    else
                        content = "(file does not exist anymore)";
                }
                    
                else
                    content = page.Editor.Text;

                files.Add(relativeToMainfile, content);
            }
            return files;
        }
        /// <summary>
        /// Génère le main script du projet.
        /// </summary>
        void GenerateCurrent(bool saveOutputFiles=true, bool compileFromMemory=false)
        {
            // Vérification de la possibilité de la compilation.
            if(m_project.MainFile == null)
            {
                if(!compileFromMemory)
                    MessageBox.Show("Le projet actuel ne contient pas de main file !");
                return;
            }
            else if( !File.Exists(m_project.GetFullFilename(m_project.MainFile)))
            {
                if(!compileFromMemory)
                    MessageBox.Show("La cible de compilation principale '" + m_project.GetFullFilename(m_project.MainFile) + "' est introuvable.");
                return;
            }

            // On arrête le timer de génération.
            m_generationTimer.Stop();
            m_errors.Clear();

            // On nettoie la liste d'erreur.
            InitErrorList();

            SetStatusMessage("Compilation en cours...");
            Clank.Core.Generation.ProjectGenerator generator = new Core.Generation.ProjectGenerator();

            // Définit le répertoire de base pour les include comme le répertoire contenant le main file.
            string dir = m_project.GetFullFilename(m_project.MainFile);

            // Configuration du préprocesseur.
            if (compileFromMemory)
            {
                var m = MapFilesInMemory();
                Clank.Core.Generation.Preprocessor.MemoryIncludeLoader loader = new Core.Generation.Preprocessor.MemoryIncludeLoader();
                foreach (var kvp in m) { loader.AddFile(kvp.Key, kvp.Value); }
                generator.Preprocessor.ScriptIncludeLoader = loader;
            }
            else
            {
                Clank.Core.Generation.Preprocessor.FileIncludeLoader loader = new Core.Generation.Preprocessor.FileIncludeLoader();
                loader.BaseDirectory = Path.GetDirectoryName(dir);
                generator.Preprocessor.ScriptIncludeLoader = loader;
            }
            List<Core.Generation.OutputFile> files = new List<Core.Generation.OutputFile>();
            DateTime start = DateTime.Now;

            bool throwOnError = false;
            if (throwOnError)
            {
                // Début de la génération
                files = generator.Generate("#include " + Path.GetFileName(m_project.MainFile), m_project.Settings.ServerTarget, m_project.Settings.ClientTargets, m_handlerFunction);
            }
            else
            {
                // Début de la génération
                try
                {
                    files = generator.Generate("#include " + Path.GetFileName(m_project.MainFile), m_project.Settings.ServerTarget, m_project.Settings.ClientTargets, m_handlerFunction);
                }
                catch (Exception e)
                {

                    m_handlerFunction(new Core.Tools.EventLog.Entry(Core.Tools.EventLog.EntryType.Error, "Erreur interne du compilateur: " + e.Message + ".\r\n" + e.StackTrace, 0, 0, "[unknown]"));
                }
                finally { }
            }

            double elapsedTime = (DateTime.Now - start).TotalSeconds;
            // Informe l'utilisateur que la compilation est terminée.
            if (files.Count != 1 + m_project.Settings.ClientTargets.Count)
                SetStatusMessage("Echec de la compilation !");
            else
                SetStatusMessage("Compilation terminée. (" + elapsedTime + "s)");
            
            // Ecriture des fichiers.
            foreach(var file in files)
            {
                string path = Path.GetDirectoryName(m_project.SavePath) + "\\" + file.Name;
                File.WriteAllText(path, file.Content);
            }

        }

        /// <summary>
        /// Sauve le fichier en cours d'édition.
        /// </summary>
        void SaveCurrent()
        {
            if (m_codeTabs.SelectedIndex == -1)
                return;

            var info = m_pageInfos[m_codeTabs.SelectedIndex];
            string filename = info.SourceFile;
            string content = info.Editor.Text;
            System.IO.File.WriteAllText(filename, content, Encoding.UTF8);
            info.Page.Text = Path.GetFileName(info.SourceFile);
            SetStatusMessage("Fichier " + filename + " sauvegardé avec succès.");
        }

        /// <summary>
        /// Appelé lorsque un fichier du projet est double cliqué : on l'ouvre.
        /// </summary>
        void OnProjectNodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node is ProjectTreeNode)
            {
                ProjectTreeNode node = (ProjectTreeNode)e.Node;
                OpenTabPage(node.File);
            }
        }

        #endregion


        #region Init
        bool __listInitialized = false;
        /// <summary>
        /// Initialise la liste d'erreurs.
        /// </summary>
        void InitErrorList()
        {
            m_errorList.Clear();
            m_errorList.Items.Clear();
            m_errorList.Groups.Clear();
            m_errorList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                    m_typeColumn,
                    m_messageColumn2,
                    m_lineColumn,
                    m_sourceColumn});
            m_errorList.Groups.Clear();
            m_errorList.Groups.Add(Core.Tools.EventLog.EntryType.Error.ToString(), "Erreurs");
            m_errorList.Groups.Add(Core.Tools.EventLog.EntryType.Warning.ToString(), "Warnings");
            m_errorList.Groups.Add(Core.Tools.EventLog.EntryType.Message.ToString(), "Messages");

            if (!__listInitialized)
            {
                m_errorList.SmallImageList = new ImageList();
                m_errorList.SmallImageList.Images.Add(Properties.Resources.error);
                m_errorList.SmallImageList.Images.Add(Properties.Resources.warning);
                m_errorList.SmallImageList.Images.Add(Properties.Resources.warning);
                m_errorList.ItemActivate += ItemActivate;
            }
            __listInitialized = true;
        }

        #endregion

        #region Events
        /// <summary>
        /// Ouvre la page contenant le fichier passé en argument, si elle n'existe pas, elle est chargée.
        /// </summary>
        /// <param name="fullpath"></param>
        void OpenTabPage(string fullPath)
        {
            string shorName = Path.GetFileName(fullPath);
            var page = GetPageByShortFilename(shorName);
            fullPath = Path.GetFullPath(fullPath);
            // Si la page n'existe pas
            if (page == null && File.Exists(fullPath))
            {
                // Charge le fichier
                CreateNewPage(fullPath, true);
                page = GetPageByShortFilename(shorName);
            }

            // Déplace le curseur à la ligne concernée.
            if (page != null)
            {
                m_codeTabs.SelectedIndex = m_pageInfos.IndexOf(page);
            }
        }
        /// <summary>
        /// Se produit lorsqu'un item de la liste est double cliqué.
        /// Ramène le curseur vers la ligne concernée.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ItemActivate(object sender, EventArgs e)
        {
            int id = m_errorList.SelectedIndices[0];
            var item = m_errors[id];
            string baseDir = Path.GetDirectoryName(m_project.GetFullFilename(m_project.MainFile));
            string fullPath = baseDir + "\\" + item.Source;
            string shorName = Path.GetFileName(fullPath);
            var page = GetPageByShortFilename(shorName);

            // Si la page n'existe pas
            if(page == null && File.Exists(fullPath))
            {
                // Charge le fichier
                CreateNewPage(fullPath, true);
                page = GetPageByShortFilename(shorName);
            }

            // Déplace le curseur à la ligne concernée.
            if (page != null && item.Line > 0)
            {
                TextLocation start = new TextLocation(0, item.Line - 1);
                TextLocation end = new TextLocation(1000, item.Line - 1);
                page.Editor.ActiveTextAreaControl.SelectionManager.SetSelection(start, end);
                page.Editor.ActiveTextAreaControl.Caret.Position = end;
                m_codeTabs.SelectedIndex = m_pageInfos.IndexOf(page);
            }
        }
        /// <summary>
        /// Ajoute une entrée sur la liste d'erreurs.
        /// </summary>
        void AddEntry(Core.Tools.EventLog.Entry entry)
        {
            string[] items = new string[] { entry.Type.ToString(), entry.Message, entry.Line.ToString(), entry.Source };
            int imageId;
            switch (entry.Type)
            {
                case Core.Tools.EventLog.EntryType.Error: imageId = 0; break;
                case Core.Tools.EventLog.EntryType.Warning: imageId = 1; break;
                case Core.Tools.EventLog.EntryType.Message: imageId = 2; break;
                default: imageId = 2; break;
            }
            ListViewItem lstItem = new ListViewItem(items, imageId, m_errorList.Groups[entry.Type.ToString()]);
            
            m_errorList.Items.Add(lstItem);
            m_errors.Add(entry);

        }

        /// <summary>
        /// Ferme l'onglet courrant (demande confirmation si travail non sauvé).
        /// </summary>
        void CloseCurrentPage()
        {
            if (m_codeTabs.TabCount != 0 && m_codeTabs.SelectedIndex != -1)
            {
                int index = m_codeTabs.SelectedIndex;
                PageInfo info = m_pageInfos[index];
                if (info.Page.Text.Contains("*"))
                {
                    // Page non sauvegardée :
                    var res = MessageBox.Show("Le fichier '" + info.SourceFile + "' contient des modifications non enregistrées. Voulez-vous les sauvegarder ?",
                        "Modifications non enregistrées", MessageBoxButtons.YesNoCancel);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                        SaveCurrent();
                    else if (res == System.Windows.Forms.DialogResult.Cancel)
                        return;
                        
                }
                m_pageInfos.RemoveAt(m_codeTabs.SelectedIndex);
                m_codeTabs.TabPages[m_codeTabs.SelectedIndex].Dispose();
                
            }
        }

        /// <summary>
        /// Ouvre un projet existant en le chargeant depuis le fichier donné.
        /// </summary>
        /// <param name="filename"></param>
        void OpenProjectFile(string filename)
        {
            ProjectNode proj = ProjectNode.Load(filename);
            m_project = proj;
            m_projectTree.SetProject(proj);
            m_codeTabs.TabPages.Clear();
            m_pageInfos.Clear();
            Text = "Clank.IDE - " + m_project.Name + " (" + m_project.SavePath + ")";
            m_configuration.AddRecentProject(filename);
            m_configuration.Save("config.xml");
            SetStatusMessage("Projet '" + m_project.Name + "' ouvert avec succès.");
        }
        #endregion

        #region Status
        /// <summary>
        /// Définit le message de la barre de statut.
        /// </summary>
        /// <param name="status"></param>
        public void SetStatusMessage(string status)
        {
            m_statusLabel.Text = status;
        }
        #endregion

        #endregion

        #region Tools
        /// <summary>
        /// Retourne la page ayant le nom (nom du fichier court) passé en paramètre.
        /// [Déconseillé]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PageInfo GetPageByShortFilename(string name)
        {
            foreach(PageInfo info in m_pageInfos)
            {
                string src = Path.GetFileName(info.SourceFile);
                if (name == src)
                    return info;
            }
            return null;
        }
        /// <summary>
        /// Retourne la page ayant le fichier source (full path) passé en paramètre.
        /// </summary>
        public PageInfo GetPageByFullFilename(string fullpath)
        {
            string fullPathNormalized = fullpath.Replace("/", "\\");
            foreach (PageInfo info in m_pageInfos)
            {
                string src = info.SourceFile.Replace("/", "\\");
                if (fullPathNormalized == src)
                    return info;
            }
            return null;
        }
        #endregion

        #region Code du générateur
        private void m_openButton_Click(object sender, EventArgs e)
        {
            OpenPage(sender, e);
        }

        private void m_saveButton_Click(object sender, EventArgs e)
        {
            SaveCurrent();
        }

        private void m_compileButton_Click(object sender, EventArgs e)
        {
            GenerateCurrent();
        }

        private void m_setAsMain_Click(object sender, EventArgs e)
        {
            if (m_codeTabs.SelectedIndex == -1)
            {
                m_projectTree.RefreshTree();
                return;
            }
            if (m_project.ContainsFile(m_pageInfos[m_codeTabs.SelectedIndex].SourceFile))
            {
                m_project.SetMainfile(m_pageInfos[m_codeTabs.SelectedIndex].SourceFile);
                m_projectTree.RefreshTree();
                SetStatusMessage("Le fichier '" + m_project.MainFile + "' est désormais le fichier principal.");
            }
            else
            {
                MessageBox.Show("Le fichier " + m_pageInfos[m_codeTabs.SelectedIndex].SourceFile + " n'existe pas dans le projet !", "Opération invalide");
            }
        }
        private void m_setAsMainButton_Click(object sender, EventArgs e)
        {
            m_setAsMain_Click(sender, e);
        }
        private void m_closeTabButton_Click(object sender, EventArgs e)
        {
            CloseCurrentPage();
        }
        private void m_saveProject_Click(object sender, EventArgs e)
        {
            m_project.Save(m_project.SavePath);
            SetStatusMessage("Projet '" + m_project.SavePath + "' sauvegardé avec succès.");
        }

        /// <summary>
        /// Ouvre un projet définit par l'utilisateur.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_openProject_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "Clank project (*.clankproject)|*.clankproject|All files (*.*)|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OpenProjectFile(dlg.FileName);
                SetupRecentProjectsList();
            }
        }
        /// <summary>
        /// Crée un nouveau projet vide.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_newProject_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.Filter = "Clank project (*.clankproject)|*.clankproject";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ProjectNode project = new ProjectNode();
                project.SavePath = dlg.FileName;
                project.Name = "Nouveau projet.";
                project.Settings.ServerTarget = new Core.Generation.GenerationTarget("CS", "server.cs");
                project.Settings.ClientTargets = new List<Core.Generation.GenerationTarget>() { new Core.Generation.GenerationTarget("CS", "client.cs") };
                m_project = project;

                m_saveProject_Click(sender, e);
                m_errorList.Clear();
                m_projectTree.SetProject(m_project);
            }
        }
        /// <summary>
        /// Fait apparaître la boite de configuration du projet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsDialog dlg = new SettingsDialog();
            dlg.Settings = m_project.Settings;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_project.Settings = dlg.Settings;
                SetStatusMessage("Modifications appliquées avec succès.");
            }
        }
        #endregion



 








    }

    public class PageInfo
    {
        public string SourceFile { get; set; }
        public TabPage Page { get; set; }
        public TextEditorControl Editor { get; set; }
    }


}
