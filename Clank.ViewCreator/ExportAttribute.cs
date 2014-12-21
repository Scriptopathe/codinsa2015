using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.ViewCreator
{
    /// <summary>
    /// Indique qu'un champ doit être exporté par ViewCreator vers le type clank donné.
    /// </summary>
    public class ExportAttribute : Attribute
    {
        public string AttrType { get; set; }
        public string Comment { get; set; }
        public ExportAttribute(string attrtype, string comment)
        {
            AttrType = attrtype;
            Comment = comment;
        }
    }

    public class AccessAttribute : Attribute
    {
        public string ObjectSource { get; set; }
        public string Comment { get; set; }
        public AccessAttribute(string objectSource, string comment)
        {
            ObjectSource = objectSource;
            Comment = comment;
        }
    }
}
