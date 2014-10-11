using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Clank.IDE
{
    public class FileHelper
    {
        /// <summary>
        /// Renames the specified file.
        /// </summary>
        /// <param name="oldPath">Full path of file to rename.</param>
        /// <param name="newName">New file name.</param>
        public static void RenameFile(string oldPath, string newName)
        {
            if (String.IsNullOrEmpty(oldPath))
                throw new ArgumentNullException("oldPath");
            if (String.IsNullOrEmpty(newName))
                throw new ArgumentNullException("newName");

            string oldName = Path.GetFileName(oldPath);

            // if the file name is changed
            if (!String.Equals(oldName, newName, StringComparison.CurrentCulture))
            {
                string folder = Path.GetDirectoryName(oldPath);
                string newPath = Path.Combine(folder, newName);
                bool changeCase = String.Equals(oldName, newName, StringComparison.CurrentCultureIgnoreCase);

                // if renamed file already exists and not just changing case
                if (File.Exists(newPath) && !changeCase)
                {
                    throw new IOException(String.Format("File already exists:\n{0}", newPath));
                }
                else if (changeCase)
                {
                    // Move fails when changing case, so need to perform two moves
                    string tempPath = Path.Combine(folder, Guid.NewGuid().ToString());
                    Directory.Move(oldPath, tempPath);
                    Directory.Move(tempPath, newPath);
                }
                else
                {
                    Directory.Move(oldPath, newPath);
                }
            }
        }
        /// <summary>
        /// Returns the relative path to the parent folder of filename, followed by a "\\"
        /// </summary>
        /// <param name="filename"></param>
        public static string GetFolderRelativePath(string filename)
        {
            string newRelativePath = "";
            string[] strs = filename.Split('\\');
            strs[strs.Count() - 1] = "";
            
            foreach (string str in strs)
            {
                if (str != "")
                {
                    newRelativePath += str + "\\";
                }
            }
            return newRelativePath;
        }
        /// <summary>
        /// Removes the .. in the path string.
        /// </summary>
        /// <returns></returns>
        public static string GetCorrectPath(string path)
        {
            string newString = "";
            string[] units = path.Split('\\');
            for (int i = 0; i < units.Count() - 1; i++)
            {
                if (units[i + 1] == "..")
                    i++;
                else
                    newString += units[i] + "\\";
            }
            newString += units.Last();
            return newString;
        }
    }
}
