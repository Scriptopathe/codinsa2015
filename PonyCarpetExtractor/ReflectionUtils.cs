using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace PonyCarpetExtractor
{
    /// <summary>
    /// Classe contenant des outils de réflection.
    /// </summary>
    public class ReflectionUtils
    {
        /// <summary>
        /// Retourne l'assembly qui contient le type donné, parmi
        /// les assemblis donnés.
        /// </summary>
        /// <returns></returns>
        public static Assembly SeekAssemblyContainingType(IEnumerable<Assembly> assemblies, string typename)
        {
            Assembly firstMatch = null;
            foreach (Assembly a in assemblies)
            {
                if (a.GetType(typename) != null)
                {
                    if (firstMatch == null)
                        firstMatch = a;
                    else
                        throw new InterpreterException(String.Format("Type ambigü : Le type {0} est contenu dans les assemblys {1} et {2}",
                            firstMatch.FullName, a.FullName));
                }
            }
            throw new InterpreterException("Impossible de trouver une assembly contenant le type : " + typename);
        }
        /// <summary>
        /// Retourne le type représenté par la chaine donnée si le type est un type système,
        /// null sinon.
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static Type SystemType(string typename)
        {
            string end = "";
            if (typename.Contains('[')) // --> array
            {
                int firstBracket = typename.IndexOf('[');
                string tmp = typename.Substring(0, firstBracket);
                end = typename.Substring(firstBracket);
                typename = tmp;
                // Si typename vaut au départ :
                // char[][]
                // ici : typename vaut "char" et end vaut "[][]"
            }
            string startName = null;
            
            switch (typename)
            {
                    
                case "char":
                    startName = "System.Char";
                    break;
                case "short":
                    startName = "System.Int16";
                    break;
                case "ushort":
                    startName = "System.UInt16";
                    break;
                case "int":
                    startName = "System.Int32";
                    break;
                case "uint":
                    startName = "System.UInt32";
                    break;
                case "long":
                    startName = "System.Int64";
                    break;
                case "ulong":
                    startName = "System.UInt64";
                    break;
                case "byte":
                    startName = "System.Byte";
                    break;
                case "string":
                    startName = "System.String";
                    break;
                case "object":
                    startName = "System.Object";
                    break;
                case "bool":
                    startName = "System.Boolean";
                    break;
                case "float":
                    startName = "System.Single";
                    break;
                case "double":
                    startName = "System.Double";
                    break;
            }
            if (startName != null)
            {
                return Assembly.GetAssembly(typeof(int)).GetType(startName + end);
            }
            return null;
        }
        /// <summary>
        /// Retourne le nom complet du type se trouvant dans un des assemblys donnés, ainsi que
        /// l'assembly dans lequel il se trouve.
        /// </summary>
        /// <param name="context">Contexte actuel.</param>
        /// <param name="incompleteTypeName">Nom incomplet du type à retrouver.</param>
        /// <returns></returns>
        public static Tuple<Assembly, string> FindFullName(ExpressionTree.Context context, string incompleteTypeName)
        {
            // Vérifie d'abord parmi les types système.
            var sysType = SystemType(incompleteTypeName);
            if (sysType != null)
                return new Tuple<Assembly, string>(Assembly.GetAssembly(sysType), sysType.FullName);

            string fullName = null;

            // Assemblies du contexte.
            IEnumerable<Assembly> assemblies = context.GlobalContext.LoadedAssemblies.Values;
            Assembly assembly = null;

            foreach (Assembly a in assemblies)
            {
                string temp = FindFullName(context, a, incompleteTypeName, false);
                if (temp != null)
                    // Vérifie qu'on a pas déja un résultat
                    // Si c'est le cas, le type existe dans 2 assemblies.
                    if (fullName == null)
                    {
                        fullName = temp;
                        assembly = a;
                    }
                    else
                        throw new InterpreterException(String.Format("Le type {0} existe dans les assemblys {1} et {2}",
                            incompleteTypeName, a.FullName, assembly.FullName));
            }
            if (fullName == null)
                throw new InterpreterException(String.Format("Le type {0} n'a pas été trouvé.",
                    incompleteTypeName));
            else
                return new Tuple<Assembly,string>(assembly, fullName);
        }
        /// <summary>
        /// Retourne le nom complet du type se trouvant dans un des assemblys donnés, ainsi que
        /// l'assembly dans lequel il se trouve.
        /// </summary>
        /// <param name="loadedAssemblies">Assemblies chargés</param>
        /// <param name="loadedNamespaces">Namespaces chargés</param>
        /// <param name="incompleteTypeName">Nom incomplet du type à retrouver.</param>
        /// <returns></returns>
        public static Tuple<Assembly, string> FindFullName(Dictionary<string, Assembly> loadedAssemblies, List<string> loadedNamespaces, string incompleteTypeName)
        {
            ExpressionTree.GlobalContext glob = new ExpressionTree.GlobalContext();
            ExpressionTree.Context context = new ExpressionTree.Context(glob);
            glob.LoadedAssemblies = loadedAssemblies;
            glob.LoadedNamespaces = loadedNamespaces;
            return FindFullName(context, incompleteTypeName);
        }
        /// <summary>
        /// Retourne true si le nom donné est un nom de type avec les assemblies et 
        /// namespaces chargés.
        /// </summary>
        /// <param name="loadedAssemblies"></param>
        /// <param name="loadedNamespaces"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static bool IsTypeName(Dictionary<string, Assembly> loadedAssemblies, List<string> loadedNamespaces, string typename)
        {
            if (SystemType(typename) != null)
                return true;

            foreach (Assembly assembly in loadedAssemblies.Values)
            {
                foreach (string @namespace in loadedNamespaces)
                {
                    if (assembly.GetType(@namespace + "." + typename) != null)
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Retourne le nom complet du type dans l'assembly donné.
        /// En gros cela rajoute le nom de l'assembly au nom du type donné.
        /// </summary>
        /// <param name="context">Contexte actuel.</param>
        /// <param name="assembly">Assembly contenant le type.</param>
        /// <param name="incompleteTypeName">Nom incomplet du type à retrouver.</param>
        /// <returns></returns>
        public static string FindFullName(ExpressionTree.Context context, Assembly assembly, string incompleteTypeName, bool throwOnError)
        {
            // Vérifie d'abord parmi les types système.
            var sysType = SystemType(incompleteTypeName);
            if (sysType != null)
                return sysType.FullName;

            string namespaceName = null;

            // Vérifie que le nom du type ne soit pas déjà complet :
            if (assembly.GetType(incompleteTypeName) != null)
                return incompleteTypeName;

            // Si ce n'est pas le cas :
            foreach (string _namespace in context.GlobalContext.LoadedNamespaces)
            {
                // On ajoute Namespace+Type
                string temp = String.Format("{0}.{1}", _namespace, incompleteTypeName);
                // Si le type existe dans cet assembly.
                if (assembly.GetType(temp) != null)
                {
                    if (namespaceName == null)
                        namespaceName = _namespace;
                    else
                        // Balance l'exception même si throwOnError vaut true.
                        throw new InterpreterException(String.Format("Le type {0} existe dans les namespaces {1} et {2}",
                            incompleteTypeName, namespaceName, _namespace));
                }
            }
            if (namespaceName == null)
            {
                if (throwOnError)
                    throw new InterpreterException(String.Format("Le type {0} ne peut pas être reconnu dans l'assembly {1}",
                        incompleteTypeName, assembly.FullName));
                else
                    return null;
            }
            else
            {
                return String.Format("{0}.{1}", namespaceName, incompleteTypeName);
            }
        }
        /// <summary>
        /// Retourne une valeur indiquant, pour le nom de type donné :
        /// - si c'est un array, le(s) paramètre(s) d'initialization : 
        ///         char[9][] => retourne 9
        ///         char[9, 5][] => retourne 9, 5
        /// - sinon, null
        /// </summary>
        /// <param name="str">Le string représentant le type pour lequel on doit chercher
        /// les paramètres de taille si c'est un array.</param>
        /// <param name="corrected">String créé à partir du string original, qui contient le nom du type
        /// sans les paramètres de taille.</param>
        /// <returns></returns>
        public static bool IsTypeArray(string str, out List<int> results, out string corrected)
        {
            // Profondeur des <> (au cas où le type soit générique).
            int brackedDepth = 0;
            results = new List<int>();
            for (int i = 0; i < str.Count(); i++)
            {
                if (str[i] == '<')
                    brackedDepth++;
                else if (str[i] == '>')
                    brackedDepth--;
                else if (brackedDepth == 0)
                {
                    // On est obligatoirement sorti des [] :
                    // Ex : machin<t1[][], t2[][]>[6, 4, 2]
                    if (str[i] == '[')
                    {
                        int nextBrace = str.IndexOf(']', i);
                        string subString = str.Substring(i+1, nextBrace - i-1);
                        // On doit avoir 9, 4, 2
                        string[] paramss = subString.Split(',');
                        foreach (string param in paramss)
                        {
                            if(param != "")
                                results.Add(int.Parse(param));
                        }
                        // et voilou
                        // On fait la correction du string : 
                        // Déjà, on prévoit les virgules si besoin est :
                        StringBuilder comas = new StringBuilder();
                        for (int j = 1; j < paramss.Count(); j++) { comas.Append(','); }

                        corrected = String.Format("{0}{2}{1}",
                            str.Substring(0, i+1), str.Substring(nextBrace), comas.ToString());
                        return true;
                    }                      
                }
            }
            corrected = str;
            return false;
        }

        /// <summary>
        /// Recherche un type défini par un nom donné.
        /// Cette méthode n'est pas faîte pour être exécutée un trop grand nombre de fois.
        /// Il est préférable de mettre en cache les résultats.
        /// </summary>
        /// <param name="context">Contexte où se trouve le type.</param>
        /// <param name="typename">Nom complet du type. Ex : Machin.Truc.Bidule.
        /// Si le nom n'est pas complet, le paramètre isFullTypeName doit valoir false.</param>
        /// <param name="assemblyNamme">Nom de l'assembly contenant le type.
        /// Null ou "default" signifie qu'il s'agit de l'assembly en cours d'exécution.</param>
        /// <returns>Un tuple contenant : le type de l'objet, le type des arguments génériques si le type
        /// concerné est générique, sinon, null.</returns>
        public static Type FindType(ExpressionTree.Context context,
            string assemblyName,
            string typename,
            List<ExpressionTree.IGettable> genericParameters,
            List<List<ExpressionTree.IGettable>> indexingParameters,
            bool isFullTypeName)
        {
            // Assembly content le type
            Assembly assembly;
            bool isGeneric = genericParameters.Count != 0;
            bool isArray = indexingParameters.Count != 0;

            // Build le vrai nom complet du type.
            // ----------------------------------
            StringBuilder fullTypeNameBuilder = new StringBuilder();
            fullTypeNameBuilder.Append(typename);
            if (isGeneric)
            {
                fullTypeNameBuilder.Append('`');
                fullTypeNameBuilder.Append(genericParameters.Count);
            }
            if (isArray)
            {
                foreach (List<ExpressionTree.IGettable> parameters in indexingParameters)
                {
                    fullTypeNameBuilder.Append('[');
                    for (int i = 0; i < parameters.Count - 1; i++) { fullTypeNameBuilder.Append(','); }
                    fullTypeNameBuilder.Append(']');
                }
            }

            typename = fullTypeNameBuilder.ToString();

            // Trouve l'assembly contenant le nom du type.
            if (assemblyName == null)
                assembly = null;
            else if (assemblyName == "default")
                // méthode sale pour obtenir l'assembly contenant les types systèmes.
                assembly = Assembly.GetAssembly(typeof(char));
            else
                assembly = context.GlobalContext.LoadedAssemblies[assemblyName];

            // Maintenant il faut juste trouver le type dans un assembly.
            // -----------------------------------------------------------

            // Si le nom du type n'est pas complet, on essaie de retrouver le nom complet : 
            if (!isFullTypeName)
            {
                if (assembly == null)
                {
                    // Assembly / nom complet du type
                    var tup = FindFullName(context, typename);
                    assembly = tup.Item1;
                    typename = tup.Item2;
                }
                else
                {
                    typename = FindFullName(context, assembly, typename, true);
                }
            }
            else
            {
                // Si on a pas le nom de l'assembly : on va chercher !
                if (assembly == null)
                    assembly = SeekAssemblyContainingType(context.GlobalContext.LoadedAssemblies.Values, typename);
            }
            if (genericParameters.Count == 0)
            {
                // On n'a qu'à chercher le type dans l'assembly.
                return assembly.GetType(typename);
            }
            else
            {
                Type[] genericParams = new Type[genericParameters.Count];
                for(int i = 0; i < genericParameters.Count; i++)
                {
                    genericParams[i] = ((ExpressionTree.InternalTypeRepresentation)genericParameters[i].GetValue(context)).T;
                }
                return assembly.GetType(typename).MakeGenericType(genericParams);
            }
        }
        /*/// <summary>
        /// Recherche un type défini par un nom donné.
        /// Cette méthode n'est pas faîte pour être exécutée un trop grand nombre de fois.
        /// Il est préférable de mettre en cache les résultats.
        /// </summary>
        /// <param name="context">Contexte où se trouve le type.</param>
        /// <param name="typename">Nom complet du type. Ex : Machin.Truc.Bidule.
        /// Si le nom n'est pas complet, le paramètre isFullTypeName doit valoir false.</param>
        /// <param name="assemblyNamme">Nom de l'assembly contenant le type.
        /// Null ou "default" signifie qu'il s'agit de l'assembly en cours d'exécution.</param>
        /// <returns>Un tuple contenant : le type de l'objet, le type des arguments génériques si le type
        /// concerné est générique, sinon, null.</returns>
        public static FindTypeResult FindTypeOld(ExpressionTree.Context context, string assemblyName, string typename, bool isFullTypeName)
        {
            // Assembly content le type
            List<int> sizeParameters; // paramètres de taille s'il s'agit d'un array.
            Assembly assembly;
            bool isGeneric = typename.Contains('<');

            // Première chose à faire : si l'expression est un array : virer les paramètres de taille.
            // ex : char[45][] => char[][]
            string correctedStr;
            bool isArray = IsTypeArray(typename, out sizeParameters, out correctedStr);
            typename = correctedStr;

            // On cherche dans l'assembly par défaut :
            if (assemblyName == null)
                assembly = null;
            else if (assemblyName == "default")
                // méthode sale pour obtenir l'assembly contenant les types systèmes.
                assembly = Assembly.GetAssembly(typeof(char)); 
            else
                assembly = context.GlobalContext.LoadedAssemblies[assemblyName];

            // Cas n°1 (le plus simple) : le type n'est pas générique :
            if (!isGeneric)
            {
                // Si le nom du type n'est pas complet, on essaie de retrouver le nom complet : 
                if (!isFullTypeName)
                {
                    if (assembly == null)
                    {
                        // Assembly / nom complet du type
                        var tup = FindFullName(context, typename);
                        assembly = tup.Item1;
                        typename = tup.Item2;
                    }
                    else
                    {
                        typename = FindFullName(context, assembly, typename, true);
                    }
                }
                else
                {
                    // Si on a pas le nom de l'assembly : on va chercher !
                    if (assembly == null)
                        assembly = SeekAssemblyContainingType(context.GlobalContext.LoadedAssemblies.Values, typename);
                }
                // On n'a qu'à chercher le type dans l'assembly.
                return new FindTypeResult(assembly.GetType(typename), null, sizeParameters.ToArray());
            }
            else
            {
                // Et là, c'est le drame.... on a un type générique, merde !             
                // Si le nom du type n'est pas complet, on essaie de retrouver le nom complet : 
                
                // Le nom compréhensible pour l'assembly : nous on reçoit du
                // List<Machin>
                // Et on doit donner à l'assembly du : System.Collection.Generic.List`1
                string trueTypeName; 
                Tuple<string, string[]> genericArgs;
                string endingBrackets = Parsing.TypeNameGetEndingArrayBrackets(typename);
                if (!isFullTypeName)
                {
                    // Le nom n'est pas complet, il faut :
                    // 1. trouver le nom complet
                    // 2. transformer les trucs du genre List<truc> en List`1
                    if (assembly == null)
                    {
                        // Assembly / nom complet du type
                        var typeIdentifier = FindFullName(context, typename);
                        assembly = typeIdentifier.Item1;
                        trueTypeName = typeIdentifier.Item2;
                    }
                    else
                    {
                        trueTypeName = FindFullName(context, assembly, typename, true);
                    }
                    
                    // Nom du type, nom des paramètres de type types génériques
                    genericArgs = Parsing.TypeNameParseGenericArguments(trueTypeName);
                    // Nom du type reconnu par l'assembly.
                    trueTypeName = String.Format("{0}`{1}", genericArgs.Item1, 
                        genericArgs.Item2.Count());
                }
                else
                {
                    // On obtient un tuple qui contient :
                    // Nom du type, nom des paramètres de type types génériques
                    genericArgs = Parsing.TypeNameParseGenericArguments(typename);
                    // Nom du type reconnu par l'assembly.
                    trueTypeName = String.Format("{0}`{1}", genericArgs.Item1,
                        genericArgs.Item2.Count());
                }

                // Si l'assembly n'existe pas encore :
                if(assembly == null)
                    assembly = SeekAssemblyContainingType(context.GlobalContext.LoadedAssemblies.Values, trueTypeName);

                // Paramètres de Type générique
                Type[] genericTypeParameters = new Type[genericArgs.Item2.Count()];
                for(int i = 0; i < genericArgs.Item2.Count(); i++)
                {
                    genericTypeParameters[i] = FindType(context, null, genericArgs.Item2[i], false).Type;
                }

                // Le type final
                Type type;
                if (!isArray)
                {
                    type = assembly.GetType(trueTypeName).MakeGenericType(genericTypeParameters);
                    return new FindTypeResult(type, null, sizeParameters.ToArray());
                }
                else
                {
                    // is array
                    if (isGeneric)
                    {
                        // On construit le type générique à la base du tableau
                        type = assembly.GetType(trueTypeName).MakeGenericType(genericTypeParameters);
                        // On construit les types tableaux dérivés :
                        // /!\ Il faut partir de la fin des tableaux, le tableau le plus extérieur
                        // étant le premier de gauche à droite !
                        // Normalement, endingBrackets ne contient pas les paramètres de taille
                        // du tableau.
                        bool opened = false;
                        List<int> arraySizes = new List<int>();
                        int arrayId = 0;
                        for (int i = 0; i < endingBrackets.Count(); i++)
                        {
                            if (endingBrackets[i] == '[')
                            {
                                opened = true;
                                arraySizes.Add(1);
                                
                            }
                            else if (endingBrackets[i] == ']')
                            {
                                opened = false;
                                arrayId++;
                            }
                            else if (endingBrackets[i] == ',')
                            {
                                if (opened)
                                    arraySizes[arrayId]++;
                                else
                                    throw new InterpreterException("Erreur de syntaxe : ',' non attendu dans la déclaration de tableau.");
                            }
                            else
                            {
                                throw new InterpreterException(String.Format("Erreur de syntaxe : Caractère innatendu '{0}' dans la déclaration de tableau.",
                                    endingBrackets[i]));
                            }

                        }
                        // Inverse l'ordre des éléments
                        foreach (int size in arraySizes)
                        {
                            type = type.MakeArrayType(size);
                        }

                        return new FindTypeResult(type, genericTypeParameters, sizeParameters.ToArray());
                    }
                    else
                    {
                        // Rajoute les [] à la fin du type.
                        type = assembly.GetType(trueTypeName+endingBrackets);
                        return new FindTypeResult(type, genericTypeParameters, sizeParameters.ToArray());
                    }
                }
                
            }
        }*/
        /// <summary>
        /// Appelle une fonction func d'un objet obj, avec le string
        /// qui représente ses arguments argStr.
        /// Retourne la valeur retournée par la fonction.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <param name="argsStr"></param>
        public static object CallFunction(object obj, string func, string argStr, object[] parameters)
        {
            Type type = obj.GetType();
            // Methode avec paramètres.
            if (argStr != "default" && argStr != "")
            {
                // Types des paramètres
                Type[] parametersTypes = new Type[parameters.Count()];
                for (int i = 0; i < parameters.Count(); i++)
                {
                    parametersTypes[i] = parameters[i].GetType();
                }

                // On récupère la méthode qui correspond aux paramètres d'entrée.
                System.Reflection.MethodInfo info;
                info = type.GetMethod(func, parametersTypes);
                if (info == null)
                    throw new InterpreterException("la méthode appelée n'existe pas avec les paramètres donnés");

                // On invoque la méthode
                return info.Invoke(obj, parameters);
            }
            else
            {
                System.Reflection.MethodInfo info = type.GetMethod(func, Type.EmptyTypes);
                if (info == null)
                    throw new InterpreterException("la méthode appelée n'existe pas avec les paramètres donnés");
                return info.Invoke(obj, null);
            }
        }

        /// <summary>
        /// Retourne la valeur d'un champ donné.
        /// </summary>
        /// <param name="obj">object sur lequel rechercher le champ</param>
        /// <param name="propertyName">nom de la propriété.</param>
        /// <returns>la valeur de la propriété</returns>
        public static object GetFieldValue(object obj, string propertyName)
        {
            Type t = obj.GetType();
            // On cherche un champ
            System.Reflection.FieldInfo fieldInfo;
            fieldInfo = t.GetField(propertyName);
            if (fieldInfo != null)
                // Ici c'est OK, on peut prendre sa valeur
                return fieldInfo.GetValue(obj);

            // Sinon on cherche une propriété
            System.Reflection.PropertyInfo propInfo;
            propInfo = t.GetProperty(propertyName);
            if (propInfo != null)
                return propInfo.GetValue(obj, null);

            // Sinon bah c'est mort
            throw new InterpreterException("La propriété " + propertyName +
                                " n'existe pas pour l'objet de type " + t.Name);
        }
        /// <summary>
        /// Change la valeur d'un champ donné.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetFieldValue(object obj, string propertyName, object value)
        {
            Type t = obj.GetType();
            try
            {
                bool isOK = false;
                // On cherche un champ
                System.Reflection.FieldInfo fieldInfo;
                fieldInfo = t.GetField(propertyName);
                if (fieldInfo != null)
                {
                    // Ici c'est OK, on peut prendre sa valeur
                    fieldInfo.SetValue(obj, value);
                    isOK = true;
                }

                // Sinon on cherche une propriété
                System.Reflection.PropertyInfo propInfo;
                propInfo = t.GetProperty(propertyName);
                if (propInfo != null)
                {
                    propInfo.SetValue(obj, value, null);
                    isOK = true;
                }
                // Si on a fait quelque chose, on retourne, sinon, on se mange l'exception.
                if (isOK)
                    return;
            }
            catch
            {
                throw new InterpreterException("La propriété " + propertyName + " n'est pas de type " + value.GetType().ToString());
            }
            // Sinon bah c'est mort
            throw new InterpreterException("La propriété " + propertyName +
                                " n'existe pas pour l'objet de type " + t.FullName);
        }
    }
}
