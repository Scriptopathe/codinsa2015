using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Generation.Languages
{
    /// <summary>
    /// Attribut que doivent posséder tous les languages.
    /// </summary>
    public class LanguageGeneratorAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Court string permettant d'identifier un langage.
        /// </summary>
        public string LanguageIdentifier { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de LanguageGeneratorAttribute, avec l'identificateur passé
        /// en argument.
        /// </summary>
        public LanguageGeneratorAttribute(string languageIdentifier)
        {
            LanguageIdentifier = languageIdentifier;
        }
        #endregion
    }
}
