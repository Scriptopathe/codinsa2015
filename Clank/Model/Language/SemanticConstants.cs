using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language
{
    /// <summary>
    /// Contient des constantes de sémantique du langage.
    /// </summary>
    public static class SemanticConstants
    {
        #region Special variables
        public const string ClientID = "clientId";
        public const string State = "state";
        public const string StateClass = "State";
        public const string ArrayElementTypeFunc = "getArrayElementType()";
        #endregion

        #region Language Semantics
        public const string JsonArray = "array";
        public const string JsonObject = "object";
        public const string IsSerializable = "serializable";
        public const string Public = "public";
        public const string Static = "static";
        public const string Constructor = "constructor";
        public const string New = "new";
        public const string Return = "return";
        public const string Class = "class";
        public const string Enum = "enum";
        #endregion

        #region Blocks
        public const string MacroBk = "macro";
        public const string AccessBk = "access";
        public const string WriteBk = "write";
        public const string StateBk = "state";
        #endregion

        #region Macro
        public const string SelfKW = "@self";
        public const string ReplaceChr = "$";
        #endregion
        #region Others


        #endregion
    }
}
