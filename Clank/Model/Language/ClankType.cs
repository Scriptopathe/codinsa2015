using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    public enum JSONType
    {
        String,
        Int,
        Float,
        Array,
        Object,
        Bool,
        UnknownGeneric
    }
    /// <summary>
    /// Représente un type de base du language Clank.Core.
    /// 
    /// Les types de base ne véhiculent pas d'informations sur les arguments génériques, ni sur les tableaux.
    /// Les informations qu'ils véhiculent servent au mapping des méthodes de types built-in dans les différents langages, ou de
    /// types custom dont les méthodes contiennent du code pour chaque language supporté.
    /// </summary>
    public class ClankType
    {
        /// <summary>
        /// Nom du type.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Nom des paramètres génériques du type.
        /// </summary>
        public List<string> GenericArgumentNames { get; set; }
        /// <summary>
        /// Variables d'instances contenues dans ce type.
        /// </summary>
        public Dictionary<string, Variable> InstanceVariables { get; set; }
        /// <summary>
        /// Retourne les méthodes d'instance contenues dans ce type.
        /// </summary>
        public Dictionary<string, FunctionDeclaration> InstanceMethods { get; set; }
        /// <summary>
        /// Retourne vrai si le type est public.
        /// </summary>
        public virtual bool IsPublic
        {
            get;
            set;
        }
        /// <summary>
        /// Obtient ou définit une valeur indiquant si ce type est un type énuméré.
        /// </summary>
        public virtual bool IsEnum
        {
            get;
            set;
        }
        /// <summary>
        /// Si le type est un type énuméré, représente les différentes valeurs contenues 
        /// dans l'énumération. Fait correspondre à chaque nom sa valeur.
        /// </summary>
        public Dictionary<string, int> EnumValues { get; set; }
        /// <summary>
        /// Obtient ou définit une valeur indiquant si ce type est un type "macro".
        /// </summary>
        public virtual bool IsMacro
        {
            get;
            set;
        }
        /// <summary>
        /// Obtient ou définit une valeur indiquant si ce type peut être sérialisé tel quel.
        /// /!\ Un type ayant SupportSerialization a true ne peut pas forcément être sérialisé en tant
        /// que paramètre générique. (ex : types built in).
        /// </summary>
        public virtual bool SupportSerialization
        {
            get;
            set;
        }

        /// <summary>
        /// Méta-datas données aux générateurs de code concernant cette classe.
        /// Ces méta-data sont interprétées différemment selon le générateur utilisé.
        /// Clefs : language identifier, nom de propriété => valeur de la propriété.
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> LanguageMetadata { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur si ce type peut être sérialisé dans le cas où il
        /// est paramètre générique d'un type ayant pour variable d'instance une variable
        /// de ce type.
        /// Ex :
        /// class Ex&lt;T&gt;
        /// {
        ///     T var;
        /// }
        /// Ex&lt;string&gt; var;
        /// 
        /// var est sérializable si string supporte la sérialization as generic.
        /// </summary>
        public virtual bool SupportSerializationAsGeneric
        {
            get;
            set;
        }
        /// <summary>
        /// Retourne vrai ce type est built-in dans le langage.
        /// </summary>
        public virtual bool IsBuiltIn
        {
            get;
            set;
        }
        /// <summary>
        /// Obtient ou définit le type JSON associé à ce type.
        /// </summary>
        public virtual JSONType JType
        {
            get;
            set;
        }

        /// <summary>
        /// Si JType == Array, obtient ou définit le type des éléments de cet Array.
        /// </summary>
        public ClankType JArrayElementType
        {
            get;
            set;
        }


        /// <summary>
        /// Crée une nouvelle instance de ClankType.
        /// </summary>
        public ClankType()
        {
            InstanceVariables = new Dictionary<string, Variable>();
            InstanceMethods = new Dictionary<string, FunctionDeclaration>();
            GenericArgumentNames = new List<string>();
            LanguageMetadata = new Dictionary<string, Dictionary<string, string>>();
            EnumValues = new Dictionary<string, int>();
            IsPublic = false;
            IsEnum = false;
            JType = JSONType.Object;
        }

        /// <summary>
        /// Retourne une valeur indiquant si ce type a des variables d'instances génériques, 
        /// ou contient des variables d'instance ayant des variables d'instances génériques.
        /// Cette méthode permet de déterminer si la totalité des types des membres de la 
        /// classe peuvent être assimilés à des types connus à la compilation.
        /// </summary>
        /// <returns></returns>
        public bool HasGenericParameterTypeMembers()
        {
            foreach (var kvp in InstanceVariables)
            {
                if (kvp.Value.Type.BaseType is Language.GenericParameterType)
                    return true;
                else if (kvp.Value.Type.BaseType.HasGenericParameterTypeMembers())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Retourne le nom par lequel se référer à ce type dans une TypeTable.
        /// </summary>
        /// <returns></returns>
        public virtual string GetFullName()
        {
            return Name;
        }

        /// <summary>
        /// Retourne une instance de ce type, avec les paramètres génériques non renseignés.
        /// </summary>
        public ClankTypeInstance AsInstance()
        {
            Language.ClankTypeInstance instance = new Language.ClankTypeInstance();
            instance.BaseType = this;
            // Crée une instance du type, avec ses paramètres génériques.
            int i = 0;
            instance.GenericArguments.AddRange(this.GenericArgumentNames.Select((string str) =>
            {
                int haha = i;
                i++;
                return new Language.ClankTypeInstance()
                {
                    BaseType = new Language.GenericParameterType()
                    {
                        Prefix = this.GetFullName(),
                        ParamId = haha,
                        Name = str
                    },
                };
            }));

            return instance;
        }
        /// <summary>
        /// Retourne le nom du type avec ses arguments génériques.
        /// </summary>
        /// <returns></returns>
        public virtual string GetFullNameAndGenericArgs()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name);
            if (GenericArgumentNames.Count != 0)
            {
                builder.Append("<");
                foreach (string arg in GenericArgumentNames)
                    builder.Append(arg + (arg == GenericArgumentNames.Last() ? "" : ","));
                builder.Append(">");
            }
            return builder.ToString();
        }
        #region Static
        public static ClankType Int32 = new ClankType() { Name = "int", IsPublic = true, JType = JSONType.Int, IsBuiltIn = true, SupportSerialization = true};
        public static ClankType Float = new ClankType() { Name = "float", IsPublic = true, JType = JSONType.Float, IsBuiltIn = true , SupportSerialization = true};
        public static ClankType String = new ClankType() { Name = "string", IsPublic = true, JType = JSONType.String, IsBuiltIn = true, SupportSerialization = true };
        public static ClankType Void = new ClankType() { Name = "void", IsPublic = true, JType = JSONType.Object, IsBuiltIn = true };
        public static ClankType Bool = new ClankType() { Name = "bool", IsPublic = true, JType = JSONType.Bool, IsBuiltIn = true , SupportSerialization = true};
        public static ClankType GenericParameter = new ClankType() { Name = "GenericParameter", IsPublic = true};
        #endregion

        public override string ToString()
        {
            return GetFullNameAndGenericArgs();
        }
    }

    /// <summary>
    /// Représente un type énuméré.
    /// </summary>
    public class ClankTypeEnum : ClankType
    {
        public override bool IsEnum
        {
            get
            {
                return true;
            }
            set
            {
            }
        }
        /// <summary>
        /// Membres de l'enum.
        /// </summary>
        public List<string> Members { get; set; }

        /// <summary>
        /// Crée une nouvelle instance de ClankTypeEnum.
        /// </summary>
        public ClankTypeEnum()
        {
            Members = new List<string>();
        }


    }
}
