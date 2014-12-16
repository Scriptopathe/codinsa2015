using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyCarpetExtractor
{
    /// <summary>
    /// Classe d'exceptions levées par l'interpréteur. 
    /// </summary>
    public class InterpreterException : Exception
    {
        public InterpreterException(string message)
            : base(message)
        {
            
        }
    }
}
