using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor.ExpressionTree
{
    /// <summary>
    /// Représente une structure de donnée dont la valeur peut être modifiée.
    /// </summary>
    public interface ISettable
    {
        /// <summary>
        /// Change la valeur représentée par cet ISettable.
        /// </summary>
        void SetValue(Context context, object value);
    }
}
