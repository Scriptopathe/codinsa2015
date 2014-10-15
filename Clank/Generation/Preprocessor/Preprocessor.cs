using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Generation.Preprocessor
{
    /// <summary>
    /// Représente un pré-processeur de code, permettant de modifier le script à
    /// compiler avant son exécution.
    /// </summary>
    public class Preprocessor
    {
        const int MAX_DEPTH = 100;
        #region Variables
        IScriptIncludeLoader m_scriptIncludeLoader;
        #endregion


        #region Classes
        class ReplaceItem
        {
            public string SrcValue;
            public int SrcLine;
            public string DstValue;
            public string SourceFilename;
            public ReplaceItem(string src, string dst, string filename, int line)
            {
                SrcValue = src;
                DstValue = dst;
                SrcLine = line;
                SourceFilename = filename;
            }
        }

        /// <summary>
        /// Représente une information sur une ligne d'un script "final" : ligne de la source, et nom de la source.
        /// </summary>
        public class LineInfo
        {
            public string Source;
            public int Line;
            public LineInfo(string src, int line)
            {
                Source = src;
                Line = line;
            }
            public override string ToString()
            {
                return Source + ":" + Line;
            }
        }
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
        /// Remplis le range donné du mapping avec des lignes commençant avec le numéro de firstLine.
        /// </summary>
        void FillRange(ref Dictionary<int, LineInfo> mapping, int start, int end, LineInfo firstLine)
        {
            for(int i = start; i < end; i++)
            {
                LineInfo info = new LineInfo(firstLine.Source, firstLine.Line + i - start);
                if (mapping.ContainsKey(i))
                    mapping[i] = info;
                else
                    mapping.Add(i, info);
            }
        }
        /// <summary>
        /// Lance le pre-processor sur le script donné..
        /// </summary>
        public string Run(string script,  ref Dictionary<int, LineInfo> lineMapping, int depth=0, int firstLineAbsolute=0, string source="memory")
        {
            if (depth > MAX_DEPTH)
                throw new RessourceNotFoundException("Preprocesseur: Inclusion circulaire détectée.");

            
            // Recherche tous les include
            List<ReplaceItem> toReplace = new List<ReplaceItem>();
            string[] lines = script.Split('\n');
            for(int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Contains("#include "))
                {
                    string lineVal = line;
                    string filename = line.Trim().Replace("#include ", "");
                    toReplace.Add(new ReplaceItem(lineVal, ScriptIncludeLoader.Load(filename), filename, i));
                }
            }
            // On remplit ce qu'il y a avant notre remplacement.
            FillRange(ref lineMapping, firstLineAbsolute, firstLineAbsolute+lines.Length, new LineInfo(source, 1));

            // Remplacement
            int offset = 0;
            foreach(var tup in toReplace)
            {
                string val = Run(tup.DstValue, ref lineMapping, depth+1, tup.SrcLine + offset, tup.SourceFilename);
                int len = val.Split('\n').Length;
                
                // On remplit ce qu'il y a après ce qu'on remplace.
                FillRange(ref lineMapping, tup.SrcLine + offset + len, offset + len + lines.Length, new LineInfo(source, tup.SrcLine+2));

                script = script.Replace(tup.SrcValue, val);
                offset += len - 1;
            }

            return script;
        }
        #endregion

    }
}
