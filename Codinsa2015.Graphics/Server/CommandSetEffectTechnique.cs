using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Graphics.Server
{
    /// <summary>
    /// Représente un appel à Effect.CurrentTechnique = Effect.Techniques["name"];
    /// </summary>
    public class CommandSetEffectTechnique :Command
    {
        public RemoteEffect Effect { get; set; }
        public string TechniqueName { get; set; }

        public CommandSetEffectTechnique() { }
        public CommandSetEffectTechnique(RemoteEffect effect, string techniqueName)
        {
            Effect = effect;
            TechniqueName = techniqueName;
        }

    }
}
