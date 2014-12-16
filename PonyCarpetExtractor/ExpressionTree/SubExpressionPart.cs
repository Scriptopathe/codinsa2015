using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree
{
    /// <summary>
    /// Représentation interne d'un type.
    /// Utilisé pour l'appel de méthodes statiques.
    /// </summary>
    public class InternalTypeRepresentation
    {
        public Type T
        {
            get;
            set;
        }
        public InternalTypeRepresentation()
        {

        }
        public InternalTypeRepresentation(Type t)
        {
            T = t;
        }
    }
    /// <summary>
    /// Représentation interne d'un event.
    /// </summary>
    public class InternalEventRepresentation
    {
        public object Owner
        {
            get;
            set;
        }
        public System.Reflection.EventInfo Event
        {
            get;
            set;
        }
        public InternalEventRepresentation(object owner, System.Reflection.EventInfo evt)
        {
            Owner = owner;
            Event = evt;
        }
    }

    public class SubExpressionPart
    {
        #region Enums
        /// <summary>
        /// Définit les types de sous expressions possibles.
        /// </summary>
        public enum ExpTypes
        {
            Event,
            Variable,
            Method,
            ConstantObject,
            ConstantTypeName,
            NewObject,
            ExpressionGroup,
        }

        #endregion

        #region Variables / cache
        /// <summary>
        /// Paramètres passés en argument pour un appel de méthode ou une instantiation.
        /// </summary>
        List<IGettable> m_parameters;
        /// <summary>
        /// Paramètres génériques passés en argument pour un appel de méthode ou une instantiation.
        /// </summary>
        List<IGettable> m_genericParameters;
        /// <summary>
        /// Paramètres d'indexation passés en argument pour :
        /// - une variable
        /// - une instentiation (permet de trouver le nom du type).
        /// </summary>
        List<List<IGettable>> m_indexingParameters;
        /// <summary>
        /// Cache de la valeur constante retournée si l'expression est de type ConstantObject ou
        /// ConstantTypeName.
        /// </summary>
        object constantValueCache;
        /// <summary>
        /// Cache du résultat de FindType().
        /// </summary>
        Type findTypeResultCache;

        object[] m_parametersCache;
        Type[] m_parametersTypeCache;
        #endregion


        #region Properties
        /// <summary>
        /// Paramètres génériques passés en argument pour un appel de méthode ou une instantiation.
        /// </summary>
        public List<IGettable> GenericParameters
        {
            get { return m_genericParameters; }
            set { m_genericParameters = value; }
        }
        /// <summary>
        /// Paramètres d'indexation passés en argument pour :
        /// - une variable
        /// - une instentiation (permet de trouver le nom du type).
        /// 
        /// C'est une liste contenant des listes d'IGettable, car un type array peut 
        /// contenir un autre type array, ex :
        /// truc[,][]
        /// -> IndexingParameters va contenir deux listes : une contenant 2 arguments, 
        ///     l'autre un seul.
        /// </summary>
        public List<List<IGettable>> IndexingParameters
        {
            get { return m_indexingParameters; }
            set { m_indexingParameters = value; }
        }
        /// <summary>
        /// Type de sous expression (variable, méthode, constante...)
        /// </summary>
        public ExpTypes SubExpType
        {
            get;
            set;
        }
        /// <summary>
        /// Nom de la sous expression.
        /// Dans le cas d'une constante, permet d'obtenir sa valeur.
        /// 
        /// Si c'est une constante de type, name doit être dans le format : 
        /// assembly::name.space.type
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Groupe représentant cette sous expression.
        /// N'est défini que pour les SubExpressionPart de type
        /// ExpressionGroup.
        /// </summary>
        public ExpressionGroup Group
        {
            get;
            set;
        }
        /// <summary>
        /// Liste de paramètres si le type d'expression est Method ou NewObject.
        /// </summary>
        public List<IGettable> Parameters
        {
            protected get { return m_parameters; }
            set
            {
                m_parameters = value;
                // Opère un reset du cache des paramètres
                m_parametersCache = null;
                m_parametersTypeCache = null;
            }
        }
        /// <summary>
        /// Retourne true si la valeur de cet expression est accessible en écriture.
        /// </summary>
        public bool IsSettable
        {
            get { return SubExpType == ExpTypes.Variable; }
        }
        #endregion


        #region Methods
        /// <summary>
        /// Retourne la valeur d'une variable / méthode liée à un objet.
        /// </summary>
        /// <param name="context">contexte</param>
        /// <param name="o">object auquel est liée la variable / méthode</param>
        /// <returns></returns>
        public object GetObjectBoundValue(Context context, object o)
        {
#if DEBUG
            if (SubExpType == ExpTypes.ConstantObject || SubExpType == ExpTypes.ConstantTypeName ||
                SubExpType == ExpTypes.NewObject || SubExpType == ExpTypes.ExpressionGroup)
                throw new Exception();

#endif
            // Si l'objet lié est nul, on a fait un truc du genre :
            // machin.chouette ou machin vaut null -> exception.
            if (o == null)
                throw new InterpreterException(
                    String.Format("Le membre {0} n'est pas défini pour 'null'", Name));

            bool isStatic = (o is InternalTypeRepresentation);
            // Objet pour lequelle la méthode sera appelée : null si isStatic.
            object declObj = isStatic ? null : o;
            // Type possesseur du membre
            Type memberOwner = (Type)(isStatic ? ((InternalTypeRepresentation)o).T : o.GetType());

            if (!memberOwner.IsEnum)
            {
                // Flag supplémentaire pour la recherche de membre : Static ou Instance
                System.Reflection.BindingFlags flagSupp = isStatic ? System.Reflection.BindingFlags.Static : System.Reflection.BindingFlags.Instance;
                switch (this.SubExpType)
                {
                    case ExpTypes.Variable:
                        System.Reflection.MemberInfo[] infos = memberOwner.GetMember(Name, System.Reflection.BindingFlags.GetField |
                            System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Public | flagSupp);
                        // Vérifie que l'on aie bien un unique membre.
                        if (infos.Count() != 1)
                        {
                            throw new InterpreterException(String.Format("Le membre {0} {1} pour l'objet de type {2}.",
                                Name,
                                infos.Count() == 0 ? "n'existe pas" : "est ambigu",
                                memberOwner.Name));
                        }
                        // On récupère le MemberInfo qui nous intéresse.
                        object val;
                        System.Reflection.MemberInfo info = infos.First();
                        if (info is System.Reflection.PropertyInfo)
                            val = ((System.Reflection.PropertyInfo)info).GetValue(declObj, null);
                        else if (info is System.Reflection.FieldInfo)
                            val = ((System.Reflection.FieldInfo)info).GetValue(declObj);
                        else
                            throw new Exception("Erreur...");

                        // On regarde s'il y a des paramètres d'indexation :
                        if (IndexingParameters.Count == 0)
                        {
                            return val;
                        }
                        else
                        {
                            // On parcours les indexers.
                            // TODO : support des arrays multi-dimensionels.
                            for (int i = 0; i < IndexingParameters.Count; i++)
                            {
                                List<IGettable> parameters = IndexingParameters[i];
                                if (parameters.Count == 0)
                                    throw new Exception("Expected indexing parameter");
                                // On laisse faire le runtime pour la résolution dynamique =D
                                dynamic tmp = val;
                                dynamic index = IndexingParameters[0][0].GetValue(context);
                                val = tmp[index];
                            }
                            return val;
                        }
                    case ExpTypes.Method:
                        PrepareCacheParameters();
                        for (int i = 0; i < Parameters.Count; i++)
                        {
                            m_parametersCache[i] = Parameters[i].GetValue(context);
                            m_parametersTypeCache[i] = m_parametersCache[i].GetType();
                        }
                        // 
                        System.Reflection.MethodInfo method = memberOwner.GetMethod(Name,
                            System.Reflection.BindingFlags.Public | flagSupp,
                            null, m_parametersTypeCache, null);

                        if (method == null)
                            method = memberOwner.GetMethod(Name,
                                System.Reflection.BindingFlags.Public | flagSupp);

                        // Lève une exception si la méthode toujours pas n'existe pas.
                        if (method == null)
                        {
                            throw new InterpreterException(String.Format("La méthode {0} n'existe pas pour {1} {2}",
                                Name,
                                isStatic ? "Le type" : "l'objet",
                                memberOwner.ToString()
                                ));

                        }
                        return method.Invoke(declObj, m_parametersCache);
                    case ExpTypes.Event:
                        // Retournera un event info
                        System.Reflection.EventInfo evtInfo = memberOwner.GetEvent(Name, System.Reflection.BindingFlags.Public | flagSupp);
                        return new InternalEventRepresentation(memberOwner, evtInfo);

                }
                throw new NotImplementedException();
            }
            else
            {
                return Enum.Parse((Type)memberOwner, Name);
            }
        }
        /// <summary>
        /// Prépare le cache des paramètres.
        /// </summary>
        void PrepareCacheParameters()
        {
            if (m_parametersCache == null || m_parametersCache.Count() != Parameters.Count)
            {
                m_parametersCache = new object[Parameters.Count];
                m_parametersTypeCache = new Type[Parameters.Count];
            }
        }
        /// <summary>
        /// Retourne la valeur de cette expression, si celle ci n'est liée à aucun objet.
        /// (i.e si elle est de la forme :
        /// machin() ou machin MAIS PAS truc.machin
        /// </summary>
        /// <returns></returns>
        public object GetObjectUnboundValue(Context context)
        {
            switch (this.SubExpType)
            {
                // Groupe d'expression.
                // --------------------------------------------------------------
                case ExpTypes.ExpressionGroup:
                    return Group.GetValue(context);
                // Expression constante d'un objet
                // --------------------------------------------------------------
                case ExpTypes.ConstantObject:
                    // Objet constant : 0, "jiji", 0.0, true, etc...
                    if (constantValueCache == null)
                        constantValueCache = Parsing.ParseBasicType(this.Name);
                    return constantValueCache;

                // Expression constante d'un type.
                // --------------------------------------------------------------
                case ExpTypes.ConstantTypeName:
                    if(constantValueCache == null)
                        constantValueCache = new InternalTypeRepresentation(
                            ReflectionUtils.FindType(
                                context,
                                null,
                                Name,
                                GenericParameters,
                                IndexingParameters,
                                false));
                    
                    return constantValueCache;
                // Expression "new"
                // --------------------------------------------------------------
                case ExpTypes.NewObject:
                    // Si le Type de l'instance n'est pas encore connu,
                    if (findTypeResultCache == null)
                        findTypeResultCache = ReflectionUtils.FindType(
                            context,
                            null,
                            Name,
                            GenericParameters,
                            IndexingParameters,
                            false);

                    // Raccourci pour faire moins moche
                    var result = findTypeResultCache;

                    // C'est s'il n'y a pas de paramètres.
                    int paramsCount = Parameters == null ? 0 : Parameters.Count;
                    // Le nombre de paramètres d'index est donné par le nombre d'éléments
                    // de la dernière liste de la liste de paramètres d'indexation.
                    int indexingParametersCount = IndexingParameters.Count == 0 ? 0 : IndexingParameters.Last().Count;
                    
                    object[] args = new object[indexingParametersCount + paramsCount];
                    int j = 0;

                    // On ajoute les paramètres de taille d'array
                    if (IndexingParameters.Count != 0)
                    {
                        foreach (IGettable gettable in IndexingParameters.Last())
                        {
                            args[j] = gettable.GetValue(context);
                            j++;
                        }
                    }
                    // On ajoute les paramètres du constructeur.
                    if (Parameters != null)
                    {
                        foreach (IGettable obj in Parameters)
                        {
                            args[j] = obj.GetValue(context);
                            j++;
                        }
                    }
                    // Note : les paramètres génériques sont inclus dans le type.
                    return Activator.CreateInstance(findTypeResultCache, args);

                // Expression "new"
                // --------------------------------------------------------------
                case ExpTypes.Variable:
                    // C'est une variable locale ou globale, non associée à un objet.
                    object val;
                    if (context.LocalVariables.ContainsKey(Name))
                        val = context.LocalVariables[Name].Value;
                    else if (context.GlobalContext.Variables.ContainsKey(Name))
                        val = context.GlobalContext.Variables[Name].Value;
                    else
                    {
                        Type t = ReflectionUtils.FindType(context, null, Name, GenericParameters, IndexingParameters, false);
                        
                        if(t == null)
                            throw new InterpreterException(String.Format("Impossible de trouver la variable locale ou globale '{0}'.", Name));

                        val = new InternalTypeRepresentation(t);
                    }


                    // On regarde s'il y a des paramètres d'indexation :
                    if (IndexingParameters.Count == 0)
                    {
                        return val;
                    }
                    else
                    {
                        // On parcours les indexers.
                        // TODO : support des arrays multi-dimensionels.
                        for (int i = 0; i < IndexingParameters.Count; i++)
                        {
                            List<IGettable> parameters = IndexingParameters[i];
                            if (parameters.Count == 0)
                                throw new Exception("Expected indexing parameter");

                            // On laisse faire le runtime pour la résolution dynamique =D
                            dynamic tmp = val;
                            dynamic index = IndexingParameters[0][0].GetValue(context);
                            val = tmp[index];
                        }
                        return val;
                    }
                case ExpTypes.Method:
                    // C'est une méthode locale ou globale, non associée à un objet.
                    // Cela appellera la méthode.
                    Function func;
                    if (context.LocalVariables.ContainsKey(Name))
                    {
                        if (context.LocalVariables[Name].Value is Function)
                            func = (Function)context.LocalVariables[Name].Value;
                        else
                            throw new InterpreterException(String.Format("Impossible de trouver la fonction locale ou globale {0}.", Name));
                    }
                    else
                        throw new InterpreterException(String.Format("Impossible de trouver la fonction locale ou globale {0}.", Name));
                    // Obtient les valeurs des paramètres.
                    object[] parameterValues = new object[Parameters.Count];
                    for(int i = 0; i < Parameters.Count; i++)
                    {
                        parameterValues[i] = Parameters[i].GetValue(context);
                    }
                    return func.Call(parameterValues, context);
            }
            throw new Exception();
        }
        /// <summary>
        /// Modifie la valeur de cette expression non liée à un objet.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        public void SetObjectUnboundValue(Context context, object value)
        {
            if (SubExpType != ExpTypes.Variable)
                throw new InterpreterException(String.Format(
                    "Impossible de modifier la valeur d'une expression de type {0}",
                    SubExpType));

            // C'est une variable locale ou globale, non associée à un objet.
            if (IndexingParameters.Count == 0)
            {
                if (context.GlobalContext.Variables.ContainsKey(Name))
                    context.GlobalContext.Variables[Name].Value = value;
                else if (context.LocalVariables.ContainsKey(Name))
                    context.LocalVariables[Name].Value = value;
                else
                {
                    context.LocalVariables.Add(Name, new Mutable(value));
                    return;
                }
            }
            else
            {
                object var = null;
                if (context.GlobalContext.Variables.ContainsKey(Name))
                    var = context.GlobalContext.Variables[Name].Value;
                else if (context.LocalVariables.ContainsKey(Name))
                    var = context.LocalVariables[Name].Value;

                SetElementValue(var, value, context);
            }
        }
        /// <summary>
        /// Modifie la valeur de cette expression liée à un objet, uniquement si c'est une variable.
        /// </summary>
        public void SetObjectBoundValue(Context context, object owner, object value)
        {
            if (SubExpType != ExpTypes.Variable)
                throw new InterpreterException(String.Format(
                    "Impossible de modifier la valeur d'une expression de type {0}",
                    SubExpType));
            bool isStatic = (owner is InternalTypeRepresentation);
            // Objet pour lequelle la méthode sera appelée : null si isStatic.
            object declObj = isStatic ? null : owner;
            
            // Type possesseur du membre
            Type memberOwner = (Type)(isStatic ? ((InternalTypeRepresentation)owner).T : owner.GetType());
            // Flag supplémentaire pour la recherche de membre : Static ou Instance
            System.Reflection.BindingFlags flagSupp = isStatic ? System.Reflection.BindingFlags.Static : System.Reflection.BindingFlags.Instance;
            System.Reflection.MemberInfo[] infos = memberOwner.GetMember(Name, System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Public | flagSupp);
            // Vérifie que l'on aie bien un unique membre.
            if (infos.Count() != 1)
            {
                throw new InterpreterException(String.Format("Le membre {0} {1} pour l'objet de type {2}.",
                    Name,
                    infos.Count() == 0 ? "n'existe pas" : "est ambigu",
                    memberOwner.Name));
            }
            // On récupère le MemberInfo qui nous intéresse.
            System.Reflection.MemberInfo info = infos.First();
            if (IndexingParameters.Count == 0)
            {
                if (info is System.Reflection.PropertyInfo)
                    ((System.Reflection.PropertyInfo)info).SetValue(declObj, value, null);
                else if (info is System.Reflection.FieldInfo)
                    ((System.Reflection.FieldInfo)info).SetValue(declObj, value);
                else
                    throw new Exception("Erreur...");
            }
            else
            {
                object val;
                if (info is System.Reflection.PropertyInfo)
                    val = ((System.Reflection.PropertyInfo)info).GetValue(declObj, null);
                else if (info is System.Reflection.FieldInfo)
                    val = ((System.Reflection.FieldInfo)info).GetValue(declObj);
                else
                    throw new Exception("Erreur...");

                SetElementValue(val, value, context);
            }
        }
        /// <summary>
        /// Modifie la valeur d'un élément d'une collection.
        /// L'élément est désigné par les paramètres d'indexation.
        /// </summary>
        /// <param name="var"></param>
        /// <param name="indexingParameters"></param>
        void SetElementValue(object val, object newValue, Context context)
        {
            dynamic newValueDynamic = newValue;
            dynamic index;
            // TODO : support des arrays multi-dimensionels.
            if (IndexingParameters.Count > 1)
            {
                for (int i = 0; i <= IndexingParameters.Count - 1; i++)
                {
                    List<IGettable> parameters = IndexingParameters[i];
                    if (parameters.Count == 0)
                        throw new Exception("Expected indexing parameter");

                    // On laisse faire le runtime pour la résolution dynamique =D
                    dynamic tmp = val;
                    index = IndexingParameters[0][0].GetValue(context);
                    val = tmp[index];
                }
                dynamic enumValFinal = val;
                index = IndexingParameters.Last().First().GetValue(context);
                enumValFinal[index] = newValueDynamic;
                
            }
            else if (IndexingParameters.Count == 1)
            {
                dynamic enumValFinal = val;
                index = IndexingParameters.First().First().GetValue(context);
                enumValFinal[index] = newValueDynamic;
            }
        }
        #endregion

        /// <summary>
        /// Crée une nouvelle partie de sous expression.
        /// </summary>
        /// <param name="expType">Type de sous expression.</param>
        /// <param name="Name">Nom/Valeur de la sous-expression.</param>
        /// <param name="Parameters">Paramètres de la sous expression (si méthode ou constructeur)</param>
        public SubExpressionPart(string name, ExpTypes expType, List<IGettable> parameters)
        {
            SubExpType = expType;
            Name = name;
            Parameters = parameters;
            GenericParameters = new List<IGettable>();
            IndexingParameters = new List<List<IGettable>>();
        }
        /// <summary>
        /// Crée une SubExpressionPart à partir d'un groupe d'expression.
        /// </summary>
        /// <param name="group"></param>
        public SubExpressionPart(ExpressionGroup group)
        {
            Group = group;
            SubExpType = ExpTypes.ExpressionGroup;
            Name = "";
            Parameters = null;
            GenericParameters = new List<IGettable>();
            IndexingParameters = new List<List<IGettable>>();
        }
        /// <summary>
        /// Crée une nouvelle partie de sous expression.
        /// </summary>
        /// <param name="expType">Type de sous expression.</param>
        /// <param name="Name">Nom/Valeur de la sous-expression.</param>
        /// <param name="Parameters">Paramètres de la sous expression (si méthode ou constructeur)</param>
        public SubExpressionPart(string name, ExpTypes expType)
        {
            SubExpType = expType;
            Name = name;
            GenericParameters = new List<IGettable>();
            IndexingParameters = new List<List<IGettable>>();
        }
        /// <summary>
        /// Crée une nouvelle partie de sous expression.
        /// Le type de sous expression par défaut est "Constant"
        /// </summary>
        /// <param name="Name">Nom/Valeur de la sous-expression.</param>
        public SubExpressionPart(string name)
        {
            Name = name;
            SubExpType = ExpTypes.ConstantObject;
            GenericParameters = new List<IGettable>();
            IndexingParameters = new List<List<IGettable>>();
        }


    }
}
