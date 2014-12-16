using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace PonyCarpetExtractor.ExpressionTree
{
    /// <summary>
    /// Contexte d'un bloc.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Variables locales connues dans ce contexte.
        /// </summary>
        public Dictionary<string, Mutable> LocalVariables;
        /// <summary>
        /// Référence vers le contexte global parent.
        /// </summary>
        public GlobalContext GlobalContext
        {
            get;
            set;
        }
        #region Methods
        /// <summary>
        /// Crée une nouvelle instance du contexte.
        /// </summary>
        public Context(GlobalContext c)
        {
            LocalVariables = new Dictionary<string, Mutable>();
            GlobalContext = c;
        }
        /// <summary>
        /// Inclut le contexte donné dans ce contexte.
        /// </summary>
        /// <param name="c"></param>
        public void Include(Context c)
        {
            foreach (var kvp in c.LocalVariables)
            {
                this.LocalVariables[kvp.Key] = kvp.Value;
            }
        }
        /// <summary>
        /// Retourne une copie de ce contexte.
        /// </summary>
        public Context Copy()
        {
            Context ret = new Context(GlobalContext);
            ret.Include(this);
            return ret;
        }
        /// <summary>
        /// Effectue un reset du contexte.
        /// </summary>
        public void Reset()
        {
            LocalVariables.Clear();
        }
        #endregion  
    }
    /// <summary>
    /// Contexte global.
    /// </summary>
    public class GlobalContext
    {
        /// <summary>
        /// Liste des assemblys chargés.
        /// </summary>
        public Dictionary<string, Assembly> LoadedAssemblies
        {
            get;
            set;
        }
        private List<string> m_loadedNamespaces;
        /// <summary>
        /// Liste des namespaces chargés.
        /// </summary>
        public List<string> LoadedNamespaces
        {
            get { return m_loadedNamespaces; }
            set { m_loadedNamespaces = value; }
        }
        /// <summary>
        /// Variables locales connues dans ce contexte.
        /// </summary>
        public Dictionary<string, Mutable> Variables;

        /// <summary>
        /// Constructeur du contexte global.
        /// </summary>
        public GlobalContext()
        {
            Variables = new Dictionary<string, Mutable>();
            LoadedNamespaces = new List<string>();
            LoadedAssemblies = new Dictionary<string, Assembly>();
        }
    }
}
