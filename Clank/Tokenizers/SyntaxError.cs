using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Tokenizers
{
    /// <summary>
    /// Classe représentant une erreur de syntaxe.
    /// </summary>
    public class SyntaxError : Exception
    {
        public int Line { get; set; }
        public string Source { get; set; }
        public SyntaxError() : base() { }
        public SyntaxError(string msg, int line, string source) : base(msg) { Line = line; Source = source; }
    }
}
