﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Représente une instance de type utilisable dans le language Clank.Core.
    /// 
    /// Ces instances de type ne servent qu'à connaître les paramètres du type pour la génération de code.
    /// </summary>
    public class ClankTypeInstance
    {
        /// <summary>
        /// Type de base de cette instance de type.
        /// </summary>
        public ClankType BaseType { get; set; }
        /// <summary>
        /// Indique si ce type est générique.
        /// Un tableau ne peut pas être générique.
        /// </summary>
        public bool IsGeneric { get { return GenericArguments.Count > 0; } }
        /// <summary>
        /// Obtient une valeur indiquant si ce type est un array.
        /// </summary>
        public bool IsArray { get { return BaseType.GetFullName() == "Array"; } }
        /// <summary>
        /// Retourne la dimension de ce type, si c'est un type array >1.
        /// Sinon = 0.
        /// </summary>
        public int ArrayDimensions
        {
            get
            {
                if (IsArray)
                    return 1 + GenericArguments[0].ArrayDimensions;
                else
                    return 0;
            }
        }
        /// <summary>
        /// Indique le type d'arguments génériques de ce type.
        /// </summary>
        public List<ClankTypeInstance> GenericArguments { get; set; }

        /// <summary>
        /// Crée une nouvelle instance de ClankTypeInstance.
        /// </summary>
        public ClankTypeInstance()
        {
            GenericArguments = new List<ClankTypeInstance>();
        }

        /// <summary>
        /// Retourne une valeur indiquant si ce type :
        /// 1. est privé
        /// OU
        /// 2. a des paramètres génériques privés.
        /// </summary>
        /// <returns></returns>
        public bool IsPrivateOrHasPrivateGenericArgs()
        {
            if (!BaseType.IsPublic)
                return true;
            foreach(ClankTypeInstance inst in GenericArguments)
            {
                if (inst.IsPrivateOrHasPrivateGenericArgs())
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Retourne une valeur indiquant si ce type ansi que ses arguments génériques 
        /// supportent la sérialisation.
        /// </summary>
        /// <returns></returns>
        public bool DoesSupportSerialization()
        {
            if (!BaseType.SupportSerialization)
                return false;

            foreach(ClankTypeInstance inst in GenericArguments)
            {
                if (!inst.DoesSupportSerialization())
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Nom par lequel se référer à ce type.
        /// </summary>
        /// <returns></returns>
        public virtual string GetFullName()
        {
            if(IsGeneric)
            {
                StringBuilder b = new StringBuilder();
                foreach(ClankTypeInstance type in GenericArguments)
                {
                    b.Append(type.GetFullName());
                    if(type != GenericArguments.Last())
                        b.Append(",");
                }
                return BaseType.GetFullName() + "<" + b.ToString() + ">";
            }

            return BaseType.GetFullName();
        }

        /// <summary>
        /// Crée une instance de ce type pour laquelle les types génériques (du type de base) sont remplacés par
        /// les instances de type appropriés, contenus dans genArgs.
        /// </summary>
        public ClankTypeInstance Instanciate(List<ClankTypeInstance> genArgs)
        {
            ClankTypeInstance newInstance = new ClankTypeInstance();
            // Si on est une instance de type générique, on se fait remplacer par
            // l'argument générique convenable.
            if(this.BaseType is GenericParameterType)
            {
                GenericParameterType parameter = (GenericParameterType)this.BaseType;
                return genArgs[parameter.ParamId];
            }

            // Sinon, on instancie tous les arguments génériques.
            newInstance.GenericArguments = new List<ClankTypeInstance>();
  
            foreach (ClankTypeInstance inst in GenericArguments)
            {
                newInstance.GenericArguments.Add(inst.Instanciate(genArgs));
            }
            newInstance.BaseType = BaseType;
           
            return newInstance;
        }

        /// <summary>
        /// Retourne vrai si les 2 types sont égaux.
        /// </summary>
        public bool Equals (ClankTypeInstance t2)
        {
            if (t2 == null)
                return false;
            return GetFullName() == t2.GetFullName();
        }

    }

    /// <summary>
    /// Représente un type considéré comme paramètre générique.
    /// </summary>
    public class GenericParameterType : ClankType
    {
        /// <summary>
        /// Retourne le numéro de ce paramètre générique dans la déclaration du type parent.
        /// </summary>
        public int ParamId { get; set; }

        /// <summary>
        /// Obtient ou définit le préfixe de ce paramètre générique, permettant de le retrouver dans la TypeTable.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Obtient une valeur indiquant si ce type est public. (vaut true pour un paramètre générique).
        /// </summary>
        public override bool IsPublic
        {
            get
            {
                return true;
            }
            set
            {
                //throw new InvalidOperationException();
            }
        }
        /// <summary>
        /// Obtient une valeur indiquant si ce type supporte la sérialisation (false pour un paramètre générique).
        /// </summary>
        public override bool SupportSerialization
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }
        /// <summary>
        /// Obtient le nom par lequel se référer à se type dans une TypeTable.
        /// </summary>
        /// <returns></returns>
        public override string GetFullName()
        {
            return Prefix + base.GetFullName();
        }
    }
}
