using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Model.Language
{
    /// <summary>
    /// Contient des constantes de sémantique du langage.
    /// </summary>
    public static class SemanticConstants
    {
        #region Special variables
        public const string ClientID = "clientId";
        public const string State = "state";

        #endregion

        #region Language Semantics
        public const string Public = "public";
        public const string Static = "static";
        public const string Constructor = "constructor";
        public const string New = "New";
        public const string Return = "return";
        public const string Class = "class";
        #endregion

        #region Blocks
        public const string MacroBk = "macro";
        public const string AccessBk = "access";
        public const string WriteBk = "write";
        public const string StateBk = "state";
        #endregion
        #region Others


        #endregion
    }
}
