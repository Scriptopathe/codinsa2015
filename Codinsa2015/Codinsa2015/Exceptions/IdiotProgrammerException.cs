using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Exceptions
{
    /// <summary>
    /// Type d'exception levée lorsqu'une action illicite est effectuée par le programmeur.
    /// </summary>
    public class IdiotProgrammerException : Exception
    {
        public IdiotProgrammerException(string message) : base(message) { }
    }
}
