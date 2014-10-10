using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Generation.Preprocessor
{
    /// <summary>
    /// Représente un pré-processeur de code, permettant de modifier le script à
    /// compiler avant son exécution.
    /// </summary>
    public class Preprocessor
    {
        const int MAX_DEPTH = 5000;
        #region Variables
        IScriptIncludeLoader m_scriptIncludeLoader;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit une référence vers un objet fournissant des méchanismes
        /// de chargement de scripts à partir d'une uri.
        /// 
        /// Par défaut, il s'agit d'un FileIncludeLoader, qui recherche les fichiers sur le disque.
        /// </summary>
        public IScriptIncludeLoader ScriptIncludeLoader
        {
            get { return m_scriptIncludeLoader; }
            set { m_scriptIncludeLoader = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance du pré-processeur.
        /// </summary>
        public Preprocessor()
        {
            ScriptIncludeLoader = new FileIncludeLoader();
        }
        /// <summary>
        /// Lance le pre-processor sur le script donné..
        /// </summary>
        public string Run(string script, int depth=0)
        {
            if (depth > MAX_DEPTH)
                throw new InvalidOperationException("Preprocesseur: Inclusion circulaire détectée.");

            // Recherche tous les include
            List<Tuple<string, string>> toReplace = new List<Tuple<string, string>>();
            string[] lines = script.Split('\n');
            foreach(string line in lines)
            {
                if (line.Contains("#include "))
                {
                    string lineVal = line;
                    string filename = line.Trim().Replace("#include ", "");
                    toReplace.Add(new Tuple<string, string>(lineVal, filename));
                }
            }

            // Remplacement
            foreach(var tup in toReplace)
            {
                script = script.Replace(tup.Item1, Run(ScriptIncludeLoader.Load(tup.Item2), depth+1));
            }

            return script;
        }
        #endregion

    }
}
