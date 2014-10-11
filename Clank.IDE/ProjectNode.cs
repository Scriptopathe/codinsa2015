using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
namespace Clank.IDE
{
    /// <summary>
    /// Représente un projet en langage clank.
    /// </summary>
    public class ProjectNode
    {
        /// <summary>
        /// Nom du projet.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Retourne le fichier où est enregistré le projet.
        /// </summary>
        public string SavePath { get; set; }
        /// <summary>
        /// Fichiers sources.
        /// </summary>
        public List<string> SourceFiles { get; set; }
        /// <summary>
        /// Fichier principal à compiler.
        /// </summary>
        public string MainFile { get; set; }
        /// <summary>
        /// Paramètres de compilation.
        /// </summary>
        public ProjectSettings Settings { get; set; }

        public ProjectNode()
        {
            SourceFiles = new List<string>();
            Settings = new ProjectSettings();
            Name = "My project.";
            SavePath = "./";
        }

        /// <summary>
        /// Ajoute une source au projet actuel.
        /// </summary>
        /// <param name="sourceFullPath"></param>
        public void AddSource(string sourceFullPath)
        {
            SourceFiles.Add(GetStoredFilename(sourceFullPath));
        }

        /// <summary>
        /// Définit le chemin d'accès au fichier principal.
        /// </summary>
        /// <param name="fullpath"></param>
        public void SetMainfile(string fullpath)
        {
            MainFile = GetStoredFilename(fullpath);
        }

        /// <summary>
        /// Obtient le nom de fichier à stocker dans SourceFiles / MainFile : relatif au dossier
        /// de SavePath.
        /// </summary>
        /// <param name="sourceFullpath"></param>
        /// <returns></returns>
        public string GetStoredFilename(string sourceFullpath)
        {
            string dir = Path.GetFullPath(Path.GetDirectoryName(SavePath));

            Uri newUri = new Uri(Path.GetFullPath(sourceFullpath));
            Uri from = new Uri(dir).MakeRelativeUri(newUri);
            return from.ToString();
        }

        /// <summary>
        /// Retourne le chemin d'accès réel du fichier.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string GetFullFilename(string filename)
        {
            Uri dir = new Uri(Path.GetFullPath(Path.GetDirectoryName(SavePath)));
            Uri fileRel = new Uri(dir.ToString() + "\\..\\" + filename);
            string final = Uri.UnescapeDataString(fileRel.AbsolutePath);
            return final;
        }
        /// <summary>
        /// Retourne vrai si le projet contient le fichier donné.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool ContainsFile(string filename)
        {
            Uri testUri = new Uri(filename);
            foreach(string source in SourceFiles)
            {
                Uri srcUri = new Uri(GetFullFilename(source));
                
                if (srcUri == testUri)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Sauvegarde le projet dans le fichier donné.
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            Stream f = File.Open(filename, FileMode.Create);
            XmlSerializer ser = new XmlSerializer(typeof(ProjectNode));
            ser.Serialize(f, this);
            f.Close();
        }

        /// <summary>
        /// Charge un projet depuis le fichier donné.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static ProjectNode Load(string filename)
        {
            Stream f = File.Open(filename, FileMode.Open);
            XmlSerializer ser = new XmlSerializer(typeof(ProjectNode));
            ProjectNode proj = (ProjectNode)ser.Deserialize(f);
            proj.SavePath = filename;
            f.Close();
            return proj;
        }
    }
}
