using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language.Macros
{
    /// <summary>
    /// Représente une séquence d'instruction (dépendante du language) permettant de traiter les messages côté serveur.
    /// </summary>
    public class ProcessMessageMacro : Instruction
    {
        public AccessContainer Access { get; set; }
        public WriteContainer Write { get; set; }

        public ProcessMessageMacro(AccessContainer access, WriteContainer write) : base()
        {
            Access = access;
            Write = write;
            Source = "generated";
        }
    }
}
