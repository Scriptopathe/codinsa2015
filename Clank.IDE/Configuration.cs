using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
namespace Clank.IDE
{
    /// <summary>
    /// Représente le fichier de configuration de l'IDE Clank.
    /// </summary>
    public class Configuration
    {
        List<string> m_recentProjects;
        /// <summary>
        /// Obtient la liste des projets récents.
        /// </summary>
        public List<string> RecentProjects
        {
            get { return m_recentProjects; }
            private set { m_recentProjects = (List<string>)value; }
        }

        /// <summary>
        /// Crée une nouvelle instance de Configuration.
        /// </summary>
        public Configuration()
        {
            RecentProjects = new List<string>();
        }

        /// <summary>
        /// Ajoute un projet à la liste des projets récents.
        /// </summary>
        public void AddRecentProject(string path)
        {
            if (m_recentProjects.Contains(path))
                m_recentProjects.Remove(path);
            else if (m_recentProjects.Count > 10)
                m_recentProjects.RemoveAt(0);

            m_recentProjects.Add(path);
        }
        /// <summary>
        /// Sauvegarde le projet dans le fichier donné.
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            Stream f = File.Open(filename, FileMode.Create);
            XmlSerializer ser = new XmlSerializer(typeof(Configuration));
            ser.Serialize(f, this);
            f.Close();
        }

        /// <summary>
        /// Charge un projet depuis le fichier donné.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Configuration Load(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {

                    Stream f = File.Open(filename, FileMode.Open);
                    XmlSerializer ser = new XmlSerializer(typeof(Configuration));
                    Configuration proj = (Configuration)ser.Deserialize(f);
                    f.Close();
                    return proj;
                }
            }
            catch (Exception e) { }
            
            return new Configuration();
        }
    }
}
