using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Model.Language
{
    public enum JSONType
    {
        String,
        Int,
        Float,
        Array,
        Object,
        Bool,
    }
    /// <summary>
    /// Représente un type de base du language Clank.
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
        /// Obtient ou définit une valeur indiquant si ce type est un type "macro".
        /// </summary>
        public virtual bool IsMacro
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
        /// Crée une nouvelle instance de ClankType.
        /// </summary>
        public ClankType()
        {
            InstanceVariables = new Dictionary<string, Variable>();
            InstanceMethods = new Dictionary<string, FunctionDeclaration>();
            GenericArgumentNames = new List<string>();
            IsPublic = false;
            IsEnum = false;
            JType = JSONType.Object;
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
        /// Retourne le nom du type avec ses arguments génériques.
        /// </summary>
        /// <returns></returns>
        public virtual string GetFullNameAndGenericArgs()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name);
            builder.Append("<");
            foreach (string arg in GenericArgumentNames)
                builder.Append(arg + (arg == GenericArgumentNames.Last() ? "" : ","));
            builder.Append(">");
            return builder.ToString();
        }
        #region Static
        public static ClankType Array = new ClankType() { Name = "Array", IsPublic = true, JType = JSONType.Array};
        public static ClankType Int32 = new ClankType() { Name = "int", IsPublic = true, JType = JSONType.Int };
        public static ClankType Float = new ClankType() { Name = "float", IsPublic = true, JType = JSONType.Float };
        public static ClankType String = new ClankType() { Name = "string", IsPublic = true, JType = JSONType.String };
        public static ClankType Void = new ClankType() { Name = "void", IsPublic = true, JType = JSONType.Object };
        public static ClankType List = new ClankType() { Name = "List", IsPublic = true, JType = JSONType.Array};
        public static ClankType Dictionary = new ClankType() { Name = "Dictionary", IsPublic = true, JType = JSONType.Array };
        public static ClankType Bool = new ClankType() { Name = "bool", IsPublic = true, JType = JSONType.Bool };
        public static ClankType GenericParameter = new ClankType() { Name = "GenericParameter", IsPublic = true };
        #endregion
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
