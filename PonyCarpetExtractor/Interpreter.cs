using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using PonyCarpetExtractor.ExpressionTree;
using PonyCarpetExtractor.SyntaxParsing;
using PonyCarpetExtractor.SemanticParsing;
using System.Reflection;
namespace PonyCarpetExtractor
{
    /// <summary>
    /// Instance principale de l'interpréteur.
    /// </summary>
    public class Interpreter
    {
        #region Delegates / events
        public delegate void PutsDelegate(string str);
        /// <summary>
        /// Action produite lorsque l'interpréteur appelle la méthode 
        /// Interpreter.Puts(string)
        /// </summary>
        public PutsDelegate OnPuts
        {
            get;
            set;
        }
        /// <summary>
        /// Action produite lorsque l'interpréteur envoie un exception.
        /// </summary>
        public PutsDelegate OnError
        {
            get;
            set;
        }
        #endregion

        #region Variables / Properties

        /// <summary>
        /// Contexte principal.
        /// </summary>
        Context m_mainContext;

        /// <summary>
        /// Accède au contexte principal de l'interpréteur.
        /// </summary>
        public Context MainContext
        {
            get { return m_mainContext; }
        }


        /// <summary>
        /// Obtient ou définit une valeur indiquant si l'interpréteur doit lever une exception lors
        /// d'une erreur.
        /// </summary>
        public bool ThrowOnError
        {
            get;
            set;
        }
        
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de l'interpreter PCE.
        /// </summary>
        public Interpreter()
        {
            Operators.InitOperators();
            Reset();
        }
        /// <summary>
        /// Remet l'interpréteur dans son état d'origine.
        /// </summary>
        public void Reset()
        {
            GlobalContext globalContext = new GlobalContext();
            // Namespaces de base
            globalContext.LoadedNamespaces.Add("");
            globalContext.LoadedNamespaces.Add("System");
            globalContext.LoadedNamespaces.Add("System.Text");
            globalContext.LoadedNamespaces.Add("System.Collections.Generic");
            globalContext.LoadedNamespaces.Add("Interpreter");
            // Assemblies de base
            globalContext.LoadedAssemblies = new Dictionary<string, Assembly>();
            globalContext.LoadedAssemblies.Add("mscorlib", Assembly.GetAssembly(typeof(char)));
            globalContext.LoadedAssemblies.Add("local", Assembly.GetExecutingAssembly());
            

            m_mainContext = new Context(globalContext);
            m_mainContext.LocalVariables.Add("Interpreter", new Mutable(this));
        }
        /// <summary>
        /// Interprête une chaine.
        /// </summary>
        public void Eval(string str)
        {
            if (ThrowOnError)
            {
                var block = SemanticParser.ParseBlock(SyntaxicParser.Parse(str));
                block.Execute(m_mainContext, null, null, null, false);
            }
            else
            {
                try
                {

                    var block = SemanticParser.ParseBlock(SyntaxicParser.Parse(str));
                    block.Execute(m_mainContext, null, null, null, false);

                }
                catch (Exception e)
                {
                    OnError(e.Message);
                }
            }
        }
        /// <summary>
        /// Envoie l'objet spécifié dans la fonction Puts précisée
        /// pour cette instance de l'interpréteur.
        /// </summary>
        /// <param name="str"></param>
        public void Puts(object obj)
        {
            if(OnPuts != null)
                OnPuts(obj.ToString());
            
        }
        #endregion
    }
}